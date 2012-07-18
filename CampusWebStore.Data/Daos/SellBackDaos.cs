using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;

namespace CampusWebStore.Data.Daos
{
    #region "Interface"

    public interface ISellBackDaos
    {
        #region "Public Methods"

        IEnumerable<SellBackBookModel> GetBookData(string storeId, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);

        SellBackParamModel GetSellBackParams(string storeId, object myVars, string userName, string userPwd, string dbType,
                          string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                          string useEncryption, string d3PortNumber);

        string SetSellBackParams(string storeId, string storeCreditPercent, string retailPercent, string retailRounding,
                                 string retailCoin, string wholeSalepercent, string wholeSaleRounding,
                                 string wholeSaleCoin, string userName, string userPwd, string dbType, string uvAddress,
                                 string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                                 string useEncryption, string d3PortNumber);


        #endregion
    }

    #endregion

   public class SellBackDaos:ISellBackDaos
    {

        #region "Properties"

        [Dependency]
        public IDbAccess DbAccess { get; set; }

        #endregion

        public IEnumerable<SellBackBookModel> GetBookData(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
          
            //Geting the xml from the databas3e
            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.SellBack, myVars, userName, userPwd, dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);

            //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var sellbackBookModels=new List<SellBackBookModel>() ;

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
               //var err = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {
                 sellbackBookModels = (from sellBack in xmlResult.Descendants("DETAIL")
                                          select
                                              new SellBackBookModel
                                                  {
                                                      Isbn = sellBack.Element("ISBN") != null ? sellBack.Element("ISBN").Value : "",
                                                      Author=sellBack.Element("AUTHOR")!=null?sellBack.Element("AUTHOR").Value:"",
                                                      Title = sellBack.Element("TITLE") != null ? sellBack.Element("TITLE").Value : "",
                                                      Publisher = sellBack.Element("PUBLISHER") != null ? sellBack.Element("PUBLISHER").Value : "",
                                                      Copyright = sellBack.Element("COPYRIGHT") != null ? sellBack.Element("COPYRIGHT").Value : "",
                                                      Edition = sellBack.Element("EDITION") != null ? sellBack.Element("EDITION").Value : "",
                                                      RetailPrice = sellBack.Element("RETAILPRICE") != null ? sellBack.Element("RETAILPRICE").Value : "",
                                                      WhslrPrice = sellBack.Element("WHSLEPRICE") != null ? sellBack.Element("WHSLEPRICE").Value : "",
                                                      RetStoreCredit = sellBack.Element("RETSTORECREDIT") != null ? sellBack.Element("RETSTORECREDIT").Value : "",
                                                      NeedLimit = sellBack.Element("NEEDLIMIT") != null ? sellBack.Element("NEEDLIMIT").Value : "",
                                                      NeedWording = sellBack.Element("NEEDWORDING") != null ? sellBack.Element("NEEDWORDING").Value : "",
                                                      RetailWording = sellBack.Element("RETAILWORDING") != null ? sellBack.Element("RETAILWORDING").Value : "",
                                                      WholeSaleWording = sellBack.Element("WHOLESALEWORDING") != null ? sellBack.Element("WHOLESALEWORDING").Value : "",
                                                      BuyingWording = sellBack.Element("BUYINGWORDING") != null ? sellBack.Element("BUYINGWORDING").Value : "",
                                                      NotBuyingWording = sellBack.Element("NOTBUYINGWORDING") != null ? sellBack.Element("NOTBUYINGWORDING").Value : "",
                                                      WhsleStoreCredit = sellBack.Element("WHSLESTORECREDIT") != null ? sellBack.Element("WHSLESTORECREDIT").Value : "",
                                                   

                                                  }).ToList();

            }
            return sellbackBookModels;
        }
        public SellBackParamModel GetSellBackParams(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            //Geting the xml from the databas3e
            object myObject = new { STOREID = 1 };
          
            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.SellBackParams, myObject, userName, userPwd, dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);
           
            //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var sellBackParamModel = new SellBackParamModel();

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                //var err = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {
                var xmlStoreCredit = doc.GetElementsByTagName("STORECREDIT");
                var storecredit = xmlStoreCredit[0].InnerXml;

                var xmlRetailPercent = doc.GetElementsByTagName("RETAILPERCENT");
                var retailpercent = xmlRetailPercent[0].InnerXml;

                var xmlRetailRounding = doc.GetElementsByTagName("RETAILROUNDING");
                var retailrounding = xmlRetailRounding[0].InnerXml;

                var xmlRetailCoin = doc.GetElementsByTagName("RETAILCOIN");
                var retailcoin = xmlRetailCoin[0].InnerXml;

                var xmlWholesalePercent = doc.GetElementsByTagName("WHOLESALEPERCENT");
                var wholesalepercent = xmlWholesalePercent[0].InnerXml;

                var xmlWholesaleRounding = doc.GetElementsByTagName("WHOLESALEROUNDING");
                var wholesalerounding = xmlWholesaleRounding[0].InnerXml;

                var xmlWholesaleCoin = doc.GetElementsByTagName("WHOLESALECOIN");
                var wholesalecoin = xmlWholesaleCoin[0].InnerXml;
                sellBackParamModel.RetailPercent = retailpercent;
                sellBackParamModel.Retailcoin = retailcoin;
                sellBackParamModel.Retailrounding = retailrounding;
                sellBackParamModel.Storecredit = storecredit;
                sellBackParamModel.WholesalePercent = wholesalepercent;
                sellBackParamModel.Wholesalecoin = wholesalecoin;
                sellBackParamModel.Wholesalerounding = wholesalerounding;

            }
            return sellBackParamModel;
        }
        public string SetSellBackParams(string storeId, string storeCreditPercent, string retailPercent, string retailRounding, string retailCoin, string wholeSalepercent, string wholeSaleRounding, string wholeSaleCoin, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            //Geting the xml from the databas3e
            object myObject = new
            {
                STOREID = storeId,
                STORECREDIT = storeCreditPercent,
                RETAILPERCENT = retailPercent ,
                RETAILROUNDING = retailRounding,
                RETAILCOIN = retailCoin,
                WHOLESALEPERCENT = wholeSalepercent,
                WHOLESALEROUNDING = wholeSaleRounding,
                WHOLESALECOIN = wholeSaleCoin
            };

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.SetsellBackParams, myObject, userName, userPwd, dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);

            //Read XML data
            //var doc = new XmlDocument();

            //doc.LoadXml(strPickDataReturn);

            //var sellBackParamModel = new SellBackParamModel();

            //var xmlResult = XElement.Parse(strPickDataReturn);

            //if (xmlResult.Descendants("ERROR").Any())
            //{
            //    //If not set error message
            //    //var err = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            //}
            //else
            //{
            //    var xmlStoreCredit = doc.GetElementsByTagName("STORECREDIT");
            //    var storecredit = xmlStoreCredit[0].InnerXml;

            //    var xmlRetailPercent = doc.GetElementsByTagName("RETAILPERCENT");
            //    var retailpercent = xmlRetailPercent[0].InnerXml;

            //    var xmlRetailRounding = doc.GetElementsByTagName("RETAILROUNDING");
            //    var retailrounding = xmlRetailRounding[0].InnerXml;

            //    var xmlRetailCoin = doc.GetElementsByTagName("RETAILCOIN");
            //    var retailcoin = xmlRetailCoin[0].InnerXml;

            //    var xmlWholesalePercent = doc.GetElementsByTagName("WHOLESALEPERCENT");
            //    var wholesalepercent = xmlWholesalePercent[0].InnerXml;

            //    var xmlWholesaleRounding = doc.GetElementsByTagName("WHOLESALEROUNDING");
            //    var wholesalerounding = xmlWholesaleRounding[0].InnerXml;

            //    var xmlWholesaleCoin = doc.GetElementsByTagName("WHOLESALECOIN");
            //    var wholesalecoin = xmlWholesaleCoin[0].InnerXml;
            //    sellBackParamModel.RetailPercent = retailpercent;
            //    sellBackParamModel.Retailcoin = retailcoin;
            //    sellBackParamModel.Retailrounding = retailrounding;
            //    sellBackParamModel.Storecredit = storecredit;
            //    sellBackParamModel.WholesalePercent = wholesalepercent;
            //    sellBackParamModel.Wholesalecoin = wholesalecoin;
            //    sellBackParamModel.Wholesalerounding = wholesalerounding;

            //}
            return "dfd";
        }

   }
}
