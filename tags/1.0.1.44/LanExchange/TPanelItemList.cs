﻿#define NOUSE_NORTHWIND_DATA

using System;
using System.Collections.Generic;
using System.Text;
using OSTools;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NLog;

namespace LanExchange
{
    public class TPanelItemList 
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        
        private Dictionary<string, TPanelItem> Data = null;
        public List<string> Keys = null;
        private String Filter = "";

        public TPanelItemList()
        {
            Data = new Dictionary<string, TPanelItem>();
            Keys = new List<string>();
        }
   
        public void Add(TPanelItem Comp)
        {
            if (Comp != null)
              if (!String.IsNullOrEmpty(Comp.Name))
                if (!Data.ContainsKey(Comp.Name))
                    Data.Add(Comp.Name, Comp);
        }

        public void Delete(TPanelItem Comp)
        {
            Data.Remove(Comp.Name);
        }

        public TPanelItem Get(string key)
        {
            TPanelItem Result = null;
            if (Data.TryGetValue(key, out Result))
            {
                Result.Name = key;
                return Result;
            }
            else
                return null;
        }

        public void Clear()
        {
            Data.Clear();
        }

        private bool GoodForFilter(string[] A, string Filter1, string Filter2)
        {
            for (int i = 0; i < A.Length; i++)
            {
                if (i == 0)
                {
                    if (TPuntoSwitcher.RussianContains(A[i], Filter1) || (TPuntoSwitcher.RussianContains(A[i], Filter2)))
                        return true;
                } else
                if (Filter1 != null && A[i].Contains(Filter1) || Filter2 != null && A[i].Contains(Filter2))
                    return true;
            }
            return false;
        }

        public void ApplyFilter()
        {
            bool bFiltered = IsFiltered;
            Keys.Clear();
            string Filter1 = FilterText.ToUpper();
            string Filter2 = TPuntoSwitcher.Change(FilterText);
            if (Filter2 != null) Filter2 = Filter2.ToUpper();
            foreach (var Pair in Data)
            {
                string[] A = Pair.Value.getStrings();
                if (!bFiltered || Pair.Value.Name == ".." || GoodForFilter(A, Filter1, Filter2))
                    Keys.Add(Pair.Value.Name);
            }
        }

        public bool IsFiltered
        {
            get { return !String.IsNullOrEmpty(Filter); }
        }

        // Возвращает количество компов в списке
        public int Count
        {
            get { return Data.Count; }
        }

        // Возвращает число записей в фильтре
        public int FilterCount
        {
            get { return Keys.Count; }
        }

        public String FilterText
        {
            get { return Filter; }
            set
            {
                Filter = value;
                ApplyFilter();
            }
        }

#if USE_NORTHWIND_DATA
        struct TItem
        {
            public string name;
            public string comment;

            public TItem(string s1, string s2)
            {
                name = s1;
                comment = s2;
            }
        };

        public static void AddNorthWindData(List<TPanelItem> Result)
        {
            TItem[] Data = new TItem[38] {
                new TItem("GLADKIH-A", "Гладких Андрей"),
                new TItem("ILINA-YU", "Ильина Юлия"),
                new TItem("KLIMOV-S", "Климов Сергей"),
                new TItem("KOREPIN-V", "Корепин Вадим"),
                new TItem("KULIKOV-E", "Куликов Евгений"),
                new TItem("NOVIKOV-N", "Новиков Николай"),
                new TItem("OZHOGINA-I", "Ожогина Инна"),
                new TItem("POPKOVA-D", "Попкова Дарья"),
                new TItem("SERGIENKO-M", "Сергиенко Мария"),
                new TItem("KOSTERINA-O", "Костерина Ольга"),
                new TItem("VERNYJ-G", "Верный Григорий"),
                new TItem("EGOROV-V", "Егоров Владимир"),
                new TItem("OMELCHENKO-S", "Омельченко Светлана"),
                new TItem("PESOTSKIJ-S", "Песоцкий Станислав"),
                new TItem("SHASHKOV-R", "Шашков Руслан"),
                new TItem("VRONSKIJ-YU", "Вронский Юрий"),
                new TItem("PODKOLZINA-E", "Подколзина Екатерина"),
                new TItem("ERJOMENKO-A", "Ерёменко Алексей"),
                new TItem("GRACHEV-N", "Грачев Николай"),
                new TItem("OREHOV-A", "Орехов Алексей"),
                new TItem("VOLODIN-V", "Володин Виктор"),
                new TItem("TUMANOV-A", "Туманов Александр"),
                new TItem("GORNOZHENKO-D", "Горноженко Дмитрий"),
                new TItem("SALIMZYANOVA-D", "Салимзянова Дина"),
                new TItem("USHAKOV-V", "Ушаков Валерий"),
                new TItem("VAZHIN-F", "Важин Филип"),
                new TItem("MISHKOVA-E", "Мишкова Екатерина"),
                new TItem("EFIMOV-A", "Ефимов Александр"),
                new TItem("FOMIN-G", "Фомин Георгий"),
                new TItem("TOLMACHEV-V", "Толмачев Виктор"),
                new TItem("IGNATOV-S", "Игнатов Степан"),
                new TItem("ENTIN-M", "Энтин Михаил"),
                new TItem("SHUTOV-I", "Шутов Игнат"),
                new TItem("BORISOV-S", "Борисов Сергей"),
                new TItem("IVANOV-A", "Иванов Андрей"),
                new TItem("TIMOFEEVA-K", "Тимофеева Кристина"),
                new TItem("BEREZIN-A", "Березин Артур"),
                new TItem("YARTSEV-S", "Ярцев Семен")
            };
            foreach (TItem Item in Data)
            {
                TComputerItem Comp = new TComputerItem(Item.name, Item.comment, 500, 5, 1, 0);
                Result.Add(Comp);
            }
        }
#endif

        // получим список всех компьюетеров
        public static List<TPanelItem> GetServerList()
        {
            LocalNetwork.SERVER_INFO_101 si;
            IntPtr pInfo = IntPtr.Zero;
            int entriesread = 0;
            int totalentries = 0;
            List<TPanelItem> Result = new List<TPanelItem>();
            try
            {
                logger.Info("WINAPI NetServerEnum");
                LocalNetwork.NERR err = LocalNetwork.NetServerEnum(null, 101, out pInfo, -1, ref entriesread, ref totalentries, LocalNetwork.SV_101_TYPES.SV_TYPE_ALL, null, 0);
                logger.Info("WINAPI NetServerEnum: result={0}, entriesread={1}, totalentries={2}", err, entriesread, totalentries);
                if ((err == LocalNetwork.NERR.NERR_Success || err == LocalNetwork.NERR.ERROR_MORE_DATA) && pInfo != IntPtr.Zero)
                {
                    int ptr = pInfo.ToInt32();
                    for (int i = 0; i < entriesread; i++)
                    {
                        si = (LocalNetwork.SERVER_INFO_101)Marshal.PtrToStructure(new IntPtr(ptr), typeof(LocalNetwork.SERVER_INFO_101));
                        // в режиме пользователя не сканируем: сервера, контроллеры домена
                        //bool bServer = (si.sv101_type & 0x8018) != 0;
                        //if (Program.AdminMode || !bServer)
                        Result.Add(new TComputerItem(si.sv101_name, si.sv101_comment, si.sv101_platform_id, si.sv101_version_major, si.sv101_version_minor, si.sv101_type));
                        ptr += Marshal.SizeOf(si);
                    }
                }
            }
            catch (Exception) { /* обработка ошибки нифига не делаем :(*/ }
            finally
            { // освобождаем выделенную память
                if (pInfo != IntPtr.Zero) LocalNetwork.NetApiBufferFree(pInfo);
            }
            #if USE_NORTHWIND_DATA
            AddNorthWindData(Result);
            #endif
            return Result;
        }

        public static List<TPanelItem> EnumNetShares(string Server)
        {
            List<TPanelItem> Result = new List<TPanelItem>();
            Result.Add(new TShareItem("..", @"\\" + Server, 0, Server));
            int entriesread = 0;
            int totalentries = 0;
            int resume_handle = 0;
            int nStructSize = Marshal.SizeOf(typeof(LocalNetwork.SHARE_INFO_1));
            IntPtr bufPtr = IntPtr.Zero;
            StringBuilder server = new StringBuilder(Server);
            logger.Info("WINAPI NetShareEnum");
            int ret = LocalNetwork.NetShareEnum(server, 1, ref bufPtr, LocalNetwork.MAX_PREFERRED_LENGTH, ref entriesread, ref totalentries, ref resume_handle);
            if (ret == LocalNetwork.NERR_Success)
            {
                logger.Info("WINAPI NetServerEnum result: entriesread={0}, totalentries={1}", entriesread, totalentries);
                IntPtr currentPtr = bufPtr;
                for (int i = 0; i < entriesread; i++)
                {
                    LocalNetwork.SHARE_INFO_1 shi1 = (LocalNetwork.SHARE_INFO_1)Marshal.PtrToStructure(currentPtr, typeof(LocalNetwork.SHARE_INFO_1));
                    if ((shi1.shi1_type & (uint)LocalNetwork.SHARE_TYPE.STYPE_IPC) != (uint)LocalNetwork.SHARE_TYPE.STYPE_IPC)
                        Result.Add(new TShareItem(shi1.shi1_netname, shi1.shi1_remark, shi1.shi1_type, Server));
                    else
                        logger.Info("Skiping IPC$ share");
                    currentPtr = new IntPtr(currentPtr.ToInt32() + nStructSize);
                }
                LocalNetwork.NetApiBufferFree(bufPtr);
            }
            else
            {
                logger.Info("WINAPI NetServerEnum error: {0}", ret);
            }

            TPanelItemComparer comparer = new TPanelItemComparer();
            Result.Sort(comparer);
            return Result;
        }


        public List<string> ListView_GetSelected(ListView LV, bool bAll)
        {
            List<string> Result = new List<string>();
            if (LV.FocusedItem != null)
                Result.Add(LV.FocusedItem.Text);
            else
                Result.Add("");
            if (bAll)
                for (int index = 0; index < LV.Items.Count; index++)
                    Result.Add(Keys[index]);
            else
                foreach (int index in LV.SelectedIndices)
                    Result.Add(Keys[index]);
            return Result;
        }
        
        public void ListView_SetSelected(ListView LV, List<string> SaveSelected)
        {
            LV.SelectedIndices.Clear();
            LV.FocusedItem = null;
            if (LV.VirtualListSize > 0)
            {
                for (int i = 0; i < SaveSelected.Count; i++)
                {
                    int index = Keys.IndexOf(SaveSelected[i]);
                    if (index == -1) continue;
                    if (i == 0)
                    {
                        LV.FocusedItem = LV.Items[index];
                        LV.EnsureVisible(index);
                    }
                    else
                        LV.SelectedIndices.Add(index);
                }
            }
        }

        // <summary>
        // Выбор компьютера по имени в списке.
        // </summary>
        public void ListView_SelectComputer(ListView LV, string CompName)
        {
            int index = -1;
            // пробуем найти запомненный элемент
            if (CompName != null)
            {
                index = this.Keys.IndexOf(CompName);
                if (index == -1) index = 0;
            }
            else
                index = 0;
            // установка текущего элемента
            if (LV.VirtualListSize > 0)
            {
                LV.SelectedIndices.Add(index);
                LV.FocusedItem = LV.Items[index];
                LV.EnsureVisible(index);
            }
        }

        #region Привязка своего обьекта к ListView

        public static void ListView_SetObject(ListView LV, TPanelItemList ItemList)
        {
            LV.Tag = ItemList;
        }

        public static TPanelItemList ListView_GetObject(ListView LV)
        {
            return LV.Tag as TPanelItemList;
        }

        public static TPanelItemList ListView_CreateObject(ListView LV)
        {
            TPanelItemList Result = new TPanelItemList();
            logger.Info("Create object {0}", Result.ToString());
            ListView_SetObject(LV, Result);
            return Result;
        }

        public static void ListView_DeleteObject(ListView LV)
        {
            TPanelItemList List = ListView_GetObject(LV);
            if (List != null)
                ListView_SetObject(LV, null);
        }
        #endregion

        public List<string> ToList()
        {
            List<string> Result = new List<string>();
            foreach (var Pair in Data)
                Result.Add(Pair.Value.Name);
            return Result;
        }
    }
}
