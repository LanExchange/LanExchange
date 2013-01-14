﻿using System;
using System.Text;
using LanExchange.View;
using LanExchange.Model;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Net;
using NLog;
using System.Reflection;

namespace LanExchange.Presenter
{
    /// <summary>
    /// Presenter for Settings (model) and AboutForm (view).
    /// </summary>
    public class AboutPresenter
    {
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IAboutView m_View;
        private readonly BackgroundWorker DoCheckVersion;
        private readonly BackgroundWorker DoUpdate;

        private string UpdateError;
        private bool bNeedRestart;
        private string FileListContent;


        public AboutPresenter(IAboutView view)
        {
            // initialize background workers
            DoCheckVersion = new BackgroundWorker();
            DoCheckVersion.DoWork += DoCheckVersion_DoWork;
            DoCheckVersion.RunWorkerCompleted += DoCheckVersion_RunWorkerCompleted;
            DoUpdate = new BackgroundWorker();
            DoUpdate.DoWork += DoUpdate_DoWork;
            DoUpdate.RunWorkerCompleted += DoUpdate_RunWorkerCompleted;

            m_View = view;
            m_View.ShowMessage("Проверка обновлений...", Color.Gray);
            if (!DoCheckVersion.IsBusy)
                DoCheckVersion.RunWorkerAsync();
        }

        public void LoadFromModel()
        {
            m_View.WebText += Settings.Instance.GetWebSiteUrl();
            m_View.EmailText += Settings.Instance.GetEmailAddress();
        }

        public void StartUpdate()
        {
            m_View.ShowProgressBar();
            if (!DoUpdate.IsBusy)
                DoUpdate.RunWorkerAsync();
        }

        public static void OpenWebLink()
        {
            Process.Start("http://" + Settings.Instance.GetWebSiteUrl());
        }

        public static void OpenEmailLink()
        {
            Process.Start("mailto:" + Settings.Instance.GetEmailAddress());
        }

        public static string GetFileListURL()
        {
            return Settings.Instance.GetUpdateUrl() + "filelist.php";
        }

        private static void DoCheckVersion_DoWork(object sender, DoWorkEventArgs e)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.Proxy = null;
                    string URL = GetFileListURL();
                    logger.Info("Downloading text from url [{0}]", URL);
                    e.Result = client.DownloadString(URL);
                }
                catch (Exception ex)
                {
                    logger.Error("DoCheckVersion_DoWork() {0}", ex.Message);
                    e.Cancel = true;
                }
            }
        }

        private void DoCheckVersion_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                m_View.ShowMessage("Не удалось подключиться к серверу обновлений.", Color.Red);
                return;
            }
            FileListContent = (string)e.Result;
            using (var Reader = new StringReader(FileListContent))
            {
                var siteVersion = new Version(Reader.ReadLine());
                var assembly = Assembly.GetExecutingAssembly();
                if (assembly.GetName().Version.CompareTo(siteVersion) < 0)
                    m_View.ShowUpdateButton(siteVersion);
                else
                    m_View.ShowMessage("Установлена последняя версия LanExchange.", Color.Gray);
            }
        }

        private void DoUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                using (var client = new WebClient { Proxy = null })
                {
                    using (var StrReader = new StringReader(FileListContent))
                    {
                        StrReader.ReadLine();
                        string line;
                        string LocalFName;
                        string LocalDirName;
                        string[] Arr;
                        string ExeName = Settings.GetExecutableFileName();
                        string ExePath = Path.GetDirectoryName(ExeName);
                        while (!String.IsNullOrEmpty(line = StrReader.ReadLine()))
                        {
                            Arr = line.Split('|');
                            string RemoteMD5 = Arr[0];
                            int RemoteFSize = Int32.Parse(Arr[1]);
                            string RemoteFName = Arr[2];
                            string MustChangeFName = Arr[3];
                            LocalFName = Path.Combine(ExePath, MustChangeFName);
                            LocalDirName = Path.GetDirectoryName(LocalFName);
                            bool bNeedDownload = false;
                            if (File.Exists(LocalFName))
                            {
                                bool verify = verifyMd5File(LocalFName, RemoteFSize, RemoteMD5);
                                if (!verify)
                                    bNeedDownload = true;
                            }
                            else
                                bNeedDownload = true;
                            if (bNeedDownload)
                            {
                                if (LocalFName.Equals(ExeName))
                                {
                                    string FName = Path.ChangeExtension(LocalFName, ".old.exe");
                                    if (File.Exists(FName))
                                        File.Delete(FName);
                                    File.Move(LocalFName, FName);
                                    bNeedRestart = true;
                                }
                                else
                                    if (File.Exists(LocalFName))
                                        File.Delete(LocalFName);
                                    else
                                        if (!Directory.Exists(LocalDirName))
                                            Directory.CreateDirectory(LocalDirName);
                                string URL = Settings.Instance.GetUpdateUrl() + RemoteFName;
                                logger.Info("Downloading file from url [{0}] and saving to [{1}]", URL, LocalFName);
                                client.DownloadFile(URL, LocalFName);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                UpdateError = ex.Message;
                logger.Info("Error: ", ex.Message);
            }
        }

        private void DoUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                m_View.ShowMessage("Обновление не удалось: " + UpdateError, Color.Red);
            }
            else
            {
                m_View.ShowMessage("Обвновление успешно завершено.", Color.Gray);
                if (bNeedRestart)
                {
                    // закрываем окно
                    m_View.CancelView();
                    // перезапуск приложения
                    MainPresenter.Instance.MainView.Restart();
                }
            }
        }

        private static bool verifyMd5File(string LocalFName, int RemoteFSize, string RemoteMD5)
        {
            bool Result;
            using (var FS = File.Open(LocalFName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (FS.Length == RemoteFSize)
                {
                    byte[] content = new byte[FS.Length];
                    FS.Read(content, 0, (int)FS.Length);

                    var md5Hasher = MD5.Create();
                    byte[] data = md5Hasher.ComputeHash(content);
                    var sBuilder = new StringBuilder();
                    for (int i = 0; i < data.Length; i++)
                        sBuilder.Append(data[i].ToString("x2"));
                    string hashOfInput = sBuilder.ToString();

                    var comparer = StringComparer.OrdinalIgnoreCase;
                    Result = (0 == comparer.Compare(hashOfInput, RemoteMD5));
                }
                else
                    Result = false;
                FS.Close();
            }
            return Result;
        }
    }
}
