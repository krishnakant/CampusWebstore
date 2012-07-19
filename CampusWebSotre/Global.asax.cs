﻿using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.WebPages;
using CampusWebSotre.Utils;
using CampusWebStore.Business.Unity;

namespace CampusWebSotre {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
              ConfigureUnity();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DisplayModes.Modes.Insert(0, new DefaultDisplayMode("iPhone")
                                                             {
                                                                 ContextCondition = (context => context.GetOverriddenUserAgent().IndexOf
                                                                                   ("iPhone", StringComparison.OrdinalIgnoreCase) >= 0)
                                                             });

            DisplayModes.Modes.Insert(0, new DefaultDisplayMode("Android")
            {
                ContextCondition = (context => context.GetOverriddenUserAgent().IndexOf
                                  ("Android", StringComparison.OrdinalIgnoreCase) >= 0)
            });
            //DisplayModes.Modes.Insert(0, new DefaultDisplayMode("mobile")
            //{
            //    ContextCondition = (context => context.GetOverriddenUserAgent().IndexOf
            //                      ("mobile", StringComparison.OrdinalIgnoreCase) >= 0)
            //});

        }

        private static void ConfigureUnity()
        {
            // Set container for Controller Factory
            MvcUnityContainer.Container = BusinessUnity.Unity();
                //.RegisterType<IHostUtil, HostUtil>()

                //.RegisterType<IFormsAuthenticationUtil, FormsAuthenticationUtil>()

                //.RegisterType<IRedeezeEntityModels, RedeezeEntityModels>(new InjectionConstructor("RedeezeConnection"))

                // TODO: ask Sonu why this was used? (Sept 11)
                /*populate Publication Model*/
                //.RegisterType<IPublicationService, PublicationService>(new HttpContextLifetimeManager<Publication>())

                /* Object Context */
                // see http://weblogs.asp.net/shijuvarghese/archive/2010/05/07/dependency-injection-in-asp-net-mvc-nerddinner-app-using-unity-2-0.aspx
          

            // Set for Controller Factory
            ControllerBuilder.Current.SetControllerFactory(typeof(UnityControllerFactory));
        }
    }
}