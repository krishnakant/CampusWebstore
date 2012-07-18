

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



    public interface ITermDaos
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
        IEnumerable<TermModel> GetTerms(string storeId, string callName, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);
        /// <summary>
        /// Verify the terms 
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
        void VerifyTerms(string storeId, object myVars, string userName, string userPwd, string dbType,
                     string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                     string useEncryption, string d3PortNumber);

        #endregion
    }

    public class TermDaos : ITermDaos
    {
        
        #region Properties
        [Dependency]
        public IDbAccess DbAccess { get; set; }
       
        #endregion
        
        #region Public Methods

        public IEnumerable<TermModel> GetTerms(string storeId, string callName, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
                var strPickDataReturn= DbAccess.GetStringResult(storeId, callName, myVars, userName, userPwd, dbType, uvAddress,
                                                uvAccount,cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);

                var doc = new XmlDocument();

                doc.LoadXml(strPickDataReturn);

                var xmlTerm = XElement.Parse(strPickDataReturn);

                var termModel = (from terms in xmlTerm.Descendants("TERM")
                                 let xElement = terms.Element("ID")
                                 where xElement != null
                                 let element = terms.Element("DESC")
                                 where element != null
                                 select new TermModel
                                 {

                                     TermId = xElement.Value,

                                     Name = element.Value,

                                 }).ToList();

                return termModel;
            }
            catch(Exception x)
            {
                throw;
            }
        }
        public void VerifyTerms(string storeId,object myVars, string userName, string userPwd, string dbType,
                     string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                     string useEncryption, string d3PortNumber)
        {
            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.VerifyTerm, myVars, userName, userPwd, dbType, uvAddress,
                                              uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);

            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlTerm = XElement.Parse(strPickDataReturn);

        }

        #endregion
    }
}
