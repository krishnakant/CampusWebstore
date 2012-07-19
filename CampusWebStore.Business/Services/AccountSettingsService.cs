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
    public interface IAccountSettingsService
    {
        #region Properties
        #endregion

        #region Public Methods

        /// <summary>
        /// Add User in database : Declaration 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="address"></param>
        /// <param name="address2"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <param name="country"></param>
        /// <param name="email"></param>
        /// <param name="dayphone"></param>
        /// <param name="evephone"></param>
        /// <param name="shiptoname"></param>
        /// <param name="shipaddress"></param>
        /// <param name="shipaddress2"></param>
        /// <param name="shipcity"></param>
        /// <param name="shipstate"></param>
        /// <param name="shipzip"></param>
        /// <param name="shipcountry"></param>
        /// <param name="shipinstruc"></param>
        /// <param name="emailoptin"> </param>
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
        UserModel AddUser(string storeId, string login, string password, string firstname, string lastname,
                          string address, string address2, string city,
                          string state, string zip, string country, string email, string dayphone, string evephone,
                          string shiptoname, string shipaddress, string shipaddress2,
                          string shipcity, string shipstate, string shipzip, string shipcountry, string shipinstruc,
                          string emailoptin, string userName, string userPwd, string dbType, string uvAddress,
                          string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                          string useEncryption, string d3PortNumber);

        /// <summary>
        /// To check if user is exist...
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="login"></param>
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
        bool CheckUser(string storeId, string login, string userName, string userPwd,
                       string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber);

        /// <summary>
        /// To check if user is UConn Activated...
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="myUserId"></param>
        /// <param name="userName"> </param>
        /// /// <param name="userPwd"></param>
        /// <param name="dbType"></param>
        /// <param name="uvAddress"></param>
        /// <param name="uvAccount"></param>
        /// <param name="cacheTIme"></param>
        /// <param name="dblCache"></param>
        /// <param name="strd3PortNumber"></param>
        /// <param name="useEncryption"></param>
        /// <param name="d3PortNumber"></param>
        /// <returns>true if exist otherwise false</returns>
        bool CheckIsUConnActivated(string storeId, string myUserId, string userName, string userPwd,
                       string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber);


        /// <summary>
        /// To check UConn Activated using MemberID...
        /// </summary>
        /// <param name="storeId"></param>
        /// /// <param name="memberID"></param>
        /// /// <param name="emailList"></param>
        /// <param name="myUserId"></param>
        /// <param name="userName"> </param>
        /// /// <param name="userPwd"></param>
        /// <param name="dbType"></param>
        /// <param name="uvAddress"></param>
        /// <param name="uvAccount"></param>
        /// <param name="cacheTIme"></param>
        /// <param name="dblCache"></param>
        /// <param name="strd3PortNumber"></param>
        /// <param name="useEncryption"></param>
        /// <param name="d3PortNumber"></param>
        /// <returns>true if exist otherwise false</returns>
        string CheckIsUConnActivatedMemberID(string storeId, string memberID, string emailList, string myUserId, string userName, string userPwd,
                       string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber);

        /// <summary>
        /// Validate User
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
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
        UserModel ValidUser(string storeId, string login, string password, string userName, string userPwd,
                         string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                         string strd3PortNumber, string useEncryption, string d3PortNumber);

        /// <summary>
        /// Update user details..
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="login"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="address"></param>
        /// <param name="address2"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <param name="country"></param>
        /// <param name="email"></param>
        /// <param name="dayphone"></param>
        /// <param name="evephone"></param>
        /// <param name="shiptoname"></param>
        /// <param name="shipaddress"></param>
        /// <param name="shipaddress2"></param>
        /// <param name="shipcity"></param>
        /// <param name="shipstate"></param>
        /// <param name="shipzip"></param>
        /// <param name="shipcountry"></param>
        /// <param name="shipinstruc"></param>
        /// <param name="emailoptin"></param>
        ///    /// <param name="accountType"></param>
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
        UserModel UpdateUser(string storeId, string login, string firstname, string lastname,
                             string address, string address2, string city,
                             string state, string zip, string country, string email, string dayphone, string evephone,
                             string shiptoname, string shipaddress, string shipaddress2,
                             string shipcity, string shipstate, string shipzip, string shipcountry, string shipinstruc,
                             string emailoptin, string accountType, string userName, string userPwd, string dbType, string uvAddress,
                             string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                             string useEncryption, string d3PortNumber);

        bool ChangePassword(string storeId, string login, string oldpassword, string newpassword, string userName,
                            string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                            string strd3PortNumber, string useEncryption, string d3PortNumber);

        string ResetPassword(string storeId, string login, string userName, string newpassword, string userPwd,
                             string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                             string strd3PortNumber, string useEncryption, string d3PortNumber);

        #endregion
    }

    class AccountSettingsService : IAccountSettingsService
    {

        #region Properties
        [Dependency]
        public IAccountSettingsDao AccountSettingsDao { get; set; }

        #endregion

        /// <summary>
        /// To check user is valid or not...
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
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
        public UserModel ValidUser(string storeId, string login, string password, string userName, string userPwd,
                         string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                         string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            //Create UserModel object..
            var objUserModel = new UserModel();


            object myObject1 = new { LOGIN = login };

            var saltstring = AccountSettingsDao.SaltUser(storeId, myObject1, userName, userPwd, dbType,
                                                         uvAddress, uvAccount, cacheTIme,
                                                         dblCache, strd3PortNumber, useEncryption,
                                                         d3PortNumber);

            var md5Password = Constants.GetMd5Hash(saltstring + password);


            var myVars = new { LOGIN = login, MD5 = md5Password + ":" + saltstring, PASSWORD = password };


            //Get XML Result of valid user availablitiy
            objUserModel = AccountSettingsDao.ValidUser(storeId, myVars, userName,
                                                                 userPwd, dbType, uvAddress, uvAccount, cacheTIme,
                                                                 dblCache, strd3PortNumber, useEncryption, d3PortNumber);


            return objUserModel;
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
        public bool CheckIsUConnActivated(string storeId, string myUserId, string userName, string userPwd,
                       string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            var myVars = new { USERID = myUserId };

            return AccountSettingsDao.CheckIsUConnActivated(storeId, myVars, userName,
                                                            userPwd, dbType, uvAddress, uvAccount, cacheTIme,
                                                             dblCache, strd3PortNumber, useEncryption, d3PortNumber);


        }
        /// <summary>
        /// To check UConn Activated using MemberID...
        /// </summary>
        /// <param name="storeId"></param>
        /// /// <param name="memberID"></param>
        /// /// <param name="emailList"></param>
        /// <param name="myUserId"></param>
        /// <param name="userName"> </param>
        /// /// <param name="userPwd"></param>
        /// <param name="dbType"></param>
        /// <param name="uvAddress"></param>
        /// <param name="uvAccount"></param>
        /// <param name="cacheTIme"></param>
        /// <param name="dblCache"></param>
        /// <param name="strd3PortNumber"></param>
        /// <param name="useEncryption"></param>
        /// <param name="d3PortNumber"></param>
        /// <returns>true if exist otherwise false</returns>
        public string CheckIsUConnActivatedMemberID(string storeId, string memberID, string emailList, string myUserId, string userName, string userPwd,
                       string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            var myVars = new
            {
                MEMBERID = memberID,
                JOINEMAILLIST = emailList,
                USERID = myUserId
            };

            return AccountSettingsDao.CheckIsUConnActivatedMemberID(storeId, myVars, userName,
                                                               userPwd, dbType, uvAddress, uvAccount, cacheTIme,
                                                                dblCache, strd3PortNumber, useEncryption, d3PortNumber);
        }

        /// <summary>
        /// To Check user is exist or not
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
        public bool CheckUser(string storeId, string login, string userName, string userPwd,
                       string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            var myObject = new { LOGIN = login };

            //Get XML Result of Check user availablitiy
            return AccountSettingsDao.CheckUser(storeId, myObject, userName,
                                                userPwd, dbType, uvAddress, uvAccount, cacheTIme,
                                                dblCache, strd3PortNumber, useEncryption, d3PortNumber);

        }


        /// <summary>
        /// Add User in database : Implementation 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="address"></param>
        /// <param name="address2"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <param name="country"></param>
        /// <param name="email"></param>
        /// <param name="dayphone"></param>
        /// <param name="evephone"></param>
        /// <param name="shiptoname"></param>
        /// <param name="shipaddress"></param>
        /// <param name="shipaddress2"></param>
        /// <param name="shipcity"></param>
        /// <param name="shipstate"></param>
        /// <param name="shipzip"></param>
        /// <param name="shipcountry"></param>
        /// <param name="shipinstruc"></param>
        /// <param name="emailoptin"> </param>
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
        public UserModel AddUser(string storeId, string login, string password, string firstname, string lastname, string address, string address2, string city,
            string state, string zip, string country, string email, string dayphone, string evephone, string shiptoname, string shipaddress, string shipaddress2,
            string shipcity, string shipstate, string shipzip, string shipcountry, string shipinstruc, string emailoptin, string userName, string userPwd, string dbType, string uvAddress,
            string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            //Create UserModel object..
            var objUserModel = new UserModel();

            try
            {
                //Check user name is available to create new user..

                //Get XML Result of Check user availablitiy
                var isUserNameAvailable = CheckUser(storeId, login, userName,
                                                    userPwd, dbType, uvAddress, uvAccount, cacheTIme,
                                                    dblCache, strd3PortNumber, useEncryption, d3PortNumber);
                if (!isUserNameAvailable)
                {
                    objUserModel.ErrorMsg = "That username is taken";
                }
                else
                {
                    //if yes then add new user with given username and password..
                    var myObject2 = new
                    {
                        LOGIN = login,
                        PASSWORD = password
                    };

                    //Get User Model
                    objUserModel = AccountSettingsDao.AddUser(storeId, myObject2, userName, userPwd,
                                                              dbType, uvAddress, uvAccount, cacheTIme, dblCache,
                                                              strd3PortNumber, useEncryption, d3PortNumber);

                    //Check if any error
                    if (string.IsNullOrEmpty(objUserModel.ErrorMsg))
                    {
                        //If not then update other details..
                        var myObject3 = new
                        {
                            FIRSTNAME = firstname,
                            LASTNAME = lastname,
                            ADDRESS1 = address,
                            ADDRESS2 = !string.IsNullOrEmpty(address2) ? address2 : string.Empty,
                            CITY = city,
                            STATE = state,
                            ZIP = zip,
                            COUNTRY = country,
                            EMAIL = email,
                            DAYPHONE = dayphone,
                            EVEPHONE = evephone,
                            LOGIN = login,
                            SHIPTONAME = shiptoname,
                            SHIPTOADDRESS1 = shipaddress,
                            SHIPTOADDRESS2 = !string.IsNullOrEmpty(shipaddress2) ? shipaddress2 : string.Empty,
                            SHIPTOCITY = shipcity,
                            SHIPTOSTATE = shipstate,
                            SHIPTOZIP = shipzip,
                            SHIPTOCOUNTRY = shipcountry,
                            SHIPINSTRUCTIONS = !string.IsNullOrEmpty(shipinstruc) ? shipinstruc : string.Empty,
                            EMAILOPTIN = "Y"
                        };

                        //Update user...
                        objUserModel = AccountSettingsDao.UpdateUser(storeId, myObject3, userName, userPwd,
                                                                       dbType, uvAddress, uvAccount, cacheTIme, dblCache,
                                                                       strd3PortNumber, useEncryption, d3PortNumber);

                    }
                }

            }
            catch (Exception ex)
            {
                objUserModel.ErrorMsg = ex.Message;
            }

            return objUserModel;
        }

        /// <summary>
        /// Update the user details..
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="login"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="address"></param>
        /// <param name="address2"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <param name="country"></param>
        /// <param name="email"></param>
        /// <param name="dayphone"></param>
        /// <param name="evephone"></param>
        /// <param name="shiptoname"></param>
        /// <param name="shipaddress"></param>
        /// <param name="shipaddress2"></param>
        /// <param name="shipcity"></param>
        /// <param name="shipstate"></param>
        /// <param name="shipzip"></param>
        /// <param name="shipcountry"></param>
        /// <param name="shipinstruc"></param>
        ///  /// <param name="accountType"></param>
        /// <param name="emailoptin"></param>
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
        public UserModel UpdateUser(string storeId, string login, string firstname, string lastname, string address, string address2, string city,
           string state, string zip, string country, string email, string dayphone, string evephone, string shiptoname, string shipaddress, string shipaddress2,
           string shipcity, string shipstate, string shipzip, string shipcountry, string shipinstruc, string emailoptin, string accountType, string userName, string userPwd, string dbType, string uvAddress,
           string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            //Create UserModel object..
            var objUserModel = new UserModel();

            try
            {
                //Set User Properties..
                var myObject = new
                {
                    FIRSTNAME = firstname,
                    LASTNAME = lastname,
                    ADDRESS1 = address,
                    ADDRESS2 = !string.IsNullOrEmpty(address2) ? address2 : string.Empty,
                    CITY = city,
                    STATE = state,
                    ZIP = zip,
                    COUNTRY = country,
                    EMAIL = email,
                    DAYPHONE = dayphone,
                    EVEPHONE = evephone,
                    LOGIN = login,
                    SHIPTONAME = shiptoname,
                    SHIPTOADDRESS1 = shipaddress,
                    SHIPTOADDRESS2 = !string.IsNullOrEmpty(shipaddress2) ? shipaddress2 : string.Empty,
                    SHIPTOCITY = shipcity,
                    SHIPTOSTATE = shipstate,
                    SHIPTOZIP = shipzip,
                    SHIPTOCOUNTRY = shipcountry,
                    SHIPINSTRUCTIONS = !string.IsNullOrEmpty(shipinstruc) ? shipinstruc : string.Empty,
                    EMAILOPTIN = "Y",
                    ACCTYPE = accountType
                };

                //Update user...
                objUserModel = AccountSettingsDao.UpdateUser(storeId, myObject, userName, userPwd,
                                                             dbType, uvAddress, uvAccount, cacheTIme, dblCache,
                                                             strd3PortNumber, useEncryption, d3PortNumber);
            }
            catch (Exception ex)
            {
                objUserModel.ErrorMsg = ex.Message;
            }

            return objUserModel;
        }

        /// <summary>
        /// To change password of user
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="login"></param>
        /// <param name="oldpassword"></param>
        /// <param name="newpassword"></param>
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
        public bool ChangePassword(string storeId, string login, string oldpassword, string newpassword, string userName, string userPwd,
                         string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                         string strd3PortNumber, string useEncryption, string d3PortNumber)
        {


            object myObject1 = new { LOGIN = login };

            var saltstring = AccountSettingsDao.SaltUser(storeId, myObject1, userName, userPwd, dbType,
                                                         uvAddress, uvAccount, cacheTIme,
                                                         dblCache, strd3PortNumber, useEncryption,
                                                         d3PortNumber);

            var md5Oldpassword = Constants.GetMd5Hash(saltstring + oldpassword);

            md5Oldpassword = md5Oldpassword + ":" + saltstring;

            var newPassword = newpassword;

            var random = new Random();

            var newRandom = random.Next(1000000000, Convert.ToInt32(2100000000));

            var saltMd5 = Constants.GetMd5Hash(Convert.ToString(newRandom));

            var realsalt = saltMd5.Substring(0, 2);

            var saltandpassMD5 = Constants.GetMd5Hash(realsalt + newPassword);

            var md5ToSendToPick = saltandpassMD5 + ":" + realsalt;


            var myVars = new
            {
                LOGIN = login,
                PASSWORD = md5ToSendToPick,
                OLDMD5PASSWORD = md5Oldpassword,
                OLDPASSWORD = oldpassword
            };

            return AccountSettingsDao.ChangePassword(storeId, myVars, userName,
                                                     userPwd, dbType, uvAddress, uvAccount, cacheTIme,
                                                     dblCache, strd3PortNumber, useEncryption, d3PortNumber);

        }

        public string ResetPassword(string storeId, string login, string newpassword, string userName, string userPwd,
                         string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                         string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            var random = new Random();

            var newRandom = random.Next(1000000000, Convert.ToInt32(2100000000));

            var saltMd5 = Constants.GetMd5Hash(newRandom.ToString(CultureInfo.InvariantCulture));

            var realsalt = saltMd5.Substring(0, 2);

            var saltandpassMd5 = Constants.GetMd5Hash(realsalt + newpassword);

            var md5ToSendToPick = saltandpassMd5 + ":" + realsalt;

            var myVars = new
            {
                LOGIN = login,
                MD5 = md5ToSendToPick
            };

            return AccountSettingsDao.ResetPassword(storeId, myVars, userName,
                                                    userPwd, dbType, uvAddress, uvAccount, cacheTIme,
                                                    dblCache, strd3PortNumber, useEncryption, d3PortNumber);

        }
    }
}
