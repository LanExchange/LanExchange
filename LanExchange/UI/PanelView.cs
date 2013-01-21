﻿using System;
using System.Globalization;
using System.Windows.Forms;
using LanExchange.Strategy;
using LanExchange.View;
using LanExchange.Presenter;
using NLog;
using LanExchange.Utils;
using System.Reflection;
using LanExchange.Model;
using System.Collections.Generic;
using LanExchange.WMI;
using LanExchange.Model.Panel;

namespace LanExchange.UI
{
    public partial class PanelView : UserControl, IPanelView, IListViewItemGetter
    {
        #region Class declarations and constructor
        private readonly static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly static FormPlacement m_WMIPlacement = new FormPlacement();

        private readonly PanelPresenter m_Presenter;
        private readonly ListViewItemCache m_Cache;

        public event EventHandler FocusedItemChanged;

        public PanelView()
        {
            InitializeComponent();
            // init presenters
            m_Presenter = new PanelPresenter(this);
            m_Presenter.CurrentPathChanged += CurrentPath_Changed;

            // Enable double buffer for ListView
            var mi = typeof(Control).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
            mi.Invoke(LV, new object[] { ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true });
            // setup items cache
            m_Cache = new ListViewItemCache(this);
            //LV.CacheVirtualItems += m_Cache.CacheVirtualItems;
            LV.RetrieveVirtualItem += m_Cache.RetrieveVirtualItem;
            // set mycomputer image
            mComp.Image = LanExchangeIcons.SmallImageList.Images[LanExchangeIcons.CompDefault];
            mFolder.Image = LanExchangeIcons.SmallImageList.Images[LanExchangeIcons.FolderNormal];
            // set dropdown direction for sub-menus (actual for dual-monitor system)
            mComp.DropDownDirection = ToolStripDropDownDirection.AboveLeft;
            mFolder.DropDownDirection = ToolStripDropDownDirection.AboveLeft;
            mSendToTab.DropDownDirection = ToolStripDropDownDirection.AboveLeft;

            //mSendToNewTab.Click += new EventHandler(TabController.mSendToNewTab_Click);
        }
        #endregion

        private void CurrentPath_Changed(object sender, EventArgs e)
        {
            var path = sender as ObjectPath;
            if (path != null)
                ePath.Text = path.ToString();
        }

        public void SetupMenu(ContextMenuStrip popTop)
        {
            ToolStripItem[] MyItems = new ToolStripItem[mComp.DropDownItems.Count];
            for (int i = 0; i < MyItems.Length; i++)
            {
                var TI = mComp.DropDownItems[i];
                if (TI is ToolStripSeparator)
                    MyItems[i] = new ToolStripSeparator();
                else
                    if (TI is ToolStripMenuItem)
                        MyItems[i] = MenuUtils.Clone(TI as ToolStripMenuItem);
            }
            popTop.Items.Clear();
            popTop.Items.AddRange(MyItems);
        }

        #region IListViewItemGetter interface implementation
        /// <summary>
        /// IListViewItemGetter implementation. 
        /// This method will be called by ListViewItemCache.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ListViewItem GetListViewItemAt(int index)
        {
            if (m_Presenter.Objects == null)
                return null;
            if (index < 0 || index > Math.Min(m_Presenter.Objects.FilterCount, LV.VirtualListSize) - 1)
                return null;
            ListViewItem Result = new ListViewItem();
            var PItem = m_Presenter.Objects.GetAt(index);
            if (PItem != null)
            {
                Result.Text = PItem[0].ToString();
                for (int i = 1; i < PItem.CountColumns; i++)
                    Result.SubItems.Add(PItem[i].ToString());
                Result.ImageIndex = PItem.ImageIndex;
                Result.ToolTipText = PItem.ToolTipText;
            }
            return Result;
        }
        #endregion

        #region IPanelView interface implementation

        public IFilterView Filter
        {
            get
            {
                return pFilter;
            }
        }

        public IEnumerable<int> SelectedIndices
        {
            get
            {
                foreach (int index in LV.SelectedIndices)
                    yield return index;
            }
        }

        public string FocusedItemText
        {
            get
            {
                return LV.FocusedItem == null ? null : LV.FocusedItem.Text;
            }
        }

        public int FocusedItemIndex
        {
            get
            {
                return LV.FocusedItem == null ? -1 : LV.FocusedItem.Index;
            }
        }

        public void SelectItem(int index)
        {
            LV.SelectedIndices.Add(index);
        }


        public void SetVirtualListSize(int count)
        {
            LV.VirtualListSize = count;
            if (LV.FocusedItem != null)
                LV.FocusedItem.Selected = true;
        }

        public void RedrawFocusedItem()
        {
            if (LV.FocusedItem != null)
            {
                int FocusedIndex = LV.FocusedItem.Index;
                LV.RedrawItems(FocusedIndex, FocusedIndex, false);
            }
        }

        #endregion

        #region PanelView class implementation

        public PanelPresenter GetPresenter()
        {
            return m_Presenter;
        }

        //public ImageList SmallImageList
        //{
        //    get
        //    {
        //        return LV.SmallImageList;
        //    }
        //}

        //public ImageList LargeImageList
        //{
        //    get
        //    {
        //        return LV.LargeImageList;
        //    }
        //    set
        //    {
        //        LV.LargeImageList = value;
        //    }
        //}



        private void DoFocusedItemChanged()
        {
            if (FocusedItemChanged != null)
                FocusedItemChanged(this, EventArgs.Empty);
        }

        public void lvComps_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                DoFocusedItemChanged();
        }

        public void lvComps_KeyPress(object sender, KeyPressEventArgs e)
        {
            pFilter.GetPresenter().LinkedControl_KeyPress(sender, e);
        }

        public void lvComps_KeyDown(object sender, KeyEventArgs e)
        {
            pFilter.GetPresenter().LinkedControl_KeyDown(sender, e);
            ListView lv = (sender as ListView);
            // Ctrl+A - Select all items
            if (e.Control && e.KeyCode == Keys.A)
            {
                User32Utils.SelectAllItems(lv);
                e.Handled = true;
            }
            // Shift+Enter - Open current item
            if (e.Shift && e.KeyCode == Keys.Enter)
            {
                AbstractPanelItem PItem = m_Presenter.GetFocusedPanelItem(false);
                if (PItem is ComputerPanelItem)
                    mCompOpen_Click(mCompOpen, EventArgs.Empty);
                if (PItem is SharePanelItem)
                    mFolderOpen_Click(mFolderOpen, EventArgs.Empty);
                e.Handled = true;
            }
            // Ctrl+Enter - Run RAdmin for computer and FAR for folder
            if (Settings.Instance.AdvancedMode && e.Control && e.KeyCode == Keys.Enter)
            {
                AbstractPanelItem PItem = m_Presenter.GetFocusedPanelItem(false);
                if (PItem is ComputerPanelItem)
                    mCompOpen_Click(mRadmin1, EventArgs.Empty);
                if (PItem is SharePanelItem)
                    if (!(PItem as SharePanelItem).SHI.IsPrinter)
                        mFolderOpen_Click(mFAROpen, EventArgs.Empty);
                e.Handled = true;
            }
            // Backspace - Go level up
            if (e.KeyCode == Keys.Back)
            {
                m_Presenter.LevelUp();
                e.Handled = true;
            }
            // Ctrl+Ins - Copy to clipboard (similar to Ctrl+C)
            // Ctrl+Alt+C - Copy to clipboard and close window
            if (e.Control && e.KeyCode == Keys.Insert || 
                e.Control && e.Alt && e.KeyCode == Keys.C)
            {
                if (mCopyCompName.Enabled)
                    m_Presenter.CopyValueCommand(0);
                else
                    if (mCopyPath.Enabled)
                        m_Presenter.CopyPathCommand();
                if (e.KeyCode == Keys.C)
                    MainForm.Instance.Hide();
                e.Handled = true;
            }

            // TODO need delete only for user items
            //if (e.KeyCode == Keys.Delete)
            //{
            //    for (int i = LV.SelectedIndices.Count - 1; i >= 0; i--)
            //    {
            //        int Index = LV.SelectedIndices[i];
            //        PanelItem Comp = m_Objects.Get(m_Objects.Keys[Index]);
            //        if (Comp != null)
            //            m_Objects.Delete(Comp);
            //    }
            //    LV.SelectedIndices.Clear();
            //    m_Objects.ApplyFilter();
            //    LV.VirtualListSize = m_Objects.FilterCount;
            //}
        }

        private void lvComps_ItemActivate(object sender, EventArgs e)
        {
            m_Presenter.LevelDown();
        }

        private void mWMI_Click(object sender, EventArgs e)
        {
            // check advanced mode
            if (!Settings.Instance.AdvancedMode) return;
            // get focused computer
            ComputerPanelItem comp = m_Presenter.GetFocusedComputer();
            if (comp == null) return;
            // create wmi form
            WMIForm form = new WMIForm(comp);
            // try connect to computer via wmi
            if (!form.GetPresenter().ConnectToComputer())
            {
                form.Dispose();
                return;
            }
            // asynchronous load avaible wmi classes list, if needed
            if (!WMIClassList.Instance.Loaded)
            {
                WMIClassList.Instance.IncludeClasses.Clear();
                foreach (string str in Settings.Instance.WMIClassesInclude)
                    WMIClassList.Instance.IncludeClasses.Add(str);
                BackgroundWorkers.Instance.Add(new BackgroundContext(new WMIClassesInitStrategy()));
            }
            // set MyComputer icon to form
            form.Icon = LanExchangeIcons.GetSmallIcon(LanExchangeIcons.CompDefault);
            // display wmi form
            m_WMIPlacement.AttachToForm(form);
            try
            {
                form.ShowDialog();
            }
            finally
            {
                m_WMIPlacement.DetachFromForm(form);
                form.Dispose();
            }
        }

        public void mCompOpen_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem MenuItem = sender as ToolStripMenuItem;
            if (MenuItem != null)
                m_Presenter.RunCmdOnFocusedItem(MenuItem.Tag.ToString(), PanelPresenter.COMPUTER_MENU);
        }

        public void mFolderOpen_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem MenuItem = sender as ToolStripMenuItem;
            if (MenuItem != null)
                m_Presenter.RunCmdOnFocusedItem(MenuItem.Tag.ToString(), PanelPresenter.FOLDER_MENU);
        }

        private void UpdateViewTypeMenu()
        {
            mCompLargeIcons.Checked = false;
            mCompSmallIcons.Checked = false;
            mCompList.Checked = false;
            mCompDetails.Checked = false;
            switch (LV.View)
            {
                case System.Windows.Forms.View.LargeIcon:
                    mCompLargeIcons.Checked = true;
                    break;
                case System.Windows.Forms.View.SmallIcon:
                    mCompSmallIcons.Checked = true;
                    break;
                case System.Windows.Forms.View.List:
                    mCompList.Checked = true;
                    break;
                case System.Windows.Forms.View.Details:
                    mCompDetails.Checked = true;
                    break;
            }
        }

        public void popComps_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (LV.FocusedItem != null)
                if (LV.FocusedItem.Selected)
                    DoFocusedItemChanged();
            UpdateViewTypeMenu();

            AbstractPanelItem PItem = m_Presenter.GetFocusedPanelItem(false);
            bool bCompVisible = false;
            bool bFolderVisible = false;
            if (PItem != null)
            {
                if (PItem is ComputerPanelItem)
                {
                    var comp = PItem as ComputerPanelItem;
                    mComp.Text = @"\\" + comp.Name;
                    bCompVisible = Settings.Instance.AdvancedMode;
                }
                if (PItem is SharePanelItem)
                {
                    var share = PItem as SharePanelItem;
                    mComp.Text = @"\\" + share.ComputerName;
                    bCompVisible = Settings.Instance.AdvancedMode;
                    if (!String.IsNullOrEmpty(share.Name))
                    {
                        mFolder.Text = String.Format(@"\\{0}\{1}", share.ComputerName, share.Name);
                        mFolder.Image = LanExchangeIcons.SmallImageList.Images[share.ImageIndex];
                        bFolderVisible = true;
                        mFAROpen.Enabled = !share.SHI.IsPrinter;
                    }
                }
            }
            mComp.Enabled = bCompVisible;
            mComp.Visible = Settings.Instance.AdvancedMode;
            if (Settings.Instance.AdvancedMode && !bCompVisible)
            {
                mComp.Text = @"\\<ИмяКомпьютера>";
            }
            SetEnabledAndVisible(mFolder, bFolderVisible);

            const bool bSenderIsComputer = true; // (Pages.SelectedIndex > 0) /*|| (CompBrowser.InternalStack.Count == 0)*/;
            SetEnabledAndVisible(new ToolStripItem[] { 
                mCopyCompName, mCopyComment, mCopySelected, 
                mSendSeparator, mSendToTab }, bSenderIsComputer);
            SetEnabledAndVisible(mCopyPath, !bSenderIsComputer);

            mSeparatorAdmin.Visible = bCompVisible || bFolderVisible || Settings.Instance.AdvancedMode;

            // resolve computer related and folder related shortcut conflict
            mCompOpen.ShowShortcutKeys = bCompVisible && !bFolderVisible;
            mRadmin1.ShowShortcutKeys = bCompVisible && !bFolderVisible;
        }

        private static void SetEnabledAndVisible(ToolStripItem item, bool value)
        {
            item.Enabled = value;
            item.Visible = value;
        }

        private static void SetEnabledAndVisible(ToolStripItem[] items, bool value)
        {
            Array.ForEach(items, item => SetEnabledAndVisible(item, value));
        }

        private void mLargeIcons_Click(object sender, EventArgs e)
        {
            var menuItem = sender as ToolStripMenuItem;
            if (menuItem == null) return;
            int tag;
            if (!int.TryParse(menuItem.Tag.ToString(), out tag))
                tag = 0;
            switch (tag)
            {
                case 1:
                    LV.View = System.Windows.Forms.View.LargeIcon;
                    break;
                case 2:
                    LV.View = System.Windows.Forms.View.SmallIcon;
                    break;
                case 3:
                    LV.View = System.Windows.Forms.View.List;
                    break;
                case 4:
                    LV.View = System.Windows.Forms.View.Details;
                    break;
            }
        }

        private void mCopyCompName_Click(object sender, EventArgs e)
        {
            m_Presenter.CopyValueCommand(0);
        }

        private void mCopyComment_Click(object sender, EventArgs e)
        {
            m_Presenter.CopyValueCommand(1);
        }

        private void mCopySelected_Click(object sender, EventArgs e)
        {
            m_Presenter.CopySelectedCommand();
        }

        private void mCopyPath_Click(object sender, EventArgs e)
        {
            m_Presenter.CopyPathCommand();
        }

        private void mContextClose_Click(object sender, EventArgs e)
        {
            MainForm.Instance.Hide();
        }

        public void FocusListView()
        {
            ActiveControl = LV;
            if (LV.FocusedItem != null)
            {
                if (!LV.FocusedItem.Selected)
                    LV.FocusedItem.Selected = true;
            }
            else
            if (LV.VirtualListSize > 0)
            {
                // TODO need select item with index 0
                LV.Items[0].Selected = true;
                LV.Select();
            }
            DoFocusedItemChanged();
        }
        #endregion

        private void pFilter_FilterCountChanged(object sender, EventArgs e)
        {
            m_Presenter.UpdateItemsAndStatus();
        }

        private void LV_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //TODO column sort for panelitems
            //var sorter = new PanelItemComparer(e.Column, PanelItemComparer.ColumnSortOrder.Ascending);
            //m_Presenter.Objects.Sort(sorter);
        }

        private void ePath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C || e.Control && e.KeyCode == Keys.Insert)
            {
                if (ePath.SelectionLength == 0)
                {
                    Clipboard.SetText(ePath.Text);
                } else
                    Clipboard.SetText(ePath.SelectedText);
                e.Handled = true;
            }
        }
    }
}
