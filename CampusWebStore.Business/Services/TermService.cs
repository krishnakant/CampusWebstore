using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CampusWebStore.Data.Daos;
using System.Web;
using Microsoft.Practices.Unity;
using CampusWebStore.Shared.Models;

//using TotalCompMobil.Data.Daos;
namespace CampusWebStore.Business.Services
{
    public interface ITermServices
    {
        #region Properties
        #endregion

        #region Public Methods
        /// <summary>
        /// Method to get the record for the store
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
      void VerifyTerm(string storeId,string termId, string userName, string userPwd, string dbType,
                   string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                   string useEncryption, string d3PortNumber);
        #endregion
    }

   public class TermServices : ITermServices
    {
        #region Properties
        [Dependency]
        public ITermDaos TermDao { get; set; }

        #endregion

        /// <summary>
        /// Method to get the list of the stores
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
        public IEnumerable<TermModel> GetTerms(string storeId, string callName, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
                //Calling the method to get the xml form the database
                var termModel = TermDao.GetTerms(storeId, callName, myVars, userName, userPwd, dbType, uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);
                
                return termModel;
            }
            catch (Exception x)
            {
                throw;
            }
        }


        public void VerifyTerm(string storeId, string termId,  string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
           try
           {
               var myVars = new { STOREID=storeId,TERMID=termId };

               TermDao.VerifyTerms(storeId,myVars, userName, userPwd, dbType, uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);

             
           }
            catch(Exception x)
            {
                
            }
        }
    }
}
