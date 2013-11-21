﻿using System.Windows.Forms;

namespace LanExchange.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                m_Hotkeys.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.popTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mTrayOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mTraySep1 = new System.Windows.Forms.ToolStripSeparator();
            this.mTrayAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mTrayExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.Status = new System.Windows.Forms.StatusStrip();
            this.lItemsCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSep1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lCompName = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusSep2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lUserName = new System.Windows.Forms.ToolStripStatusLabel();
            this.tipComps = new System.Windows.Forms.ToolTip(this.components);
            this.popTop = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pInfo = new LanExchange.UI.InfoView();
            this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.mPanel = new System.Windows.Forms.MenuItem();
            this.mPanelReRead = new System.Windows.Forms.MenuItem();
            this.mPanelSep2 = new System.Windows.Forms.MenuItem();
            this.mPanelCloseTab = new System.Windows.Forms.MenuItem();
            this.mPanelCloseOther = new System.Windows.Forms.MenuItem();
            this.mPanelSep3 = new System.Windows.Forms.MenuItem();
            this.mPanelExit = new System.Windows.Forms.MenuItem();
            this.mView = new System.Windows.Forms.MenuItem();
            this.mViewInfo = new System.Windows.Forms.MenuItem();
            this.mViewGrid = new System.Windows.Forms.MenuItem();
            this.mViewSep1 = new System.Windows.Forms.MenuItem();
            this.mViewLarge = new System.Windows.Forms.MenuItem();
            this.mViewSmall = new System.Windows.Forms.MenuItem();
            this.mViewList = new System.Windows.Forms.MenuItem();
            this.mViewDetails = new System.Windows.Forms.MenuItem();
            this.mLanguage = new System.Windows.Forms.MenuItem();
            this.mHelp = new System.Windows.Forms.MenuItem();
            this.mHelpKeys = new System.Windows.Forms.MenuItem();
            this.mHelpSep1 = new System.Windows.Forms.MenuItem();
            this.mHelpWeb = new System.Windows.Forms.MenuItem();
            this.mHelpLangs = new System.Windows.Forms.MenuItem();
            this.mHelpBugs = new System.Windows.Forms.MenuItem();
            this.mHelpSep2 = new System.Windows.Forms.MenuItem();
            this.mHelpAbout = new System.Windows.Forms.MenuItem();
            this.popTray.SuspendLayout();
            this.Status.SuspendLayout();
            this.SuspendLayout();
            // 
            // TrayIcon
            // 
            this.TrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.TrayIcon.ContextMenuStrip = this.popTray;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseUp);
            // 
            // popTray
            // 
            this.popTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mTrayOpen,
            this.mTraySep1,
            this.mTrayAbout,
            this.mTrayExit});
            this.popTray.Name = "popTray";
            this.popTray.Size = new System.Drawing.Size(180, 76);
            this.popTray.Opening += new System.ComponentModel.CancelEventHandler(this.popTray_Opening);
            // 
            // mTrayOpen
            // 
            this.mTrayOpen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.mTrayOpen.Name = "mTrayOpen";
            this.mTrayOpen.ShortcutKeyDisplayString = global::LanExchange.Properties.Resources.KeyCtrlWinX;
            this.mTrayOpen.Size = new System.Drawing.Size(179, 22);
            this.mTrayOpen.Text = global::LanExchange.Properties.Resources.MainForm_Open;
            this.mTrayOpen.Click += new System.EventHandler(this.mOpen_Click);
            // 
            // mTraySep1
            // 
            this.mTraySep1.Name = "mTraySep1";
            this.mTraySep1.Size = new System.Drawing.Size(176, 6);
            // 
            // mTrayAbout
            // 
            this.mTrayAbout.Name = "mTrayAbout";
            this.mTrayAbout.Size = new System.Drawing.Size(179, 22);
            this.mTrayAbout.Text = global::LanExchange.Properties.Resources.MainForm_About;
            this.mTrayAbout.Click += new System.EventHandler(this.mAbout_Click);
            // 
            // mTrayExit
            // 
            this.mTrayExit.Name = "mTrayExit";
            this.mTrayExit.Size = new System.Drawing.Size(179, 22);
            this.mTrayExit.Text = global::LanExchange.Properties.Resources.MainForm_Exit;
            this.mTrayExit.Click += new System.EventHandler(this.mExit_Click);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 17);
            // 
            // Status
            // 
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lItemsCount,
            this.StatusSep1,
            this.lCompName,
            this.StatusSep2,
            this.lUserName});
            this.Status.Location = new System.Drawing.Point(0, 520);
            this.Status.Name = "Status";
            this.Status.ShowItemToolTips = true;
            this.Status.Size = new System.Drawing.Size(564, 22);
            this.Status.TabIndex = 15;
            this.Status.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Status_MouseDoubleClick);
            // 
            // lItemsCount
            // 
            this.lItemsCount.Name = "lItemsCount";
            this.lItemsCount.Size = new System.Drawing.Size(503, 17);
            this.lItemsCount.Spring = true;
            this.lItemsCount.Text = global::LanExchange.Properties.Resources.EmptyText;
            this.lItemsCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lItemsCount.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lItemsCount_MouseUp);
            // 
            // StatusSep1
            // 
            this.StatusSep1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.StatusSep1.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.StatusSep1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.StatusSep1.Name = "StatusSep1";
            this.StatusSep1.Size = new System.Drawing.Size(4, 17);
            // 
            // lCompName
            // 
            this.lCompName.Name = "lCompName";
            this.lCompName.Size = new System.Drawing.Size(19, 17);
            this.lCompName.Text = global::LanExchange.Properties.Resources.EmptyText;
            this.lCompName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lCompName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lCompName_MouseDown);
            this.lCompName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lCompName_MouseUp);
            // 
            // StatusSep2
            // 
            this.StatusSep2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.StatusSep2.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.StatusSep2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.None;
            this.StatusSep2.Name = "StatusSep2";
            this.StatusSep2.Size = new System.Drawing.Size(4, 17);
            // 
            // lUserName
            // 
            this.lUserName.Name = "lUserName";
            this.lUserName.Size = new System.Drawing.Size(19, 17);
            this.lUserName.Text = global::LanExchange.Properties.Resources.EmptyText;
            this.lUserName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lCompName_MouseDown);
            // 
            // tipComps
            // 
            this.tipComps.IsBalloon = true;
            this.tipComps.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tipComps.Popup += new System.Windows.Forms.PopupEventHandler(this.tipComps_Popup);
            // 
            // popTop
            // 
            this.popTop.Name = "popTop";
            this.popTop.Size = new System.Drawing.Size(61, 4);
            this.popTop.Opening += new System.ComponentModel.CancelEventHandler(this.popTop_Opening);
            // 
            // pInfo
            // 
            this.pInfo.AllowDrop = true;
            this.pInfo.BackColor = System.Drawing.SystemColors.Window;
            this.pInfo.ContextMenuStrip = this.popTop;
            this.pInfo.CurrentItem = null;
            this.pInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pInfo.Location = new System.Drawing.Point(0, 0);
            this.pInfo.MinimumSize = new System.Drawing.Size(0, 64);
            this.pInfo.Name = "pInfo";
            this.pInfo.NumLines = 3;
            this.pInfo.Size = new System.Drawing.Size(564, 64);
            this.pInfo.TabIndex = 23;
            this.pInfo.DragDrop += new System.Windows.Forms.DragEventHandler(this.pInfo_DragDrop);
            this.pInfo.DragOver += new System.Windows.Forms.DragEventHandler(this.pInfo_DragOver);
            // 
            // MainMenu
            // 
            this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mPanel,
            this.mView,
            this.mLanguage,
            this.mHelp});
            // 
            // mPanel
            // 
            this.mPanel.Index = 0;
            this.mPanel.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mPanelReRead,
            this.mPanelSep2,
            this.mPanelCloseTab,
            this.mPanelCloseOther,
            this.mPanelSep3,
            this.mPanelExit});
            this.mPanel.Text = global::LanExchange.Properties.Resources.MenuPanel;
            this.mPanel.Popup += new System.EventHandler(this.mPanel_Popup);
            // 
            // mPanelReRead
            // 
            this.mPanelReRead.Index = 0;
            this.mPanelReRead.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
            this.mPanelReRead.Text = global::LanExchange.Properties.Resources.KeyCtrlR_;
            this.mPanelReRead.Click += new System.EventHandler(this.rereadToolStripMenuItem_Click);
            // 
            // mPanelSep2
            // 
            this.mPanelSep2.Index = 1;
            this.mPanelSep2.Text = "-";
            // 
            // mPanelCloseTab
            // 
            this.mPanelCloseTab.Index = 2;
            this.mPanelCloseTab.Shortcut = System.Windows.Forms.Shortcut.CtrlW;
            this.mPanelCloseTab.Text = global::LanExchange.Properties.Resources.PagesView_CloseTab;
            this.mPanelCloseTab.Click += new System.EventHandler(this.mCloseTab_Click);
            // 
            // mPanelCloseOther
            // 
            this.mPanelCloseOther.Index = 3;
            this.mPanelCloseOther.Text = global::LanExchange.Properties.Resources.PagesView_CloseOtherTabs;
            this.mPanelCloseOther.Click += new System.EventHandler(this.mCloseOther_Click);
            // 
            // mPanelSep3
            // 
            this.mPanelSep3.Index = 4;
            this.mPanelSep3.Text = "-";
            // 
            // mPanelExit
            // 
            this.mPanelExit.Index = 5;
            this.mPanelExit.Shortcut = System.Windows.Forms.Shortcut.F10;
            this.mPanelExit.Text = global::LanExchange.Properties.Resources.KeyF10_;
            this.mPanelExit.Click += new System.EventHandler(this.mExit_Click);
            // 
            // mView
            // 
            this.mView.Index = 1;
            this.mView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mViewInfo,
            this.mViewGrid,
            this.mViewSep1,
            this.mViewLarge,
            this.mViewSmall,
            this.mViewList,
            this.mViewDetails});
            this.mView.Text = global::LanExchange.Properties.Resources.MainForm_View;
            this.mView.Popup += new System.EventHandler(this.mView_Popup);
            // 
            // mViewInfo
            // 
            this.mViewInfo.Checked = true;
            this.mViewInfo.Index = 0;
            this.mViewInfo.Text = global::LanExchange.Properties.Resources.MainForm_InfoPanel;
            this.mViewInfo.Click += new System.EventHandler(this.mInfoPanel_Click);
            // 
            // mViewGrid
            // 
            this.mViewGrid.Checked = true;
            this.mViewGrid.Index = 1;
            this.mViewGrid.Text = global::LanExchange.Properties.Resources.MainForm_GridLines;
            this.mViewGrid.Click += new System.EventHandler(this.mGridLines_Click);
            // 
            // mViewSep1
            // 
            this.mViewSep1.Index = 2;
            this.mViewSep1.Text = "-";
            // 
            // mViewLarge
            // 
            this.mViewLarge.Index = 3;
            this.mViewLarge.RadioCheck = true;
            this.mViewLarge.Tag = "0";
            this.mViewLarge.Text = global::LanExchange.Properties.Resources.ViewLargeIcons;
            this.mViewLarge.Click += new System.EventHandler(this.mViewLarge_Click);
            // 
            // mViewSmall
            // 
            this.mViewSmall.Index = 4;
            this.mViewSmall.RadioCheck = true;
            this.mViewSmall.Tag = "2";
            this.mViewSmall.Text = global::LanExchange.Properties.Resources.ViewSmallIcons;
            this.mViewSmall.Click += new System.EventHandler(this.mViewLarge_Click);
            // 
            // mViewList
            // 
            this.mViewList.Index = 5;
            this.mViewList.RadioCheck = true;
            this.mViewList.Tag = "3";
            this.mViewList.Text = global::LanExchange.Properties.Resources.ViewList;
            this.mViewList.Click += new System.EventHandler(this.mViewLarge_Click);
            // 
            // mViewDetails
            // 
            this.mViewDetails.Checked = true;
            this.mViewDetails.Index = 6;
            this.mViewDetails.RadioCheck = true;
            this.mViewDetails.Tag = "1";
            this.mViewDetails.Text = global::LanExchange.Properties.Resources.ViewDetails;
            this.mViewDetails.Click += new System.EventHandler(this.mViewLarge_Click);
            // 
            // mLanguage
            // 
            this.mLanguage.Index = 2;
            this.mLanguage.Text = global::LanExchange.Properties.Resources.MainForm_Language;
            this.mLanguage.Visible = false;
            // 
            // mHelp
            // 
            this.mHelp.Index = 3;
            this.mHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mHelpKeys,
            this.mHelpSep1,
            this.mHelpWeb,
            this.mHelpLangs,
            this.mHelpBugs,
            this.mHelpSep2,
            this.mHelpAbout});
            this.mHelp.Text = global::LanExchange.Properties.Resources.MenuHelp;
            // 
            // mHelpKeys
            // 
            this.mHelpKeys.Index = 0;
            this.mHelpKeys.Shortcut = System.Windows.Forms.Shortcut.F1;
            this.mHelpKeys.Text = global::LanExchange.Properties.Resources.KeyF1_;
            this.mHelpKeys.Click += new System.EventHandler(this.mHelpKeys_Click);
            // 
            // mHelpSep1
            // 
            this.mHelpSep1.Index = 1;
            this.mHelpSep1.Text = "-";
            // 
            // mHelpWeb
            // 
            this.mHelpWeb.Index = 2;
            this.mHelpWeb.Text = global::LanExchange.Properties.Resources.MenuHomePage;
            this.mHelpWeb.Click += new System.EventHandler(this.mWebPage_Click);
            // 
            // mHelpLangs
            // 
            this.mHelpLangs.Index = 3;
            this.mHelpLangs.Text = global::LanExchange.Properties.Resources.MenuLocalizationPage;
            this.mHelpLangs.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // mHelpBugs
            // 
            this.mHelpBugs.Index = 4;
            this.mHelpBugs.Text = global::LanExchange.Properties.Resources.MenuBugtrackerPage;
            this.mHelpBugs.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // mHelpSep2
            // 
            this.mHelpSep2.Index = 5;
            this.mHelpSep2.Text = "-";
            // 
            // mHelpAbout
            // 
            this.mHelpAbout.Index = 6;
            this.mHelpAbout.Text = global::LanExchange.Properties.Resources.MenuAbout;
            this.mHelpAbout.Click += new System.EventHandler(this.mAbout_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 542);
            this.Controls.Add(this.pInfo);
            this.Controls.Add(this.Status);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Menu = this.MainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyUp);
            this.popTray.ResumeLayout(false);
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip popTray;
        private System.Windows.Forms.ToolStripMenuItem mTrayAbout;
        private System.Windows.Forms.ToolStripMenuItem mTrayExit;
        private System.Windows.Forms.ToolStripMenuItem mTrayOpen;
        private System.Windows.Forms.ToolStripSeparator mTraySep1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.StatusStrip Status;
        public System.Windows.Forms.ToolStripStatusLabel lItemsCount;
        private System.Windows.Forms.ToolStripStatusLabel StatusSep1;
        private System.Windows.Forms.ToolStripStatusLabel lCompName;
        public System.Windows.Forms.ToolTip tipComps;
        internal System.Windows.Forms.ContextMenuStrip popTop;
        private System.Windows.Forms.ToolStripStatusLabel StatusSep2;
        private System.Windows.Forms.ToolStripStatusLabel lUserName;
        internal InfoView pInfo;
        private System.Windows.Forms.MainMenu MainMenu;
        private System.Windows.Forms.MenuItem mHelp;
        private System.Windows.Forms.MenuItem mHelpAbout;
        private System.Windows.Forms.MenuItem mPanel;
        private System.Windows.Forms.MenuItem mPanelReRead;
        private System.Windows.Forms.MenuItem mHelpWeb;
        private System.Windows.Forms.MenuItem mPanelSep3;
        private System.Windows.Forms.MenuItem mPanelExit;
        private System.Windows.Forms.MenuItem mHelpKeys;
        private System.Windows.Forms.MenuItem mLanguage;
        private System.Windows.Forms.MenuItem mHelpLangs;
        private System.Windows.Forms.MenuItem mHelpBugs;
        private System.Windows.Forms.MenuItem mView;
        private System.Windows.Forms.MenuItem mViewLarge;
        private System.Windows.Forms.MenuItem mViewSmall;
        private System.Windows.Forms.MenuItem mViewList;
        private System.Windows.Forms.MenuItem mViewDetails;
        private System.Windows.Forms.MenuItem mPanelSep2;
        private System.Windows.Forms.MenuItem mPanelCloseTab;
        private System.Windows.Forms.MenuItem mPanelCloseOther;
        private System.Windows.Forms.MenuItem mViewInfo;
        private System.Windows.Forms.MenuItem mViewGrid;
        private System.Windows.Forms.MenuItem mViewSep1;
        private MenuItem mHelpSep1;
        private MenuItem mHelpSep2;
    }
}

