using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CampusWebStore.Utils
{
    public class ThemedRazorViewEngine : RazorViewEngine
    {
        public ThemedRazorViewEngine(string themeName)
        {
            if (themeName == null || themeName == "") {
                themeName = "Default";
            }

            FileExtensions = new string[] { "cshtml" };
            MasterLocationFormats = new string[] {
                "~/Views/{0}.cshtml",
                "~/Views/" + themeName + "/{1}/{0}.cshtml",
                "~/Views/" + themeName + "/Shared/{1}/{0}.cshtml",
                "~/Views/All/{1}/{0}.cshtml",
                "~/Views/All/Shared/{1}/{0}.cshtml"
            };
            PartialViewLocationFormats = new string[] {
                "~/Views/" + themeName + "/{0}.cshtml", 
                "~/Views/" + themeName + "/Shared/{0}.cshtml",
                "~/Views/" + themeName + "/{1}/{0}.cshtml",
                "~/Views/All/{1}/{0}.cshtml",
                "~/Views/All/Shared/{0}.cshtml"
            };
            ViewLocationFormats = new string[] {
                "~/Views/" + themeName + "/{1}/{0}.cshtml", 
                "~/Views/" + themeName + "/Shared/{1}/{0}.cshtml",
                "~/Views/All/{1}/{0}.cshtml",
                "~/Views/All/Shared/{1}/{0}.cshtml"
            };
        }
    }
}