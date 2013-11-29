﻿using System.ComponentModel;
using System.Resources;
using System.Windows.Forms;
using LanExchange.Intf;

namespace LanExchange.Utils
{
    public static class TranslationUtils
    {
        public static void TranslateControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is ITranslationable)
                    (control as ITranslationable).TranslateUI();
                else
                {
                    TranslateControls(control.Controls);
                }
            }
        }

        public static void TranslateComponents(ResourceManager resources, IContainer container)
        {
            foreach (Component component in container.Components)
            {
                if (component is ITranslationable)
                    (component as ITranslationable).TranslateUI();
                else
                {
                    if (component is ContextMenuStrip)
                        ProcessContextMenuStrip(resources, component as ContextMenuStrip);
                    if (component is MainMenu)
                        ProcessMenuItems(resources, (component as MainMenu).MenuItems);
                    //var name = ReflectionUtils.GetObjectProperty<string>(component, "Name");
                    //if (name != null)
                    //    resources.ApplyResources(component, name);
                }
            }
        }

        [Localizable(false)]
        private static void ProcessMenuItems(ResourceManager resources, Menu.MenuItemCollection menuItems)
        {
            foreach (MenuItem menuItem in menuItems)
                if (menuItem.Text != "-")
                {
                    if (!string.IsNullOrEmpty(menuItem.Name))
                        menuItem.Text = resources.GetString(menuItem.Name + "_Text");
                    if (menuItem.MenuItems.Count > 0)
                        ProcessMenuItems(resources, menuItem.MenuItems);
                }
        }

        private static void ProcessContextMenuStrip(ResourceManager resources, ContextMenuStrip menu)
        {
            foreach(var item in menu.Items)
            {
                var menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                    menuItem.Text = resources.GetString(menuItem.Name + "_Text");
            }
            
        }
    }
}
