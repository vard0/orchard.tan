﻿using System.Collections.Generic;
using System.Web;

namespace Orchard.Extensions {
    public interface IExtensionManager {
        IEnumerable<ExtensionDescriptor> AvailableExtensions();
        IEnumerable<ExtensionEntry> ActiveExtensions();
        void InstallExtension(string extensionType, HttpPostedFileBase extensionBundle);
        void UninstallExtension(string extensionType, string extensionName);
    }
}