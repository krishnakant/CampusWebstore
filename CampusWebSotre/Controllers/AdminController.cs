using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using CampusWebStore.Models;
using CampusWebStore.Business.Services;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;
using IWEB.Connector;
using Microsoft.Practices.Unity;
using System.Xml.Linq;
using System.Xml;
using CampusWebStore.Utils;


namespace CampusWebStore.Controllers
{
    public class AdminController : WebController
    {

        #region Properties
        [Dependency]
        public ICustomerServices CustomerService { get; set; }

        [Dependency]
        public IOrderService OrderService { get; set; }

        [Dependency]
        public IAccountSettingsService AccountSettingsService { get; set; }

        [Dependency]
        public IStoreServices StoreServices { get; set; }

        [Dependency]
        public ISellBackSerivces SellBackSerivces { get; set; }

        [Dependency]
        public IAdminSettingsService AdminSettingsService { get; set; }
        
        [Dependency]
        public ITermServices TermServices { get; set; }

        private string _uploadFolder { get; set; }

        #endregion
     
        //
        // GET: /Admin/
        
        public ActionResult Index()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var model = new AdminViewModel
                            {
                                IsUseBuyBack = objBaseMainConfigAdmin.IwebBuyBackConfig.Enabled,
                                IsShowIsbnTr = objBaseMainConfigAdmin.IwebConfigAdmin.ShowTradeISBN,
                                IsAsGuest = objBaseMainConfigAdmin.IwebConfigAdmin.ShopAsGuest,
                                IsShowIsbn = objBaseMainConfigAdmin.IwebConfigAdmin.ShowISBN,
                                IsShowIsbnIntegration = objBaseMainConfigAdmin.IwebConfigAdmin.ShowISBNIntegration,
                                IsLiveRates = objBaseMainConfigAdmin.IwebConfigAdmin.UseLiveShippingRates,
                                IsShowSubs = objBaseMainConfigAdmin.IwebConfigAdmin.ShowSubstitutionOption,
                                IsUseCats = objBaseMainConfigAdmin.IwebConfigAdmin.UseCatalogs,
                                IsUseQuickLinks = objBaseMainConfigAdmin.IwebConfigAdmin.UseQuickLinks,
                                IsEmailAdmins = objBaseMainConfigAdmin.IwebConfigAdmin.EmailOrdersToAdmin
                            };

            //LoadWizardValues();
            model.IsBuyBackEnable = objBaseMainConfigAdmin.IwebBuyBackConfig.Enabled;

            model.IsBuyBackShowNeed = objBaseMainConfigAdmin.IwebBuyBackConfig.ShowBuyBackNeed;

            model.IsBuyBackShowRetail= objBaseMainConfigAdmin.IwebBuyBackConfig.RetailBooks;

            model.IsBuyBackShowSholeSale = objBaseMainConfigAdmin.IwebBuyBackConfig.WholesaleBooks;

            model.IsBuyBackUseStoreCredit = objBaseMainConfigAdmin.IwebBuyBackConfig.StoreCredit;

            model.IsBuyBackDisplayPrices  = objBaseMainConfigAdmin.IwebBuyBackConfig.DisplayPrices;
            

            var sr = new StreamReader(Server.MapPath("~/contentfiles/SellBackDisclaimer.txt"));

            model.BuyBackDisclaimer = sr.ReadToEnd();
           
            sr.Close();



            var objAdminModel = AdminSettingsService.GetSettings(StoreNumber, UvUsername, UvPassword, DbType, UvAddress,
                                                                 UvAccount, CacheTime, CacheTime, Strd3PortNumber,
                                                                 UseEncryption, Strd3PortNumber);

            if (objAdminModel !=null)
            {
                model.IsPickupValid = objAdminModel.IsPickupValid;

                model.IsoosGm = objAdminModel.IsoosGm;

                model.IsoosTr = objAdminModel.IsoosTr;

                model.IsoosTx = objAdminModel.IsoosTx;

                model.GmImagePath = objAdminModel.GmImagePath;

                model.TrImagePath = objAdminModel.TrImagePath;

                model.TxImagePath = objAdminModel.TxImagePath;

                model.NewTxLowQty = objAdminModel.NewTxLowQty;

                model.NewTXoosQty = objAdminModel.NewTXoosQty;

                model.GmLowQty = objAdminModel.GmLowQty;

                model.GMoosmsg = objAdminModel.GMoosmsg;

                model.TrLowQty = objAdminModel.TrLowQty;

                model.TRoosQTY = objAdminModel.TRoosQTY;

                model.UsedTXoosQty = objAdminModel.UsedTXoosQty;

                model.UsedTxLowQty = objAdminModel.UsedTxLowQty;

                model.NewLowStockMsg = objAdminModel.NewLowStockMsg;

                model.NewOosMsg = objAdminModel.NewOosMsg;

                model.UsedLowStockMsg = objAdminModel.UsedLowStockMsg;

                model.UsedOosMsg = objAdminModel.UsedOosMsg;

                model.TRoosmsg = objAdminModel.TRoosmsg;

                model.TRlowstockmsg = objAdminModel.TRlowstockmsg;

                model.GMlowstockmsg = objAdminModel.GMlowstockmsg;

                model.GMoosmsg = objAdminModel.GMoosmsg;

                model.IsTrUseQoo = objAdminModel.IsTrUseQoo;

                model.IsGmUseQoo = objAdminModel.IsGmUseQoo;

                model.BuyBackStoreCredit = objAdminModel.BuyBackStoreCredit;

                model.BuyBackRetailPrice = objAdminModel.BuyBackRetailPrice;

                model.BuyBackRetailRounding = objAdminModel.BuyBackRetailRounding;

                model.BuyBackRetailCoin = objAdminModel.BuyBackRetailCoin;

                model.BuyBackWholesalePrice = objAdminModel.BuyBackWholesalePrice;

                model.BuyBackWholeSaleRounding = objAdminModel.BuyBackWholeSaleRounding;

                model.BuyBackWholeSaleCoin = objAdminModel.BuyBackWholeSaleCoin;

                model.BuyBackRetailRounding = objAdminModel.BuyBackRetailRounding;

                model.BuyBackWholesalePriceWording = objAdminModel.BuyBackWholesalePriceWording;

                model.BuyBackNeedWording = objAdminModel.BuyBackNeedWording;

                model.BuyBackBuyingWording = objAdminModel.BuyBackBuyingWording;

                model.BuyBackNotBuyingWording = objAdminModel.BuyBackNotBuyingWording;

            }

            return View(model);
        }

        #region "Faculty Admin"

        /// <summary>
        /// Get faculty admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult FacultyAdmin(FacultyAdminViewModel model)
        {
            //Check for the user logged in
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }

            var acctype = Session["ACCTYPE"].ToString() ?? "";

            var facultyAdminViewModel = new FacultyAdminViewModel();

            //  Check for role 
            if (acctype == "A")
            {
                facultyAdminViewModel.FacultyAdminEmail = objBaseMainConfigAdmin.IwebConfigAdmin.FacultyAdminEmailAddress;

                facultyAdminViewModel.FacultyAdminEmail = objBaseMainConfigAdmin.IwebConfigAdmin.FacultySecondaryEmailAddress;

            }

            var myObject = new { LOGIN = UvUsername };

            //Call the store service method to get the list of stores
            var lstStores = StoreServices.GetStore(StoreNumber, "TRUE",
                                                          UvUsername, UvPassword,
                                                          DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                          Strd3PortNumber, UseEncryption, Strd3PortNumber);

            var lstStoreList = new List<SelectListItem>();

            foreach (var s in lstStores)
            {
                var item = new SelectListItem { Text = s.Name, Value = s.Id };
                lstStoreList.Add(item);
            }

            ViewData["lstStoreList"] = lstStoreList;

            return View(facultyAdminViewModel);
        }
        /// <summary>
        /// Update the email address of the admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult JsonUpdateAdmin(FacultyAdminViewModel model)
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var errMsg = "";
            try
            {    //Checking for null 
                if (!(string.IsNullOrEmpty(model.FacultyAdminEmail) && string.IsNullOrEmpty(model.FacultySecondaryEmail)))
                {
                    objBaseMainConfigAdmin.IwebConfigAdmin.FacultyAdminEmailAddress = model.FacultyAdminEmail;

                    objBaseMainConfigAdmin.IwebConfigAdmin.FacultySecondaryEmailAddress = model.FacultySecondaryEmail;

                    objBaseMainConfigAdmin.SaveValues();

                    errMsg = "Successfully updated";
                }
                else
                {
                    errMsg = "Values can't be empty";
                }
            }
            catch (Exception x)
            {
                errMsg = x.Message;
            }
            return Json(new { errMsg = errMsg });
        }
        /// <summary>
        /// Return the Faculty terms list model
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonShowTerms()
        {
            try
            {
                //Call the private method to return the model
                var lstFacultyTermViewModel = GetFacultyTermList();

                return PartialView("EditorTemplates/Admin/TermList", lstFacultyTermViewModel);

            }
            catch (Exception x)
            {
                return Json(new { errMsg = x.Message });
            }
        }
        /// <summary>
        /// Verify the term
        /// </summary>
        /// <returns></returns>
        public ActionResult VerfiyTerm()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var termId = Request["TermId"] ?? "";
            var storeId = Request["StoreId"] ?? "";
            try
            {
                TermServices.VerifyTerm(storeId, termId, UvUsername, UvPassword,
                                        DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                        Strd3PortNumber, UseEncryption, Strd3PortNumber);
                return Json(new { success = true });
            }
            catch (Exception x)
            {
                return Json(new { success = false, errMsg = x.Message });
            }
        }

        #region "Private Methods"
        /// <summary>
        /// Method to return the Facultytermlist
        /// </summary>
        /// <returns></returns>
        IEnumerable<FacultyTermViewModel> GetFacultyTermList()
        {
            try
            {
                var filename = System.AppDomain.CurrentDomain.BaseDirectory.Replace("/", "\\") + "App_Data\\facultyterms.xml";

                var dataset = new DataSet();

                dataset.ReadXml(filename);

                var dt = dataset.Tables[0];

                var lstFacultyTermViewModel = new List<FacultyTermViewModel>();

                foreach (DataRow datarow in dt.Rows)
                {

                    var facultyTermViewModel = new FacultyTermViewModel
                    {
                        Id = Convert.ToString(datarow["id"]),
                        Desc = Convert.ToString(datarow["desc"]),
                        StoreId = Convert.ToString(datarow["storeid"]),
                        StoreDesc = Convert.ToString(datarow["storedesc"])
                    };


                    lstFacultyTermViewModel.Add(facultyTermViewModel);
                }

                return lstFacultyTermViewModel;
            }

            catch (Exception x)
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region "General Settings"

        #region "Manage Orders"


        /// <summary>
        /// GET: /Admin/ManageOrders
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageOrders()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var listOpenOrders = (from openorder in GetOrders("DATE", "Asending")
                                  select new OrderViewModel
                                  {
                                      OrderId = openorder.OrderId,
                                      UserId = openorder.UserId,
                                      UserName = openorder.UserName,
                                      ConfNumber = openorder.ConfNumber,
                                      Amount = openorder.Amount,
                                      DatePlaced = openorder.DatePlaced,
                                      DateProcessed = openorder.DateProcessed,
                                      Status = openorder.Status
                                  }).ToList();

            return View(listOpenOrders);
        }

        /// <summary>
        /// To Close the selected open order...
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonCloseOrders()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }

            var ordertoclose = Request["OrderToClose"];

            OrderService.CloseOrder(StoreNumber, ordertoclose, UvUsername,
                                        UvPassword, DbType, UvAddress,
                                        UvAccount, CacheTime, CacheTime,
                                        Strd3PortNumber, UseEncryption,
                                        Strd3PortNumber);

            var listOpenOrders = (from openorder in GetOrders("DATE", "Asending")
                                  select new OrderViewModel
                                  {
                                      OrderId = openorder.OrderId,
                                      UserId = openorder.UserId,
                                      UserName = openorder.UserName,
                                      ConfNumber = openorder.ConfNumber,
                                      Amount = openorder.Amount,
                                      DatePlaced = openorder.DatePlaced,
                                      DateProcessed = openorder.DateProcessed,
                                      Status = openorder.Status
                                  }).ToList();

            return PartialView("EditorTemplates/ViewOrderList", listOpenOrders);
        }

        /// <summary>
        /// To Sort the order list..
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonSortOrderList()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var sortby = Request["SortBy"];

            var sortdir = Request["SortDirection"];

            var listOpenOrders = (from openorder in GetOrders(sortby, sortdir)
                                  select new OrderViewModel
                                  {
                                      OrderId = openorder.OrderId,
                                      UserId = openorder.UserId,
                                      UserName = openorder.UserName,
                                      ConfNumber = openorder.ConfNumber,
                                      Amount = openorder.Amount,
                                      DatePlaced = openorder.DatePlaced,
                                      DateProcessed = openorder.DateProcessed,
                                      Status = openorder.Status
                                  }).ToList();

            return PartialView("EditorTemplates/ViewOrderList", listOpenOrders);
        }

        /// <summary>
        /// Get Order Details..
        /// </summary>
        /// <param name="transactionnumber"></param>
        /// <returns></returns>
        public ActionResult OrderDetails(string transactionnumber)
        {
            var objUserModel = (UserModel) Session["USERINFO"];

            var objOrderDetails = OrderService.GetOrderDetail(StoreNumber, /*"TUTTLEH"*/objUserModel.UserName, transactionnumber,
                                                              UvUsername, UvPassword, DbType, UvAddress, UvAccount,
                                                              CacheTime, CacheTime, Strd3PortNumber, UseEncryption,
                                                              Strd3PortNumber);

            var objOrderDetailViewModel = new OrderDetailViewModel
            {
                Address1 = objOrderDetails.Address1,
                Address2 = objOrderDetails.Address2,
                City = objOrderDetails.City,
                Country = objOrderDetails.Country,
                Frieght = objOrderDetails.Freight,
                FrieghtType = objOrderDetails.FreightType,
                GiftCard = objOrderDetails.GiftCard,
                Name = objOrderDetails.Name,
                OrderDate = objOrderDetails.OrderDate,
                OrderId = objOrderDetails.OrderId,
                ShipToName = objOrderDetails.ShipToName,
                ShipAddress1 = objOrderDetails.ShipAddress1,
                ShipAddress2 = objOrderDetails.ShipAddress2,
                PromoCode = objOrderDetails.PromoCode,
                ShipCity = objOrderDetails.ShipCity,
                ShipCountry = objOrderDetails.ShipCountry,
                ShipMethod = objOrderDetails.ShipMethod,
                ShipState = objOrderDetails.ShipState,
                ShipZip = objOrderDetails.ShipZip,
                Status = objOrderDetails.Status,
                StatusDate = objOrderDetails.StatusDate,
                SubTotal = objOrderDetails.SubTotal,
                Tax = objOrderDetails.Tax,
                Total = objOrderDetails.Total,
                UserId = objOrderDetails.UserId,
                Zip = objOrderDetails.Zip,
                OrderItemDetail = (from item in objOrderDetails.OrderItemDetail
                                   select new OrderItemDetailViewModel
                                   {
                                       Title = item.Title,
                                       EbookUrl = item.EbookInfo,
                                       EbookInfo = item.EbookInfo,
                                       Author = item.Author,
                                       ExtPrice = item.ExtPrice,
                                       Memo = item.Memo,
                                       Price = item.Price,
                                       Qty = item.Qty,
                                       Type = item.Type
                                   }).ToList()

            };


            return View(objOrderDetailViewModel);
        }

        /// <summary>
        /// function to get order list.....
        /// </summary>
        /// <param name="sortby"></param>
        /// <param name="sortdirection"></param>
        /// <returns></returns>
        protected List<OrderModel> GetOrders(string sortby, string sortdirection)
        {

            return OrderService.GetOpenOrders(StoreNumber, sortby, sortdirection, UvUsername,
                                              UvPassword, DbType, UvAddress,
                                              UvAccount, CacheTime, CacheTime,
                                              Strd3PortNumber, UseEncryption,
                                              Strd3PortNumber);
        }

        #endregion

        #region "Customer"
        /// <summary>
        /// Get list of customers
        /// </summary>

        /// <returns></returns>

        public ActionResult ManageCustomer()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            try
            {
                // TODO: Add delete logic here

                return View();
            }
            catch
            {
                return View();
            }
        }
        /// <summary>
        /// Get the list of customers
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonGetCustomerList()
        {
            try
            {
                //Get the variable from the querystring
                var searchText = Request["SearchText"] ?? "";

                if (searchText.Length > 1)
                {

                    var customerModels = CustomerService.GetCustomerList(StoreNumber, searchText, UvUsername,
                                                                         UvPassword,
                                                                         DbType, UvAddress, UvAccount, CacheTime,
                                                                         CacheTime,
                                                                         Strd3PortNumber, UseEncryption, Strd3PortNumber);

                    IEnumerable<CustomerViewModel> customerViewModels = (from customer in customerModels
                                                                         select new CustomerViewModel
                                                                         {
                                                                             City = customer.City,
                                                                             Email = customer.Email,
                                                                             Name = customer.Name,
                                                                             Phone = customer.Phone,
                                                                             State = customer.State,
                                                                             UserName = customer.CustId,
                                                                             Zip = customer.Zip

                                                                         }).ToList();



                    return PartialView("EditorTemplates/Admin/customerlist", customerViewModels);
                }

                return Json("Please enter atleast two charactors");
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }

        /// <summary>
        /// Get the detail of customer
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonGetCustomerDetail()
        {
            try
            {
                //Get the variable from the querystring
                var customerId = Request["CustomerId"] ?? "";

                if (customerId.Length > 1)
                {

                    var customerDetail = CustomerService.GetCustomerDetailById(StoreNumber, customerId, UvUsername,
                                                                                 UvPassword,
                                                                                 DbType, UvAddress, UvAccount, CacheTime,
                                                                                 CacheTime,
                                                                                 Strd3PortNumber, UseEncryption, Strd3PortNumber);

                    var customerDetailViewModel = new AccountSettingsViewModel
                    {
                        Address = customerDetail.Address,
                        Address2 = customerDetail.Address2,
                        City = customerDetail.City,
                        Country = customerDetail.Country,
                        DayPhone = customerDetail.DayPhone,
                        Email = customerDetail.Email,
                        FirstName = customerDetail.FirstName,
                        LastName = customerDetail.LastName,
                        ShipAddress = customerDetail.ShipAddress,
                        ShipAddress2 = customerDetail.ShipAddress2,
                        ShipCity = customerDetail.ShipCity,
                        ShipCountry = customerDetail.ShipCountry,
                        ShipInstruc = customerDetail.ShipInstruc,
                        ShipState = customerDetail.ShipState,
                        ShipToName = customerDetail.ShipToName,
                        ShipZip = customerDetail.ShipZip,
                        State = customerDetail.State,
                        UserName = customerDetail.UserName,
                        Zip = customerDetail.Zip,
                        IsEmailOptIn = customerDetail.EmailOptIn.Equals("Y"),
                        DropDownAccountType = customerDetail.TypeDescrition ?? ""
                    };

                    ViewBag.AccountType = customerDetail.TypeDescrition ?? "";
                    ViewBag.State = customerDetail.State ?? "";

                    //Get the history of te order for the customers
                    var orderModels = OrderService.GetOrderHistory(StoreNumber, customerId, UvUsername,
                                                                     UvPassword,
                                                                     DbType, UvAddress, UvAccount, CacheTime,
                                                                     CacheTime,
                                                                     Strd3PortNumber, UseEncryption, Strd3PortNumber);
                    IEnumerable<OrderViewModel> orderViewModels;

                    orderViewModels = (from order in orderModels
                                       select new OrderViewModel
                                       {
                                           OrderId = order.OrderId,
                                           ConfNumber = order.ConfNumber,
                                           DatePlaced = order.DatePlaced,
                                           DateProcessed = order.DateProcessed,
                                           Status = order.Status,
                                           Amount = order.Amount
                                       });



                    var customerViewModels = new CustomerDetailViewModel
                    {
                        CustomerDetail = customerDetailViewModel,
                        AllAccountType = customerDetail.AllAccountType,
                        OrderViewModels = orderViewModels

                    };

                    //Put the value to the session
                    Session["MAINTUSERINFO"] = customerViewModels;

                    return PartialView("EditorTemplates/Admin/customerDetail", customerViewModels);
                }

                return Json("Please enter atleast two charactors");
            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }
        [HttpPost]
        public JsonResult JsonUpdateCustomerDetail(CustomerDetailViewModel model)
        {
            if (model != null)
            {
                var accType = Request["AccType"];
                var emailoptin = Convert.ToBoolean(model.CustomerDetail.IsEmailOptIn) ? "Y" : "N";

                //Update the user details
                var objUserModel = AccountSettingsService.UpdateUser(
                    StoreNumber, model.CustomerDetail.UserName ?? "",
                    model.CustomerDetail.FirstName ?? "", 
                    model.CustomerDetail.LastName ?? "", 
                    model.CustomerDetail.Address ?? "",
                    model.CustomerDetail.Address2 ?? "",
                    model.CustomerDetail.City ?? "", 
                    model.CustomerDetail.State ?? "",
                    model.CustomerDetail.Zip ?? "", 
                    model.CustomerDetail.Country ?? "",
                    model.CustomerDetail.Email ?? "",
                    model.CustomerDetail.DayPhone ?? "",
                    model.CustomerDetail.Evening ?? "",
                    model.CustomerDetail.ShipToName ?? "",
                    model.CustomerDetail.ShipAddress ?? "",
                    model.CustomerDetail.ShipAddress2 ?? "",
                    model.CustomerDetail.ShipCity ?? "",
                    model.CustomerDetail.ShipState ?? "", 
                    model.CustomerDetail.ShipZip ?? "", 
                    model.CustomerDetail.ShipCountry ?? "",
                    model.CustomerDetail.ShipInstruc ?? "",
                    emailoptin ?? "",
                    model.CustomerDetail.DropDownAccountType ?? "",
                    UvUsername,
                    UvPassword, 
                    DbType, 
                    UvAddress, 
                    UvAccount,
                    CacheTime, 
                    CacheTime,
                    Strd3PortNumber, 
                    UseEncryption, 
                    Strd3PortNumber
                );

                if (!string.IsNullOrEmpty(objUserModel.ErrorMsg))
                {
                    ModelState.AddModelError("", objUserModel.ErrorMsg);
                }
                else
                {
                    Session["USERINFO"] = objUserModel;

                    return Json("MyAccount");
                }
            }
            return Json("MyAccount");
        }
        /// <summary>
        /// Get the customer detail
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonGetCustomerOrderDetail()
        {
            
            var customerId = Request["CustomerId"] ?? "TUTTLEH";

            var orderId = Request["OrderId"] ?? "";
            //var customerId = "max";

            //var orderId = "1*16054*WEB*1009";

            const string trasactionNo = "";

            //Get the order detail for the customer
            var orderDetail = OrderService.GetOrderDetail(StoreNumber, customerId, orderId, UvUsername,
                                                                 UvPassword,
                                                                 DbType, UvAddress, UvAccount, CacheTime,
                                                                 CacheTime,
                                                                 Strd3PortNumber, UseEncryption, Strd3PortNumber);

            var orderDetailViewModel = new OrderDetailViewModel
            {
                Address1 = orderDetail.Address1,
                Address2 = orderDetail.Address2,
                City = orderDetail.City,
                Country = orderDetail.Country,
                Frieght = orderDetail.Freight,
                FrieghtType = orderDetail.FreightType,
                GiftCard = orderDetail.GiftCard,
                Name = orderDetail.Name,
                OrderDate = orderDetail.OrderDate,
                OrderId = orderDetail.OrderId,
                ShipToName = orderDetail.ShipToName,
                ShipAddress1 = orderDetail.ShipAddress1,
                ShipAddress2 = orderDetail.ShipAddress2,
                PromoCode = orderDetail.PromoCode,
                ShipCity = orderDetail.ShipCity,
                ShipCountry = orderDetail.ShipCountry,
                ShipMethod = orderDetail.ShipMethod,
                ShipState = orderDetail.ShipState,
                ShipZip = orderDetail.ShipZip,
                Status = orderDetail.Status,
                StatusDate = orderDetail.StatusDate,
                SubTotal = orderDetail.SubTotal,
                Tax = orderDetail.Tax,
                Total = orderDetail.Total,
                UserId = orderDetail.UserId,
                Zip = orderDetail.Zip,
                OrderItemDetail = (from item in orderDetail.OrderItemDetail
                                   select new OrderItemDetailViewModel
                                   {
                                       Title = item.Title,
                                       EbookUrl = item.EbookInfo,
                                       EbookInfo = item.EbookInfo,
                                       Author = item.Author,
                                       ExtPrice = item.ExtPrice,
                                       Memo = item.Memo,
                                       Price = item.Price,
                                       Qty = item.Qty,
                                       Type = item.Type
                                   }).ToList()

            };

            return PartialView("EditorTemplates/ShowOrderDetails", orderDetailViewModel);
        }

        #endregion

        #region "Maintain Catalogs"

        /// <summary>
        /// GET MaintainCatalogs view
        ///  </summary>
        /// <returns></returns>
        public ActionResult MaintainCatalogs()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            //var tmpcat1 = new DZoft.dBug.TCS.DL.Catalogs("MAX");

            // Get the order detail for the customer
           var cataloglistViewModel=new List<CatalogViewModel>();
            object myObject = new { ALL = "1" };
            if (Session["AdminCatalog"] == null)
            {
                var lstCatalogsModels = CatalogsServices.GetCatalogsList(StoreNumber, myObject,
                                                                         UvUsername, UvPassword,
                                                                         DbType, UvAddress, UvAccount, CacheTime,
                                                                         CacheTime,
                                                                         Strd3PortNumber, UseEncryption,
                                                                         Strd3PortNumber);
                cataloglistViewModel = (from catalogItem in lstCatalogsModels
                                           select new CatalogViewModel
                                           {
                                               CatalogId =
                                                   catalogItem.CatalogId,
                                               Desc = catalogItem.Desc,
                                               SeoUrl = catalogItem.SeoUrl
                                           }).ToList();
                Session["AdminCatalog"] = cataloglistViewModel;
            }

            ViewData["AdminCatalog"] = Session["AdminCatalog"];
           

            return View(cataloglistViewModel);
        }

        /// <summary>
        /// Create new catalog
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult JsonCreateCatalog(CatalogPostModel postModel)
        {

            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(postModel.Key))
                {
                    var lstCatalogsModels = CatalogsServices.CreateNewCatalog(true, "", postModel.Description,
                                                                              postModel.StartDate, postModel.EndDate,
                                                                              postModel.DisplayTumbnail.ToString(
                                                                                  CultureInfo.InvariantCulture),
                                                                              postModel.Comment, postModel.Webcolumn,
                                                                              postModel.SortSquence,
                                                                              DZoft.dBug.TCS.DL.PickBase.openMode.Add);
                }
                else
                {
                    var lstCatalogsModels = CatalogsServices.CreateNewCatalog(false, postModel.Key,
                                                                              postModel.Description,
                                                                              postModel.StartDate, postModel.EndDate,
                                                                              postModel.DisplayTumbnail.ToString(
                                                                                  CultureInfo.InvariantCulture),
                                                                              postModel.Comment, postModel.Webcolumn,
                                                                              postModel.SortSquence,
                                                                              DZoft.dBug.TCS.DL.PickBase.openMode.Update);
                }
                return Json(new {success = true});
            }
            return Json(new {success = false});
        }

        public ActionResult JsonSearchCatalogItem()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new {ReturnUrl = Request.Url.PathAndQuery});
            }
            var sku = Request["SKU"] ?? "";
            var myObject = new
                               {
                                   STOREID = 1,
                                   SKU = sku,
                                   SOURCE = "ALL",
                                   SEARCHTYPE = "ALL"
                               };

            var lstCatalogProductLookUpItems = CatalogsServices.GetCataLogProductLookUpItems(StoreNumber, myObject, "",
                                                                                             UvUsername, UvPassword,
                                                                                             DbType, UvAddress,
                                                                                             UvAccount,
                                                                                             CacheTime, CacheTime,
                                                                                             Strd3PortNumber,
                                                                                             UseEncryption,
                                                                                             Strd3PortNumber);
            IEnumerable<AdminCatalogItemViewModel> models = (from itemLookUp in lstCatalogProductLookUpItems
                                                             select new AdminCatalogItemViewModel
                                                                        {
                                                                            Mod = itemLookUp.Description,
                                                                            Price = itemLookUp.Price,
                                                                            SalesPrice = itemLookUp.SalePrice,
                                                                            Sku = itemLookUp.Sku,
                                                                            Title = itemLookUp.SeoTitle,

                                                                        });

            Session["SearchedCatalogItems"] = models;

            return PartialView("EditorTemplates/Admin/SearchCatalogItems", models);
        }

        public ActionResult JsonCreateCatalog()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
          //  LoadData(Request["seletedvalue"].ToString());
            return PartialView("EditorTemplates/Admin/AddCatalogs");

        }
        public ActionResult JsonEditCatalog()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
             var catId = Request["catId"];
            var models = CatalogsServices.GetCatalogItemList(catId);
            var catalogPostModel=new CatalogPostModel
                                     {
                                         Key =models.Key,
                                         Comment = models.Comment,
                                         StartDate = models.StartDate,
                                         EndDate = models.EndDate,
                                         Description = models.Description,
                                         DisplayTumbnail = models.DisplayTumbnail,
                                         SortSquence = models.SortSquence,
                                         Webcolumn = models.Webcolumn,
                                         AdminCatalogItemViewModel =(from item in models.AdminCatalogItemModels select new AdminCatalogItemViewModel
                                                                         {
                                                                               Mod = item.Mod,
                                                                               Price = item.Price,
                                                                               SalesPrice = item.SalesPrice,
                                                                               Sku = item.Sku,
                                                                                Title = item.Title
                                                                         })
                                     };
            Session["EditCatalogItems"] = models.AdminCatalogItemModels.ToList();
           
            return PartialView("EditorTemplates/Admin/AddCatalogs",catalogPostModel);

        }

        public ActionResult JsonAddCatalogItems()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            if (Session["SearchedCatalogItems"] != null)
            {
                var catId = Request["catId"];
                var catalogItemId=Request["CatalogItemId"];
                var models = (IEnumerable<AdminCatalogItemViewModel>) Session["SearchedCatalogItems"];
                var model = (from m in models where m.Sku.Equals(catalogItemId) select m).SingleOrDefault();
                if (model != null)
                {
                    if (Session["EditCatalogItems"]!=null)
                    {
                        var catelogModel = (IEnumerable<AdminCatalogItemModel>) Session["EditCatalogItems"];
                        var alreadyExistItem = catelogModel.FirstOrDefault(m => m.Sku.Equals(model.Sku));
                        if (alreadyExistItem == null)
                        {
                            CatalogsServices.AddCatalogItem(catId, model.Sku, model.Mod, model.Title, model.Price,
                                                            model.SalesPrice, catelogModel);

                            var adminCatalogItemModel = new AdminCatalogItemModel
                                                            {
                                                                Mod = model.Sku,
                                                                Price = model.Price,
                                                                SalesPrice = model.SalesPrice,
                                                                 Sku = model.Sku,
                                                                  Title = model.Title
                                                            };

                            var adminCatalogModel = (from m in models
                                                     select new AdminCatalogItemViewModel
                                                     {
                                                         Mod = m.Mod,
                                                         Price = m.Price,
                                                         SalesPrice = m.SalesPrice,
                                                         Sku = m.Sku,
                                                         Title = m.Title
                                                     }
                                      ).ToList();
                            return PartialView("EditorTemplates/Admin/CatalogItems", adminCatalogModel);
                        }
                        
                            return Json(new { success = false });
                        
                    }
                   
                }
            }

            return Json(new {success = true});
        }
        public ActionResult JsonEditSalesPrice(string id)
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var newSalesPrice = Request["NewSalesPrice"] ?? "";
            var catId = Request["CatId"] ?? "";
            var needUpdate = false;
           
            if (Session["EditCatalogItems"] == null)
            {
                return PartialView("EditorTemplates/Admin/CatalogItems", new List<AdminCatalogItemViewModel>());
            }
            var models = (List<AdminCatalogItemModel>)Session["EditCatalogItems"];
            int sortOrder;
           
           var isInt= Int32.TryParse(id, out sortOrder);
           if (isInt)
           {
               float salesPrice;
               var isSalePrice = float.TryParse(newSalesPrice, out salesPrice);
               if (isSalePrice)
               {
                   var model = models.ElementAt(Convert.ToInt32(sortOrder));

                   model.SalesPrice = salesPrice.ToString(CultureInfo.InvariantCulture);
                   needUpdate = true;
               }
           }
            var adminCatalogModel = (from m in models
                                     select new AdminCatalogItemViewModel
                                     {
                                         Mod = m.Mod,
                                         Price = m.Price,
                                         SalesPrice = m.SalesPrice,
                                         Sku = m.Sku,
                                         Title = m.Title
                                     }
                                    ).ToList();
            //If there is no change in order then it will not update the catalog
            if (needUpdate)
            {
                CatalogsServices.UpdateCatalogData(1, catId, DZoft.dBug.TCS.DL.PickBase.openMode.Update, models);
            }
            return PartialView("EditorTemplates/Admin/CatalogItems", adminCatalogModel);
        }
        public ActionResult JsonMoveUpDown(string id)
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var needUpdate = false;
            var type =Convert.ToString(Request["Type"]??"").ToLower();
            var catId = Convert.ToString(Request["CatId"]??"").ToLower();
            if(Session["EditCatalogItems"]==null)
            {
                return PartialView("EditorTemplates/Admin/CatalogItems",new List<AdminCatalogItemViewModel>());
            }
            var models = (List<AdminCatalogItemModel>) Session["EditCatalogItems"];
            int sortOrder;
            var isInt = Int32.TryParse(id,out sortOrder);
            if (models.Count() - 1 == sortOrder && isInt)
            {
                var model = models.ElementAt(Convert.ToInt32(sortOrder));

                if (type.Equals("moveup"))
                {
                    if (sortOrder > 0)
                    {
                        models.RemoveAt(Convert.ToInt32(sortOrder));
                        models.Insert(sortOrder - 1, model);
                        needUpdate = true;
                    }
                }
                if (type.Equals("movedown"))
                {
                    if (sortOrder < models.Count - 1)
                    {
                        models.RemoveAt(Convert.ToInt32(sortOrder));
                        models.Insert(sortOrder + 1, model);
                        needUpdate = true;
                    }
                }
            }
            var adminCatalogModel = (from m in models
                                     select new AdminCatalogItemViewModel
                                                {
                                                    Mod = m.Mod,
                                                    Price = m.Price,
                                                    SalesPrice = m.SalesPrice,
                                                    Sku = m.Sku,
                                                    Title = m.Title
                                                }
                                    ).ToList();
            //If there is no change in order then it will not update the catalog
            if (needUpdate)
            {
                CatalogsServices.UpdateCatalogData(1, catId, DZoft.dBug.TCS.DL.PickBase.openMode.Update, models);
            }
            return PartialView("EditorTemplates/Admin/CatalogItems", adminCatalogModel);
        }

        public ActionResult JsonRemoveCatalogItem(string id)
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
           
            var catId = Convert.ToString(Request["CatId"] ?? "").ToLower();
            if (Session["EditCatalogItems"] == null)
            {
                return PartialView("EditorTemplates/Admin/CatalogItems", new List<AdminCatalogItemViewModel>());
            }
            var models = (List<AdminCatalogItemModel>)Session["EditCatalogItems"];

            var catelogItem = models.ElementAt(Convert.ToInt32(id));
            if (catelogItem != null)
            {
                models.Remove(catelogItem);

                CatalogsServices.UpdateCatalogData(1, catId, DZoft.dBug.TCS.DL.PickBase.openMode.Delete, models);

            }
            var adminCatalogModel = (from m in models
                                     select new AdminCatalogItemViewModel
                                     {
                                         Mod = m.Mod,
                                         Price = m.Price,
                                         SalesPrice = m.SalesPrice,
                                         Sku = m.Sku,
                                         Title = m.Title
                                     }
                                      ).ToList();
            return PartialView("EditorTemplates/Admin/CatalogItems", adminCatalogModel);
        }
       
        #endregion

        public ActionResult JsonSaveSettings(AdminViewModel model)
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            //Set IWEB Settings values in XML..
            objBaseMainConfigAdmin.IwebBuyBackConfig.Enabled = model.IsBuyBackEnable;

            objBaseMainConfigAdmin.IwebConfigAdmin.ShowTradeISBN = model.IsShowIsbnTr;

            objBaseMainConfigAdmin.IwebConfigAdmin.ShowISBN = model.IsShowIsbn;

            objBaseMainConfigAdmin.IwebConfigAdmin.ShowISBNIntegration = model.IsShowIsbnIntegration;

            objBaseMainConfigAdmin.IwebConfigAdmin.UseLiveShippingRates = model.IsLiveRates;

            objBaseMainConfigAdmin.IwebConfigAdmin.ShowSubstitutionOption = model.IsShowSubs;

            objBaseMainConfigAdmin.IwebConfigAdmin.UseCatalogs = model.IsUseCats;

            objBaseMainConfigAdmin.IwebConfigAdmin.UseQuickLinks = model.IsUseQuickLinks;

            objBaseMainConfigAdmin.IwebConfigAdmin.EmailOrdersToAdmin = model.IsEmailAdmins;

            objBaseMainConfigAdmin.IwebConfigAdmin.ShopAsGuest = model.IsAsGuest;

            objBaseMainConfigAdmin.IwebBuyBackConfig.DisplayPrices = model.IsBuyBackDisplayPrices;

            objBaseMainConfigAdmin.IwebBuyBackConfig.RetailBooks = model.IsBuyBackShowRetail;

            objBaseMainConfigAdmin.IwebBuyBackConfig.ShowBuyBackNeed = model.IsBuyBackShowNeed;

            objBaseMainConfigAdmin.IwebBuyBackConfig.StoreCredit = model.IsBuyBackUseStoreCredit;

            objBaseMainConfigAdmin.IwebBuyBackConfig.WholesaleBooks = model.IsBuyBackShowSholeSale;

            //Save sellbackdisclaimer text...
            var tr = new StreamWriter(Server.MapPath("~/contentfiles/sellbackdisclaimer.txt"), false);

            tr.Write(model.BuyBackDisclaimer);

            tr.Close();

           //Save in the settings.xml config file
            objBaseMainConfigAdmin.SaveValues();

            //set values in pick
            var truseqoo = Convert.ToBoolean(model.IsTrUseQoo) ? "Y" : "N";

            var gmuseqoo = Convert.ToBoolean(model.IsGmUseQoo) ? "Y" : "N";

            var ispickupvalid = Convert.ToBoolean(model.IsPickupValid) ? "Y" : "N";

            var showoosgm = Convert.ToBoolean(model.IsoosGm) ? "Y" : "N";

            var showoostr = Convert.ToBoolean(model.IsoosTr) ? "Y" : "N";

            var showoostx = Convert.ToBoolean(model.IsoosTx) ? "Y" : "N";


            AdminSettingsService.SaveSettings(StoreNumber, model.BuyBackNotBuyingWording, model.BuyBackBuyingWording,
                                              model.BuyBackNeedWording, model.BuyBackWholesalePriceWording,
                                              model.BuyBackRetailPriceWording, model.BuyBackWholeSaleCoin,
                                              model.BuyBackWholeSaleRounding, model.BuyBackWholesalePrice,
                                              model.BuyBackRetailCoin, model.BuyBackRetailRounding,
                                              model.BuyBackRetailPrice, model.BuyBackStoreCredit, gmuseqoo, truseqoo,
                                              model.GMoosmsg, model.GMlowstockmsg, model.TRlowstockmsg, model.TRoosmsg,
                                              model.UsedOosMsg, model.UsedLowStockMsg, model.NewLowStockMsg,
                                              model.NewOosMsg, model.UsedTxLowQty, model.UsedTXoosQty, model.GmLowQty,
                                              model.GMoosmsg, model.TrLowQty, model.TRoosQTY, model.NewTXoosQty,
                                              model.NewTxLowQty, model.TxImagePath, model.TrImagePath,
                                              model.GmImagePath, showoostx, showoostr, showoosgm, ispickupvalid,
                                              UvUsername, UvPassword, DbType, UvAddress, UvAccount,
                                              CacheTime, CacheTime, Strd3PortNumber, UseEncryption, Strd3PortNumber);


            return Json(new {success = true});
        }
        public ActionResult JsonGetAdminCatalogs()
        {

            return PartialView("EditorTemplates/Admin/DropDownCatalog");
        }
        #endregion

        #region File Management
        #region File Manipulation Views
        public ActionResult FileUploader()
        {
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            return View();
        }

        public ActionResult ManageImages()
        {
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            return View();
        }

        public ActionResult ViewFiles(string id)
        {
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            if (id == "MyGMImages")
            {
                ViewBag.dest = "gm";
            }
            else if (id == "MyTRImages")
            {
                ViewBag.dest = "tr";
            }
            else if (id == "MyTXImages")
            {
                ViewBag.dest = "tx";
            }
            else
            {
                ViewBag.dest = "other";
            }

            string path = Server.MapPath("~/" + id + "/");
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles("*.*");

            ViewBag.files = files;
            int port = Request.Url.Port;
            if (port.ToString() != "")
            {
                ViewBag.path = Request.Url.Host + ":" + port.ToString() + "/" + id + "/";
            }
            else
            {
                ViewBag.path = Request.Url.Host + "/" + id + "/";
            }
            ViewBag.Title = "Files in " + id;
            ViewBag.folder = id;

            return View();
        }
        #endregion

        #region Public File Manipulation
        [HttpGet]
        public ActionResult DeleteFile(string id)
        {
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            var destination = Request.QueryString["dest"] ?? string.Empty;
            var redirect = Request.QueryString["r"] ?? string.Empty;

            if (destination == "gm")
            {
                destination = "MyGMImages";
            }
            else if (destination == "tr")
            {
                destination = "MyTRImages";
            }
            else if (destination == "tx")
            {
                destination = "MyTXImages";
            }
            else
            {
                destination = "OtherFiles";
            }

            var filename = id;
            var filePath = Path.Combine(Server.MapPath("~/" + destination + "/"), filename);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            if (redirect != "")
            {
                return RedirectToAction("ViewFiles", "Admin", new { id = destination });
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public void DownloadFile(string id)
        {
            var destination = Request.QueryString["dest"] ?? string.Empty;

            if (destination == "gm")
            {
                destination = "MyGMImages";
            }
            else if (destination == "tr")
            {
                destination = "MyTRImages";
            }
            else if (destination == "tx")
            {
                destination = "MyTXImages";
            }
            else
            {
                destination = "OtherFiles";
            }

            var filename = id;
            var filePath = Path.Combine(Server.MapPath("~/" + destination + "/"), filename);

            var context = HttpContext;

            if (System.IO.File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
                context.Response.StatusCode = 404;
        }

        [HttpPost]
        public ActionResult UploadFiles(FormCollection collection)
        {
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn");
            }

            string destination = collection["destination"].ToString() ?? string.Empty;

            if (destination == "gm")
            {
                _uploadFolder = "MyGMImages";
            }
            else if (destination == "tr")
            {
                _uploadFolder = "MyTRImages";
            }
            else if (destination == "tx")
            {
                _uploadFolder = "MyTXImages";
            }
            else
            {
                _uploadFolder = "OtherFiles";
            }

            var r = new List<ViewDataUploadFilesResult>();

            foreach (string file in Request.Files)
            {
                var statuses = new List<ViewDataUploadFilesResult>();
                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    UploadWholeFile(Request, statuses, destination);
                }
                else
                {
                    UploadPartialFile(headers["X-File-Name"], Request, statuses, destination);
                }

                JsonResult result = Json(statuses);
                result.ContentType = "text/plain";

                return result;
            }

            return Json(r);
        }
        #endregion

        #region Private File Manipulation
        private string StorageRoot
        {
            get { return Path.Combine(Server.MapPath("~/" + _uploadFolder)); }
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        private void UploadPartialFile(string fileName, HttpRequestBase request, List<ViewDataUploadFilesResult> statuses, string destination)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            var inputStream = file.InputStream;

            var fullName = Path.Combine(StorageRoot + "/", Path.GetFileName(fileName));

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(new ViewDataUploadFilesResult()
            {
                name = fileName,
                size = file.ContentLength,
                type = file.ContentType,
                url = "/Admin/DownloadFile/" + fileName + "?dest=" + destination,
                delete_url = "/Admin/DeleteFile/" + fileName + "?dest=" + destination,
                thumbnail_url = @"data:image/png;base64," + EncodeFile(fullName),
                delete_type = "GET",
            });
        }

        private void UploadWholeFile(HttpRequestBase request, List<ViewDataUploadFilesResult> statuses, string destination)
        {
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];

                var fullPath = Path.Combine(StorageRoot, Path.GetFileName(file.FileName));

                file.SaveAs(fullPath);
                
                // Create Thumbnail
                ImageManager.CreateThumbnail(file, _uploadFolder);

                statuses.Add(new ViewDataUploadFilesResult()
                {
                    name = file.FileName,
                    size = file.ContentLength,
                    type = file.ContentType,
                    url = "/Admin/DownloadFile/" + file.FileName + "?dest=" + destination,
                    delete_url = "/Admin/DeleteFile/" + file.FileName + "?dest=" + destination,
                    thumbnail_url = @"data:image/png;base64," + EncodeFile(fullPath),
                    delete_type = "GET",
                });
            }
        }
        #endregion
        #endregion

        #region "Content Management"

        /// <summary>
        /// GET Calendar - Manage Events view
        ///  </summary>
        /// <returns></returns>
        public ActionResult CalendarAdmin()
        {
           if(!HasAdminPrivilege())
           {
               return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
           }
            return View();
        }

        /// <summary>
        /// GET ADDnewcalendarEvent view
        ///  </summary>
        /// <returns></returns>
        public ActionResult AddNewCalendarEvent()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            return View();
        }

        /// <summary>
        /// POST Insert button in ADDnewcalendarEvent view
        ///  </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNewCalendarEvent(CalendarViewModel model)
        {
            XDocument xmlDoc = XDocument.Load(Server.MapPath("~/Meetings.xml"));

            if (model.CalandarId != null)
            {
                string onlyDate = model.MeetingDate.Month + "/" + model.MeetingDate.Day + "/" + model.MeetingDate.Year;
                xmlDoc.Element("Meetings").Add(new XElement("Meeting",
                new XElement("id", model.CalandarId),   
                                                 new XElement("Title", model.Title),
                                                 new XElement("MeetingDate", onlyDate),
                                                 new XElement("StartTime", model.StartTime),
                                                 new XElement("EndTime",  model.EndTime),
                                                 new XElement("Location", model.Location)));

                xmlDoc.Save(Server.MapPath("~/Meetings.xml"));
            }
           
            return View();
        }

         /// <summary>
        /// Get the list of campaign
        /// </summary>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public JsonResult CalendarnGrid(string sidx, string sord, Pager objPage, int rows)
         {           
             var jsonData = new Object();
            
                objPage.PageSize = rows;
                // object for the pagging
                
                var offset = (objPage.Page - 1) * objPage.PageSize;

                var pageSize = objPage.PageSize;

                var page = objPage.Page;

                //***** Reading data from the Mettings.xml *****
                XmlDataDocument xmlDox = new XmlDataDocument();
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
                        MeetingDate = Convert.ToDateTime(meetingDataTable.Rows[i]["MeetingDate"].ToString(),new CultureInfo("en-us")),
                        StartTime = meetingDataTable.Rows[i]["StartTime"].ToString(),
                        EndTime = meetingDataTable.Rows[i]["EndTime"].ToString(),
                        Location = meetingDataTable.Rows[i]["Location"].ToString(),
                    });
                }
            
                int pageIndex = Convert.ToInt32(page) - 1;


             var listTest = listCalendarModel;
            //listTest.OrderB

                 var totalpages = objPage.ToatalPages();
                    
                jsonData = new
                    {
                        total = totalpages,
                        page,
                        records = objPage.TotalRecords,

                        rows = (
                                from calendarList in listCalendarModel
                                select new
                                {
                                    i = calendarList,
                                    cell = new string[]
                                                                        {
                                                                            calendarList.CalandarId,
                                                                            calendarList.Title,
                                                                            calendarList.MeetingDate.ToString(),
                                                                            calendarList.StartTime,
                                                                        calendarList.EndTime,
                                                                        calendarList.Location ,
                                                                        }
                                }).ToArray()
                    };
                
            
             return Json(jsonData);
         }

        /// <summary>
        /// POST Edit button in ADDnewcalendarEvent view
        ///  </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult EditCalenderXml(FormCollection form, CalendarViewModel objcal)
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            XDocument xmldoc = XDocument.Load(Server.MapPath("~/Meetings.xml"));
            //XElement xElement = xmldoc.XPathSelectElement("Meetings/Meeting/id[@id = '" + objcal.CalandarId + "']");

            var items = from item in xmldoc.Descendants("Meeting")
                        where item.Element("id").Value == objcal.CalandarId
                        select item;

            foreach (XElement itemElement in items)
            {
                string onlyDate = objcal.MeetingDate.Month + "/" + objcal.MeetingDate.Day + "/" + objcal.MeetingDate.Year;

                itemElement.SetElementValue("id", objcal.CalandarId);
                itemElement.SetElementValue("Title", objcal.Title);
                itemElement.SetElementValue("MeetingDate", onlyDate);
                itemElement.SetElementValue("StartTime", objcal.StartTime);
                itemElement.SetElementValue("EndTime", objcal.EndTime);
                itemElement.SetElementValue("Location", objcal.Location);
            }

            xmldoc.Save(Server.MapPath("~/Meetings.xml"));
            
            return View("calendarAdmin");
        }

        /// <summary>
        /// POST Delete button in ADDnewcalendarEvent view inside Grid
        ///  </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult DeleteCalendarElement()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var id = Convert.ToInt64(Request["id"] ?? "0");

            XDocument xmldoc = XDocument.Load(Server.MapPath("~/Meetings.xml"));
          
            var items = from item in xmldoc.Descendants("Meeting")
                        where item.Element("id").Value == id.ToString()
                        select item;
                    
            foreach (XElement itemElement in items)
            {
                itemElement.Remove();
                break;
            }

            xmldoc.Save(Server.MapPath("~/Meetings.xml"));

            return View("calendarAdmin");           
        }

        /// <summary>
        /// Checking availability of ID in ADDnewcalendarEvent view
        ///  </summary>
        /// <returns></returns>
        /// 
        /// 
        [HttpPost]
        public JsonResult JsonCheckIdAvailability()
        {
            var id = Convert.ToInt64(Request["CalandarId"] ?? "0");

            XDocument xmldoc = XDocument.Load(Server.MapPath("~/Meetings.xml"));

            var items = from item in xmldoc.Descendants("Meeting")
                        where item.Element("id").Value == id.ToString()
                        select item;
            
            bool isAvailable = true;
            
            foreach (XElement itemElement in items)
            {
                isAvailable = false;
                break;
            }

            return Json(new { success = isAvailable });
        }


        //********** Quick Links section ******

        /// <summary>
        /// GET LinkManager - Manage Events view
        ///  </summary>
        /// <returns></returns>
        public ActionResult LinkManagerAdmin()
        {
            return View();
        }

        public ActionResult AddLinkAdmin(string message)
        {
            ViewData["status"] = 0;

            if (message != null)
            {
                ViewData["status"] = 1;
            }

            return View();
        }

         /// <summary>
        /// POST Insert button in AddLinkADmin view
        ///  </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddLinkAdmin(AddLinkViewModel model)
        {
            XDocument xmlDoc = XDocument.Load(Server.MapPath("~/QuickLinks.xml"));

             try
             {
                if (model.LinkOrder != null)
                {
                    xmlDoc.Element("QuickLinks").Add(new XElement("QuickLink",
                    new XElement("LinkOrder", model.LinkOrder),
                                                        new XElement("LinkTitle", model.LinkTitle),
                                                        new XElement("LinkAddress", model.LinkAddress )
                                                        ));
                    xmlDoc.Save(Server.MapPath("~/QuickLinks.xml"));

                    return RedirectToAction("AddLinkAdmin", "Admin", new {message = "success"});
                }
             }
             catch (Exception ex)
             {
             

             }
           
            return View();
        }

         /// <summary>
        /// Checking availability of LinkOrder in AddLinkAdmin view
        ///  </summary>
        /// <returns></returns>
        /// 
        /// 
        [HttpPost]
        public JsonResult JsonCheckLinkOrderAvailability()
         {
             var id = Convert.ToInt64(Request["LinkOrder"] ?? "0");

             XDocument xmldoc = XDocument.Load(Server.MapPath("~/QuickLinks.xml"));

             var items = from item in xmldoc.Descendants("QuickLink")
                         where item.Element("LinkOrder").Value == id.ToString()
                         select item;

             bool isAvailable = true;

             foreach (XElement itemElement in items)
             {
                 isAvailable = false;
                 break;
             }

             return Json(new { success = isAvailable });
         }


        /// <summary>
        /// Get the list of campaign
        /// </summary>
        /// <param name="sidx"></param>
        /// <param name="sord"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public JsonResult LinkManagerGrid(string sidx, string sord, Pager objPage, int rows)
        {
            var jsonData = new Object();

            objPage.PageSize = rows;
            // object for the pagging

            var offset = (objPage.Page - 1) * objPage.PageSize;

            var pageSize = objPage.PageSize;

            var page = objPage.Page;

            //***** Reading data from the Mettings.xml *****
            XmlDataDocument xmlDox = new XmlDataDocument();
            xmlDox.DataSet.ReadXmlSchema(Server.MapPath("~/QuickLinks.xml"));
            xmlDox.Load(Server.MapPath("~/QuickLinks.xml"));

            // store in the application cache
            var meetingDataTable = xmlDox.DataSet.Tables["QuickLink"];

            var listCalendarModel = new List<AddLinkViewModel>();

            for (int i = 1; i < meetingDataTable.Rows.Count; i++)
            {
                listCalendarModel.Add(new AddLinkViewModel
                {
                    LinkOrder = meetingDataTable.Rows[i]["LinkOrder"].ToString(),
                    LinkTitle = meetingDataTable.Rows[i]["LinkTitle"].ToString(),
                    LinkAddress = meetingDataTable.Rows[i]["LinkAddress"].ToString(),
                });
            }

            int pageIndex = Convert.ToInt32(page) - 1;


            var listTest = listCalendarModel;
            //listTest.OrderB

            var totalpages = objPage.ToatalPages();

            jsonData = new
            {
                total = totalpages,
                page,
                records = objPage.TotalRecords,

                rows = (
                        from linkOrderList in listCalendarModel
                        select new
                        {
                          cell = new string[]
                            {
                                linkOrderList.LinkOrder,
                                linkOrderList.LinkTitle,
                                linkOrderList.LinkAddress,
                                                                
                            }
                        }).ToArray()
            };


            return Json(jsonData);
        }

        /// <summary>
        /// POST Edit button in LinkManagerAdmin view
        ///  </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult EditLinkOrderXml(FormCollection form, AddLinkViewModel objcal)
        {
            XDocument xmldoc = XDocument.Load(Server.MapPath("~/QuickLinks.xml"));
            //XElement xElement = xmldoc.XPathSelectElement("Meetings/Meeting/id[@id = '" + objcal.CalandarId + "']");

            var items = from item in xmldoc.Descendants("QuickLink")
                        where item.Element("LinkOrder").Value == objcal.LinkOrder
                        select item;

            foreach (XElement itemElement in items)
            {
                itemElement.SetElementValue("LinkOrder", objcal.LinkOrder);
                itemElement.SetElementValue("LinkTitle", objcal.LinkTitle);
                itemElement.SetElementValue("LinkAddress", objcal.LinkAddress);
            }

            xmldoc.Save(Server.MapPath("~/QuickLinks.xml"));

            return View("LinkManagerAdmin");
        }

        /// <summary>
        /// POST Delete button in ADDnewcalendarEvent view inside Grid
        ///  </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult DeleteLinkOrder()
        {
            var id = Convert.ToInt64(Request["id"] ?? "0");

            XDocument xmldoc = XDocument.Load(Server.MapPath("~/QuickLinks.xml"));

            var items = from item in xmldoc.Descendants("QuickLink")
                        where item.Element("LinkOrder").Value == id.ToString()
                        select item;

            foreach (XElement itemElement in items)
            {
                itemElement.Remove();
                break;
            }

            xmldoc.Save(Server.MapPath("~/QuickLinks.xml"));

            return View("LinkManagerAdmin");
        }


       

        #endregion

        #region "Policy Management"

        /// <summary>
        /// Get /Admin/PolicyMang
        /// </summary>
        /// <returns></returns>
        public ActionResult PolicyMang()
        {
            var objChkoutStream = new StreamReader(Server.MapPath("~/ContentFiles/checkoutpolicy.txt"));

            ViewData["Policy"] = Convert.ToString(objChkoutStream.ReadToEnd());

            objChkoutStream.Close();

            return View();
        }

        /// <summary>
        /// To Save policy text....
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonSavePolicyText()
        {
            var policyText = Request["PolicyText"];

            var objStreamReader = new StreamWriter(Server.MapPath("~/ContentFiles/checkoutpolicy.txt"), false);

            objStreamReader.Write(policyText);

            objStreamReader.Close();

            return Json(new { success = true });
        }

        #endregion

        #region "Content Admin"
        /// <summary>
        /// Get ContentAdmin
        /// </summary>
        /// <returns></returns>
        public ActionResult ContentAdmin()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            //Method will return the list of pages
            ViewBag.CurrentHome = objBaseMainConfigAdmin.IwebConfigAdmin.CurrentHomePage;

            var lstPageViewModel = ListOfPages();

            return View(lstPageViewModel);
        }
        /// <summary>
        /// Post ContentAdmin
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult JsonContentAdminAddPage()
        {
            var pageName = Request["PageName"] ?? "";

            var pageDesc = Request["PageDesc"] ?? "";

            var flag = true;

            var errMsg = "";

            //Checking pagename values does not contain  .,&,= and .cws            
            if (!pageName.Contains("&") && !pageName.Contains(".") && pageName != "" && !pageName.Contains("=") && !pageName.Contains(".cws"))
            {
                //Checkiing if not already exist
                if (!System.IO.File.Exists(Server.MapPath("~\\CustomPages\\" + pageName + ".cws")))
                {
                    try
                    {
                        var sw2 = System.IO.File.CreateText(Server.MapPath("~\\CustomPages\\" + pageName + ".desc"));

                        sw2.Write(pageDesc);

                        sw2.Close();

                        var sw1 = System.IO.File.CreateText(Server.MapPath("~\\CustomPages\\" + pageName + ".cws"));

                        sw1.Close();


                    }
                    catch (Exception x)
                    {
                        errMsg = x.Message;
                        flag = false;
                    }

                }
                else
                {
                    errMsg = "Form with this name already exist.";
                    flag = false;
                }

            }
            else
            {
                errMsg = "Page name should not be empty and shoud not contain .,&,= and .cws";
                flag = false;
            }

            if (flag)
            {
                //Mehtod will return the list of pages
                var lstPageViewModel = ListOfPages();

                return PartialView("EditorTemplates/Admin/ShowPages", lstPageViewModel);
            }

            return Json(new { success = flag, errMsg = errMsg });

        }

        /// <summary>
        /// Delete the pages
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonDeletePage()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var pageName = Request["PageName"] ?? "";
            //  string isCurrentHomePage = IWEBConfigAdmin.CurrentHomePage.ToString();

            var isCurrentHomePage = objBaseMainConfigAdmin.IwebConfigAdmin.CurrentHomePage; ;

            var flag = true;

            var errMsg = "";

            var filetodel = pageName + ".cws";
            //Checking is deleting page is Home page
            if (isCurrentHomePage.Equals(pageName))
            {
                errMsg = " Can't delete your current homepage!";
                flag = false;
            }
            else
            {
                System.IO.File.Delete(Server.MapPath("~\\CustomPages\\" + pageName + ".cws"));
                System.IO.File.Delete(Server.MapPath("~\\CustomPages\\" + pageName + ".desc"));
                System.IO.File.Delete(Server.MapPath("~\\CustomPages\\" + pageName + ".policy"));

            }
            //Mehtod will return the list of pages
            if (flag)
            {
                var lstPageViewModel = ListOfPages();

                return PartialView("EditorTemplates/Admin/ShowPages", lstPageViewModel);
            }

            return Json(new { success = flag, errMsg = errMsg });

        }
        /// <summary>
        /// Json chk policy
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonChkPolicy()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var pageName = Request["PageName"] ?? "";

            var chkValue = Request["ChkPolicy"] ?? "";

            if (chkValue.ToLower().Equals("on"))
            {
                System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageName + ".cws"), Server.MapPath("~\\CustomPages\\" + pageName + ".policy"), true);
            }
            else
            {
                if (chkValue.ToLower().Equals("off"))
                {
                    System.IO.File.Delete(Server.MapPath("~\\CustomPages\\" + pageName + ".policy"));
                }
            }
            //Mehtod will return the list of pages
            var lstPageViewModel = ListOfPages();

            return PartialView("EditorTemplates/Admin/ShowPages", lstPageViewModel);

        }
        /// <summary>
        /// Set the home page blank
        /// </summary>
        /// <returns></returns>
        public ActionResult SetHomePageBlank()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            objBaseMainConfigAdmin.IwebConfigAdmin.CurrentHomePage = "BLANK";

            objBaseMainConfigAdmin.SaveValues();

            var myStream = System.IO.File.Create(Server.MapPath("~\\contentfiles\\frontpage.txt"));

            myStream.Close();

            //Mehtod will return the list of pages
            var lstPageViewModel = ListOfPages();

            return PartialView("EditorTemplates/Admin/ShowPages", lstPageViewModel);
        }

        public ActionResult JsonEditPage()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var pageName = Request["PageName"] ?? "";

            var pageViewModel = new PageViewModel();

            Session["EDITCUSTOMPAGE"] = pageName;

            var sr = new StreamReader(Server.MapPath("~\\CustomPages\\" + pageName + ".cws"));

            var pageContent = sr.ReadToEnd();

            sr.Close();

            pageViewModel.PageName = pageName;

            var sr1 = new StreamReader(Server.MapPath("~\\CustomPages\\" + pageName + ".desc"));

            //find out if a backup exists for this file, if so, show the restore/view backup buttons
            if (System.IO.File.Exists(Server.MapPath("~\\CustomPages\\" + pageName + ".backup")))
            {
                pageViewModel.IsBackup = true;

            }

            pageViewModel.PageDesc = sr1.ReadToEnd();

            pageViewModel.PageContent = pageContent;

            sr1.Close();

            return PartialView("EditorTemplates/Admin/EditPages", pageViewModel);
        }
        /// <summary>
        /// Copy the same form to different form
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonCopyOrRenamePage()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var pageName = Request["PageName"] ?? "";

            var newPageName = Request["NewPageName"] ?? "";

            var operation = Request["Operation"] ?? "";

            var errMsg = "";

            var success = true;

            if (!string.IsNullOrEmpty(newPageName) && !string.IsNullOrEmpty(pageName))
            {
                //find out if tb is blank first!
                //copy orig (.cws, .desc, .backup, .descbackup) to new filename, keep orig
                if (operation.ToLower().Equals("copy"))
                {

                    if (!System.IO.File.Exists(Server.MapPath("~\\CustomPages\\" + newPageName + ".cws")))
                    {
                        //go ahead and copy with overwrite
                        System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageName + ".cws"),
                                            Server.MapPath("~\\CustomPages\\" + newPageName + ".cws"), true);
                        System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageName + ".desc"),
                                            Server.MapPath("~\\CustomPages\\" + newPageName + ".desc"), true);
                        System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageName + ".backup"),
                                            Server.MapPath("~\\CustomPages\\" + newPageName + ".backup"), true);
                        System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageName + ".descbackup"),
                                            Server.MapPath("~\\CustomPages\\" + newPageName + ".descbackup"), true);
                    }
                    else
                    {
                        errMsg = "The file you are trying to copy to already exisits, please choose another file name";
                        success = false;
                    }
                }
                else
                {
                    //copy orig (.cws, .desc, .backup, .descbackup) to new filename, delete all orig
                    if (!System.IO.File.Exists(Server.MapPath("~\\CustomPages\\" + newPageName + ".cws")))
                    {
                        //go ahead and copy with overwrite
                        System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageName + ".cws"),
                                            Server.MapPath("~\\CustomPages\\" + newPageName + ".cws"), true);
                        System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageName + ".desc"),
                                            Server.MapPath("~\\CustomPages\\" + newPageName + ".desc"), true);
                        System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageName + ".backup"),
                                            Server.MapPath("~\\CustomPages\\" + newPageName + ".backup"), true);
                        System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageName + ".descbackup"),
                                            Server.MapPath("~\\CustomPages\\" + newPageName + ".descbackup"), true);
                        //delete originals
                        System.IO.File.Delete(Server.MapPath("~\\CustomPages\\" + pageName + ".descbackup"));
                        System.IO.File.Delete(Server.MapPath("~\\CustomPages\\" + pageName + ".desc"));
                        System.IO.File.Delete(Server.MapPath("~\\CustomPages\\" + pageName + ".cws"));
                        System.IO.File.Delete(Server.MapPath("~\\CustomPages\\" + pageName + ".backup"));
                    }
                    else
                    {
                        errMsg = "The file you are trying to copy to already exisits, please choose another file name";
                        success = false;
                    }
                    //Mehtod will return the list of pages

                }

            }
            else
            {
                errMsg = "You must define a file name to copy this page to!";
                success = false;
            }
            if (success)
            {
                var lstPageViewModel = ListOfPages();

                return PartialView("EditorTemplates/Admin/ShowPages", lstPageViewModel);
            }

            return Json(new { success = success, errMsg = errMsg });

        }

        /// <summary>
        /// Save the content of th pages
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult JsonSavePageContents(PageViewModel model)
        {
            var pageContent = string.IsNullOrEmpty(model.PageContent) ? "" : model.PageContent;

            var pageName = Request["PageName"] ?? "";

            var pageDesc = Request["PageDesc"] ?? "";

            if (!string.IsNullOrEmpty(pageName))
            {
                var policy = false;
                var frontpage = false;

                //  string pagetoupdate = Session["EDITCUSTOMPAGE"].ToString();
                var pagetoupdate = pageName;
                //is this a policy?

                if (System.IO.File.Exists(Server.MapPath("~\\CustomPages\\" + pagetoupdate + ".policy")))
                    policy = true;

                //is this a front page?

                var homePage = objBaseMainConfigAdmin.IwebConfigAdmin.CurrentHomePage;

                // var sHomePage = "my first page";

                if (pagetoupdate == homePage)
                    frontpage = true;

                System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pagetoupdate + ".cws"),
                                    Server.MapPath("~\\CustomPages\\" + pagetoupdate + ".backup"), true);

                System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pagetoupdate + ".desc"),
                                    Server.MapPath("~\\CustomPages\\" + pagetoupdate + ".descbackup"), true);

                //now lets write the new data to the file
                var tw = new StreamWriter(Server.MapPath("~\\CustomPages\\" + pagetoupdate + ".desc"));

                tw.Write(pageDesc);

                tw.Close();

                var tr = new StreamWriter(Server.MapPath("~\\CustomPages\\" + pagetoupdate + ".cws"), false);

                tr.Write(pageContent);

                tr.Close();

                if (policy) //create copy for the policy to update it

                    System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pagetoupdate + ".cws"),
                                        Server.MapPath("~\\CustomPages\\" + pagetoupdate + ".policy"), true);

                if (frontpage) //create copy for the homepage to update it
                    System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pagetoupdate + ".cws"),
                                        Server.MapPath("~\\contentfiles\\frontpage.txt"), true);

                var lstPageViewModel = ListOfPages();

                return PartialView("EditorTemplates/Admin/ShowPages", lstPageViewModel);
            }

            return Json(new { success = false, errMsg = "Page content can't be saved" });


        }
        /// <summary>
        /// ViewBackup page
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonViewBackUp()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var lstPageViewModel = ListOfPages();

            return PartialView("EditorTemplates/Admin/ShowPages", lstPageViewModel);
        }
        /// <summary>
        /// Restore the backup page
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonRestoreBackUp()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var pageToEdit = Request["PageName"] ?? "";

            var pageViewModel = new PageViewModel();

            if (!string.IsNullOrEmpty(pageToEdit))
            {


                System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageToEdit + ".cws"),
                                    Server.MapPath("~\\CustomPages\\" + pageToEdit + ".temp"), true);
                System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageToEdit + ".desc"),
                                    Server.MapPath("~\\CustomPages\\" + pageToEdit + ".desctemp"), true);
                System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageToEdit + ".backup"),
                                    Server.MapPath("~\\CustomPages\\" + pageToEdit + ".cws"), true);
                System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageToEdit + ".descbackup"),
                                    Server.MapPath("~\\CustomPages\\" + pageToEdit + ".desc"), true);
                System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageToEdit + ".temp"),
                                    Server.MapPath("~\\CustomPages\\" + pageToEdit + ".backup"), true);
                System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageToEdit + ".desctemp"),
                                    Server.MapPath("~\\CustomPages\\" + pageToEdit + ".descbackup"), true);
                System.IO.File.Delete(Server.MapPath("~\\CustomPages\\" + pageToEdit + ".desctemp"));
                System.IO.File.Delete(Server.MapPath("~\\CustomPages\\" + pageToEdit + ".temp"));
                System.IO.TextReader sr =
                    new System.IO.StreamReader(Server.MapPath("~\\CustomPages\\" + pageToEdit + ".cws"));

                pageViewModel.PageContent = sr.ReadToEnd();

                sr.Close();

                var sr1 = new StreamReader(Server.MapPath("~\\CustomPages\\" + pageToEdit + ".desc"));

                var pageDesc = sr1.ReadToEnd().ToString();

                sr1.Close();

                pageViewModel.PageDesc = pageDesc;
            }
            else
            {
                return Json(new { success = false, errMsg = "There is no backup for this page or page does not exist" });
            }
            //if this does not work then you can return partial edit
            return PartialView("EditorTemplates/Admin/RichEditorControl", pageViewModel.PageContent);
        }
        /// <summary>
        /// Reload the page 
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonCancelEdit()
        {
            var lstPageViewModel = ListOfPages();

            return PartialView("EditorTemplates/Admin/ShowPages", lstPageViewModel);
        }
        /// <summary>
        /// Change the Front Page
        /// </summary>
        /// <returns></returns>
        public ActionResult JsonChangeFrontPage()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var pageName = Request["PageName"] ?? "";
            var errMsg = "";
            try
            {
                if (!string.IsNullOrEmpty(pageName))
                {

                    objBaseMainConfigAdmin.IwebConfigAdmin.CurrentHomePage = pageName;


                    objBaseMainConfigAdmin.SaveValues();

                    System.IO.File.Copy(Server.MapPath("~\\CustomPages\\" + pageName + ".cws"),
                                        Server.MapPath("~\\contentfiles\\frontpage.txt"), true);

                    var lstPageViewModel = ListOfPages();

                    return PartialView("EditorTemplates/Admin/ShowPages", lstPageViewModel);
                }
                errMsg = "Page does not exist.";

            }
            catch (Exception x)
            {
                errMsg = x.Message;
            }
            return Json(new { success = false, errMsg = errMsg });

        }

        #region "Private Methods"
        /// <summary>
        /// Return the list of pages
        /// </summary>
        /// <returns></returns>
        IEnumerable<PageViewModel> ListOfPages()
        {
            var dirinfo = new DirectoryInfo(Server.MapPath("~\\CustomPages"));

            ViewBag.CurrentHome = objBaseMainConfigAdmin.IwebConfigAdmin.CurrentHomePage;
            // List all files in LISTDIR
            FileInfo[] files = dirinfo.GetFiles("*.cws");

            var lstPageViewModel = new List<PageViewModel>();

            var homePageName = objBaseMainConfigAdmin.IwebConfigAdmin.CurrentHomePage.ToLower();

            //Filling the model
            foreach (var file in files)
            {
                var pageName = file.Name.Replace(".cws", "");

                var isSeleted = "Select";

                //Checking is the page is front page
                if (pageName.ToLower().Equals(homePageName))
                {
                    isSeleted = "Selected!";
                }

                var filename = file.Name.Replace(".cws", "") + ".policy";

                //Checking is there any polict file with the pagename 
                var isPolicy = System.IO.File.Exists(Server.MapPath("~\\CustomPages\\" + filename));

                var sr1 = new StreamReader(Server.MapPath("~\\CustomPages\\" + file.Name.Replace(".cws", "") + ".desc"));

                var desc = sr1.ReadToEnd();

                sr1.Close();

                lstPageViewModel.Add(new PageViewModel { PageName = pageName, PageDesc = desc, ChkPolicy = isPolicy, IsSelected = isSeleted });

            }
            return lstPageViewModel;
        }

        #endregion


        #region "Get Page"
        /// <summary>
        /// Get the GetPage
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPage()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            return View();
        }

        #endregion

        #endregion

        #region "buyBackWizard"
        public ActionResult BuyBackWizard()
        {
            if (!HasAdminPrivilege())
            {
                return RedirectToAction("Logon", "Account", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            var mainConfigModel=new BaseMainConfigModel();
            TextReader sr = new StreamReader(Server.MapPath("~/contentfiles/SellBackDisclaimer.txt"));
           
            mainConfigModel.Disclaimer = sr.ReadToEnd();
            sr.Close();
            mainConfigModel.DisplayPrices = objBaseMainConfigAdmin.IwebBuyBackConfig.DisplayPrices;
            mainConfigModel.RequireLogin = objBaseMainConfigAdmin.IwebBuyBackConfig.RequireLogin;
            mainConfigModel.RetailBooks = objBaseMainConfigAdmin.IwebBuyBackConfig.RetailBooks;
            mainConfigModel.ShowBuyBackNeed = objBaseMainConfigAdmin.IwebBuyBackConfig.ShowBuyBackNeed;
            mainConfigModel.StoreCredit = objBaseMainConfigAdmin.IwebBuyBackConfig.StoreCredit;
            mainConfigModel.VoucherExpire = objBaseMainConfigAdmin.IwebBuyBackConfig.VoucherExpire.ToString(CultureInfo.InvariantCulture);
            mainConfigModel.Vouchers = objBaseMainConfigAdmin.IwebBuyBackConfig.Vouchers;
            mainConfigModel.WholeSaleBooks = objBaseMainConfigAdmin.IwebBuyBackConfig.WholesaleBooks;
           object myVar = new { STOREID = StoreNumber };
           //SellBackSerivces.GetSellBackParams(StoreNumber, myVar, UvUsername, UvPassword, DbType, UvAddress, UvAccount,
           //                                   CacheTime, CacheTime, Strd3PortNumber, UseEncryption, Strd3PortNumber);
            return View(mainConfigModel);
        }
        [HttpPost]
        public ActionResult BuyBackWizard(BaseMainConfigModel postModel)
        {
            var mainConfigModel = new BaseMainConfigModel();
            objBaseMainConfigAdmin.IwebBuyBackConfig.DisplayPrices = postModel.DisplayPrices;
            objBaseMainConfigAdmin.IwebBuyBackConfig.RetailBooks = postModel.RetailBooks;
            objBaseMainConfigAdmin.IwebBuyBackConfig.WholesaleBooks = postModel.WholeSaleBooks;
            objBaseMainConfigAdmin.IwebBuyBackConfig.ShowBuyBackNeed = postModel.ShowBuyBackNeed;
            objBaseMainConfigAdmin.IwebBuyBackConfig.StoreCredit = postModel.StoreCredit;
            objBaseMainConfigAdmin.IwebBuyBackConfig.Vouchers = postModel.Vouchers;
            objBaseMainConfigAdmin.IwebBuyBackConfig.VoucherExpire = Convert.ToInt32(postModel.VoucherExpire);
            objBaseMainConfigAdmin.IwebBuyBackConfig.RequireLogin = postModel.RequireLogin;

            TextWriter tr = new StreamWriter(Server.MapPath("~/contentfiles/sellbackdisclaimer.txt"), false);
            tr.Write(postModel.Disclaimer);
            tr.Close();
            SellBackSerivces.SetSellBackParams(StoreNumber, postModel.StoreCreditPercent??"",
                                               postModel.RetailPercent??"",
                                               postModel.RetailRounding,
                                               postModel.RetailCoin,
                                               postModel.WholeSalePercent??"",
                                               postModel.WholeSaleRounding,
                                               postModel.WholeSaleCoin, UvUsername, UvPassword, DbType,
                                               UvAddress,UvAccount,
                                               CacheTime, CacheTime, Strd3PortNumber, UseEncryption, Strd3PortNumber);

            objBaseMainConfigAdmin.SaveValues();
            //write to pick the other values for percents

            return RedirectToAction("Index", "Admin");
        }

#endregion

        //
        // GET: /Admin/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Delete/5

        public ActionResult Delete(int id)
        {
          
            return View();
        }

        //
        // POST: /Admin/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


       

    }

    public class ViewDataUploadFilesResult
    {
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string delete_url { get; set; }
        public string thumbnail_url { get; set; }
        public string delete_type { get; set; }
    }
}
