//namespace CampusWebStore.Utils
//{
//    using System;
//    using System.Collections.Generic;
//    using System.IO;
//    using System.Linq;
//    using System.Web.Mvc;

//    using Microsoft.Practices.Unity;

   
//    public abstract class WebController : Controller
//    {
//        #region Properties

//        public UserProfile AuthenticatedUser { get; set; }

//        [Dependency]
//        public IUserProfileService UserProfileService { get; set; }

//        #endregion

//        #region Public Methods

//        public static bool IsValidCaptchaValue(string captchaValue)
//        {
//            object expectedHash = System.Web.HttpContext.Current.Session["CaptchaHash"];
//            return captchaValue.ToLower().Equals(expectedHash);
//        }

//        #endregion

//        #region Methods

//        protected JsonResult Api()
//        {
//            return this.Api(null);
//        }

//        protected JsonResult Api(object data)
//        {
//            return this.Api(data, null);
//        }

//        protected JsonResult Api404()
//        {
//            return this.Api(null);
//        }

//        protected override IActionInvoker CreateActionInvoker()
//        {
//            return new UnityActionInvoker();
//        }

//        protected void DoNotCache()
//        {
//            ControllerContext.HttpContext.Response.CacheControl = "no-cache";
//            ControllerContext.HttpContext.Response.AddHeader("Cache-Control", "no-cache");

//            // ControllerContext.HttpContext.Response.Expires = -1;
//        }

//        protected new JsonResult Json(object data)
//        {
//            return Json(data, JsonRequestBehavior.AllowGet);
//        }

//        protected ActionResult JsonFileNotFoundException(string name, string message)
//        {
//            Response.StatusCode = 404;

//            return Json(new List<NameValueModel> { new NameValueModel { Name = name, Value = message } });
//        }

//        protected ActionResult JsonInvalidArgumentException(string name)
//        {
//            Response.StatusCode = 400;

//            string message = "<b>" + name + "</b> is required.";
//            return Json(new List<NameValueModel> { new NameValueModel { Name = name, Value = message } });
//        }

//        protected ActionResult JsonInvalidArgumentException(string name, string message)
//        {
//            Response.StatusCode = 400;

//            return Json(new List<NameValueModel> { new NameValueModel { Name = name, Value = message } });
//        }

//        protected ActionResult JsonNotfound()
//        {
//            Response.StatusCode = 404;
//            return Json("Not Found");
//        }

//        protected JsonResult JsonPermissionDenied()
//        {
//            Response.StatusCode = 401;
//            return Json("Unauthorized");
//        }

//        protected ActionResult JsonPermissionDenied(string message)
//        {
//            if (message == null)
//            {
//                throw new ArgumentNullException("message");
//            }

//            Response.StatusCode = 401;
//            return Json(message);
//        }

//        protected ActionResult JsonSuccess(string name, string message)
//        {
//            Response.StatusCode = 200;

//            return Json(new List<NameValueModel> { new NameValueModel { Name = name, Value = message } });
//        }

//        protected override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            base.OnActionExecuting(filterContext);

//            if (User.Identity.IsAuthenticated)
//            {
//                AuthenticatedUser = UserProfileService.GetUser(new GuidId<UserProfile>(this.User.Identity.Name));
//                ViewData["User"] = AuthenticatedUser;
//            }

//            ViewData["TitlePageName"] = Constants.WebsiteName;
//        }

//        protected override void OnException(ExceptionContext filterContext)
//        {
//            base.OnException(filterContext);

//            if (filterContext.Exception.GetType().Equals(typeof(FileNotFoundException)))
//            {
//                // TODO handle error
//                View("FileNotFound").ExecuteResult(ControllerContext);
//                return;
//            }

//            // Output a nice error page
//            if (filterContext.HttpContext.IsCustomErrorEnabled)
//            {
//                filterContext.ExceptionHandled = true;
//                View("Error").ExecuteResult(ControllerContext);
//            }
//        }

//        protected void ReturnFile(string filepath, string mimeType)
//        {
//            if (filepath == null)
//            {
//                throw new ArgumentNullException("filepath");
//            }

//            if (mimeType == null)
//            {
//                throw new ArgumentNullException("mimeType");
//            }

//            ControllerContext.HttpContext.Response.Clear();
//            ControllerContext.HttpContext.Response.ContentType = mimeType;
//            ControllerContext.HttpContext.Response.WriteFile(filepath);
//            ControllerContext.HttpContext.Response.End();
//        }

//        protected void SetPageName(string pageName)
//        {
//            if (pageName == null)
//            {
//                throw new ArgumentNullException("pageName");
//            }

//            ViewData["PageName"] = pageName;
//            ViewData["TitlePageName"] = Constants.WebsiteName + " | " + pageName;
//        }

//        protected void SetSideTab(string tabName)
//        {
//            if (tabName == null)
//            {
//                throw new ArgumentNullException("tabName");
//            }

//            ViewData["SideTab"] = tabName;
//        }

//        protected void SetTopTab(string tabName)
//        {
//            if (tabName == null)
//            {
//                throw new ArgumentNullException("tabName");
//            }

//            ViewData["TopTab"] = tabName;
//        }

//        private JsonResult Api(object data, int? code)
//        {
//            JsonResultModel jsonWrapper = new JsonResultModel();
//            var errors = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors[0].ErrorMessage }).ToArray();
//            if (errors.Any())
//            {
//                jsonWrapper.Errors = errors;
//                jsonWrapper.Status = ApiConstants.BadRequest;
//            }
//            else
//            {
//                jsonWrapper.Status = ApiConstants.Successful;
//            }

//            jsonWrapper.Data = data;
//            if (code != null)
//            {
//                jsonWrapper.Status = (int)code;
//            }

//            return Json(jsonWrapper, JsonRequestBehavior.AllowGet);
//        }

//        #endregion
//    }

//    public class AjaxFileNotFoundException : FileNotFoundException
//    {
//        public AjaxFileNotFoundException()
//            : base()
//        {
//        }
//    }
//    public class AjaxArgumentNullException : ArgumentNullException
//    {
//        public AjaxArgumentNullException(string id): base(id)
//        {
//        }
//    }
//}