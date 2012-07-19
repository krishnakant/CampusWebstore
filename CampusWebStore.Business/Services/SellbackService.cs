using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CampusWebStore.Data.Daos;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;

namespace CampusWebStore.Business.Services
{
    #region "Interface"

    public interface ISellBackSerivces
    {
        #region Properties

        #endregion

        #region Public Methods
        /// <summary>
        /// Get the data for the book
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
        IEnumerable<SellBackBookModel> GetBookData(string storeId, object myVars, string userName, string userPwd, string dbType,
                         string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                         string useEncryption, string d3PortNumber);

        SellBackParamModel GetSellBackParams(string storeId, object myVars, string userName, string userPwd,
                                             string dbType, string uvAddress, string uvAccount, string cacheTIme,
                                             string dblCache, string strd3PortNumber, string useEncryption,
                                             string d3PortNumber);

        string SetSellBackParams(string storeId, string storeCreditPercent, string retailPercent, string retailRounding,
                                 string retailCoin, string wholeSalepercent, string wholeSaleRounding,
                                 string wholeSaleCoin, string userName, string userPwd, string dbType, string uvAddress,
                                 string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                                 string useEncryption, string d3PortNumber);

        #endregion
    }

    #endregion

    public class SellbackServices : ISellBackSerivces
    {
        #region "Properties"

        [Dependency]
        public ISellBackDaos SellBackDaos { get; set; }

        #endregion

        public IEnumerable<SellBackBookModel> GetBookData(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
             var sellBackBooksModel=   SellBackDaos.GetBookData(storeId, myVars, userName, userPwd, dbType,
                                                              uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                              useEncryption, d3PortNumber);
                return sellBackBooksModel;
            }
            catch (Exception x)
            {
                throw;
            }
        }
         public SellBackParamModel GetSellBackParams(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
         {
             try
             {
                 var sellBackParams = SellBackDaos.GetSellBackParams(storeId, myVars, userName, userPwd, dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);
                 return sellBackParams;
             }
             catch (Exception)
             {
                 
                 throw;
             }
         }
         public string SetSellBackParams(string storeId, string storeCreditPercent, string retailPercent, string retailRounding, string retailCoin, string wholeSalepercent, string wholeSaleRounding, string wholeSaleCoin, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
         {
             SellBackDaos.SetSellBackParams(storeId, storeCreditPercent, retailPercent, retailRounding, retailCoin,
                                            wholeSalepercent, wholeSaleRounding, wholeSaleCoin, userName, userPwd,
                                            dbType, uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                            useEncryption, d3PortNumber);
             return " ";
         }
       
    }
}
