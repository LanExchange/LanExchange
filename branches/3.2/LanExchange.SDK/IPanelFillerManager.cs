﻿using System;

namespace LanExchange.SDK
{
    public interface IPanelFillerManager
    {
        void RegisterFiller<TPanelItem>(IPanelFiller filler) where TPanelItem : PanelItemBase;
        Type GetFillType(PanelItemBase parent);
        PanelFillerResult RetrievePanelItems(PanelItemBase parent, RetrieveMode mode);
        bool FillerExists(PanelItemBase panelItem);
    }
}
