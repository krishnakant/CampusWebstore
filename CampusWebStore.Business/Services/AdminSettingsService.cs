using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CampusWebStore.Data.Daos;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;

namespace CampusWebStore.Business.Services
{
    public interface IAdminSettingsService
    {

        void SaveSettings(string storeId, string notbuyingwording, string buyingwording, string ebbneedwording,
                          string wholesalewording, string retailwording, string wholesalecoin,
                          string wholesalerounding, string wholesalepercent, string retailcoin, string retailrounding,
                          string retailpercent, string storecredit, string gmuseqoo, string trueqoo,
                          string gmstockoutmsg, string gmstocklowmsg, string trstocklowmsg, string trstockoutmsg,
                          string txusedstockoutmsg, string txusedstocklowmsg, string txnewstocklowmsg,
                          string txnewstockoutmsg, string txusedlowqty, string txusedoosqty, string gmlowqty,
                          string gmoosqty, string trlowqty, string troosqty, string txnewoosqty,
                          string txnewlowqty, string tximagepath, string trimagepath, string gmimagepath,
                          string showoostx, string showoostr, string showoosgm, string ispickupvalid,
                          string userName, string userPwd, string dbType, string uvAddress, string uvAccount,
                          string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption,
                          string d3PortNumber);

        AdminModel GetSettings(string storeId, string userName, string userPwd, string dbType, string uvAddress,
                               string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                               string useEncryption, string d3PortNumber);
    }

    public class AdminSettingsService : IAdminSettingsService
    {
        #region Properties
        [Dependency]
        public IAdminSettingsDao AdminSettingsDao { get; set; }
        #endregion


        /// <summary>
        /// Save IWEB Settings
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="notbuyingwording"></param>
        /// <param name="buyingwording"></param>
        /// <param name="ebbneedwording"></param>
        /// <param name="wholesalewording"></param>
        /// <param name="retailwording"></param>
        /// <param name="wholesalecoin"></param>
        /// <param name="wholesalerounding"></param>
        /// <param name="wholesalepercent"></param>
        /// <param name="retailcoin"></param>
        /// <param name="retailrounding"></param>
        /// <param name="retailpercent"></param>
        /// <param name="storecredit"></param>
        /// <param name="gmuseqoo"></param>
        /// <param name="trueqoo"></param>
        /// <param name="gmstockoutmsg"></param>
        /// <param name="gmstocklowmsg"></param>
        /// <param name="trstocklowmsg"></param>
        /// <param name="trstockoutmsg"></param>
        /// <param name="txusedstockoutmsg"></param>
        /// <param name="txusedstocklowmsg"></param>
        /// <param name="txnewstocklowmsg"></param>
        /// <param name="txnewstockoutmsg"></param>
        /// <param name="txusedlowqty"></param>
        /// <param name="txusedoosqty"></param>
        /// <param name="gmlowqty"></param>
        /// <param name="gmoosqty"></param>
        /// <param name="trlowqty"></param>
        /// <param name="troosqty"></param>
        /// <param name="txnewoosqty"></param>
        /// <param name="txnewlowqty"></param>
        /// <param name="tximagepath"></param>
        /// <param name="trimagepath"></param>
        /// <param name="gmimagepath"></param>
        /// <param name="showoostx"></param>
        /// <param name="showoostr"></param>
        /// <param name="showoosgm"></param>
        /// <param name="ispickupvalid"></param>
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
        public void SaveSettings(string storeId, string notbuyingwording, string buyingwording, string ebbneedwording,
                                 string wholesalewording,
                                 string retailwording, string wholesalecoin, string wholesalerounding,
                                 string wholesalepercent, string retailcoin, string retailrounding,
                                 string retailpercent, string storecredit, string gmuseqoo, string trueqoo,
                                 string gmstockoutmsg, string gmstocklowmsg, string trstocklowmsg,
                                 string trstockoutmsg, string txusedstockoutmsg, string txusedstocklowmsg,
                                 string txnewstocklowmsg, string txnewstockoutmsg, string txusedlowqty,
                                 string txusedoosqty, string gmlowqty, string gmoosqty, string trlowqty, string troosqty,
                                 string txnewoosqty, string txnewlowqty, string tximagepath,
                                 string trimagepath, string gmimagepath, string showoostx, string showoostr,
                                 string showoosgm, string ispickupvalid, string userName, string userPwd,
                                 string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                                 string strd3PortNumber, string useEncryption, string d3PortNumber)

        {

            //Create new settings object....
            var myObject = new
                               {
                                   NOTBUYINGWORDING =
                                       !string.IsNullOrEmpty(notbuyingwording) ? notbuyingwording : string.Empty,
                                   BUYINGWORDING = !string.IsNullOrEmpty(buyingwording) ? buyingwording : string.Empty,
                                   EBBNEEDWORDING = !string.IsNullOrEmpty(ebbneedwording) ? ebbneedwording : string.Empty,
                                   WHOLESALEWORDING = !string.IsNullOrEmpty(wholesalewording) ? wholesalewording : string.Empty,
                                   RETAILWORDING = !string.IsNullOrEmpty(retailwording) ? retailwording : string.Empty ,
                                   WHOLESALECOIN = !string.IsNullOrEmpty(wholesalecoin) ? wholesalecoin : string.Empty,
                                   WHOLESALEROUNDING = !string.IsNullOrEmpty(wholesalerounding) ? wholesalerounding : string.Empty,
                                   WHOLESALEPERCENT = !string.IsNullOrEmpty(wholesalepercent) ? wholesalepercent : string.Empty,
                                   RETAILCOIN = !string.IsNullOrEmpty(retailcoin) ? retailcoin : string.Empty,
                                   RETAILROUNDING = !string.IsNullOrEmpty(retailrounding) ? retailrounding : string.Empty,
                                   RETAILPERCENT = !string.IsNullOrEmpty(retailpercent) ? retailpercent : string.Empty,
                                   STORECREDIT = !string.IsNullOrEmpty(storecredit) ? storecredit : string.Empty,
                                   GMUSEQOO = !string.IsNullOrEmpty(gmuseqoo) ? gmuseqoo : string.Empty,
                                   TRUEQOO = !string.IsNullOrEmpty(trueqoo) ? trueqoo : string.Empty,
                                   GMSTOCKOUTMSG = !string.IsNullOrEmpty(gmstockoutmsg) ? gmstockoutmsg : string.Empty,
                                   GMSTOCKLOWMSG = !string.IsNullOrEmpty(gmstocklowmsg) ? gmstocklowmsg : string.Empty,
                                   TRSTOCKLOWMSG = !string.IsNullOrEmpty(trstocklowmsg) ? trstocklowmsg : string.Empty,
                                   TRSTOCKOUTMSG = !string.IsNullOrEmpty(trstockoutmsg) ? trstockoutmsg : string.Empty,
                                   TXUSEDSTOCKOUTMSG = !string.IsNullOrEmpty(txusedstockoutmsg) ? txusedstockoutmsg : string.Empty,
                                   TXUSEDSTOCKLOWMSG = !string.IsNullOrEmpty(txusedstocklowmsg) ? txusedstocklowmsg : string.Empty,
                                   TXNEWSTOCKLOWMSG = !string.IsNullOrEmpty(txnewstocklowmsg) ? txnewstocklowmsg : string.Empty,
                                   TXNEWSTOCKOUTMSG = !string.IsNullOrEmpty(txnewstockoutmsg) ? txnewstockoutmsg : string.Empty,
                                   TXUSEDLOWQTY = !string.IsNullOrEmpty(txusedlowqty) ? txusedlowqty : string.Empty,
                                   TXUSEDOOSQTY = !string.IsNullOrEmpty(txusedoosqty) ? txusedoosqty : string.Empty,
                                   GMLOWQTY = !string.IsNullOrEmpty(gmlowqty) ? gmlowqty : string.Empty,
                                   GMOOSQTY = !string.IsNullOrEmpty(gmoosqty) ? gmoosqty : string.Empty,
                                   TRLOWQTY = !string.IsNullOrEmpty(trlowqty) ? trlowqty : string.Empty,
                                   TROOSQTY = !string.IsNullOrEmpty(troosqty) ? troosqty : string.Empty,
                                   TXNEWOOSQTY = !string.IsNullOrEmpty(txnewoosqty) ? txnewoosqty : string.Empty,
                                   TXNEWLOWQTY = !string.IsNullOrEmpty(txnewlowqty) ? txnewlowqty : string.Empty,
                                   TXIMAGEPATH = !string.IsNullOrEmpty(tximagepath) ? tximagepath : string.Empty,
                                   TRIMAGEPATH = !string.IsNullOrEmpty(trimagepath) ? trimagepath : string.Empty,
                                   GMIMAGEPATH = !string.IsNullOrEmpty(gmimagepath) ? gmimagepath : string.Empty,
                                   SHOWOOSTX = !string.IsNullOrEmpty(showoostx) ? showoostx : string.Empty,
                                   SHOWOOSTR = !string.IsNullOrEmpty(showoostr) ? showoostr : string.Empty,
                                   SHOWOOSGM = !string.IsNullOrEmpty(showoosgm) ? showoosgm : string.Empty,
                                   ISPICKUPVALID = !string.IsNullOrEmpty(ispickupvalid) ? ispickupvalid : string.Empty,
                                   STOREID = storeId
                               };

            AdminSettingsDao.SaveSettings(storeId, myObject, userName, userPwd, dbType, uvAddress, uvAccount, cacheTIme,
                                          dblCache, strd3PortNumber, useEncryption, d3PortNumber);
        }

        /// <summary>
        /// Get IWEB Settings
        /// </summary>
        /// <param name="storeId"></param>
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
        public AdminModel GetSettings(string storeId, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme,
            string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            var myObject = new
                               {
                                   STOREID = storeId
                               };


            return AdminSettingsDao.GetSettings(storeId, myObject, userName, userPwd, dbType, uvAddress, uvAccount,
                                                cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);
        }
    }
}
