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
    public interface IDepartmentServices
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

        IEnumerable<DepartmentModel> GetDepartments(string storeId, string callName, object myVars, string userName, string userPwd, string dbType,
                     string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                     string useEncryption, string d3PortNumber);

        #endregion
    }

 public class DepartmentServices : IDepartmentServices
    {
        #region Properties
        [Dependency]
     public IDepartmentDaos DepartmentDaos { get; set; }

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
        public IEnumerable<DepartmentModel> GetDepartments(string storeId, string callName, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
                //Calling the method to get the xml form the database
                var departmentModels = DepartmentDaos.GetDepartments(storeId, callName, myVars, userName, userPwd, dbType, uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);

                return departmentModels;
            }
            catch (Exception x)
            {
                throw;
            }
        }
    }
}
