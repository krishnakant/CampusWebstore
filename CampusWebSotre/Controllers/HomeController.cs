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
    public class HomeController : WebController
    {

        #region "Properties"

        [Dependency]
        public IStoreServices StoreService { get; set; }

        [Dependency]
        public ITermServices TermServices { get; set; }

        [Dependency]
        public IDepartmentServices DepartmentServices { get; set; }

        [Dependency]
        public ICourseServices CourseServices { get; set; }

        [Dependency]
        public ISectionServices SectionServices { get; set; }

        [Dependency]
        public ISellBackSerivces SellBackSerivces { get; set; }

        #endregion

        public ActionResult Index()
        {
            string homePage = objBaseMainConfigAdmin.IwebConfigAdmin.CurrentHomePage ?? null;

            if (homePage != null)
            {
                ViewBag.CustomFrontPage = true;
                string path = Server.MapPath("~/CustomPages/" + homePage + ".cws");
                bool exists = System.IO.File.Exists(path);

                if (exists)
                {
                    ViewBag.CustomFrontPage = true;
                    var contentStream = new StreamReader(path);
                    ViewBag.content = contentStream.ReadToEnd();
                    contentStream.Close();
                }
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult BookSearch()
        {
            {
                var myObject = new { LOGIN = UvUsername };

                var lstStores = StoreService.GetStores(StoreNumber, Constants.GetStores, myObject,
                                                          UvUsername, UvPassword,
                                                          DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                          Strd3PortNumber, UseEncryption, Strd3PortNumber);
                var lstStoreList = new List<SelectListItem>();
                foreach (var s in lstStores)
                {
                    var item = new SelectListItem();
                    item.Text = s.Name;
                    item.Value = s.Id;
                    lstStoreList.Add(item);
                }

                ViewData["lstStoreList"] = lstStoreList;

                return View();
            }
        }

        public ActionResult CourseSearch()
        {
            {
                var myObject = new { LOGIN = UvUsername };

                var lstStores = StoreService.GetStores(StoreNumber, Constants.GetStores, myObject,
                                                          UvUsername, UvPassword,
                                                          DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                          Strd3PortNumber, UseEncryption, Strd3PortNumber);
                var lstStoreList = new List<SelectListItem>();
                foreach (var s in lstStores)
                {
                    var item = new SelectListItem();
                    item.Text = s.Name;
                    item.Value = s.Id;
                    lstStoreList.Add(item);
                }

                ViewData["lstStoreList"] = lstStoreList;

                return View();
            }
        }

        /// <summary>
        /// Get Stores
        /// </summary>
        /// <returns></returns>
        public ActionResult GetStores()
        {
            var myObject = new { LOGIN = UvUsername };

            var key = StoreNumber + ":" + Constants.GetStores + ":" + myObject;

            var useCache = System.Configuration.ConfigurationManager.AppSettings["USECACHE"];

            var strCachedFile = "";

            var cachedFile = new object();
            try
            {
                if (useCache == "TRUE")
                {
                    cachedFile = System.Web.HttpContext.Current.Cache.Get(key);

                }

                if (!string.IsNullOrEmpty(strCachedFile))
                {
                    strCachedFile = cachedFile.ToString();
                }
                else
                {

                    var storeModels = StoreService.GetStores(StoreNumber, Constants.GetStores, myObject,
                                                            UvUsername, UvPassword,
                                                            DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                            Strd3PortNumber, UseEncryption, Strd3PortNumber);

                    return Json(new { storeModels });
                }
                return Json(new { strCachedFile });
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }
        /// <summary>
        /// Get the terms for the stores
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTerms()
        {
            var storeId = Request["StoreId"] ?? "";


            var myObject = new { STOREID = storeId };

            var key = StoreNumber + ":" + Constants.GetTerms + ":" + myObject;

            var useCache = System.Configuration.ConfigurationManager.AppSettings["USECACHE"];

            var strCachedFile = "";

            var cachedFile = new object();
            try
            {
                if (useCache == "TRUE")
                {
                    cachedFile = System.Web.HttpContext.Current.Cache.Get(key);

                }

                if (!string.IsNullOrEmpty(strCachedFile))
                {
                    strCachedFile = cachedFile.ToString();
                }
                else
                {

                    var termModels = TermServices.GetTerms(StoreNumber, Constants.GetTerms, myObject,
                                                            UvUsername, UvPassword,
                                                            DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                            Strd3PortNumber, UseEncryption, Strd3PortNumber);

                    return Json(new { termModels }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { strCachedFile });
            }
            catch (Exception x)
            {
                return Json("Try Hard");
            }
        }

        public ActionResult GetDepartments()
        {
            var storeId = Request["StoreId"] ?? "";

            var termId = Request["TermId"] ?? "";

            var myObject = new { STOREID = storeId, TERMID = termId };

            var key = StoreNumber + ":" + Constants.GetDepartments + ":" + myObject;

            var useCache = System.Configuration.ConfigurationManager.AppSettings["USECACHE"];

            var strCachedFile = "";

            var cachedFile = new object();
            try
            {
                if (useCache == "TRUE")
                {
                    cachedFile = System.Web.HttpContext.Current.Cache.Get(key);

                }

                if (!string.IsNullOrEmpty(strCachedFile))
                {
                    strCachedFile = cachedFile.ToString();
                }
                else
                {

                    var departmentModels = DepartmentServices.GetDepartments(StoreNumber, Constants.GetDepartments, myObject,
                                                            UvUsername, UvPassword,
                                                            DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                            Strd3PortNumber, UseEncryption, Strd3PortNumber);

                    return Json(new { departmentModels }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { strCachedFile });
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }
        public ActionResult GetCourses()
        {
            var storeId = Request["StoreId"] ?? "";

            var termId = Request["TermId"] ?? "";

            var deptId = Request["DeptId"] ?? "";

            var myObject = new { STOREID = storeId, TERMID = termId, DEPTID = deptId };


            var key = StoreNumber + ":" + Constants.GetDepartments + ":" + myObject;

            var useCache = System.Configuration.ConfigurationManager.AppSettings["USECACHE"];

            var strCachedFile = "";

            var cachedFile = new object();
            try
            {
                if (useCache == "TRUE")
                {
                    cachedFile = System.Web.HttpContext.Current.Cache.Get(key);

                }

                if (!string.IsNullOrEmpty(strCachedFile))
                {
                    strCachedFile = cachedFile.ToString();
                }
                else
                {

                    var courseModels = CourseServices.GetCourses(StoreNumber, Constants.GetCourse, myObject,
                                                            UvUsername, UvPassword,
                                                            DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                            Strd3PortNumber, UseEncryption, Strd3PortNumber);

                    return Json(new { courseModels }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { strCachedFile });
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }
        public ActionResult GetSections()
        {
            var storeId = Request["StoreId"] ?? "";

            var termId = Request["TermId"] ?? "";

            var deptId = Request["DeptId"] ?? "";

            var courseId = Request["CourseId"] ?? "";

            var myObject = new { STOREID = storeId, TERMID = termId, DEPTID = deptId, COURSEID = courseId };

            var key = StoreNumber + ":" + Constants.GetDepartments + ":" + myObject;

            var useCache = System.Configuration.ConfigurationManager.AppSettings["USECACHE"];

            var strCachedFile = "";

            var cachedFile = new object();
            try
            {
                if (useCache == "TRUE")
                {
                    cachedFile = System.Web.HttpContext.Current.Cache.Get(key);

                }

                if (!string.IsNullOrEmpty(strCachedFile))
                {
                    strCachedFile = cachedFile.ToString();
                }
                else
                {

                    var sectionModels = SectionServices.GetSections(StoreNumber, Constants.GetSections, myObject,
                                                            UvUsername, UvPassword,
                                                            DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                            Strd3PortNumber, UseEncryption, Strd3PortNumber);

                    return Json(new { sectionModels }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { strCachedFile });
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }

        /// <summary>
        /// Get the books for the sections
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBooks()
        {
            var storeId = Request.Params["storeID"];

            var items = Request.Params["items"];
            string classCartXml = "<DocumentElement>";
            if (!string.IsNullOrEmpty(items))
            {

                items = items.Substring(0, items.Length - 1);
            }

            if (items != null)
            {
                var splitItems = items.Split('_');
                foreach (var item in splitItems)
                {
                    var splitItem = item.Split(',');

                    const string star = "*";

                    //create the key int the xml format to get the books
                    var key1 = splitItem[0] + star + splitItem[2] + star + splitItem[4] + star +
                               splitItem[6] +
                               star + splitItem[8];
                    classCartXml += "<CourseCartView>";
                    classCartXml += "<key>" + key1 + "</key>";
                    classCartXml += "<store>" + splitItem[0] + "</store>";
                    classCartXml += "<storeDescription>" + splitItem[1] + "</storeDescription>";
                    classCartXml += "<term>" + splitItem[2] + "</term>";
                    classCartXml += "<termDescription>" + splitItem[3] + "</termDescription>";
                    classCartXml += "<dept>" + splitItem[4] + "</dept>";
                    classCartXml += "<deptDescription>" + splitItem[5] + "</deptDescription>";
                    classCartXml += "<course>" + splitItem[6] + "</course>";
                    classCartXml += "<CourseDescription>" + splitItem[7] + "</CourseDescription>";
                    classCartXml += "<section>" + splitItem[8] + "</section>";
                    classCartXml += "<sectionDescription>" + splitItem[9] + "</sectionDescription>";
                    classCartXml += "</CourseCartView>";

                }
                classCartXml += "</</DocumentElement>>";
            }

            var myObject = new { STOREID = StoreNumber, CLASSCARTXML = classCartXml };

            var key = StoreNumber + ":" + Constants.GetBulkAddOption + ":" + myObject;

            var useCache = System.Configuration.ConfigurationManager.AppSettings["USECACHE"];

            var strCachedFile = "";

            var cachedFile = new object();
            try
            {
                if (useCache == "TRUE")
                {
                    cachedFile = System.Web.HttpContext.Current.Cache.Get(key);

                }

                if (!string.IsNullOrEmpty(strCachedFile))
                {
                    strCachedFile = cachedFile.ToString();
                    return Json(new { strCachedFile });
                }
                else
                {
                    // Call the service to get the books
                    var courseBookModels = SectionServices.GetSectionBooks(StoreNumber, Constants.GetBulkAddOption, myObject,
                                                            UvUsername, UvPassword,
                                                            DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                            Strd3PortNumber, UseEncryption, Strd3PortNumber);

                    // Place books in session
                    Session["BOOKS"] = courseBookModels;

                    return Json(new { courseBookModels });
                }
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }
        /// <summary>
        /// Add the book items to cart
        /// </summary>
        /// <returns></returns>
        public ActionResult AddToShoppingCart()
        {

            //Get the query strings
            var isbn = Request["Isbn"];

            var isAdded = false;

            var priceType = (Request["PriceType"] ?? "").ToLower();

            try
            {
                if (Session["BOOKS"] != null)
                {
                    var courseBooks = (IEnumerable<CourseSectionModel>)Session["BOOKS"];

                    var courseSectionModel = courseBooks.FirstOrDefault();
                    if (courseSectionModel != null)
                    {
                        var item = courseSectionModel.ItemModels.SingleOrDefault(b => b.Isbn.Equals(isbn));

                        //Check if the books is not out of stock then if will execute 
                        // oos=Out of Stock
                        if (!priceType.Equals("oos") && (priceType.Equals("new") || priceType.Equals("used") || priceType.Equals("rental") || priceType.Equals("ebook")))
                        {
                            if (item != null)
                            {
                                var newPrice = 0.00;
                                var usedPrice = 0.00;
                                var rentalPrice = 0.00;
                                var ebookPrice = 0.00;
                                var cartPrice = 0.00;
                                var type = "new";

                                try
                                {
                                    newPrice = Convert.ToDouble(item.NewPrice.Replace("$", ""));
                                    usedPrice = Convert.ToDouble(item.UsedPrice.Replace("$", ""));
                                    rentalPrice = Convert.ToDouble(item.RentalPrice.Replace("$", ""));
                                    ebookPrice = Convert.ToDouble(item.EbookPrice.Replace("$", ""));
                                }
                                catch (Exception e)
                                {
                                    newPrice = 0.00;
                                    usedPrice = 0.00;
                                    rentalPrice = 0.00;
                                    ebookPrice = 0.00;
                                }

                                if (priceType.Equals("new"))
                                {
                                    cartPrice = newPrice;
                                }
                                else if (priceType.Equals("used"))
                                {
                                    cartPrice = usedPrice;
                                    type = "used";
                                }
                                else if (priceType.Equals("rental"))
                                {
                                    cartPrice = rentalPrice;
                                    type = "tx-rent";
                                }
                                else if (priceType.Equals("ebook"))
                                {
                                    cartPrice = ebookPrice;
                                    type = "ebook";
                                }

                                var shoppingCartModel = new ShoppingCartModel
                                {
                                    Id = isbn,
                                    Isbn = isbn,
                                    ActualPrice = cartPrice,
                                    NewUsed = priceType,
                                    Type = type,
                                    Title = item.Title,
                                    NewPrice = newPrice,
                                    OldPrice = usedPrice,
                                    Detail = courseBooks.FirstOrDefault() != null ? courseBooks.FirstOrDefault().Class : "",
                                    Qty = 1,
                                    Course = courseBooks.FirstOrDefault().Class
                                };

                                shoppingCartModel.Total = 1 * shoppingCartModel.ActualPrice;

                                //Checking for null
                                if (Session["CARTITEMS"] == null)
                                {
                                    var lstShoppingCartModels = new List<ShoppingCartModel> { shoppingCartModel };

                                    Session["CARTITEMS"] = lstShoppingCartModels;

                                }
                                else
                                {
                                    var lstShoppingCartModels = ((List<ShoppingCartModel>)Session["CARTITEMS"]).ToList();

                                    var shoppingModel =
                                        (from model in lstShoppingCartModels where model.Id.Equals(isbn) select model).
                                            FirstOrDefault();

                                    if (shoppingModel != null)
                                    {
                                        if (priceType.Equals(shoppingModel.Type.ToLower()))
                                        {
                                            return Json(new { success = false, errMsg = "Already exist" });
                                        }
                                        shoppingModel.ActualPrice = cartPrice;
                                        shoppingModel.Type = (type);
                                        Session["CARTITEMS"] = lstShoppingCartModels;
                                        return Json(new { success = true, errMsg = "", });
                                    }

                                    lstShoppingCartModels.Add(shoppingCartModel);

                                    //Item that added to the cart latest item added t0 cart
                                    Session["LASTITEMADDED"] = shoppingCartModel.Title;

                                    Session["CARTITEMS"] = lstShoppingCartModels;

                                }

                                ViewData["CarTItems"] = (List<ShoppingCartModel>)Session["CARTITEMS"];

                                isAdded = true;
                            }


                        }
                    }
                }

                return Json(new { success = isAdded, errMsg = "", });

            }
            catch (Exception x)
            {
                return Json(new { success = isAdded, errMsg = x.Message });
            }
        }
        /// <summary>
        /// Get the list of item added to the cart
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewCartDetail()
        {

            var lstCartItems = new List<ShoppingCartModel>();
            try
            {

                //Check for the null
                if (Session["CARTITEMS"] != null)
                {
                    lstCartItems = (List<ShoppingCartModel>)Session["CARTITEMS"];

                }
                if (lstCartItems.Count > 0)
                {
                    return View(lstCartItems);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }
        /// <summary>
        /// Update the item qty
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateItemQty()
        {
            var id = Request["Id"] ?? "";

            id = id.Replace("-", "*");

            var qty = Convert.ToInt32(Request["Quantity"] ?? "1");

            var lstCartItems = new List<ShoppingCartModel>();
            try
            {

                //Check for the null
                if (Session["CARTITEMS"] != null)
                {
                    lstCartItems = (List<ShoppingCartModel>)Session["CARTITEMS"];

                    var cartItem = lstCartItems.Where(lst => lst.Id.Equals(id)).FirstOrDefault();
                    //Update the total quantity 
                    if (cartItem != null)
                    {
                        cartItem.Qty = qty;

                        cartItem.Total = qty * cartItem.ActualPrice;

                    }

                    Session["CARTITEMS"] = lstCartItems;
                }
                return PartialView("EditorTemplates/ViewCartItems", lstCartItems);
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }

        /// <summary>
        /// Remove the items form the cart
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoveCartItems()
        {
            var id = Request["Id"] ?? "";

            id = id.Replace("-", "*");

            var lstCartItems = new List<ShoppingCartModel>();

            try
            {

                //Check for the null
                if (Session["CARTITEMS"] != null)
                {
                    lstCartItems = (List<ShoppingCartModel>)Session["CARTITEMS"];

                    var cartItem = lstCartItems.Where(lst => lst.Id.Equals(id)).FirstOrDefault();

                    //Code block to remove the items form the cart
                    if (cartItem != null)
                    {
                        lstCartItems.Remove(cartItem);
                    }

                    Session["CARTITEMS"] = lstCartItems;
                }
                return PartialView("EditorTemplates/ViewCartItems", lstCartItems);
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }

        /// <summary>
        /// To Search items in site
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {

            return View();
        }

        /// <summary>
        /// Get Search Results
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonGetSearch()
        {
            var searchinput = Request["SearchInput"];

            var source = Request["Source"];

            var searchtype = Request["Type"];

            var pageIndex = Convert.ToInt32(Request["PageNo"]) - 1;

            var pageSize = Convert.ToInt32(Request["Pagesize"]);


            var myObject = new
            {
                STOREID = StoreNumber,
                SKU = searchinput,
                SEARCHTYPE = searchtype,
                SOURCE = source
            };

            //Get Search Result according to search parameter...
            var lstCatalogProductLookUpItems = CatalogsServices.GetCataLogProductLookUpItems(StoreNumber, myObject, source,
                                                            UvUsername, UvPassword,
                                                            DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                            Strd3PortNumber, UseEncryption, Strd3PortNumber).ToList();

            if (lstCatalogProductLookUpItems.Count == 0)
            {
                //return if no record found
                return Json(new { success = false, error = "No items were found with your search criteria, please try again." });
            }

            if (lstCatalogProductLookUpItems.Count > 350)
            {
                //return if result > 350
                return Json(new { success = false, ishuge = lstCatalogProductLookUpItems.Count, error = lstCatalogProductLookUpItems.Count + " results found" });
            }

            //Set result in session to do paging
            Session["SearchList"] = lstCatalogProductLookUpItems;


            var totalRecords = lstCatalogProductLookUpItems.Count;

            var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);

            lstCatalogProductLookUpItems =
                       ((List<ItemLookUpModel>)Session["SearchList"]).Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return Json(new { success = true, totalrecords = totalRecords, totalpages = totalPages, lstCatalogProductLookUpItems });
        }

        /// <summary>
        /// Do paging on list....
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonSearchResultPaging()
        {
            var pageIndex = Convert.ToInt32(Request["PageNo"]) - 1;

            var pageSize = Convert.ToInt32(Request["Pagesize"]);

            var lstCatalogProductLookUpItems = (List<ItemLookUpModel>)Session["SearchList"];

            var totalRecords = lstCatalogProductLookUpItems.Count;

            var totalPages = (int)Math.Ceiling(totalRecords / (float)pageSize);

            lstCatalogProductLookUpItems =
                       ((List<ItemLookUpModel>)Session["SearchList"]).Skip(pageIndex * pageSize).Take(pageSize).ToList();

            return Json(new { success = true, totalrecords = totalRecords, totalpages = totalPages, lstCatalogProductLookUpItems });

        }


        /// <summary>
        /// Get SellBackBooks
        /// </summary>
        /// <returns></returns>
        public ActionResult SellBackBooks()
        {
            try
            {
                return View();

            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }

        /// <summary>
        /// Post the book data based on the isbn
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SellBackBooks(int id = 0)
        {
            var isbn = Request["Isbn"] ?? "";

            //Get the id and replace the - with to make the sku
            isbn = isbn.Trim();

            try
            {
                //Create the object 
                object myObject = new
                {
                    STOREID = StoreNumber,
                    ISBN = isbn
                };

                var sellBackBookModels = SellBackSerivces.GetBookData(StoreNumber, myObject,
                                                                 UvUsername, UvPassword,
                                                                 DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                                 Strd3PortNumber, UseEncryption, Strd3PortNumber);

                Session["SELLBACKITEM"] = sellBackBookModels;

                return PartialView("EditorTemplates/ViewSellBackBooks", sellBackBookModels);

            }
            catch (Exception x)
            {
                return Json(new { errMsg = x.Message });
            }
        }
        /// <summary>
        /// Add sell back item to the cart
        /// </summary>
        /// <returns></returns>
        public ActionResult AddSellBackToCart()
        {
            var isbn = Request["Isbn"];

            var purchaseType = (Request["PurchaseType"] ?? "").ToLower();

            try
            {
                if (Session["SELLBACKITEM"] != null)
                {
                    var lstSellBackItems = (IEnumerable<SellBackBookModel>)Session["SELLBACKITEM"];

                    var sellBackItem = lstSellBackItems.Where(item => item.Isbn.Equals(isbn)).SingleOrDefault();

                    var lstSellBackCartItems = new List<SellBackCartViewModel>();

                    if (sellBackItem != null)
                    {
                        double actualCashPrice = 0.00;

                        double actualStoreCredit = 0.00;

                        var type = String.Empty;

                        var title = sellBackItem.Title;

                        //If this condition is true then add the item price retail price
                        if (purchaseType.Equals("retailbuy"))
                        {
                            type = "Retail";

                            actualCashPrice = Math.Round(Convert.ToDouble(sellBackItem.WhslrPrice), 2);

                            actualStoreCredit = Math.Round(Convert.ToDouble(sellBackItem.WhsleStoreCredit), 2);
                        }

                        //If this condition is for wholeselbuy
                        if (purchaseType.Equals("wholesalebuy"))
                        {
                            type = "Wholesale";

                            actualCashPrice = Math.Round(Convert.ToDouble(sellBackItem.RetailPrice), 2);

                            actualStoreCredit = Math.Round(Convert.ToDouble(sellBackItem.RetStoreCredit), 2);

                        }

                        var sellBackCartModel = new SellBackCartViewModel
                        {
                            Isbn = sellBackItem.Isbn,
                            CashPrice = actualCashPrice,
                            StoreCredit = actualStoreCredit,
                            Title = title,
                            Type = type

                        };


                        //chck for null
                        if (Session["SELLBACKCART"] != null)
                        {
                            lstSellBackCartItems = (List<SellBackCartViewModel>)Session["SELLBACKCART"];

                            lstSellBackCartItems.Add(sellBackCartModel);

                        }
                        else
                        {
                            lstSellBackCartItems.Add(sellBackCartModel);
                        }

                        Session["SELLBACKCART"] = lstSellBackCartItems;

                    }
                    return PartialView("EditorTemplates/ViewSellBackbooksCart", lstSellBackCartItems);
                }
                return Json(new { success = false, errMsg = "Internal error can't be added" });
            }
            catch (Exception x)
            {
                return Json(new { success = false, errMsg = x.Message });
            }

        }
        /// <summary>
        /// Remove Sellback items
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoveSellBackCartItem()
        {
            var id = Request["Id"] ?? "0";

            try
            {
                if (Session["SELLBACKITEM"] != null)
                {
                    var lstSellBackItems = ((IEnumerable<SellBackBookModel>)Session["SELLBACKITEM"]).ToList();

                    var sellBackItem = lstSellBackItems.Where(item => item.Isbn.Equals(id)).SingleOrDefault();
                    if (sellBackItem != null)
                    {
                        lstSellBackItems.Remove(sellBackItem);
                    }
                    Session["SELLBACKITEM"] = lstSellBackItems;

                    return PartialView("EditorTemplates/ViewSellBackbooksCart", lstSellBackItems);
                }
                return Json(new { success = false, errMsg = "Internal error can't be added" });
            }

            catch (Exception x)
            {
                return Json(new { success = false, errMsg = x.Message });
            }

        }



        /// <summary>
        /// Clear all the items of the cart
        /// </summary>
        /// <returns></returns>
        public ActionResult EmptyCart()
        {
            try
            {
                Session["CARTITEMS"] = null;
                return RedirectToAction("index", "Home");

            }
            catch (Exception x)
            {
                throw;
            }
        }

        //***** It is for calendar menu *****
        public ActionResult Calendar()
        {
            return View();
        }

        public ActionResult CalendarData()
        {
            //***** Reading data from the Mettings.xml *****
            var xmlDox = new XmlDataDocument();
            xmlDox.DataSet.ReadXmlSchema(Server.MapPath("~/Meetings.xml"));
            xmlDox.Load(Server.MapPath("~/Meetings.xml"));

            // store in the application cache
            var meetingDataTable = xmlDox.DataSet.Tables["Meeting"];

            var listCalendarModel = new List<CalendarViewModel>();

            for (int i = 1; i < meetingDataTable.Rows.Count; i++)
            {
                listCalendarModel.Add(new CalendarViewModel
                {
                    CalandarId = meetingDataTable.Rows[i]["id"].ToString(),
                    Title = meetingDataTable.Rows[i]["Title"].ToString(),
                    MeetingDate = Convert.ToDateTime(meetingDataTable.Rows[i]["MeetingDate"].ToString(), new CultureInfo("en-us")),
                    StartTime = meetingDataTable.Rows[i]["MeetingDate"] + " " + meetingDataTable.Rows[i]["StartTime"],
                    EndTime = meetingDataTable.Rows[i]["MeetingDate"] + " " + meetingDataTable.Rows[i]["EndTime"],
                    Location = meetingDataTable.Rows[i]["Location"].ToString(),
                });
            }

            return View(listCalendarModel);
        }

        //public ActionResult CalendarSave()
        //{
        //    //Console.WriteLine("I am here");
        //}
    }
}