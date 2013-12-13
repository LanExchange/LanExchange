﻿using System.Windows.Forms;
using LanExchange.Plugin.Users.Properties;
using LanExchange.SDK;

namespace LanExchange.Plugin.Users
{
    internal sealed class UserFactory : PanelItemFactoryBase
    {
        public override PanelItemBase CreatePanelItem(PanelItemBase parent, string name)
        {
            return new UserPanelItem(parent, name);
        }

        public override PanelItemBase CreateDefaultRoot()
        {
            return new UserRoot();
        }

        public override void RegisterColumns(IPanelColumnManager columnManager)
        {
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.UserName));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Title));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.WorkPhone, 80));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Office));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Department));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Email));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Company));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Nickname));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Pre2000Logon) { Visible = false });
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Description, 200) { Visible = false });
        }
    }
}
