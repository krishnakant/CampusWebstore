using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CampusWebStore.Controllers;
using CampusWebStore.Business.Services;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;

namespace CampusWebStore.Helpers
{
    public interface ICatalogsHelper
    {

        #region Public Methods

        IEnumerable<CatalogModel> GetCatalogList();
       
        #endregion
    }

    public class CatalogsHelper :WebController,ICatalogsHelper
    {


        #region Properties

        [Dependency]
        public ICatalogsServices CatalogsServices { get; set; }

        public string UseCache;

        public string UvUsername;

        public string UvPassword;

        public string DbType;

        public string UvAddress;

        public string UvAccount;

        public string CacheTime;

        public string Strd3PortNumber;

        public string UseEncryption;

        public int D3PortNumber;

        //  public static string DblcacheTime =(System.Configuration.ConfigurationManager.AppSettings["USECACHE"]);
        public CatalogsHelper()
        {
            UseCache = System.Configuration.ConfigurationManager.AppSettings["USECACHE"];

            UvUsername = System.Configuration.ConfigurationManager.AppSettings["UV_USERNAME"] as string;

            UvPassword = System.Configuration.ConfigurationManager.AppSettings["UV_PASSWORD"] as string;

            DbType = System.Configuration.ConfigurationManager.AppSettings["DBTYPE"] as string;

            UvAddress = System.Configuration.ConfigurationManager.AppSettings["DB_ADDRESS"] as string;

            UvAccount = System.Configuration.ConfigurationManager.AppSettings["DB_ACCOUNT"] as string;

            CacheTime = System.Configuration.ConfigurationManager.AppSettings["CACHETIME"] as string;

            Strd3PortNumber = System.Configuration.ConfigurationManager.AppSettings["D3PORT"] as string;

            UseEncryption = System.Configuration.ConfigurationManager.AppSettings["ENCRYPTALL"] as string;

            D3PortNumber = Convert.ToInt32(Strd3PortNumber);
        }

      
        #endregion


        #region Methods

        public IEnumerable<CatalogModel> GetCatalogList()
        {
            try
            {
                object myObject = new {ALL = "1"};

                var lstCatalogsModels = CatalogsServices.GetCatalogsList(StoreNumber, myObject,
                                                                         UvUsername, UvPassword,
                                                                         DbType, UvAddress, UvAccount, CacheTime,
                                                                         CacheTime,
                                                                         Strd3PortNumber, UseEncryption, Strd3PortNumber);
                return lstCatalogsModels;
            }

            catch (Exception x)
            {
                throw;
            }
        }

        #endregion

      
    }
}