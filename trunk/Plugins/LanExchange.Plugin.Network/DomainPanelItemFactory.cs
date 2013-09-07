﻿using System.Security.Permissions;
using LanExchange.SDK;

namespace LanExchange.Plugin.Network
{
    public sealed class DomainPanelItemFactory : PanelItemFactoryBase
    {
        public override PanelItemBase CreatePanelItem(PanelItemBase parent, string name)
        {
            return new DomainPanelItem(parent, name);
        }

        /// <summary>
        /// Starts with curent users's workgroup/domain as root.
        /// </summary>
        /// <returns></returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand, Unrestricted = true)]
        public override PanelItemBase CreateDefaultRoot()
        {
            var domain = NetApi32Utils.GetMachineNetBiosDomain(null);
            return new DomainPanelItem(PluginNetwork.ROOT_OF_DOMAINS, domain);
        }
    }
}
