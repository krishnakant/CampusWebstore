

using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;
using DZoft.dBug.TCS.DL;

namespace CampusWebStore.Data.Daos
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Practices.Unity;



    public interface ICatalogsDaos
    {
        #region Public Methods

        /// <summary>
        /// Get the list of catalogs 
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
        string GetCatalogsList(string storeId, object myVars, string userName, string userPwd, string dbType,
                               string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                               string strd3PortNumber,
                               string useEncryption, string d3PortNumber);

        /// <summary>
        /// creating new catalog 
        /// </summary>
        /// <param name="catDesc"></param>
        /// <param name="catStartDate"></param>
        /// <param name="catEndDate"></param>
        /// <param name="catThumbNails"></param>
        /// <param name="catComments"></param>
        /// <param name="catWebColumns"></param>
        /// <param name="catSortSequence"></param>
        ///   /// <param name="cateId"></param>
        ///   /// <param name="isNew"></param>
        /// <returns></returns>
        int CreateNewCatalog(bool isNew, string cateId, string catDesc, string catStartDate, string catEndDate,
                             string catThumbNails,
                             string catComments, string catWebColumns, string catSortSequence, PickBase.openMode openMode);

        /// <summary>
        /// Get the list of product for the catalog
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
        IEnumerable<CatalogItemModel> CataLogProductList(string storeId, object myVars, string userName, string userPwd,
                                                     string dbType,
                                                     string uvAddress, string uvAccount, string cacheTIme,
                                                     string dblCache,
                                                     string strd3PortNumber,
                                                     string useEncryption, string d3PortNumber);
        /// <summary>
        /// /Get catalog products items
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
        IEnumerable<ItemLookUpModel> GetCataLogProductLookUpItems(string storeId, object myVars, string userName,
                                                                  string userPwd, string dbType,
                                                                  string uvAddress, string uvAccount, string cacheTIme,
                                                                  string dblCache,
                                                                  string strd3PortNumber,
                                                                  string useEncryption, string d3PortNumber);
        /// <summary>
        /// Get top Catalogs
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
        IEnumerable<CatalogModel> GetTopCatalogs(string storeId, object myVars, string userName,
                                                                 string userPwd, string dbType,
                                                                 string uvAddress, string uvAccount, string cacheTIme,
                                                                 string dblCache,
                                                                 string strd3PortNumber,
                                                                 string useEncryption, string d3PortNumber);
        //./Get the the list of the catalogs by catelog id
      AdminCatalogModel GetCatalogItemList(string cateId);

        void AddCatalogItem(string cateId, string sku, string source, string title, string price, string saleprice,
                            IEnumerable<AdminCatalogItemModel> models);

        void UpdateCatalogData(int index, string cateId, DZoft.dBug.TCS.DL.PickBase.openMode opMode,
                               IEnumerable<AdminCatalogItemModel> models);

        #endregion
    }

    public class CatalogsDaos : ICatalogsDaos
    {

        #region Properties

        [Dependency]
        public IDbAccess DbAccess { get; set; }

        #endregion

        #region Public Methods

        public string GetCatalogsList(string storeId, object myVars, string userName, string userPwd, string dbType,
                                      string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                                      string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
                var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.GetCatalogs, myVars, userName, userPwd, dbType,
                                                uvAddress,
                                                uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption,
                                                d3PortNumber);
                return strPickDataReturn;

            }
            catch (Exception x)
            {
                throw;
            }
        }

        public int CreateNewCatalog(bool isNew,string cateId,string catDesc, string catStartDate, string catEndDate,
                                                       string catThumbNails,
                                                       string catComments, string catWebColumns, string catSortSequence, PickBase.openMode openMode)
        {
            try
            {

                Catalogs t;
                if (isNew)
                    t = new Catalogs();
                else
                    t = new Catalogs(cateId);

                //Value to insert
                t.AddVariable("UPDATETYPE", "CATALOG");
                //t.AddVariable("CATALOG", catid);
                t.AddVariable("CATDESC", catDesc);
                t.AddVariable("STARTDATE", catStartDate);
                t.AddVariable("ENDDATE", catEndDate);
                t.AddVariable("THUMBS", catThumbNails);
                t.AddVariable("COMMENTS", catComments);
                t.AddVariable("COLUMNS", catWebColumns);
                t.AddVariable("SORT", catSortSequence);
                int key = t.Write();
               
                return key;
            }
            catch (Exception x)
            {
                throw;
            }
        }

        public IEnumerable<CatalogItemModel> CataLogProductList(string storeId, object myVars, string userName,
                                                            string userPwd, string dbType,
                                                            string uvAddress, string uvAccount, string cacheTIme,
                                                            string dblCache,
                                                            string strd3PortNumber, string useEncryption,
                                                            string d3PortNumber)
        {
            try
            {

                var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.GetCatalogItems, myVars, userName,
                                                                 userPwd, dbType,
                                                                 uvAddress,
                                                                 uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                                 useEncryption,
                                                                 d3PortNumber);

                var doc = new XmlDocument();

                XElement xmlCatalogItmes = XElement.Parse(strPickDataReturn);



                var catalogItems = (from catalogItem in xmlCatalogItmes.Descendants("ITEM")
                                    select new CatalogItemModel
                                               {
                                                   Sku = catalogItem.Element("SKU")!=null?catalogItem.Element("SKU").Value:"",
                                                   Source = catalogItem.Element("SOURCE") != null ? catalogItem.Element("SOURCE").Value : "",
                                                   Title =catalogItem.Element("SKU")!=null ?catalogItem.Element("TITLE").Value:"",
                                                   Images = catalogItem.Element("TITLE") != null ? catalogItem.Element("IMAGE").Value : "",
                                                   Price = catalogItem.Element("PRICE") != null ? catalogItem.Element("PRICE").Value : "",
                                                   CatId = catalogItem.Element("CATID") != null ? catalogItem.Element("CATID").Value : "",
                                                   SeoTitle = catalogItem.Element("SEOTITLE") != null ? catalogItem.Element("SEOTITLE").Value : "",
                                                   RealPrice = catalogItem.Element("REALPRICE") != null ? catalogItem.Element("REALPRICE").Value : "",
                                                   SeoSku = catalogItem.Element("SEOSKU") != null ? catalogItem.Element("SEOSKU").Value : "",
                                                   Comments = catalogItem.Element("COMMENTS") != null ? catalogItem.Element("COMMENTS").Value : "",
                                                   CatalogPrice = catalogItem.Element("CATALOGPRICE") != null ? catalogItem.Element("CATALOGPRICE").Value : "",
                                                   Desc = catalogItem.Element("DESC") != null ? catalogItem.Element("DESC").Value : "",
                                                   DisplayText = catalogItem.Element("DISPLAYTEXT") != null ? catalogItem.Element("DISPLAYTEXT").Value : "",
                                                   Author = catalogItem.Element("AUTHOR") != null ? catalogItem.Element("AUTHOR").Value : "",
                                                  
                                               }).ToList();
                return catalogItems;
            }
            catch (Exception x)
            {
                throw;
            }


        }


        public IEnumerable<ItemLookUpModel> GetCataLogProductLookUpItems(string storeId, object myVars, string userName,
                                                                         string userPwd, string dbType, string uvAddress,
                                                                         string uvAccount, string cacheTIme,
                                                                         string dblCache, string strd3PortNumber,
                                                                         string useEncryption, string d3PortNumber)
        {
            try
            {
                var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.GetItems, myVars, userName, userPwd,
                                                                 dbType,
                                                                 uvAddress,
                                                                 uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                                 useEncryption,
                                                                 d3PortNumber);

                var doc = new XmlDocument();

                var xmlCatalogLoolUpItems = XElement.Parse(strPickDataReturn);

                //var isMatrixItems=(from catalogLoolUpItems in xmlCatalogLoolUpItems.Descendants("ITEM") where (bool)catalogLoolUpItems.Element("ISMATRIX").Value select catalogLoolUpItems).FirstOrDefault();

                List<ItemLookUpModel> catalogLookUpItems;
               
                catalogLookUpItems = (from catalogLoolUpItems in xmlCatalogLoolUpItems.Descendants("ITEM")
                                          
                                           select new ItemLookUpModel
                                                        {
                                                            Sku =catalogLoolUpItems.Element("SKU")!=null?catalogLoolUpItems.Element("SKU").Value:"",
                                                            Source =catalogLoolUpItems.Element("SOURCE")!=null? catalogLoolUpItems.Element("SOURCE").Value:"",
                                                            Description = catalogLoolUpItems.Element("DESCRIPTION") != null ? catalogLoolUpItems.Element("DESCRIPTION").Value : catalogLoolUpItems.Element("DESC") != null ? catalogLoolUpItems.Element("DESC").Value : "",
                                                            Vendor =catalogLoolUpItems.Element("VENDOR")!=null? catalogLoolUpItems.Element("VENDOR").Value:"",
                                                            Style = catalogLoolUpItems.Element("STYLE") != null ? catalogLoolUpItems.Element("STYLE").Value : "",

                                                            Comments = catalogLoolUpItems.Element("COMMENTS") != null ? catalogLoolUpItems.Element("COMMENTS").Value : "",
                                                            Manf = catalogLoolUpItems.Element("MANF") != null ? catalogLoolUpItems.Element("MANF").Value : "",
                                                            Edition = catalogLoolUpItems.Element("EDITION") != null ? catalogLoolUpItems.Element("EDITION").Value : "",
                                                            Binding = catalogLoolUpItems.Element("BINDING") != null ? catalogLoolUpItems.Element("BINDING").Value : "",
                                                            Vol = catalogLoolUpItems.Element("VOL") != null ? catalogLoolUpItems.Element("VOL").Value : "",

                                                            Price = catalogLoolUpItems.Element("PRICE") != null ? catalogLoolUpItems.Element("PRICE").Value : "",
                                                            Qoh = catalogLoolUpItems.Element("QOH") != null ? catalogLoolUpItems.Element("QOH").Value : "",
                                                            Publisher = catalogLoolUpItems.Element("PUBLISHER") != null ? catalogLoolUpItems.Element("PUBLISHER").Value : "",
                                                            Image = catalogLoolUpItems.Element("IMAGE") != null ? catalogLoolUpItems.Element("IMAGE").Value : "",
                                                            SalePrice = catalogLoolUpItems.Element("SALEPRICE") != null ? catalogLoolUpItems.Element("SALEPRICE").Value : "",

                                                            StockMsg = catalogLoolUpItems.Element("STOCKMSG") != null ? catalogLoolUpItems.Element("STOCKMSG").Value : "",
                                                            SeoSku = catalogLoolUpItems.Element("SEOSKU") != null ? catalogLoolUpItems.Element("SEOSKU").Value : "",
                                                            SeoTitle = catalogLoolUpItems.Element("SEOTITLE") != null ? catalogLoolUpItems.Element("SEOTITLE").Value: "",
                                                            FlatFreight = catalogLoolUpItems.Element("FLATFREIGHT") != null ? catalogLoolUpItems.Element("FLATFREIGHT").Value : "",
                                                            FreeShipping = catalogLoolUpItems.Element("FREESHIPPING") != null ? catalogLoolUpItems.Element("FREESHIPPING").Value : "",

                                                            UsedQoh = catalogLoolUpItems.Element("USEDQOH") != null ? catalogLoolUpItems.Element("USEDQOH").Value : "",
                                                            NewQoh = catalogLoolUpItems.Element("NEWQOH") != null ? catalogLoolUpItems.Element("NEWQOH").Value : "",
                                                            UsedPrice = catalogLoolUpItems.Element("USEDPRICE") != null ? catalogLoolUpItems.Element("USEDPRICE").Value : "",
                                                            NewPrice = catalogLoolUpItems.Element("NEWPRICE") != null ? catalogLoolUpItems.Element("NEWPRICE").Value : "",

                                                            IsMatrix = catalogLoolUpItems.Element("ISMATRIX")!=null?catalogLoolUpItems.Element("ISMATRIX").Value:"0",
                                                            MatrixItem =
                                                                (from matrix in
                                                                     catalogLoolUpItems.Descendants("MATRIXITEM")
                                                                 where !catalogLoolUpItems.Element("ISMATRIX").Equals("0")
                                                                 select new MatrixItemModel
                                                                            {
                                                                                MatrixLine =matrix.Element("MATRIXLINE")!=null?matrix.Element("MATRIXLINE").Value:"",
                                                                                Size =matrix.Element("SIZE")!=null?
                                                                                    matrix.Element("SIZE").Value:"",
                                                                                Price =matrix.Element("PRICE")!=null?
                                                                                    matrix.Element("PRICE").
                                                                                    Value:"",
                                                                                DLdsc =matrix.Element("DLDESC")!=null?
                                                                                    matrix.Element("DLDESC").
                                                                                    Value:"",
                                                                                Desc = matrix.Element("DESC")!=null?
                                                                                    matrix.Element("DESC").Value:"",
                                                                                Qqh =matrix.Element("QOH").Value!=null?
                                                                                    matrix.Element("QOH").Value:"",

                                                                                MatrixId =matrix.Element("MATRIXID")!=null?
                                                                                    matrix.Element("MATRIXID").Value:"",

                                                                                Color = matrix.Element("COLOR")!=null?matrix.Element("COLOR").Value:"",


                                                                            })
                                                        }).ToList();
                   
              
                return catalogLookUpItems;
            }
            catch (Exception x)
            {
                throw;
            }
        }

        #endregion}


        public IEnumerable<CatalogModel> GetTopCatalogs(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
                var strPickDataReturn= DbAccess.GetStringResult(storeId, Constants.GetTopCatalogs, myVars, userName, userPwd, dbType,
                                                uvAddress,
                                                uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption,
                                                d3PortNumber);
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

        public AdminCatalogModel GetCatalogItemList(string cateId)
        {
            var tmpCat = new DZoft.dBug.TCS.DL.Catalogs(cateId);

            var xmlSerializer = new XmlSerializer(typeof(DataSet));
            DataSet tmpDS = tmpCat.Read();
            var memoryStream = new MemoryStream();
            TextWriter streamWriter = new StreamWriter(memoryStream);
            xmlSerializer.Serialize(streamWriter, tmpDS);
            var strPickDataReturn = Encoding.UTF8.GetString(memoryStream.ToArray());

            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            //Parsing the xml
            XElement xmlCatalogs = XElement.Parse(strPickDataReturn);

            //Reading the xml and filling model
            AdminCatalogModel adminCatalogsModels = (from adminCatalogs in xmlCatalogs.Descendants("DETAIL")
                                                     
                                                     select new AdminCatalogModel()
                                                                {
                                                                    Key = adminCatalogs.Element("KEY") != null ? adminCatalogs.Element("KEY").Value : "",
                                                                    Comment = adminCatalogs.Element("COMMENTS") != null ? adminCatalogs.Element("COMMENTS").Value : "",
                                                                    Description = adminCatalogs.Element("DESC") != null ? adminCatalogs.Element("DESC").Value : "",
                                                                    DisplayTumbnail = Convert.ToBoolean(adminCatalogs.Element("THUMBNAIL") != null ? adminCatalogs.Element("THUMBNAIL").Value :"false"),
                                                                    StartDate = adminCatalogs.Element("START") != null ? adminCatalogs.Element("START").Value : "",
                                                                    EndDate = adminCatalogs.Element("END") != null ? adminCatalogs.Element("END").Value : "",
                                                                    SortSquence = adminCatalogs.Element("SORT") != null ? adminCatalogs.Element("SORT").Value : "",
                                                                    Webcolumn = adminCatalogs.Element("COLUMNS") != null ? adminCatalogs.Element("COLUMNS").Value : "",
                                                                    AdminCatalogItemModels = (from catalogItems in xmlCatalogs.Descendants("ITEMS") select  new AdminCatalogItemModel
                                                                                                                                                                {
                                                                                                                                                                    Mod =  catalogItems.Element("SOURCE") != null ? catalogItems.Element("SOURCE").Value : "",
                                                                                                                                                                    Sku =  catalogItems.Element("SKU") != null ? catalogItems.Element("SKU").Value : "",
                                                                                                                                                                    Price =  catalogItems.Element("PRICE") != null ? catalogItems.Element("PRICE").Value : "",
                                                                                                                                                                    SalesPrice = catalogItems.Element("LIST") != null ? catalogItems.Element("LIST").Value : "",
                                                                                                                                                                    Title = catalogItems.Element("TITLE") != null ? catalogItems.Element("TITLE").Value : "",
                                                                                                                                                                     
                                                                                                                                                                })
                                                     
                                                    
                                                                }).SingleOrDefault();



            return adminCatalogsModels;
        }

      public void AddCatalogItem(string cateId,string sku, string source, string title, string price, string saleprice,IEnumerable<AdminCatalogItemModel>models )
      {
          var t = new DZoft.dBug.TCS.DL.Catalogs(cateId);

          source = source.ToLower();
           if (source.Equals("textbook"))
           {

               source = "TX";

           }
           else
           {
               source = source.Equals("Tradebook") ? "TR" : "GM";
           }
          foreach (var model in models)
          {

              t.SKUS = model.Sku;
              t.Sources = model.Mod;

              t.Titles = model.Title;
              t.Prices = model.Price;
              t.SalePrices = model.SalesPrice;
          }
         //Value to insert
          t.AddVariable("UPDATETYPE", "ITEMS");
          //t.AddVariable("CATALOG", ddlCats.SelectedValue);
          t.SKUS = sku;
          t.Sources = source;
          t.Titles = title;
          t.Prices = price;
          t.SalePrices = saleprice;
          t.Write();
      }

      public void UpdateCatalogData(int index,string cateId, DZoft.dBug.TCS.DL.PickBase.openMode opMode,IEnumerable<AdminCatalogItemModel>models )
      {
          DZoft.dBug.TCS.DL.Catalogs t = new DZoft.dBug.TCS.DL.Catalogs(cateId);
          //t.AddVariable("CATALOG", ddlCats.SelectedValue);
          foreach (var model in models)
          {
              t.SKUS = model.Sku;
              t.Sources = model.Mod;
              t.Titles = model.Title;
              t.Prices = model.Price;
              t.SalePrices = model.SalesPrice;
             
          }
          t.AddVariable("UPDATETYPE", "ITEMS");
          t.Write();
       
      }
    }


}
