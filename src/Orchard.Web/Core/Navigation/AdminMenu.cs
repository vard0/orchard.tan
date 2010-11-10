﻿using Orchard.Localization;
using Orchard.UI.Navigation;

namespace Orchard.Core.Navigation {
    public class AdminMenu : INavigationProvider {
        public Localizer T { get; set; }
        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder) {
            //todo: - add new menu? and list menus? ...and remove hard-coded menu name here
            builder.Add(T("Navigation"), "8",
                        menu => menu
                                    .Add(T("Main Menu"), "6.0", item => item.Action("Index", "Admin", new { area = "Navigation" }).Permission(Permissions.ManageMainMenu)));
        }
    }
}