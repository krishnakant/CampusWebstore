using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using CampusWebStore.Business.Services;
using CampusWebStore.Data.Daos;

namespace CampusWebStore.Business.Unity
{
    public static class BusinessUnity
    {
        #region Public Methods

        public static IUnityContainer Unity()
        {
            var container = new UnityContainer()

                .RegisterType<IStoreDaos, StoreDaos>()

                .RegisterType<IStoreServices, StoreService>()

                .RegisterType<IDepartmentServices, DepartmentServices>()

                .RegisterType<IDepartmentDaos, DepartmentDaos>()

                .RegisterType<ICourseServices, CourseServices>()

                .RegisterType<ICourseDaos, CourseDaos>()

                .RegisterType<ISectionServices, SectionServices>()

                .RegisterType<ISectionDaos, SectionDaos>()

                .RegisterType<IDbAccess, DbAccess>()

                .RegisterType<ITermDaos, TermDaos>()

                .RegisterType<ITermServices, TermServices>()

                .RegisterType<IAccountSettingsDao, AccountSettingsDao>()

                .RegisterType<IAccountSettingsService, AccountSettingsService>()

                .RegisterType<ICatalogsServices, CatalogsServices>()

                .RegisterType<ICatalogsDaos, CatalogsDaos>()

                .RegisterType<IShippingService, ShippingService>()

                .RegisterType<IShippingDao, ShippingDao>()

                .RegisterType<ISellBackDaos, SellBackDaos>()

                .RegisterType<ISellBackSerivces, SellbackServices>()

                .RegisterType<ICustomerDaos, CustomerDaos>()

                .RegisterType<ICustomerServices, CustomerServices>()

                .RegisterType<IOrderDaos, OrderDaos>()

                .RegisterType<IOrderService, OrderService>()

                .RegisterType<IAdminSettingsDao, AdminSettingsDao>()

                .RegisterType<IAdminSettingsService, AdminSettingsService>();
                 
            return container;
        }

        #endregion
    }
}
