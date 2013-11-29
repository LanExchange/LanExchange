﻿using System;
using LanExchange.SDK;

namespace LanExchange.Intf
{
    public class DataReadyArgs : EventArgs
    {
        public DataReadyArgs(PanelItemBase item)
        {
            Item = item;
        }

        public PanelItemBase Item { get; private set; }
    }
}