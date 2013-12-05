﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LanExchange.SDK
{
    public sealed class PanelFillerResult
    {
        public PanelFillerResult()
        {
            Items = new Collection<PanelItemBase>();
        }

        public ICollection<PanelItemBase> Items { get; private set; }
        public Type ItemsType { get; set; }
    }
}