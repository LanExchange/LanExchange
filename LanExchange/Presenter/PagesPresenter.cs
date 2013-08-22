﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using LanExchange.Model;
using LanExchange.Properties;
using LanExchange.SDK;
using LanExchange.UI;

namespace LanExchange.Presenter
{
    public class PagesPresenter
    {
        private readonly IPagesView m_View;
        private readonly PagesModel m_Model;

        public event EventHandler PanelViewFocusedItemChanged;
        public event EventHandler PanelViewFilterTextChanged;

        public PagesPresenter(IPagesView pages)
        {
            m_View = pages;
            m_Model = new PagesModel();
            m_Model.AfterAppendTab += Model_AfterAppendTab;
            m_Model.AfterRemove += Model_AfterRemove;
            m_Model.AfterRename += Model_AfterRename;
            m_Model.IndexChanged += Model_IndexChanged;
        }

        public PagesModel GetModel()
        {
            return m_Model;
        }

        private void CheckDuplicateOnNew(object sender, CancelEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var form = control.Parent as InputBoxForm;
            if (form == null) return;
            if (m_Model.TabNameExists(form.Value))
            {
                form.SetError(Resources.InputBoxPresenter_TabNameAlreadyExists);
                e.Cancel = true;
            }
        }

        private void CheckDuplicateOnRename(object sender, CancelEventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;
            var form = control.Parent as InputBoxForm;
            if (form == null) return;
            var index = m_View.PopupSelectedIndex;
            var itemList = GetModel().GetItem(index);
            if (itemList == null) return;
            if ((string.Compare(form.Value, itemList.TabName, StringComparison.CurrentCultureIgnoreCase) != 0) && 
                m_Model.TabNameExists(form.Value))
            {
                form.SetError(Resources.InputBoxPresenter_TabNameAlreadyExists);
                e.Cancel = true;
            }
        }

        public void CommandNewTab()
        {
            using (
                var form = InputBoxForm.CreateAskForm(Resources.PagesPresenter_NewTab,
                                                      Resources.PagesPresenter_EnterTabName, ""))
            {
                form.InputValidating += InputBoxForm.ValidatingEmpty;
                form.InputValidating += CheckDuplicateOnNew;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    m_Model.AddTab(new PanelItemList(form.Value));
                    m_Model.SaveSettings();
                }
            }
        }

        public void CommandProperties()
        {
            var index = m_View.PopupSelectedIndex;
            var itemList = GetModel().GetItem(index);
            if (itemList == null) return;
            using (var form = InputBoxForm.CreateAskForm(Resources.PagesPresenter_TabProperties,
                                                         Resources.PagesPresenter_TabName, itemList.TabName))
            {
                form.InputValidating += InputBoxForm.ValidatingEmpty;
                form.InputValidating += CheckDuplicateOnRename;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    m_Model.RenameTab(index, form.Value);
                    m_Model.SaveSettings();
                }
            }
        }

        public void CommandSendToNewTab()
        {
            var newTabName = m_Model.GenerateTabName();
            var sourcePV = m_View.ActivePanelView;
            if (sourcePV == null) return;
            var indexes = sourcePV.SelectedIndexes.GetEnumerator();
            if (!indexes.MoveNext()) return;
            var sourceObjects = sourcePV.Presenter.Objects;
            var destObjects = new PanelItemList(newTabName);
            destObjects.DataType = sourceObjects.DataType;
            //foreach (int index in sourcePV.SelectedIndexes)
            //{
            //    var PItem = sourceObjects.GetItemAt(index);
            //    if (PItem == null || PItem.Name == PanelItemBase.s_DoubleDot)
            //        continue;
            //    if (!(PItem is ComputerPanelItem))
            //        continue;
            //    // copy computer and set partent to null
            //    var newItem = new ComputerPanelItem(null, (PItem as ComputerPanelItem).SI);
            //    newItem.ParentSubject = ConcreteSubject.s_UserItems;
            //    // add item to new panel
            //    destObjects.Items.Add(newItem);
            //}
            // add tab
            m_Model.AddTab(destObjects);
            m_View.SelectedIndex = m_Model.Count - 1;
            m_Model.SaveSettings();
        }

        public void CommandCloseTab()
        {
            var index = m_View.PopupSelectedIndex;
            if (CanCloseTab())
            {
                m_Model.DelTab(index);
                m_Model.SaveSettings();
            }
        }

        public bool CanCloseTab()
        {
            return true;
        }

        public void Model_AfterAppendTab(object sender, PanelItemListEventArgs e)
        {
            // create panel
            var panelView = m_View.CreatePanelView(e.Info);
            // set update event
            IPanelPresenter presenter = panelView.Presenter;
            presenter.Objects = e.Info;
            e.Info.Changed += (o, args) => presenter.UpdateItemsAndStatus();
            //e.Info.SubscriptionChanged += Item_SubscriptionChanged;
            // update items
            //e.Info.DataChanged(null, ConcreteSubject.s_UserItems);
            presenter.ResetSortOrder();
            e.Info.SyncRetrieveData();
        }


        public void Model_AfterRemove(object sender, PanelIndexEventArgs e)
        {
            m_View.RemoveTabAt(e.Index);
        }

        public void Model_AfterRename(object sender, PanelItemListEventArgs e)
        {
            m_View.SelectedTabText = e.Info.TabName;
        }

        public void Model_IndexChanged(object sender, PanelIndexEventArgs e)
        {
            m_View.SelectedIndex = e.Index;
            m_View.FocusPanelView();
        }

        public void PV_FocusedItemChanged(object sender, EventArgs e)
        {
            if (PanelViewFocusedItemChanged != null)
                PanelViewFocusedItemChanged(sender, e);
        }

        public void PV_FilterTextChanged(object sender, EventArgs e)
        {
            if (PanelViewFilterTextChanged != null)
                PanelViewFilterTextChanged(sender, e);
        }

        public void Item_SubscriptionChanged(object sender, EventArgs e)
        {
            PanelItemList Item = sender as PanelItemList;
            if (Item == null) return;
            int Index = m_Model.GetItemIndex(Item);
            if (Index != -1)
                m_View.SetTabToolTip(Index, Item.ToolTipText);
        }
    }

}
