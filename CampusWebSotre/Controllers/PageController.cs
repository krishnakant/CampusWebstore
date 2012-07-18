using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml;
using CampusWebStore.Models;
using CampusWebStore.Business.Services;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;
using System.Xml.Linq;
using System.IO;

namespace CampusWebStore.Controllers
{
    public class PageController : WebController
    {  
        public ActionResult GetPage(string name)
        {
            var pagename1 = name ?? "";
            string path = Server.MapPath("~/CustomPages/" + name);
            string contentPath = path + ".cws";
            string headerPath = path + ".desc";
            bool exists = System.IO.File.Exists(headerPath);

            if (!exists)
            {
                throw new FileNotFoundException("The page you requested was not found.");
            }

            // Page Title
            var headerStream = new StreamReader(headerPath);
            ViewBag.header = headerStream.ReadToEnd();
            headerStream.Close();

            // Page Content
            var contentStream = new StreamReader(contentPath);
            ViewBag.content = contentStream.ReadToEnd();
            contentStream.Close();

            return View();
        }
    }
}