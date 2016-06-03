﻿using LanExchange.Application.Interfaces;
using LanExchange.Plugin.Network.Properties;

namespace LanExchange.Plugin.Network
{
    public sealed class DomainRoot : PanelItemRootBase
    {
        protected override string GetName()
        {
            return Resources.Network;
        }

        public override string ImageName
        {
            get { return PanelImageNames.DOMAIN; }
        }

        public override object Clone()
        {
            return new DomainRoot();
        }
    }
}
