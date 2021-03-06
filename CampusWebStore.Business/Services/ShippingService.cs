﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CampusWebStore.Data.Daos;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;

namespace CampusWebStore.Business.Services
{
    public interface IShippingService
    {
        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Shipping Methods Details....
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="userid"></param>
        /// <param name="ecourse"></param>
        /// <param name="etype"></param>
        /// <param name="eqty"></param>
        /// <param name="eprice"></param>
        /// <param name="eisbn"></param>
        /// <param name="allowsub"></param>
        /// <param name="shiptoname"></param>
        /// <param name="shipaddress"></param>
        /// <param name="shipaddress2"></param>
        /// <param name="shipcity"></param>
        /// <param name="shipstate"></param>
        /// <param name="shipzip"></param>
        /// <param name="shipcountry"></param>
        /// <param name="shipinstruc"></param>
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
        ShippingRateModel GetShippingMethodDetails(string storeId, string userid, string ecourse, string etype, string eqty,
                                      string eprice, string eisbn, string allowsub, string shiptoname,
                                      string shipaddress, string shipaddress2, string shipcity, string shipstate,
                                      string shipzip, string shipcountry,
                                      string shipinstruc, string userName, string userPwd, string dbType,
                                      string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                                      string strd3PortNumber, string useEncryption, string d3PortNumber, bool useLiveRates);


        TaxModel GetShippingTax(string storeId, string displaytable, string confnumber, string frieghttable,
                                      string frieghtamount,string userName, string userPwd, string dbType,
                                      string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                                      string strd3PortNumber, string useEncryption, string d3PortNumber);


        CheckoutMessageModel DoPayment(string storeId, string confnumber, string userid, string cardnumber, string cid,
                       string expirydate, string promocode, string cardtype, string userName, string userPwd,
                       string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber);

        #endregion
    }

    class ShippingService : IShippingService
    {

        #region Properties
        [Dependency]
        public IShippingDao ShippingDao { get; set; }
        #endregion

        /// <summary>
        /// Get Shipping Details...
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="userid"></param>
        /// <param name="ecourse"></param>
        /// <param name="etype"></param>
        /// <param name="eqty"></param>
        /// <param name="eprice"></param>
        /// <param name="eisbn"></param>
        /// <param name="allowsub"></param>
        /// <param name="shiptoname"></param>
        /// <param name="shipaddress"></param>
        /// <param name="shipaddress2"></param>
        /// <param name="shipcity"></param>
        /// <param name="shipstate"></param>
        /// <param name="shipzip"></param>
        /// <param name="shipcountry"></param>
        /// <param name="shipinstruc"></param>
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
        public ShippingRateModel GetShippingMethodDetails(string storeId, string userid, string ecourse, string etype, string eqty,
                                             string eprice, string eisbn, string allowsub, string shiptoname,
                                             string shipaddress, string shipaddress2,
                                             string shipcity, string shipstate, string shipzip, string shipcountry,
                                             string shipinstruc,string userName, string userPwd,
                                             string dbType, string uvAddress,
                                             string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber,
                                             string useEncryption, string d3PortNumber, bool useLiveRates)
        {
            var myObject = new
                               {
                                   USERID = userid,
                                   COURSES = ecourse,
                                   MODULES = etype,
                                   QTYS = eqty,
                                   PRICES = eprice,
                                   ISBNS = eisbn,
                                   STOREID = storeId,
                                   ALLOWSUB = allowsub,
                                   SHIPSTATE = shipstate,
                                   SHIPZIP = shipzip,
                                   SHIPINSTRUC = shipinstruc,
                                   SHIPCOUNTRY = shipcountry,
                                   SHIPCITY = shipcity,
                                   SHIPADD2 = shipaddress2,
                                   SHIPADD1 = shipaddress,
                                   SHIPNAME = shiptoname,
                                   SHIPTOSTATE = shipstate,
                                   SHIPTOZIP = shipzip,
                                   SHIPINSTRUCTIONS = shipinstruc,
                                   SHIPTPCOUNTRY = shipcountry,
                                   SHIPTOCITY = shipcity,
                                   SHIPTOADDRESS2 = shipaddress2,
                                   SHIPTOADDRESS1 = shipaddress,
                                   SHIPTONAME = shiptoname
                               };

            return ShippingDao.GetShippingMethodDetails(storeId, myObject, userName, userPwd, dbType, uvAddress,
                                                        uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption,
                                                        d3PortNumber, useLiveRates, shipzip);
        }

        /// <summary>
        /// Get tax from selected shipping method
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="displaytable"></param>
        /// <param name="confnumber"></param>
        /// <param name="frieghttable"></param>
        /// <param name="frieghtamount"></param>
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
        public TaxModel GetShippingTax(string storeId, string displaytable, string confnumber, string frieghttable,
                                      string frieghtamount,string userName, string userPwd, string dbType,
                                      string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                                      string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            var myObject = new
            {
                STOREID = storeId,
                DISPLAYTABLE = displaytable,
                CONFNUMBER = confnumber,
                FREIGHTTABLE = frieghttable,
                FREIGHTAMOUNT = frieghtamount
            };

            return ShippingDao.GetShippingRate(storeId, myObject, userName, userPwd, dbType, uvAddress, uvAccount,
                                               cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);

        }

        public CheckoutMessageModel DoPayment(string storeId, string confnumber, string userid, string cardnumber, string cid,
                       string expirydate, string promocode, string cardtype, string userName, string userPwd,
                       string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            int lengthccminuslastfour = cardnumber.Length - 4;
            string lastfour = cardnumber.Substring(0, lengthccminuslastfour);
            var myObject = new
            {
                STOREID = storeId,
                CONFNUMBER = confnumber,
                USERID = userid,
                CARDNUMBER = cardnumber,
                CARDID = cid,
                EXPDATE = expirydate,
                PROMOCODE = promocode,
                PAYTYPE = cardtype,
                 GIFTCARD = "",
                 LASTFOUR = lastfour
            };

            //object myObject = new
            //{
            //    STOREID = storeid,
            //    CONFNUMBER = Session["CONFNUMBER"].ToString(),
            //    USERID = userID,
            //    CARDNUMBER = ShoppingCreditCardPaymentOptions1.PayCCNumberTextValue,
            //    CARDID = ShoppingCreditCardPaymentOptions1.PayCIDNumberTextValue,
            //    EXPDATE = expdate,
            //    PROMOCODE = ShoppingCreditCardPaymentOptions1.PromoCode,
            //    PAYTYPE = ShoppingCreditCardPaymentOptions1.GetPayCardTypeSelectedValue,
            //    GIFTCARD = giftcardnums,
            //    LASTFOUR = lastfour
            //};

         return   ShippingDao.DoPayment(storeId, myObject, userName, userPwd, dbType, uvAddress, uvAccount,
                                               cacheTIme, dblCache, strd3PortNumber, useEncryption, d3PortNumber);
        }
    }


}
