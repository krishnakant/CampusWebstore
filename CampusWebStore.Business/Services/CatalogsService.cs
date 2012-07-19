using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using AmazonAssociatesAPILinqtoXML;
using CampusWebStore.Data.Daos;
using CampusWebStore.Shared.Models;
using DZoft.dBug.TCS.DL;
using Microsoft.Practices.Unity;
using IWEB;
using IWEB.Connector;
//using TotalCompMobil.Data.Daos;

namespace CampusWebStore.Business.Services
{
    public interface ICatalogsServices
    {
        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Method to get the record for the store
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="myVars"></param>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <param name="dbType"></param>
        /// <param name="uvAddress"></param>
        /// <param name="uvAccount"></param>
        /// <param name="cacheTIme"></param>
        /// <param name="dblCache"></param>
        /// <param name="strd3PortNumber"></param>
        /// <param name="useEncryption"></param>
        /// <param name="d3PortNumber"></param>
        /// <returns></returns>
        IEnumerable<CatalogModel> GetCatalogsList(string storeId, object myVars, string userName, string userPwd,
                                                  string dbType,
                                                  string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                                                  string strd3PortNumber,
                                                  string useEncryption, string d3PortNumber);


        /// <summary>
        /// Method to create new catalog
        /// </summary>
        /// <param name="catDesc"></param>
        /// <param name="catStartDate"></param>
        /// <param name="catEndDate"></param>
        /// <param name="catThumbNails"></param>
        /// <param name="catComments"></param>
        /// <param name="catWebColumns"></param>
        /// <param name="catSortSequence"></param>
        ///  /// <param name="isNew"></param>
        ///  /// <param name="cateId"></param>
        /// <returns></returns>
       int CreateNewCatalog(bool isNew, string cateId, string catDesc, string catStartDate, string catEndDate,
                         string catThumbNails,
                         string catComments, string catWebColumns, string catSortSequence,PickBase.openMode openMode);
            
       
        
        /// <summary>
        /// Get thelist of Catalog product with the items in the ItemLookUpModel model
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="myVars"></param>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <param name="dbType"></param>
        /// <param name="uvAddress"></param>
        /// <param name="uvAccount"></param>
        /// <param name="cacheTIme"></param>
        /// <param name="dblCache"></param>
        /// <param name="strd3PortNumber"></param>
        /// <param name="useEncryption"></param>
        /// <param name="d3PortNumber"></param>
        ///  /// <param name="source"></param>
        /// <returns></returns>
        IEnumerable<ItemLookUpModel> GetCataLogProductLookUpItems(string storeId, object myVars, string source,
                                                                  string userName, string userPwd, string dbType,
                                                                  string uvAddress, string uvAccount, string cacheTIme,
                                                                  string dblCache, string strd3PortNumber,
                                                                  string useEncryption, string d3PortNumber);

        /// <summary>
        /// Get the list of the Catalog products only
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="myVars"></param>
        /// <param name="userName"></param>
        /// <param name="userPwd"></param>
        /// <param name="dbType"></param>
        /// <param name="uvAddress"></param>
        /// <param name="uvAccount"></param>
        /// <param name="cacheTIme"></param>
        /// <param name="dblCache"></param>
        /// <param name="strd3PortNumber"></param>
        /// <param name="useEncryption"></param>
        /// <param name="d3PortNumber"></param>
        /// <returns></returns>
        IEnumerable<CatalogItemModel> CataLogProducts(string storeId, object myVars, string userName, string userPwd,
                                                      string dbType,
                                                      string uvAddress, string uvAccount, string cacheTIme,
                                                      string dblCache, string strd3PortNumber,
                                                      string useEncryption, string d3PortNumber);

        IEnumerable<CatalogModel> GetTopCatalogs(string storeId, string userName, string userPwd,
                                                 string dbType,
                                                 string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                                                 string strd3PortNumber,
                                                 string useEncryption, string d3PortNumber);

     AdminCatalogModel GetCatalogItemList(string cateId);

        /// <summary>
        /// Add catalog items
        /// </summary>
        /// <param name="cateId"></param>
        /// <param name="sku"></param>
        /// <param name="source"></param>
        /// <param name="title"></param>
        /// <param name="price"></param>
        /// <param name="saleprice"></param>
     /// /// <param name="models"></param>
        void AddCatalogItem(string cateId, string sku, string source, string title, string price, string saleprice,
                            IEnumerable<AdminCatalogItemModel> models);

        void UpdateCatalogData(int index, string cateId, DZoft.dBug.TCS.DL.PickBase.openMode opMode,
                               IEnumerable<AdminCatalogItemModel> models);

        #endregion
    }

    public class CatalogsServices : ICatalogsServices
    {
        #region Properties

        [Dependency]
        public ICatalogsDaos CatalogsDao { get; set; }

        #endregion

        #region ICatalogsServices Members

        public int CreateNewCatalog(bool isNew,string cateId,string catDesc, string catStartDate, string catEndDate, string catThumbNails,
                                                       string catComments, string catWebColumns, string catSortSequence, PickBase.openMode openMode)
        {
            //Calling the method to get the xml form the database
            var key = CatalogsDao.CreateNewCatalog(isNew,cateId,catDesc, catStartDate, catEndDate, catThumbNails, catComments,
                                                                     catWebColumns,  catSortSequence,openMode);
            
           

            return key;
        }

        public IEnumerable<CatalogModel> GetCatalogsList(string storeId, object myVars, string userName, string userPwd,
                                                         string dbType, string uvAddress, string uvAccount,
                                                         string cacheTIme, string dblCache, string strd3PortNumber,
                                                         string useEncryption, string d3PortNumber)
        {
            try
            {
                //Calling the method to get the xml form the database
                string strPickDataReturn = CatalogsDao.GetCatalogsList(storeId, myVars, userName, userPwd, dbType,
                                                                       uvAddress, uvAccount, cacheTIme, dblCache,
                                                                       strd3PortNumber, useEncryption, d3PortNumber);

                var doc = new XmlDocument();

                doc.LoadXml(strPickDataReturn);

                //Parsing the xml
                XElement xmlCatalogs = XElement.Parse(strPickDataReturn);

                //Reading the xml and filling model
                List<CatalogModel> catalogsModels = (from catalogs in xmlCatalogs.Descendants("CATALOG")
                                                     where !string.IsNullOrEmpty(catalogs.Element("SEOURL").Value)
                                                     select new CatalogModel
                                                                {
                                                                    CatalogId = catalogs.Element("CATID").Value,
                                                                    Desc = catalogs.Element("DESC").Value,
                                                                    SeoUrl = catalogs.Element("SEOURL").Value,
                                                                }).ToList();

                return catalogsModels;
            }
            catch (Exception x)
            {
                throw;
            }
        }


        public IEnumerable<ItemLookUpModel> GetCataLogProductLookUpItems(string storeId, object myVars, string source,
                                                                         string userName, string userPwd, string dbType,
                                                                         string uvAddress, string uvAccount,
                                                                         string cacheTIme, string dblCache,
                                                                         string strd3PortNumber, string useEncryption,
                                                                         string d3PortNumber)
        {
            try
            {
                var itemLookUpModel = CatalogsDao.GetCataLogProductLookUpItems(storeId, myVars,
                                                                                                        userName,
                                                                                                        userPwd, dbType,
                                                                                                        uvAddress,
                                                                                                        uvAccount,
                                                                                                        cacheTIme,
                                                                                                        dblCache,
                                                                                                        strd3PortNumber,
                                                                                                        useEncryption,
                                                                                                        d3PortNumber);

             
                if (source.ToLower().Equals("gm"))
                {
                    foreach (var item in itemLookUpModel)
                    {
                       
                        var flatAmount = item.FlatFreight;

                        decimal dFlatAmount = 0;

                        if (!String.IsNullOrEmpty(flatAmount))
                        {
                            dFlatAmount = Convert.ToDecimal(flatAmount) / 100;
                        }

                        item.FlatFreight = dFlatAmount.ToString();
                        
                    }
                }
                if (source.ToLower().Equals("tr"))
                {
                    string isbn = "";
                    foreach (var item in itemLookUpModel)
                    {

                        //item.Image = "http://content-3.powells.com/cgi-bin/imageDB.cgi?isbn=" + item.Sku ;
                        isbn = item.Sku;
                    }

                    //IWEB.ValidateIsbn.CheckIsbn(ref isbn); //returns a 10 digit isbn with correct check digit
                    //AmazonData a = new AmazonData();
                    //AmazonURLClass urlClass = new AmazonURLClass();
                    
                    //string itemURL = urlClass.DoItemSingleSearch(isbn);
                    ////string itemURL = urlClass.DoItemSearch(isbn);
                    //List<AmazonStrongType> myList = a.ParseData(itemURL);
                    //List<AmazonStrongType> myList1 = a.SimilarityParseData(itemURL);
                }
            
                return itemLookUpModel;
            }

            catch (Exception x)
            {
                throw;
            }
        }


        public IEnumerable<CatalogItemModel> CataLogProducts(string storeId, object myVars, string userName,
                                                             string userPwd, string dbType, string uvAddress,
                                                             string uvAccount, string cacheTIme, string dblCache,
                                                             string strd3PortNumber, string useEncryption,
                                                             string d3PortNumber)
        {
            try
            {
                IEnumerable<CatalogItemModel> cataItemModels = CatalogsDao.CataLogProductList(storeId, myVars, userName,
                                                                                              userPwd, dbType, uvAddress,
                                                                                              uvAccount,
                                                                                              cacheTIme, dblCache,
                                                                                              strd3PortNumber,
                                                                                              useEncryption,
                                                                                              d3PortNumber);
                return cataItemModels;
            }
            catch (Exception x)
            {
                throw;
            }
        }

        #endregion


        public IEnumerable<CatalogModel> GetTopCatalogs(string storeId, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            object myVars = new { STOREID = storeId };

            return  CatalogsDao.GetTopCatalogs(storeId, myVars, userName, userPwd, dbType,
                                                                       uvAddress, uvAccount, cacheTIme, dblCache,
                                                                       strd3PortNumber, useEncryption, d3PortNumber);
        }


        public AdminCatalogModel GetCatalogItemList(string cateId)
        {
            return CatalogsDao.GetCatalogItemList(cateId);
        }

       public void AddCatalogItem(string cateId,string sku, string source, string title, string price, string saleprice,IEnumerable<AdminCatalogItemModel> models )
       {

           CatalogsDao.AddCatalogItem(cateId,sku,source,title,price,saleprice,models);
       }

        public void UpdateCatalogData(int index,string cateId, DZoft.dBug.TCS.DL.PickBase.openMode opMode,IEnumerable<AdminCatalogItemModel>models )
        {
            CatalogsDao.UpdateCatalogData(index,cateId,PickBase.openMode.Update, models);
        }
    }
}