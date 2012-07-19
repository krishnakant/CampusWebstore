using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CampusWebStore.Data.Daos;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Practices.Unity;

namespace CampusWebStore.Business.Services
{
    public interface ICustomerServices
    {
        #region Properties
        #endregion

        #region Public Methods

        /// <summary>
        /// Get the list of customers
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="searchText"> </param>
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
        /// <returns> return UserModel</returns>
        IEnumerable<CustomerModel> GetCustomerList(string storeId, string searchText, string userName, string userPwd, string dbType, string uvAddress,
                          string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                          string useEncryption, string d3PortNumber);
        /// <summary>
        /// Get the detail of the customers by id
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="cutomerId"></param>
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
        UserModel GetCustomerDetailById(string storeId, string cutomerId, string userName, string userPwd, string dbType, string uvAddress,
                        string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);



        #endregion
    }

    class CustomerServices : ICustomerServices
    {

        #region Properties
        [Dependency]
        public ICustomerDaos CustomerDao { get; set; }

        #endregion

        /// <summary>
        /// Get the list of customers
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="searchText"></param>
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
        public IEnumerable<CustomerModel> GetCustomerList(string storeId, string searchText, string userName, string userPwd, string dbType, string uvAddress,
                          string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                          string useEncryption, string d3PortNumber)
        {
            try
            {
                //Create UserModel object..
                var objUserModel = new UserModel();

                object myObject = new { SEARCHTERM = searchText };

                var customerModels = CustomerDao.GetCustomerList(storeId, myObject, userName, userPwd, dbType,
                                                                 uvAddress, uvAccount, cacheTIme,
                                                                 dblCache, strd3PortNumber, useEncryption,
                                                                 d3PortNumber);

                return customerModels;
            }
            catch
            {
                throw;
            }
        }
        public UserModel GetCustomerDetailById(string storeId, string cutomerId, string userName, string userPwd, string dbType, string uvAddress,
                         string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                         string useEncryption, string d3PortNumber)
        {
            try
            {
                //Create UserModel object..
                var objUserModel = new UserModel();

                object myObject = new { CUSTID = cutomerId };

                var customerDetail = CustomerDao.GetCustomerDetailById(storeId, myObject, userName, userPwd, dbType,
                                                                      uvAddress, uvAccount, cacheTIme,
                                                                      dblCache, strd3PortNumber, useEncryption,
                                                                      d3PortNumber);

                return customerDetail;
            }
            catch
            {
                throw;
            }
        }

    }
}
