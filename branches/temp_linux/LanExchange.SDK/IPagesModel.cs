﻿using System;

namespace LanExchange.SDK
{
    public interface IPagesModel : IModel, IDisposable
    {
        event EventHandler<PanelModelEventArgs> AfterAppendTab;
        event EventHandler<PanelIndexEventArgs> AfterRemove;
        event EventHandler<PanelIndexEventArgs> AfterRename;
        event EventHandler<PanelIndexEventArgs> IndexChanged;

        bool AddTab(IPanelModel model);

        bool TabNameExists(string tabName);

        void DelTab(int index);

        int Count { get; }
        int SelectedIndex { get; set; }
        IPanelModel GetItem(int index);
        void RenameTab(int index, string newTabName);
        string GetTabName(int index);

        void SaveSettings();
        void LoadSettings(out IPagesModel model);

        void SetLoadedModel(IPagesModel model);
    }
}
