﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.Mvc;
using System.Configuration;
using System.Web.Configuration;
using CampusWebStore.Models;
using CampusWebStore.Business.Services;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;

namespace CampusWebStore.Controllers
{
    public class WebController : Controller
    {
        #region Properties

        public BaseMainConfigAdmin objBaseMainConfigAdmin = new BaseMainConfigAdmin();


        [Dependency]
        public ICatalogsServices CatalogsServices { get; set; }

        public static string StoreNumber = System.Configuration.ConfigurationManager.AppSettings["STOREID"] as string;

        public static string UseCache = System.Configuration.ConfigurationManager.AppSettings["USECACHE"];

        public static string UvUsername = System.Configuration.ConfigurationManager.AppSettings["UV_USERNAME"] as string;

        public static string UvPassword = System.Configuration.ConfigurationManager.AppSettings["UV_PASSWORD"] as string;

        public static string DbType = System.Configuration.ConfigurationManager.AppSettings["DBTYPE"] as string;

        public static string UvAddress = System.Configuration.ConfigurationManager.AppSettings["DB_ADDRESS"] as string;

        public static string UvAccount = System.Configuration.ConfigurationManager.AppSettings["DB_ACCOUNT"] as string;

        public static string CacheTime = System.Configuration.ConfigurationManager.AppSettings["CACHETIME"] as string;

        public static string Strd3PortNumber = System.Configuration.ConfigurationManager.AppSettings["D3PORT"] as string;

        public static string UseEncryption = System.Configuration.ConfigurationManager.AppSettings["ENCRYPTALL"] as string;

        public static int D3PortNumber = Convert.ToInt32(Strd3PortNumber);

        //public static BuyBack _a;

        //public static AdminConfig _b;

        //public static Configuration c = WebConfigurationManager.OpenWebConfiguration("~");

        //public static BuyBack IWEBBuyBackConfig
        //{
        //    get { return _a; }
        //    set { _a = value; }
        //}
        //public static AdminConfig IWEBConfigAdmin
        //{
        //    get { return _b; }
        //    set { _b = value; }
        //}
        //public static void SaveValues()
        //{
        //    c.Save(ConfigurationSaveMode.Full);
        //}

        //  public static string DblcacheTime =(System.Configuration.ConfigurationManager.AppSettings["USECACHE"]);

        #endregion

        /// <summary>
        /// Get the total items and total price of item added to the cart
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewCart()
        {

            var lstCartItems = new List<ShoppingCartModel>();
            var total = 0.0;
            try
            {

                //Check for the null
                if (Session["CARTITEMS"] != null)
                {
                    lstCartItems = (List<ShoppingCartModel>)Session["CARTITEMS"];

                }

                total += lstCartItems.Sum(lst => lst.Qty * lst.ActualPrice);

                return Json(new { total = Math.Round(total, 2), items = lstCartItems.Count });
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }


        #region Proctected Methods
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);

                //Get the list of the catalog on the action executing and assing the list of catalogs to the viewdata
                if (Session["Session"] == null)
                {
                    Session["Session"] = CatalogsServices.GetTopCatalogs(StoreNumber,
                                                                         UvUsername, UvPassword,
                                                                         DbType, UvAddress, UvAccount, CacheTime,
                                                                         CacheTime,
                                                                         Strd3PortNumber, UseEncryption, Strd3PortNumber);
                }

                ViewData["CatalogList"] = Session["Session"];

                //Put the cart items to the viewdata;
                var lstCartItems = new List<ShoppingCartModel>();

                //Check for the null
                if (Session["CARTITEMS"] != null)
                {
                    lstCartItems = (List<ShoppingCartModel>)Session["CARTITEMS"];

                }
                ViewData["CarTItems"] = lstCartItems;



            }
            catch
            {
                //throw new FileNotFoundException();
            }

            // Get Theme for use in templates
            ViewBag.objBaseMainConfigAdmin = objBaseMainConfigAdmin;
            PagesSection pages = ConfigurationManager.GetSection("system.web/pages") as PagesSection;
            string theme = pages.Theme ?? "Default";
            ViewBag.Theme = theme;

            if (ViewBag.Theme != "Default")
            {
                ViewBag.JsPath = "/App_Themes/" + theme + "/js/";
                ViewBag.CssPath = "/App_Themes/" + theme + "/css/";
                ViewBag.ImgPath = "/App_Themes/" + theme + "/img/";
            }
            else
            {
                ViewBag.JsPath = "/js/";
                ViewBag.CssPath = "/Content/Styles/";
                ViewBag.ImgPath = "/Content/images/";
            }
        }
        #endregion

        #region "public Methods"
        public bool HasAdminPrivilege()
        {
            var acctype = Session["ACCTYPE"];
            if (Session["USERINFO"] == null || string.IsNullOrEmpty(acctype.ToString()) || !acctype.Equals("A"))
            {
                return false;
            }

            return true;
        }
        public bool IsAuthenticated()
        {

            if (Session["USERINFO"] == null)
            {
                return false;
            }

            return true;
        }
        #endregion
    }

}
