



using System.Xml;
using System.Xml.Linq;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;

namespace CampusWebStore.Data.Daos
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Practices.Unity;



    public interface IStoreDaos
    {
        #region Public Methods
        /// <summary>
        /// Get the record for the store
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="callName"></param>
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
        IEnumerable<StoreModel> GetStore(string storeId, string callName, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);
        /// <summary>
        /// Get store for based on the facFlag
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="facFlag"></param>
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
        IEnumerable<StoreModel> GetStore(string storeId,string facFlag, string userName, string userPwd, string dbType,
                      string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                      string useEncryption, string d3PortNumber);

        

        #endregion
    }

    public class StoreDaos : IStoreDaos
    {
        
        #region Properties
        [Dependency]
        public IDbAccess DbAccess { get; set; }
       
        #endregion
        
        #region Public Methods

        public IEnumerable<StoreModel> GetStore(string storeId, string callName, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
                var strPickDataReturn= DbAccess.GetStringResult(storeId, callName, myVars, userName, userPwd, dbType, uvAddress,
                                                uvAccount,cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);
                var doc = new XmlDocument();

                doc.LoadXml(strPickDataReturn);

                var xmlStore = XElement.Parse(strPickDataReturn);

                var storeModels = (from store in xmlStore.Descendants("STORE")
                                   let xElement = store.Element("ID")
                                   where xElement != null
                                   let element = store.Element("NAME")
                                   where element != null
                                   select new StoreModel
                                   {

                                       Id = xElement.Value,

                                       Name = element.Value,

                                   }).ToList();

                return storeModels;
            }
            catch(Exception x)
            {
                throw;
            }
        }
        #endregion


        public IEnumerable<StoreModel> GetStore(string storeId, string facFlag, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

             try
            {
                 object myVars = new { FACFLAG = facFlag };

                var strPickDataReturn= DbAccess.GetStringResult(storeId, Constants.GetStores, myVars, userName, userPwd, dbType, uvAddress,
                                                uvAccount,cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);
                var doc = new XmlDocument();

                doc.LoadXml(strPickDataReturn);

                var xmlStore = XElement.Parse(strPickDataReturn);

                var storeModels = (from store in xmlStore.Descendants("STORE")
                                   let xElement = store.Element("ID")
                                   where xElement != null
                                   let element = store.Element("NAME")
                                   where element != null
                                   select new StoreModel
                                   {

                                       Id = xElement.Value,

                                       Name = element.Value,

                                   }).ToList();

                return storeModels;
            }
            catch(Exception x)
            {
                throw;
            }
        
        }
    }
}
