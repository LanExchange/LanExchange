﻿using LanExchange.Plugin.Users.Properties;
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
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.UserName, 180));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Title));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.WorkPhone));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Office));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Department));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Email, 180));
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Company)  { Visible = false });
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Nickname) { Visible = false });
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Pre2000Logon) { Visible = false });
            columnManager.RegisterColumn<UserPanelItem>(new PanelColumnHeader(Resources.Description, 200) { Visible = false });
        }
    }
}