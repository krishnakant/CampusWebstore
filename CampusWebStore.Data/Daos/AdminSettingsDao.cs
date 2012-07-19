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
    public interface IAdminSettingsDao
    {
        void SaveSettings(string storeId, object myVars, string userName, string userPwd, string dbType,
                           string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                           string strd3PortNumber, string useEncryption, string d3PortNumber);

        AdminModel GetSettings(string storeId, object myVars, string userName, string userPwd, string dbType,
                           string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                           string strd3PortNumber, string useEncryption, string d3PortNumber);

    }

    public class AdminSettingsDao:IAdminSettingsDao
    {
        #region  "Properties"

        [Dependency]
        public IDbAccess DbAccess { get; set; }

        #endregion

        /// <summary>
        /// Save IWEB Settings....
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
        public void SaveSettings(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount,
            string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {


            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.IwebSetSettings, myVars, userName, userPwd,
                                                             dbType, uvAddress, uvAccount, cacheTIme, dblCache,
                                                             strd3PortNumber, useEncryption, d3PortNumber);

            //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
               // xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
        }

        /// <summary>
        /// To Get Settings of IWEB
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
        public AdminModel GetSettings(string storeId, object myVars, string userName, string userPwd, string dbType,
                           string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                           string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            var objAdminModel = new AdminModel();

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.IwebSetSettings, myVars, userName, userPwd,
                                                             dbType, uvAddress, uvAccount, cacheTIme, dblCache,
                                                             strd3PortNumber, useEncryption, d3PortNumber);

            //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                // xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {
                objAdminModel = (from setting in xmlResult.Descendants("GENPARMS")
                                 select new AdminModel
                                            {
                                               IsPickupValid  =  setting.Element("ISPICKUPVALID").Value=="Y",
                                               IsoosGm = setting.Element("SHOWOOSGM").Value == "Y",
                                               IsoosTr = setting.Element("SHOWOOSTR").Value == "Y",
                                               IsoosTx = setting.Element("SHOWOOSTX").Value == "Y",
                                               GmImagePath = setting.Element("GMIMAGEPATH").Value,
                                               TrImagePath = setting.Element("TRIMAGEPATH").Value,
                                               TxImagePath = setting.Element("TXIMAGEPATH").Value,
                                               NewTxLowQty = setting.Element("TXNEWLOWQTY").Value,
                                               NewTXoosQty = setting.Element("TXNEWOOSQTY").Value,
                                               GmLowQty = setting.Element("GMLOWQTY").Value,
                                               GMoosmsg = setting.Element("GMOOSQTY").Value,
                                               TrLowQty = setting.Element("TRLOWQTY").Value,
                                               TRoosQTY = setting.Element("TROOSQTY").Value,
                                               UsedTXoosQty = setting.Element("TXUSEDOOSQTY").Value,
                                               UsedTxLowQty = setting.Element("TXUSEDLOWQTY").Value,
                                               NewLowStockMsg = setting.Element("TXNEWSTOCKLOWMSG").Value,
                                               NewOosMsg = setting.Element("TXNEWSTOCKOUTMSG").Value,
                                               UsedLowStockMsg = setting.Element("TXUSEDSTOCKLOWMSG").Value,
                                               UsedOosMsg = setting.Element("TXUSEDSTOCKOUTMSG").Value,
                                               TRoosmsg = setting.Element("TRSTOCKOUTMSG").Value,
                                               TRlowstockmsg = setting.Element("TRSTOCKLOWMSG").Value,
                                               GMlowstockmsg = setting.Element("GMSTOCKLOWMSG").Value,
                                               //GMoosmsg = setting.Element("GMSTOCKOUTMSG").Value,
                                               IsTrUseQoo = setting.Element("TRUSEQOO").Value == "Y",
                                               IsGmUseQoo = setting.Element("GMUSEQOO").Value == "Y",
                                            }).SingleOrDefault();


                if (objAdminModel !=null)
                {
                    objAdminModel.BuyBackStoreCredit =
                        xmlResult.Descendants("SELLBACKPARMS").Elements("STORECREDIT").SingleOrDefault().Value;

                    objAdminModel.BuyBackRetailPrice =
                       xmlResult.Descendants("SELLBACKPARMS").Elements("RETAILPERCENT").SingleOrDefault().Value;

                    objAdminModel.BuyBackRetailRounding =
                       xmlResult.Descendants("SELLBACKPARMS").Elements("RETAILROUNDING").SingleOrDefault().Value;

                    objAdminModel.BuyBackRetailCoin =
                       xmlResult.Descendants("SELLBACKPARMS").Elements("RETAILCOIN").SingleOrDefault().Value;


                    objAdminModel.BuyBackWholesalePrice =
                      xmlResult.Descendants("SELLBACKPARMS").Elements("WHOLESALEPERCENT").SingleOrDefault().Value;


                    objAdminModel.BuyBackWholeSaleRounding =
                      xmlResult.Descendants("SELLBACKPARMS").Elements("WHOLESALEROUNDING").SingleOrDefault().Value;

                    objAdminModel.BuyBackWholeSaleCoin =
                      xmlResult.Descendants("SELLBACKPARMS").Elements("WHOLESALECOIN").SingleOrDefault().Value;

                    objAdminModel.BuyBackRetailRounding =
                    xmlResult.Descendants("SELLBACKPARMS").Elements("RETAILWORDING").SingleOrDefault().Value;

                    objAdminModel.BuyBackWholesalePriceWording =
                    xmlResult.Descendants("SELLBACKPARMS").Elements("WHOLESALEWORDING").SingleOrDefault().Value;

                    objAdminModel.BuyBackNeedWording =
                    xmlResult.Descendants("SELLBACKPARMS").Elements("EBBWORDING").SingleOrDefault().Value;
                    
                    objAdminModel.BuyBackBuyingWording =
                    xmlResult.Descendants("SELLBACKPARMS").Elements("BUYINGWORDING").SingleOrDefault().Value;

                    objAdminModel.BuyBackNotBuyingWording =
                   xmlResult.Descendants("SELLBACKPARMS").Elements("NOTBUYINGWORDING").SingleOrDefault().Value;

                }
            }
           
            return objAdminModel;
        }
    }
}
