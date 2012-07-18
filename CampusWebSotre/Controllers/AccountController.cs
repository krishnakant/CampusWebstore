using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using CampusWebStore.Models;
using CampusWebStore.Utils;
using CampusWebStore.Business.Services;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;
using CampusWebStore.Data.Daos;

namespace CampusWebStore.Controllers
{
    [Authorize]
    public class AccountController : WebController
    {

        #region "Properties"

        [Dependency]
        public IAccountSettingsService AccountSettingsService { get; set; }

        [Dependency]
        public IOrderService OrderService { get; set; }


        #endregion

        //
        // GET: /Account/LogOn

        [AllowAnonymous]
        public ActionResult LogOn()
        {
            if (Session["USERINFO"] != null)
            {
                return RedirectToAction("MyAccount");
            }

            return ContextDependentView();
        }

        //
        // POST: /Account/JsonLogOn
        [AllowAnonymous]
        [HttpPost]
        public JsonResult JsonLogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return Json(new { success = true, redirect = returnUrl });
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        //
        // POST: /Account/LogOn

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string ReturnUrl)
        {
            if (string.IsNullOrEmpty(ReturnUrl))
            {
                ReturnUrl = Request.QueryString["ReturnUrl"] ?? string.Empty;
            }

            if (ModelState.IsValid)
            {
                //Check user valid and do login..
                var objUserModel = AccountSettingsService.ValidUser(StoreNumber, model.UserName,
                                                                    model.Password, UvUsername, UvPassword,
                                                                    DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                                    Strd3PortNumber, UseEncryption, Strd3PortNumber);


                if (!string.IsNullOrEmpty(objUserModel.ErrorMsg))
                {
                    ModelState.AddModelError("", objUserModel.ErrorMsg);
                }
                else
                {
                    //Set Session to place user information...
                    Session["USERINFO"] = objUserModel;

                    Session["ACCTYPE"] = objUserModel.Type;

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                  
                    return RedirectToAction("MyAccount");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// GET: /Account/Logout
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult LogOut()
        {
            Session["USERINFO"] = null;

            Session["CartItem"] = null;

            Session["ACCTYPE"] = null;

            //Master.HideLogout();
            //Master.DisplayUser();
            //Master.CalcCartTotal();
            Session.Clear();

            return View();
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// GET: /Account/MyAccount
        /// Dashboard of Logged in user..
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult MyAccount()
        {
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn");
            }

            return ContextDependentView(); 
        }

        /// <summary>
        /// Get : /Account/AccountDetails
        /// Show user details
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult AccountDetails()
        {
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn");
            }

            var objUserModel = (UserModel) Session["USERINFO"];

            var objAccountSettingModel = new AccountSettingsViewModel
                                             {
                                                 UserName = objUserModel.UserName,
                                                 FirstName = objUserModel.FirstName,
                                                 LastName = objUserModel.LastName,
                                                 Address = objUserModel.Address,
                                                 Address2 = objUserModel.Address2,
                                                 City = objUserModel.City,
                                                 State = objUserModel.State,
                                                 Zip = objUserModel.Zip,
                                                 Country = objUserModel.Country,
                                                 Email = objUserModel.Email,
                                                 DayPhone = objUserModel.DayPhone,
                                                 Evening = objUserModel.Evening,
                                                 ShipToName = objUserModel.ShipToName,
                                                 ShipAddress = objUserModel.ShipAddress,
                                                 ShipAddress2 = objUserModel.ShipAddress2,
                                                 ShipCity = objUserModel.ShipCity,
                                                 ShipState = objUserModel.ShipState,
                                                 ShipZip = objUserModel.ShipZip,
                                                 ShipCountry = objUserModel.ShipCountry,
                                                 ShipInstruc = objUserModel.ShipInstruc,
                                                 IsEmailOptIn = objUserModel.EmailOptIn == "Y"
                                             };


            return View(objAccountSettingModel); 
        }

        /// <summary>
        /// Update User details..
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AccountDetails(AccountSettingsViewModel model)
        {
              var emailoptin =model.IsEmailOptIn?"Y":"N";
            //They are not used in this context but don't remove them they are do make model valid

            ModelState.Remove("Password");
            model.Password = "1234567";
            model.Password = "1234567";
            model.ConfirmPassword = "1234567";
            if (ModelState.IsValid)
             {
                 //Update the user details
                 var objUserModel = AccountSettingsService.UpdateUser(StoreNumber, model.UserName,
                                                                      model.FirstName, model.LastName, model.Address,
                                                                      model.Address2,
                                                                      model.City, model.State, model.Zip, model.Country,
                                                                      model.Email, model.DayPhone, model.Evening??"",
                                                                      model.ShipToName,
                                                                      model.ShipAddress, model.ShipAddress2,
                                                                      model.ShipCity,
                                                                      model.ShipState, model.ShipZip, model.ShipCountry,
                                                                      model.ShipInstruc, emailoptin,"", UvUsername,
                                                                      UvPassword, DbType, UvAddress, UvAccount,
                                                                      CacheTime, CacheTime,
                                                                      Strd3PortNumber, UseEncryption, Strd3PortNumber);

                 if (!string.IsNullOrEmpty(objUserModel.ErrorMsg))
                 {
                     ModelState.AddModelError("", objUserModel.ErrorMsg);
                 }
                 else
                 {
                     Session["USERINFO"] = objUserModel;

                     return RedirectToAction("MyAccount");
                 }
             }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

      
        /// <summary>
        /// To Change user password
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult JsonChangePassword()
        {
            var username = Convert.ToString(Request["UserName"]??"");

            var oldPassword = Convert.ToString(Request["OldPassword"]??"");

            var newPassword = Convert.ToString(Request["NewPassword"]??"");
            if (!string.IsNullOrEmpty(username)&&!string.IsNullOrEmpty(oldPassword)&&!string.IsNullOrEmpty(newPassword))
            {
                var isChanged = AccountSettingsService.ChangePassword(StoreNumber, username, oldPassword, newPassword,
                                                                      UvUsername,
                                                                      UvPassword, DbType, UvAddress, UvAccount,
                                                                      CacheTime, CacheTime,
                                                                      Strd3PortNumber, UseEncryption, Strd3PortNumber);
                return Json(new { success = isChanged });
            }

            return Json(new {success = false});
        }

        /// <summary>
        /// Get : /Account/OrderHistory
        /// Show user details
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult OrderHistory()
        {
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn");
            }

          // var userid = "MAX";  /*((UserModel) Session["USERINFO"]).UserName;*/

          var userid =((UserModel) Session["USERINFO"]).UserName;

            ViewBag.UserId = userid;

            var orderModels = OrderService.GetOrderHistory(StoreNumber, userid, UvUsername,
                                                           UvPassword, DbType, UvAddress, UvAccount, CacheTime,
                                                           CacheTime, Strd3PortNumber, UseEncryption, Strd3PortNumber);

            var orderViewModels = (from order in orderModels
                                   select new OrderViewModel
                                              {
                                                  OrderId = order.OrderId,
                                                  UserId = order.UserId,
                                                  ConfNumber = order.ConfNumber,
                                                  DatePlaced = order.DatePlaced,
                                                  DateProcessed = order.DateProcessed,
                                                  Status = order.Status,
                                                  Amount = order.Amount
                                              });

            return View(orderViewModels);
        }


        /// <summary>
        /// Get the customer detail
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult JsonGetOrderDetail()
        {
            var orderId = Request["OrderId"];

            var customerId = Request["CustomerId"];

            //Get the order detail for the customer
            var orderDetail = OrderService.GetOrderDetail(StoreNumber, customerId, orderId, UvUsername,
                                                          UvPassword, DbType, UvAddress, UvAccount, CacheTime,
                                                          CacheTime, Strd3PortNumber, UseEncryption, Strd3PortNumber);

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

        /// <summary>
        /// GET: /Account/ForgotPassword
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {


            return View();
        }

        /// <summary>
        /// To send reset password to user... 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult JsonForgotPassword()
        {
            var username = Request["UserName"];

            var newpassword = PasswordGeneratorUtil.Generate();


            var email = AccountSettingsService.ResetPassword(StoreNumber, username,newpassword, UvUsername,
                                                                   UvPassword, DbType, UvAddress, UvAccount, CacheTime,
                                                                   CacheTime, Strd3PortNumber, UseEncryption,
                                                                   Strd3PortNumber);

            if (email.Contains("@"))
            {
                var msg = string.Empty;

                msg += "Your password has been reset. Your new password is: <br /><br />" + newpassword + "<br /><br />";

                msg += "Please visit the website and login with this new password. You can then change your password after you log in.";
                
                var objEmaulUtil = new EmailUtil();

                if (!objEmaulUtil.Send(email, "Password Information", msg))
                {
                    return Json(new { success = false, errorMsg = "Password has been reset but message could not be sent." });
                }
            }
            else
            {
                return Json(new {success = false, errorMsg = email});
            }

            return Json(new {success = true});
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return ContextDependentView();
        }

        //
        // POST: /Account/JsonRegister

        [AllowAnonymous]
        [HttpPost]
        public ActionResult JsonRegister(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, createPersistentCookie: false);
                    return Json(new { success = true });
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        //
        // POST: /Account/Register

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, createPersistentCookie: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// Get: /Account/CreateNewUser
        /// To create new user.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult CreateNewUser()
        {
            return View();
        }

        /// <summary>
        /// Save new user details....
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CreateNewUser(AccountSettingsViewModel model)
        {
            var emailoptin = Convert.ToBoolean(model.IsEmailOptIn) ? "Y" : "N";

            if (ModelState.IsValid)
            {
                var objUserModel = AccountSettingsService.AddUser(
                    StoreNumber ?? "",
                    model.UserName ?? "",
                    model.Password ?? "",
                    model.FirstName ?? "",
                    model.LastName ?? "",
                    model.Address ?? "",
                    model.Address2 ?? "",
                    model.City ?? "",
                    model.State ?? "",
                    model.Zip ?? "",
                    model.Country ?? "",
                    model.Email ?? "",
                    model.DayPhone ?? "",
                    model.Evening ?? "",
                    model.ShipToName ?? "",
                    model.ShipAddress ?? "",
                    model.ShipAddress2 ?? "",
                    model.ShipCity ?? "",
                    model.ShipState ?? "",
                    model.ShipZip ?? "",
                    model.ShipCountry ?? "",
                    model.ShipInstruc ?? "",
                    emailoptin ?? "",
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

                    Session["ACCTYPE"] = objUserModel.Type;

                    return RedirectToAction("MyAccount");
                }

            }


            // If we got this far, something failed, redisplay form
            return View(model);
        }

        /// <summary>
        /// To check username is available or not....
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult JsonCheckUserAvailability()
        {
            var username = Convert.ToString(Request["UserName"]);

            var isAvailable = AccountSettingsService.CheckUser(StoreNumber, username, UvUsername,
                                                               UvPassword, DbType, UvAddress, UvAccount,
                                                               CacheTime, CacheTime, Strd3PortNumber, UseEncryption,
                                                               Strd3PortNumber);

            return Json(new {success = isAvailable});
        }

        //
        // GET: /Account/ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        private ActionResult ContextDependentView()
        {
            string actionName = ControllerContext.RouteData.GetRequiredString("action");
            if (Request.QueryString["content"] != null)
            {
                ViewBag.FormAction = "Json" + actionName;
                return PartialView();
            }
            else
            {
                ViewBag.FormAction = actionName;
                return View();
            }
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors
                .Select(error => error.ErrorMessage));
        }

        #region Checkout
        [AllowAnonymous]
        public ActionResult Checkout()
        {
            // If User is not logged in send to login
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn", new { ReturnUrl = Request.Url.PathAndQuery });
            }

            // Check for uconnactivation
            string uconnActivation = System.Configuration.ConfigurationManager.AppSettings["UCONNACTIVATION"];

            if (uconnActivation == "YES")
            {
                string canContinue = Request.QueryString["checkout"];

                if (canContinue != "1go")
                {
                    return RedirectToAction("UconnActivation", "Account", new { checkout = 1 });
                }

            }

            // Check for items in cart
            if (Session["CARTITEMS"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Check for element payment
            string elementEnabled = System.Configuration.ConfigurationManager.AppSettings["USE_ELEMENT_PS"];

            if (elementEnabled == "YES")
            {
                ViewBag.element = true;
            }

            var model = (IEnumerable<ShoppingCartModel>)Session["CARTITEMS"];

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult JsonSetSubTotal()
        {
            string total = Request["SubTotal"].ToString();
            Session["Total"] = total;

            return Json(new { success = true });
        }

        [AllowAnonymous]
        public ActionResult SecureCheckout()
        {

            // If User is not logged in send to login
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn", new { ReturnUrl = Request.Url.PathAndQuery });
            }

            // Check for uconnactivation
            string uconnActivation = System.Configuration.ConfigurationManager.AppSettings["UCONNACTIVATION"];

            if (uconnActivation == "YES")
            {
                string canContinue = Request.QueryString["checkout"];

                if (canContinue != "1go")
                {
                    return RedirectToAction("UconnActivation", "Account", new { checkout = 1 });
                }

            }
            string url = Request.Url.Host.ToString();

            if (!Request.Url.Port.Equals(null))
            {
                url += ":" + Request.Url.Port.ToString();
            }

            // Check for items in cart
            if (Session["Total"] == null)
            {
                return RedirectToAction("Checkout", "Account");
            }

            // Check for element payment
            string elementEnabled = System.Configuration.ConfigurationManager.AppSettings["USE_ELEMENT_PS"];

            if (elementEnabled == "YES")
            {
                if (Request.HttpMethod != "POST")
                {
                    ElementUtil element = new ElementUtil();
                    string path = element.TransactionPath(url);
                    string total = Session["Total"].ToString();

                    ElementResponseModel elementResponse = element.SetupTransaction(Session["USERINFO"] as UserModel, total, path);
                    Session["valcodebefore"] = elementResponse.ValidationCode;
                    ViewData["tansactionUrl"] = elementResponse.GetTransactionURL();
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        [AllowAnonymous]
        public ActionResult ProcessTransaction()
        {
            // Check for user login status
            if (Session["USERINFO"] == null)
            {
                throw new Exception("Error: Not logged in.");
            }

            // Set variables
            UserModel user = Session["USERINFO"] as UserModel;
            string paymentAccountId = Request.QueryString["PaymentAccountID"].ToString();
            string lastFour = Request.QueryString["LastFour"].ToString();
            string validationCode = Request.QueryString["ValidationCode"].ToString();
            string validcodes = System.Configuration.ConfigurationManager.AppSettings["elementps_acceptable_avscodes"] as string; // TODO: Make function for extracting web.config data

            // Do AVS check through element
            ElementUtil element = new ElementUtil();
            string avsRespCode = element.PerformAVSCheck(user, paymentAccountId);

            // Verify response from element
            if ((Request.QueryString["HostedPaymentStatus"].ToString() == "Complete") && (validcodes.IndexOf(avsRespCode) >= 0))
            {
                ViewData["ElementResponse"] = "Your order has been received. You will not be charged until the order has been processed.";

                // TODO: Make this look better
                // Process order through IWEB.PAYMENT
                ProcessElementOrder(
                    System.Configuration.ConfigurationManager.AppSettings["STOREID"] as string, // Store id
                    Session["CONFNUMBER"].ToString(), // Conf number
                    "", // Gift card numbers
                    user.UserName, // user id
                    paymentAccountId,
                    "", // Card id
                    lastFour, // last four
                    "", // exp Date
                    "", // Promo codes
                    validationCode, // paytype
                    "" // Substitutions
                );

                // TODO: Do this better
                // Destroy cart
                Session["CARTITEMS"] = null;
            }
            else
            {
                ViewData["ElementResponse"] = "There was an error processing this transaction.";
            }

            return View();
        }
        #endregion

        /// <summary>
        /// UconnActivation before Checkout
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult UconnActivation()
        {
            string checkoutFlag = "";
            checkoutFlag = Request.QueryString["checkout"];
            Session["UCONNCHECKOUTFLAG"] = checkoutFlag;

            try
            {
                if (!string.IsNullOrEmpty(checkoutFlag))
                {
                    var userModel = (UserModel)Session["USERINFO"];

                    string userID = userModel.UserName;

                    bool isActivated = AccountSettingsService.CheckIsUConnActivated(StoreNumber, userID, UvUsername,
                                                               UvPassword, DbType, UvAddress, UvAccount,
                                                               CacheTime, CacheTime, Strd3PortNumber, UseEncryption,
                                                               Strd3PortNumber);
                    //isActivated = true;
                    if (isActivated)
                    {

                        return RedirectToAction("CheckOut", "Account", new { checkout = "1go" });

                    }
                }
            }
            catch (Exception err)
            {

                return RedirectToAction("CheckOut", "Account", new { checkout = "1go" });

            }

            return View();
        }

        /// <summary>
        /// Get Sellback Checkout
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult SellBackCheckout()
        {
            //Checking for the authentication
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn", new { ReturnUrl = Request.Url.PathAndQuery });
            }
            if (Session["SELLBACKCART"] != null)
            {
                //Get the sell back cart model in to the variable
                var lstSellBackCartItems = (IEnumerable<SellBackCartViewModel>)Session["SELLBACKCART"];

                return View(lstSellBackCartItems);
            }
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Get Print Voucher
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult PrintVoucher()
        {
            try
            {
                //Check for null 
                if (Session["USERINFO"] != null)
                {
                    var printVoucherViewModel = new PrintVoucherViewModel();

                    List<SellBackCartViewModel> lstSellBackCartItems;

                    var userModel = (UserModel)Session["USERINFO"];

                    //Check for sellback book cart model is null
                    if (Session["SELLBACKCART"] != null)
                    {
                        //Get the sell back cart model in to the variable
                        lstSellBackCartItems = (List<SellBackCartViewModel>)Session["SELLBACKCART"];

                        printVoucherViewModel.SellBackModel = lstSellBackCartItems;

                        printVoucherViewModel.UserModel = userModel;

                        @ViewData["VoucherNo"] = "123456789";

                        return View(printVoucherViewModel);
                    }

                    Session["SELLBACKCART"] = null;
                    ViewData["objBaseMainConfigAdmin"] = objBaseMainConfigAdmin;


                }
                return RedirectToAction("LogOn", new { ReturnUrl = Request.Url.PathAndQuery });

            }
            catch (Exception x)
            {

                ModelState.AddModelError("", x.Message);

            }

            return View();
        }

         /// <summary>
        /// Checking UConn Activation using MemberID
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
         public ActionResult JsonCheckUConnActivation()
         {

             var MEMBERID = Request["MemberIDCheck"] ?? "";

             var JOINEMAILLIST = Request["EmailLIST"] ?? "";

             string checkoutFlag = "";

             var isactivated = string.Empty;


             checkoutFlag = Session["UCONNCHECKOUTFLAG"].ToString();

             //MEMBERID = tbMemberID.Text,
             //   JOINEMAILLIST = cbEmailList.Checked.ToString(),
             //   USERID = userID

             try
             {
                 if (!string.IsNullOrEmpty(checkoutFlag))
                 {
                     var userModel = (UserModel)Session["USERINFO"];

                     string userID = userModel.UserName;

                     isactivated = AccountSettingsService.CheckIsUConnActivatedMemberID(StoreNumber, MEMBERID, JOINEMAILLIST, userID, UvUsername,
                                                                UvPassword, DbType, UvAddress, UvAccount,
                                                                CacheTime, CacheTime, Strd3PortNumber, UseEncryption,
                                                                Strd3PortNumber);

                 }
             }
             catch (Exception err)
             {
                 return Json(new { Activated = isactivated });
             }

             return Json(new { Activated = isactivated, CheckOutFlag = checkoutFlag });
            
         }
          [AllowAnonymous]
        [Authorize]
        public ActionResult OrderDone()
        {
            if (Session["USERINFO"] == null)
            {
                return RedirectToAction("LogOn");
            }
            Session["CARTITEMS"] = null;
            Session["CONFNUMBER"] = null;
            string orderId = Session["TMPORDERNUMBER"].ToString();
            var customerId = ((UserModel)Session["USERINFO"]).UserName;
            var orderDetail = OrderService.GetOrderDetail(StoreNumber, customerId, orderId, UvUsername,
                                                        UvPassword, DbType, UvAddress, UvAccount, CacheTime,
                                                        CacheTime, Strd3PortNumber, UseEncryption, Strd3PortNumber);

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

            
            //connectToDB myDB = new connectToDB();
            //object myObject = new { USERID = userID, ORDERID = ordernumber };
            //string strPickDataReturn = myDB.getStringResult(1, "IWEB.ORDER.DETAIL", myObject);

            //XmlDocument doc = new XmlDocument();
            //string test = strPickDataReturn; 
            return View(orderDetailViewModel);
        }

          // TODO: Abstract this out
          public void ProcessElementOrder(
              string storeId,
              string confNumber,
              string giftNums,
              string userId,
              string cardNum,
              string cardId,
              string lastFour,
              string expDate,
              string promoCode,
              string payType,
              string allowSub)
          {
              object transaction = new
              {
                  STOREID = storeId,
                  CONFNUMBER = confNumber,
                  GIFTCARD = giftNums,
                  USERID = userId,
                  CARDNUMBER = cardNum,
                  CARDID = cardId,
                  LASTFOUR = lastFour,
                  EXPDATE = expDate,
                  PROMOCODE = promoCode,
                  PAYTYPE = payType,
                  ALLOWSUBSTITUTION = allowSub
              };

              // This is not the most efficient way to call this ...
              DbAccess db = new DbAccess();
              string orderResult = db.GetStringResult(StoreNumber, "IWEB.PAYMENT", transaction, UvUsername,
                                  UvPassword, DbType, UvAddress, UvAccount, CacheTime,
                                  CacheTime, Strd3PortNumber, UseEncryption, Strd3PortNumber) as string;

              System.Diagnostics.Debug.Write(orderResult);
          }


        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
