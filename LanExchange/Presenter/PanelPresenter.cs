﻿using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using LanExchange.Model;
using LanExchange.Model.Panel;
using LanExchange.SDK;
using LanExchange.UI;
using LanExchange.Utils;

namespace LanExchange.Presenter
{
    public class PanelPresenter : IPanelPresenter
    {
        public const string COMPUTER_MENU = "computer";
        public const string FOLDER_MENU = "folder";
        public const string FILE_MENU = "file";

        private IPanelModel m_Objects;
        private readonly IPanelView m_View;

        public event EventHandler CurrentPathChanged;

        public PanelPresenter(IPanelView view)
        {
            m_View = view;
        }

        public void SetupColumns()
        {
            if (m_Objects.Count > 0)
            {
                var panelItem = m_Objects.GetItemAt(0);
                if (panelItem != null)
                {
                    m_View.ColumnsClear();
                    for (int i = 0; i < panelItem.CountColumns; i++)
                        m_View.AddColumn(panelItem.CreateColumnHeader(i));
                }
            }
        }

        public void UpdateItemsAndStatus()
        {
            if (m_Objects == null) return;
            // refresh only for current page
            var model = AppPresenter.MainPages.GetModel();
            var currentItemList = model.GetItem(model.SelectedIndex);
            if (currentItemList == null || !m_Objects.Equals(currentItemList)) 
                return;
            // get number of visible items (filtered) and number of total items
            var showCount = m_Objects.FilterCount;
            var totalCount = m_Objects.Count;
            if (m_Objects.HasBackItem)
            {
                showCount--;
                totalCount--;
            }
            if (showCount != totalCount)
                MainForm.Instance.ShowStatusText("Items: {0} of {1}", showCount, totalCount);
            else
                MainForm.Instance.ShowStatusText("Items: {0}", showCount);
            SetupColumns();
            m_View.SetVirtualListSize(m_Objects.FilterCount);
            if (m_Objects.FilterCount > 0)
            {
                var index = Objects.IndexOf(currentItemList.FocusedItemText);
                m_View.FocusedItemIndex = index;
            }
            m_View.Filter.UpdateFromModel(Objects);
        }

        private void CurrentPath_Changed(object sender, EventArgs e)
        {
            if (m_Objects != null && CurrentPathChanged != null)
                CurrentPathChanged(sender, e);
        }

        public void CommandCopyValue(int index)
        {
            if (m_Objects == null) return;
            StringBuilder S = new StringBuilder();
            foreach (int selIndex in m_View.SelectedIndexes)
            {
                if (S.Length > 0)
                    S.AppendLine();
                PanelItemBase PItem = m_Objects.GetItemAt(selIndex);
                if (PItem != null)
                    S.Append(@"\\" + PItem[index]);
            }
            if (S.Length > 0)
                m_View.SetClipboardText(S.ToString());
        }

        public void CommandCopySelected()
        {
            if (m_Objects == null) return;
            StringBuilder S = new StringBuilder();
            foreach (int index in m_View.SelectedIndexes)
            {
                if (S.Length > 0)
                    S.AppendLine();
                PanelItemBase PItem = m_Objects.GetItemAt(index);
                if (PItem != null)
                {
                    S.Append(@"\\" + PItem[0]);
                    S.Append("\t");
                    S.Append(PItem[1]);
                }
            }
            if (S.Length > 0)
                m_View.SetClipboardText(S.ToString());
        }

        public void CommandCopyPath()
        {
            // TODO UNCOMMENT THIS
            //if (m_Objects == null) return;
            //StringBuilder S = new StringBuilder();
            //foreach (int index in m_View.SelectedIndexes)
            //{
            //    if (S.Length > 0)
            //        S.AppendLine();
            //    SharePanelItem PItem = m_Objects.GetItemAt(index) as SharePanelItem;
            //    if (PItem != null)
            //        S.Append(String.Format(@"\\{0}\{1}", PItem.ComputerName, PItem.Name));
            //}
            //if (S.Length > 0)
            //    m_View.SetClipboardText(S.ToString());
        }

        internal void Items_Changed(object sender, EventArgs e)
        {
            UpdateItemsAndStatus();
        }

        public IPanelModel Objects
        {
            get { return m_Objects; }
            set
            {
                if (m_Objects != null)
                    m_Objects.CurrentPath.Changed -= CurrentPath_Changed;
                m_Objects = value;
                if (m_Objects != null)
                    m_Objects.CurrentPath.Changed += CurrentPath_Changed;
                m_View.Filter.Presenter.SetModel(value);
                //m_View.SetVirtualListSize(m_Objects.Count);
            }
        }

        public PanelItemBase GetFocusedPanelItem(bool pingAndAsk, bool canReturnParent)
        {
            var item = m_Objects.GetItem(m_View.FocusedItemText);
            // TODO UNCOMMENT THIS! MOVE TO PLUGIN
            //var comp = item as ComputerPanelItem;
            //if (comp != null && pingAndAsk)
            //{
            //    bool bPingResult = PingThread.FastPing(comp.Name);
            //    if (comp.IsPingable != bPingResult)
            //    {
            //        comp.IsPingable = bPingResult;
            //        m_View.RedrawFocusedItem();
            //    }
            //    if (!bPingResult)
            //    {
            //        DialogResult Result = MessageBox.Show(
            //            String.Format("Компьютер «{0}» не доступен посредством PING.\nПродолжить?", comp.Name), "Запрос",
            //            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            //        if (Result != DialogResult.Yes)
            //            item = null;
            //    }
            //}
            if (canReturnParent && item != null && item.Name == PanelItemBase.s_DoubleDot)
                item = item.Parent;
            return item;
        }

        /// <summary>
        /// Run parametrized cmdline for focused panel item.
        /// {0} is computer name
        /// {1} is folder name
        /// </summary>
        /// <param name="tagCmd">cmdline from Tag of menu item</param>
        /// <param name="tagParent">Can be "computer" or "folder"</param>
        public void RunCmdOnFocusedItem(string tagCmd, string tagParent)
        {
            // TODO UNCOMMENT THIS!
            //PanelItemBase PItem = GetFocusedPanelItem(true, false);
            //if (PItem == null) return;

            //string CmdLine;
            //string FmtParam = null;

            //switch (tagParent)
            //{
            //    case COMPUTER_MENU:
            //        if (PItem is ComputerPanelItem)
            //            FmtParam = (PItem as ComputerPanelItem).Name;
            //        else
            //            if (PItem is SharePanelItem)
            //                FmtParam = (PItem as SharePanelItem).ComputerName;
            //        break;
            //    case FOLDER_MENU:
            //        if (PItem is ComputerPanelItem)
            //            return;
            //        if (PItem is SharePanelItem)
            //            FmtParam = String.Format(@"\\{0}\{1}", (PItem as SharePanelItem).ComputerName, (PItem as SharePanelItem).Name);
            //        break;
            //}

            //if (!Kernel32.Is64BitOperatingSystem())
            //    CmdLine = tagCmd.Replace("%ProgramFiles(x86)%", "%ProgramFiles%");
            //else
            //    CmdLine = tagCmd;

            //CmdLine = String.Format(Environment.ExpandEnvironmentVariables(CmdLine), FmtParam);
            //string FName;
            //string Params;
            //AutorunUtils.ExplodeCmd(CmdLine, out FName, out Params);
            //try
            //{
            //    Process.Start(FName, Params);
            //}
            //catch
            //{
            //    m_View.ShowRunCmdError(CmdLine);
            //}
        }

        /// <summary>
        /// Returns computer either focused item is computer or focused item is subitem of computer.
        /// </summary>
        /// <returns>a ComputerPanelItem or null</returns>
        public PanelItemBase GetFocusedComputer(bool pingAndAsk)
        {
            var PItem = GetFocusedPanelItem(pingAndAsk, false);
            if (PItem == null)
                return null;
            // TODO UNCOMMENT THIS!
            //while (!(PItem is ComputerPanelItem) && (PItem.Parent != null))
            //    PItem = PItem.Parent;
            return PItem;
        }

        public bool CommandLevelDown()
        {
            var panelItem = GetFocusedPanelItem(false, false);
            if (panelItem == null || Objects == null) return false;
            if (panelItem.Name == PanelItemBase.s_DoubleDot)
                return CommandLevelUp();
            //var result = AppPresenter.PanelFillers.HasStrategyForParent(panelItem);
            //if (result)
            //{
            //    Objects.CurrentPath.Push(panelItem);
            //    Objects.SyncRetrieveData();
            //}
            //return result;
            return false;
        }

        public bool CommandLevelUp()
        {
            if (Objects == null || Objects.CurrentPath.IsEmpty) 
                return false;
            var panelItem = Objects.CurrentPath.Peek() as PanelItemBase;
            if (panelItem == null) 
                return false;
            var result = AppPresenter.PanelFillers.HasStrategyForParent(panelItem);
            if (result)
            {
                Objects.FocusedItemText = panelItem.Name;
                Objects.CurrentPath.Pop();
                Objects.SyncRetrieveData();
            }
            return result;
            //return false;
        }

        public static string DetectMENU(PanelItemBase PItem)
        {
            while (PItem != null)
            {
                if (PItem.Name == PanelItemBase.s_DoubleDot)
                {
                    PItem = PItem.Parent;
                    continue;
                }
                // TODO UNCOMMENT THIS!
                //if (PItem is SharePanelItem)
                //    return FOLDER_MENU;
                //if (PItem is ComputerPanelItem)
                //    return COMPUTER_MENU;
                //if (PItem is FilePanelItem)
                //    if ((PItem as FilePanelItem).IsDirectory)
                //        return FOLDER_MENU;
                //    else
                //        return FILE_MENU;
            }
            return String.Empty;
        }

        public void CommandDeleteItems()
        {
            //bool Modified = false;
            //foreach (int index in m_View.SelectedIndexes)
            //{
            //    var comp = m_Objects.GetItemAt(index);
            //    if (comp != null && comp.ParentSubject == ConcreteSubject.s_UserItems)
            //    {
            //        m_Objects.Items.Remove(comp);
            //        Modified = true;
            //    }
            //}
            //m_View.ClearSelected();
            //if (Modified)
            //{
            //    m_Objects.DataChanged(null, ConcreteSubject.s_UserItems);
            //}
        }
    }
}
