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
    public interface ICustomerDaos
    {
        #region "Public Methods"
        /// <summary>
        /// Get the list of the customers
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
        IEnumerable<CustomerModel> GetCustomerList(string storeId, object myVars, string userName, string userPwd, string dbType,
                            string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                            string strd3PortNumber,
                            string useEncryption, string d3PortNumber);
        /// <summary>
        /// Get the detail of the customer by Id
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
        UserModel GetCustomerDetailById(string storeId, object myVars, string userName, string userPwd, string dbType,
                             string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                             string strd3PortNumber,
                             string useEncryption, string d3PortNumber);
        #endregion
    }

    public class CustomerDaos : ICustomerDaos
    {
        #region  "Properties"

        [Dependency]
        public IDbAccess DbAccess { get; set; }

        #endregion

        /// <summary>
        /// Get the list of customers
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
        public IEnumerable<CustomerModel> GetCustomerList(string storeId, object myVars, string userName, string userPwd, string dbType,
                                         string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                                         string strd3PortNumber,
                                         string useEncryption, string d3PortNumber)
        {



            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.CustomerList, myVars, userName, userPwd,
                                                             dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);

            //Read XML data

            var customerModel = new List<CustomerModel>();

            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            //if (xmlResult.Descendants("ERROR").Any())
            //{
            //    //If not set error message
            //    customerModel.ErrorMsg = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            //}
          
                customerModel = (from user in xmlResult.Descendants("CUSTOMER")
                                 select new CustomerModel
                                {
                                    CustId =user.Element("CUSTID")!=null?user.Element("CUSTID").Value:"",
                                    Name = user.Element("NAME")!=null?user.Element("NAME").Value:"",
                                    Phone =user.Element("PHONE") !=null?user.Element("PHONE").Value:"",
                                    Email =user.Element("EMAIL")!=null? user.Element("EMAIL").Value:"",
                                    City =user.Element("CITY") !=null?user.Element("CITY").Value:"",
                                    State =user.Element("STATE") !=null?user.Element("STATE").Value:"",
                                    Zip = user.Element("ZIP") != null ? user.Element("ZIP").Value : "",
                                 }).ToList();
          

            return customerModel;
        }

        public UserModel GetCustomerDetailById(string storeId, object myVars, string userName, string userPwd, string dbType,
                            string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                            string strd3PortNumber,
                            string useEncryption, string d3PortNumber)
        {
            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.GetCustomerDetail, myVars, userName, userPwd,
                                                           dbType,
                                                           uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                           useEncryption, d3PortNumber);
            var objUserModel = new UserModel();

            //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                objUserModel.ErrorMsg = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {
                objUserModel = (from user in xmlResult.Descendants("LOGIN")
                                select new UserModel
                                {
                                    UserName = user.Element("LOGINID")!=null?user.Element("LOGINID").Value:"",
                                    FirstName =user.Element("FIRSTNAME")!=null?user.Element("FIRSTNAME").Value:"",
                                    LastName =user.Element("LASTNAME")!=null?user.Element("LASTNAME").Value:"",
                                    Address =user.Element("ADDRESS1")!=null?user.Element("ADDRESS1").Value:"",
                                    Address2 = user.Element("ADDRESS2")!=null?user.Element("ADDRESS2").Value:"",
                                    City =user.Element("CITY")!=null?user.Element("CITY").Value:"",
                                    State =user.Element("STATE")!=null?user.Element("STATE").Value:"",
                                    Zip =user.Element("ZIP")!=null?user.Element("ZIP").Value:"",
                                    Country = user.Element("COUNTRY")!=null?user.Element("COUNTRY").Value:"",
                                    DayPhone = user.Element("COUNTRY")!=null?user.Element("DAYPHONE").Value:"",
                                    Evening =user.Element("EVEPHONE")!=null? user.Element("EVEPHONE").Value:"",
                                    Email =user.Element("EMAIL")!=null?user.Element("EMAIL").Value:"",
                                    ShipToName =user.Element("SHIPTONAME")!=null?user.Element("SHIPTONAME").Value:"",
                                    ShipAddress = user.Element("SHIPTOADDRESS1")!=null?user.Element("SHIPTOADDRESS1").Value:"",
                                    ShipAddress2 =user.Element("SHIPTOADDRESS2")!=null?user.Element("SHIPTOADDRESS2").Value:"",
                                    ShipCity = user.Element("SHIPTOCITY")!=null?user.Element("SHIPTOCITY").Value:"",
                                    ShipState =user.Element("SHIPTOSTATE")!=null?user.Element("SHIPTOSTATE").Value:"",
                                    ShipZip =user.Element("SHIPTOZIP")!=null?user.Element("SHIPTOZIP").Value:"",
                                    ShipCountry =user.Element("SHIPTOCOUNTRY")!=null?user.Element("SHIPTOCOUNTRY").Value:"",
                                    ShipInstruc =user.Element("SHIPINSTRUCTIONS")!=null?user.Element("SHIPINSTRUCTIONS").Value:"",
                                    EmailOptIn =user.Element("OPTIN")!=null?user.Element("OPTIN").Value:"",
                                    Type =user.Element("TYPE")!=null?user.Element("TYPE").Value:"",
                                    TypeDescrition =user.Element("TYPEDESC")!=null?user.Element("TYPEDESC").Value:"",
                                    AllAccountType = (from ddlAccountType in xmlResult.Descendants("DDINFO") select ddlAccountType.Element("TEXT") != null ? ddlAccountType.Element("TEXT").Value : "")
                                }).SingleOrDefault();
            }

            return objUserModel;
        }

    }
}
