using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace CampusWebSotre.Utils
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext reqContext, Type controllerType)
        {
            IController controller;
            if (controllerType == null)
                throw new HttpException(404, String.Format("The controller for path '{0}' could not be found or it does not implement IController.",reqContext.HttpContext.Request.Path));

            if (!typeof (IController).IsAssignableFrom(controllerType))
                throw new ArgumentException(string.Format("Type requested is not a controller: {0}",controllerType.Name),"controllerType");
            try
            {
                controller = MvcUnityContainer.Container.Resolve(controllerType) as IController;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format("Error resolving controller {0}",controllerType.Name), ex);
            }
            return controller;
        }
    }

    public static class MvcUnityContainer
    {
        public static IUnityContainer Container { get; set; }
    }

    public class UnityActionInvoker : ControllerActionInvoker
    {
        protected override ActionExecutedContext InvokeActionMethodWithFilters(ControllerContext controllerContext, IList<IActionFilter> filters, ActionDescriptor actionDescriptor, IDictionary<string, object> parameters)
        {

            foreach (var filter in filters)
            {
                MvcUnityContainer.Container.BuildUp(filter.GetType(), filter);
            }
            return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
        }
    }

}