﻿using System;
using System.Collections.Generic;
using LanExchange.Misc;
using LanExchange.SDK;

namespace LanExchange.Model
{
    /// <summary>
    /// LanExchange panel model.
    /// </summary>
    public interface IPanelModel : IFilterModel
    {
        event EventHandler Changed;
        /// <summary>
        /// Gets or sets the name of the tab.
        /// </summary>
        /// <value>
        /// The name of the tab.
        /// </value>
        string TabName { get; set; }
        /// <summary>
        /// Gets or sets the current view.
        /// </summary>
        /// <value>
        /// The current view.
        /// </value>
        PanelViewMode CurrentView { get; set; }
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int Count { get; }
        /// <summary>
        /// Gets a value indicating whether this instance has back item.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has back item; otherwise, <c>false</c>.
        /// </value>
        bool HasBackItem { get; }
        /// <summary>
        /// Gets the current path.
        /// </summary>
        /// <value>
        /// The current path.
        /// </value>
        ObjectPath<PanelItemBase> CurrentPath { get; }
        /// <summary>
        /// Gets or sets the focused item text.
        /// </summary>
        /// <value>
        /// The focused item text.
        /// </value>
        PanelItemBase FocusedItem { get; set; }
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        IList<PanelItemBase> Items { get; }
        /// <summary>
        /// Gets the tool tip text.
        /// </summary>
        /// <value>
        /// The tool tip text.
        /// </value>
        string ToolTipText { get; }
        /// <summary>
        /// Gets at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        PanelItemBase GetItemAt(int index);
        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        int IndexOf(PanelItemBase key);

        PanelItemBaseFactory ItemFactory { get; set; }
        void SyncRetrieveData(bool clearFilter = false);
        Type DataType { get; set; }
        ColumnComparer Comparer { get; }
        void Sort(IComparer<PanelItemBase> sorter);

        bool Contains(PanelItemBase panelItem);
    }

    public enum PanelSortOrder
    {
        None,
        Ascending,
        Descending,
    }

    /// <summary>
    /// ViewMode of LanExchange panel.
    /// </summary>
    public enum PanelViewMode
    {
        /// <summary>
        /// The large icons view
        /// </summary>
        LargeIcon,
        /// <summary>
        /// The details table view
        /// </summary>
        Details,
        /// <summary>
        /// The small icons view
        /// </summary>
        SmallIcon,
        /// <summary>
        /// The list view
        /// </summary>
        List
    }
}