﻿using System;
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
    public interface IAccountSettingsDao
    {
        #region "Public Methods"

        UserModel ValidUser(string storeId, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);

        string SaltUser(string storeId, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                        string strd3PortNumber, string useEncryption, string d3PortNumber);

        bool CheckUser(string storeId, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);

        bool CheckIsUConnActivated(string storeId, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);

        string CheckIsUConnActivatedMemberID(string storeId, object myVars, string userName, string userPwd, string dbType,
                       string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                       string useEncryption, string d3PortNumber);

        UserModel AddUser(string storeId, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);

        UserModel UpdateUser(string storeId, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber);

        bool ChangePassword(string storeId, object myVars, string userName, string userPwd, string dbType,
                            string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                            string strd3PortNumber,
                            string useEncryption, string d3PortNumber);

        string ResetPassword(string storeId, object myVars, string userName, string userPwd, string dbType,
                             string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                             string strd3PortNumber,string useEncryption, string d3PortNumber);

        #endregion
    }

    public class AccountSettingsDao : IAccountSettingsDao
    {
        #region  "Properties"

        [Dependency]
        public IDbAccess DbAccess { get; set; }

        #endregion

        /// <summary>
        /// Login to validate user
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
        public UserModel ValidUser(string storeId,  object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber)
        {
            var objUserModel = new UserModel();

           

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.LoginUser, myVars, userName, userPwd, dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);

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
                                    UserName = user.Element("LOGINID").Value,
                                    FirstName = user.Element("FIRSTNAME").Value,
                                    LastName = user.Element("LASTNAME").Value,
                                    Address = user.Element("ADDRESS1").Value,
                                    Address2 = user.Element("ADDRESS2").Value,
                                    City = user.Element("CITY").Value,
                                    State = user.Element("STATE").Value,
                                    Zip = user.Element("ZIP").Value,
                                    Country = user.Element("COUNTRY").Value,
                                    DayPhone = user.Element("DAYPHONE").Value,
                                    Evening = user.Element("EVEPHONE").Value,
                                    Email = user.Element("EMAIL").Value,
                                    ShipToName = user.Element("SHIPTONAME").Value,
                                    ShipAddress = user.Element("SHIPTOADDRESS1").Value,
                                    ShipAddress2 = user.Element("SHIPTOADDRESS2").Value,
                                    ShipCity = user.Element("SHIPTOCITY").Value,
                                    ShipState = user.Element("SHIPTOSTATE").Value,
                                    ShipZip = user.Element("SHIPTOZIP").Value,
                                    ShipCountry = user.Element("SHIPTOCOUNTRY").Value,
                                    ShipInstruc = user.Element("SHIPINSTRUCTIONS").Value,
                                    EmailOptIn = user.Element("OPTIN").Value,
                                    Type = user.Element("TYPE").Value,
                                    TypeDescrition = user.Element("TYPEDESC").Value
                                }).SingleOrDefault();
            }

            return objUserModel;
        }

        public string SaltUser(string storeId, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber)
        {

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.SaltUser, myVars, userName, userPwd, dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);

            //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                return string.Empty;
            }


            return xmlResult.Descendants("VALUE").SingleOrDefault().Value;

        }

        /// <summary>
        /// Check username is available or not..
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
        public bool CheckUser(string storeId,  object myVars, string userName, string userPwd, string dbType,
                     string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                     string useEncryption, string d3PortNumber)
        {

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.CheckUser, myVars, userName, userPwd, dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);

            //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                #region "Email"
                //string test = strPickDataReturn; 

                //string debugvalue = System.Configuration.ConfigurationManager.AppSettings["DEBUG"] as string; 

                //if (debugvalue == "TRUE")
                //{
                //    //IWEB.eMail.SendMail("shane.miller@total-computing.com", "shane.miller@total-computing.com", "XML REPORT", test, "SHANE TEST", false);
                //}
                #endregion
                return false;
            }

            return true;
        }

        /// <summary>
        /// To check if user is UConn Activated...
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="myUserId"></param>
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
        /// <returns>true if exist otherwise false</returns>
        public bool CheckIsUConnActivated(string storeId, object myUserId, string userName, string userPwd, string dbType,
                     string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                     string useEncryption, string d3PortNumber)
        {

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.CheckIsUConnActivated, myUserId, userName, userPwd, dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);

            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                return false;
            }

          
            if (xmlResult.Descendants("ACTIVATED").SingleOrDefault().Value == "False")
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// To check if user is UConn Activated...
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="myUserId"></param>
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
        /// <returns>true if exist otherwise false</returns>
        public string CheckIsUConnActivatedMemberID(string storeId, object myUserId, string userName, string userPwd, string dbType,
                     string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                     string useEncryption, string d3PortNumber)
        {
            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.CheckIsUConnActivatedMemberId, myUserId, userName, userPwd, dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                return "ERROR";
            }

            return xmlResult.Descendants("SUCCESS").SingleOrDefault().Value;
        }

        /// <summary>
        /// Add User...
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
        public UserModel AddUser(string storeId, object myVars, string userName, string userPwd, string dbType,
                        string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                        string useEncryption, string d3PortNumber)
        {

            var objUserModel = new UserModel();

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.AddUser, myVars, userName, userPwd,
                                                             dbType, uvAddress, uvAccount, cacheTIme, dblCache,
                                                             strd3PortNumber, useEncryption, d3PortNumber);

            //Read XML result data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            //Check if any error
            if (xmlResult.Descendants("ERROR").Any())
            {
                //If yes then set error message
                objUserModel.ErrorMsg = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {
                //If not then update other details..
                objUserModel = (from user in xmlResult.Descendants("LOGIN")
                                select new UserModel
                                           {
                                               UserName = user.Element("LOGINID").Value,
                                               FirstName = user.Element("FIRSTNAME").Value,
                                               LastName = user.Element("LASTNAME").Value,
                                               Address = user.Element("ADDRESS1").Value,
                                               Address2 = user.Element("ADDRESS2").Value,
                                               City = user.Element("CITY").Value,
                                               State = user.Element("STATE").Value,
                                               Zip = user.Element("ZIP").Value,
                                               Country = user.Element("COUNTRY").Value,
                                               DayPhone = user.Element("DAYPHONE").Value,
                                               Evening = user.Element("EVEPHONE").Value,
                                               Email = user.Element("EMAIL").Value,
                                               ShipToName = user.Element("SHIPTONAME").Value,
                                               ShipAddress = user.Element("SHIPTOADDRESS1").Value,
                                               ShipAddress2 = user.Element("SHIPTOADDRESS2").Value,
                                               ShipCity = user.Element("SHIPTOCITY").Value,
                                               ShipState = user.Element("SHIPTOSTATE").Value,
                                               ShipZip = user.Element("SHIPTOZIP").Value,
                                               ShipCountry = user.Element("SHIPTOCOUNTRY").Value,
                                               ShipInstruc = user.Element("SHIPINSTRUCTIONS").Value,
                                               EmailOptIn = user.Element("OPTIN").Value,
                                               Type = user.Element("TYPE").Value,
                                               TypeDescrition = user.Element("TYPEDESC").Value
                                           }).SingleOrDefault();
            }

            return objUserModel;
        }

        /// <summary>
        /// Update User....
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
        public UserModel UpdateUser(string storeId, object myVars, string userName, string userPwd, string dbType,
                       string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                       string useEncryption, string d3PortNumber)
        {

            var objUserModel = new UserModel();

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.UpdateUser, myVars, userName, userPwd, dbType,
                                                            uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                            useEncryption, d3PortNumber);

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
                                    UserName = user.Element("LOGINID").Value,
                                    FirstName = user.Element("FIRSTNAME").Value,
                                    LastName = user.Element("LASTNAME").Value,
                                    Address = user.Element("ADDRESS1").Value,
                                    Address2 = user.Element("ADDRESS2").Value,
                                    City = user.Element("CITY").Value,
                                    State = user.Element("STATE").Value,
                                    Zip = user.Element("ZIP").Value,
                                    Country = user.Element("COUNTRY").Value,
                                    DayPhone = user.Element("DAYPHONE").Value,
                                    Evening = user.Element("EVEPHONE").Value,
                                    Email = user.Element("EMAIL").Value,
                                    ShipToName = user.Element("SHIPTONAME").Value,
                                    ShipAddress = user.Element("SHIPTOADDRESS1").Value,
                                    ShipAddress2 = user.Element("SHIPTOADDRESS2").Value,
                                    ShipCity = user.Element("SHIPTOCITY").Value,
                                    ShipState = user.Element("SHIPTOSTATE").Value,
                                    ShipZip = user.Element("SHIPTOZIP").Value,
                                    ShipCountry = user.Element("SHIPTOCOUNTRY").Value,
                                    ShipInstruc = user.Element("SHIPINSTRUCTIONS").Value,
                                    EmailOptIn = user.Element("OPTIN").Value,
                                    Type = user.Element("TYPE").Value,
                                    TypeDescrition = user.Element("TYPEDESC").Value
                                }).SingleOrDefault();
            }

            return objUserModel;
        }

        /// <summary>
        /// Change password of a user
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
        public bool ChangePassword(string storeId, object myVars, string userName, string userPwd, string dbType,
                       string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                       string useEncryption, string d3PortNumber)
        {
            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.ChangePassword, myVars, userName, userPwd, dbType,
                                                            uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                            useEncryption, d3PortNumber);

            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                return false;
            }

            return true;
        }

        public string ResetPassword(string storeId, object myVars, string userName, string userPwd, string dbType,
                             string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                             string strd3PortNumber,string useEncryption, string d3PortNumber)
        {
            var email = string.Empty;

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.ResetPassword, myVars, userName, userPwd, dbType,
                                                           uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                           useEncryption, d3PortNumber);

            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                email = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {
               email = xmlResult.Descendants("EMAIL").SingleOrDefault().Value; 
            }

            return email;
        }
    }
}
