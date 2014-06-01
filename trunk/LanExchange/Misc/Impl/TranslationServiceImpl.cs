﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using LanExchange.Helpers;
using LanExchange.SDK;

namespace LanExchange.Misc.Impl
{
    [Localizable(false)]
    public class TranslationServiceImpl : ITranslationService
    {
        private const string ID_LANGUAGE = "@LANGUAGE_NAME";
        private const string ID_AUTHOR   = "@AUTHOR";
        private const string ID_BASE     = "@BASE";
        private const string ID_TRANSLIT = "@TRANSLIT";
        private const string ID_RTL      = "@RTL";
        private const string TRUE        = "true";

        private readonly IList<string> m_CurrentLanguageLines;
        private readonly IDictionary<string, Type> m_Translits;
        private string m_CurrentLanguage;
        private ITranslitStrategy m_CurrentTranslit;

        public TranslationServiceImpl()
        {
            m_CurrentLanguageLines = new List<string>();
            m_Translits = new Dictionary<string, Type>();
            CurrentLanguage = SourceLanguage;
            //CurrentLanguage = "Russian";
        }
        
        public string SourceLanguage
        {
            get { return "English"; }
        }

        private IEnumerable<string> ReadAllLines(string fileName)
        {
            string line;
            using (var sr = new StreamReader(fileName, Encoding.UTF8))
                while ((line = sr.ReadLine()) != null)
                    yield return line;
        }

        public string CurrentLanguage
        {
            get { return m_CurrentLanguage; } 
            set
            {
                if (String.Compare(SourceLanguage, value, StringComparison.OrdinalIgnoreCase) == 0)
                    m_CurrentLanguage = value;
                else
                    foreach(var fileName in App.FolderManager.GetLanguagesFiles())
                    {
                        var lang = Path.GetFileNameWithoutExtension(fileName);
                        if (lang != null && String.Compare(lang, value, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            RightToLeft = TranslateFromPO(fileName, ID_RTL).Equals(TRUE);
                            var fname = GetBaseFileName(fileName);
                            m_CurrentLanguageLines.Clear();
                            foreach(var line in ReadAllLines(fname))
                                m_CurrentLanguageLines.Add(line);
                            m_CurrentLanguage = value;
                            break;
                        }
                    }
            }
        }

        private string GetBaseFileName(string fileName)
        {
            m_CurrentTranslit = null;
            var baseLanguage = TranslateFromPO(fileName, ID_BASE);
            if (!string.IsNullOrEmpty(baseLanguage))
            {
                var dirName = Path.GetDirectoryName(fileName);
                if (dirName != null)
                {
                    var fname = Path.Combine(dirName, baseLanguage);
                    if (File.Exists(fname))
                    {
                        SetupLanguage(fileName);
                        return fname;
                    }
                }
            }
            return fileName;
        }

        private void SetupLanguage(string fileName)
        {
            Type tp;
            var translit = TranslateFromPO(fileName, ID_TRANSLIT);
            if (m_Translits.TryGetValue(translit, out tp))
                m_CurrentTranslit = (ITranslitStrategy) Activator.CreateInstance(tp);
        }

        public bool RightToLeft { get; private set; }

        public IDictionary<string, string> GetLanguagesNames()
        {
            var sorted = new SortedDictionary<string, string>();
            sorted.Add(SourceLanguage, SourceLanguage);
            foreach (var fileName in App.FolderManager.GetLanguagesFiles())
            {
                var lang = Path.GetFileNameWithoutExtension(fileName);
                if (lang == null) continue;
                try
                {
                    var langName = TranslateFromPO(fileName, ID_LANGUAGE);
                    var translit = TranslateFromPO(fileName, ID_TRANSLIT);
                    if (!string.IsNullOrEmpty(langName))
                        if (string.IsNullOrEmpty(translit) || m_Translits.ContainsKey(translit))
                            sorted.Add(langName, lang);
                }
                catch(ArgumentException)
                {
                }
            }
            return sorted.ToDictionary(pair => pair.Value, pair => pair.Key);
        }

        public IDictionary<string, string> GetTranslations()
        {
            var result = new SortedDictionary<string, string>();
            foreach (var fileName in App.FolderManager.GetLanguagesFiles())
            {
                var lang = Path.GetFileNameWithoutExtension(fileName);
                if (lang == null) continue;
                try
                {
                    var author = TranslateFromPO(fileName, ID_AUTHOR);
                    if (!string.IsNullOrEmpty(author))
                        result.Add(lang, author);
                }
                catch (ArgumentException)
                {
                }
            }
            return result;
        }

        private string InternalTranslate(IEnumerable<string> lines, string id)
        {
            var marker = string.Format(CultureInfo.InvariantCulture, "msgid \"{0}\"", id);
            var markerFound = false;
            const string MSGSTR_MARKER = "msgstr \"";
            foreach (var line in lines)
            {
                if (line.Equals(marker))
                {
                    markerFound = true;
                    continue;
                }
                if (markerFound && line.StartsWith(MSGSTR_MARKER, StringComparison.Ordinal) && 
                    line.Substring(line.Length - 1, 1).Equals("\""))
                {
                    var result = line.Substring(MSGSTR_MARKER.Length, line.Length - MSGSTR_MARKER.Length - 1);
                    return result;
                }
            }
            return id.StartsWith("@") ? string.Empty : id;
        }

        private string TranslateFromPO(string fileName, string id)
        {
            return InternalTranslate(ReadAllLines(fileName), id);
        }

        public string Translate(string id)
        {
            if (SourceLanguage.Equals(CurrentLanguage))
                return id;
            var result = InternalTranslate(m_CurrentLanguageLines, id);
            if (m_CurrentTranslit != null)
                result = m_CurrentTranslit.Transliterate(result);
            return result;
        }

        public string PluralForm(string forms, int num)
        {
            switch (CurrentLanguage)
            {
                case "Kazakh":
                    var arr = forms.Split('|');
                    var index = PluralFormKazakh(num);
                    return index <= arr.Length - 1 ? arr[index] : arr[0];
                default:
                    return forms;
            }
        }

        /// <summary>
        /// Translation of plural form in Kazakh language.
        /// </summary>
        /// <param name="num"></param>
        /// <returns>0: "-дан", 1: "-ден", 2: "-нан", 3: "-нен", 4: "-тан", 5: "-тен".</returns>
        private static int PluralFormKazakh(int num)
        {
            if (num%10 == 6 || num%10 == 9 || num%100 == 20 || num%100 == 30)
                return 0;
            if (num%10 == 1 || num%10 == 2 || num%10 == 7 || num%10 == 8 || num%100 == 50 || num%1000 == 100)
                return 1;
            if (num%100 == 10 || num%100 == 90)
                return 2;
            if (num%100 == 80)
                return 3;
            if (num%100 == 40 || num%100 == 60)
                return 4;
            if (num%10 == 3 || num%10 == 4 || num%10 == 5 || num%100 == 70)
                return 5;
            return 0;
        }

        [Localizable(false)]
        public void SetResourceManagerTo<TClass>() where TClass : class
        {
            var resourceMan = new TranslationResourceManager(typeof (TClass).FullName, typeof (TClass).Assembly);
            ReflectionUtils.SetClassPrivateField<TClass, ResourceManager>("resourceMan", resourceMan);
        }

        public void RegisterTranslit<TTranslit>() where TTranslit : ITranslitStrategy
        {
            m_Translits.Add(typeof(TTranslit).Name, typeof(TTranslit));
        }
    }
}