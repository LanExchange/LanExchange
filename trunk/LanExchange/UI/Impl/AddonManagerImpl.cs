﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using LanExchange.SDK;
using LanExchange.UI.Utils;

namespace LanExchange.UI.Impl
{
    [Localizable(false)]
    public class AddonManagerImpl : IAddonManager
    {
        private readonly IDictionary<string, AddonProgram> m_Programs;
        private readonly IDictionary<string, AddOnItemTypeRef> m_PanelItems;

        private bool m_Loaded;

        public AddonManagerImpl()
        {
            m_Programs = new Dictionary<string, AddonProgram>();
            m_PanelItems = new Dictionary<string, AddOnItemTypeRef>();
        }

        public IDictionary<string, AddonProgram> Programs 
        { 
            get
            {
                LoadAddons();
                return m_Programs;
            }
        }
        
        public IDictionary<string, AddOnItemTypeRef> PanelItems
        {
            get
            {
                LoadAddons();
                return m_PanelItems;
            }
        }

        public void LoadAddons()
        {
            if (m_Loaded) return;
            foreach (var fileName in App.FolderManager.GetAddonsFiles())
                try
                {
                    LoadAddon(fileName);
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message);
                }
            // register programs images
            foreach (var pair in m_Programs)
                if (pair.Value.ProgramImage != null)
                {
                    var imageName = string.Format(CultureInfo.InvariantCulture, PanelImageNames.ADDON_FMT, pair.Key);
                    App.Images.RegisterImage(imageName, pair.Value.ProgramImage, pair.Value.ProgramImage);
                }
            m_Loaded = true;
        }

        private void LoadAddon(string fileName)
        {
            // load addon from xml
            var addon = (AddOn)SerializeUtils.DeserializeObjectFromXmlFile(fileName, typeof(AddOn));
            // process programs
            foreach (var item in addon.Programs)
                if (!m_Programs.ContainsKey(item.Id))
                {
                    item.PrepareFileNameAndIcon();
                    m_Programs.Add(item.Id, item);
                }
            // process protocols
            foreach (var item in addon.PanelItemTypes)
                foreach(var menuItem in item.ContextMenuStrip)
                    if (ProtocolHelper.IsProtocol(menuItem.ProgramRef.Id))
                    {
                        var itemProgram = AddonProgram.CreateFromProtocol(menuItem.ProgramRef.Id);
                        if (itemProgram != null)
                            m_Programs.Add(itemProgram.Id, itemProgram);
                    }
            // process menu items
            foreach (var item in addon.PanelItemTypes)
            {
                AddOnItemTypeRef found;
                if (m_PanelItems.ContainsKey(item.Id))
                {
                    found = m_PanelItems[item.Id];
                    m_PanelItems.Remove(item.Id);
                }
                else
                    found = new AddOnItemTypeRef();
                if (item.CountVisible == 0) continue;

                // add separator to split item groups
                if (found.ContextMenuStrip.Count > 0)
                    found.ContextMenuStrip.Add(new AddonMenuItem());
                foreach (var menuItem in item.ContextMenuStrip)
                    if (menuItem.Visible)
                    {
                        if (menuItem.IsSeparator)
                            found.ContextMenuStrip.Add(menuItem);
                        else
                        {
                            if (m_Programs.ContainsKey(menuItem.ProgramRef.Id))
                                menuItem.ProgramValue = m_Programs[menuItem.ProgramRef.Id];
                            if (!found.ContextMenuStrip.Contains(menuItem) && 
                                menuItem.ProgramRef != null && 
                                menuItem.ProgramValue != null)
                                found.ContextMenuStrip.Add(menuItem);
                        }
                    }
                m_PanelItems.Add(item.Id, found);
            }
        }

        private void InternalBuildMenu(ToolStripItemCollection items, string id)
        {
            items.Clear();
            ToolStripMenuItem defaultItem = null;
            foreach (var item in PanelItems[id].ContextMenuStrip)
            {
                if (item.IsSeparator)
                    items.Add(new ToolStripSeparator());
                else
                {
                    var menuItem = new ToolStripMenuItem();
                    menuItem.Tag = item;
                    menuItem.Text = App.TR.Translate(item.Text);
                    menuItem.ShortcutKeyDisplayString = item.ShortcutKeys;
                    menuItem.Click += MenuItemOnClick;
                    if (item.ProgramValue != null)
                    {
                        if (!item.ProgramValue.Exists)
                            menuItem.Enabled = false;
                        menuItem.Image = item.ProgramValue.ProgramImage;
                    }
                    else
                        menuItem.Enabled = false;
                    // lookup last default item
                    if (item.Default)
                        defaultItem = menuItem;
                    items.Add(menuItem);
                }
            }
            if (defaultItem != null)
                defaultItem.Font = new Font(defaultItem.Font, FontStyle.Bold);
        }

        public bool BuildMenuForPanelItemType(object popTop, string id)
        {
            if (!PanelItems.ContainsKey(id))
                return false;
            if (popTop is ContextMenuStrip)
            {
                var popTop1 = popTop as ContextMenuStrip;
                if (popTop1.Tag == null || !popTop1.Tag.Equals(id))
                {
                    InternalBuildMenu(popTop1.Items, id);
                    popTop1.Tag = id;
                }
                return popTop1.Items.Count > 0;
            }

            if (popTop is ToolStripMenuItem)
            {
                var popTop2 = popTop as ToolStripMenuItem;
                if (popTop2.Tag == null || !popTop2.Tag.Equals(id))
                {
                    InternalBuildMenu(popTop2.DropDownItems, id);
                    popTop2.Tag = id;
                }
                return popTop2.DropDownItems.Count > 0;
            }
            return false;
        }

        private void InternalRunCmdLine(IPanelView pv, PanelItemBase panelItem, AddonMenuItem item)
        {
            var programFileName = item.ProgramValue.ExpandedFileName;
            var programArgs = AddonProgram.ExpandCmdLine(item.ProgramArgs);
            programArgs = MacroHelper.ExpandPublicProperties(programArgs, panelItem);
            try
            {
                if (ProtocolHelper.IsProtocol(item.ProgramRef.Id))
                    Process.Start(item.ProgramRef.Id + programArgs);
                else
                    Process.Start(programFileName, programArgs);
            }
            catch(Exception ex)
            {
                pv.ShowRunCmdError(string.Format(CultureInfo.InvariantCulture, "{0} {1}\n{2}", programFileName, programArgs, ex.Message));
            }
        }

        /// <summary>
        /// Executes external program associated with menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void MenuItemOnClick(object sender, EventArgs eventArgs)
        {
            var menuItem = (sender as ToolStripMenuItem);
            if (menuItem == null) return;
            var item = (AddonMenuItem) menuItem.Tag;
            if (item == null || item.ProgramValue == null || !item.ProgramValue.Exists) return;
            var pv = App.MainPages.View.ActivePanelView;
            if (pv == null) return;
            var panelItem = pv.Presenter.GetFocusedPanelItem(false, true);
            if (panelItem == null) return;
            InternalRunCmdLine(pv, panelItem, item);
        }

        public void ProcessKeyDown(object args)
        {
            var pv = App.MainPages.View.ActivePanelView;
            var e = args as KeyEventArgs;
            if (pv == null || e == null) return;
            var panelItem = pv.Presenter.GetFocusedPanelItem(false, true);
            if (panelItem == null) return;
            var typeId = panelItem.GetType().Name;
            if (!PanelItems.ContainsKey(typeId))
                return;
            var item = PanelItems[typeId];
            var shortcut = KeyboardUtils.KeyEventToString(e);
            foreach (var menuItem in item.ContextMenuStrip)
                if (menuItem.ShortcutPresent && menuItem.ShortcutKeys.Equals(shortcut))
                {
                    InternalRunCmdLine(pv, panelItem, menuItem);
                    e.Handled = true;
                    break;
                }
        }

        /// <summary>
        /// Run addon command marked with Default flag for current panelItem.
        /// </summary>
        public void RunDefaultCmdLine()
        {
            var pv = App.MainPages.View.ActivePanelView;
            if (pv == null) return;
            var panelItem = pv.Presenter.GetFocusedPanelItem(false, true);
            if (panelItem == null) return;
            var typeId = panelItem.GetType().Name;
            if (!PanelItems.ContainsKey(typeId))
                return;
            var item = PanelItems[typeId];
            AddonMenuItem defaultItem = null;
            foreach (var menuItem in item.ContextMenuStrip)
                if (menuItem.Default)
                    defaultItem = menuItem;
            if (defaultItem != null)
                InternalRunCmdLine(pv, panelItem, defaultItem);
        }
    }
}