﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using LanExchange.Application.Interfaces;
using LanExchange.Plugin.Network.NetApi;

namespace LanExchange.Plugin.Network
{
    public sealed class ComputerFiller : IPanelFiller
    {
        public bool IsParentAccepted(PanelItemBase parent)
        {
            // computers can be only into domains
            return parent is DomainPanelItem;
        }

        public void SyncFill(PanelItemBase parent, ICollection<PanelItemBase> result)
        {
        }

        public void AsyncFill(PanelItemBase parent, ICollection<PanelItemBase> result)
        {
            Contract.Requires<ArgumentNullException>(parent != null);

            // get server list via OS api
            foreach (var item in NetApiHelper.NetServerEnum(parent.Name, SV_101_TYPES.SV_TYPE_ALL))
            {
                var si = ServerInfo.FromNetApi32(item);
                si.ResetUtcUpdated();
                result.Add(new ComputerPanelItem(parent, si));
            }
        }
    }
}