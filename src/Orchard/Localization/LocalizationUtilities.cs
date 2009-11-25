﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Autofac;
using Orchard.Mvc;

namespace Orchard.Localization {
    public class LocalizationUtilities {
        public static Localizer Resolve(ControllerContext controllerContext, string scope) {
            var context = OrchardControllerFactory.GetRequestContainer(controllerContext.RouteData);
            return context == null ? NullLocalizer.Instance : Resolve(context, scope);
        }

        public static Localizer Resolve(IContext context, string scope) {
            var text = context.Resolve<IText>(new NamedParameter("scope", scope));
            return text.Get;
        }
    }
}