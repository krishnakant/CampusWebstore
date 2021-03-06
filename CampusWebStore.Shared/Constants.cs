﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CampusWebStore.Shared
{
    public class Constants
    {
        public static string StoreNumber = "1";

        public static string GetStores = "IWEB.GET.STORE";

        public static string GetTerms = "IWEB.GET.TERM";

        public static string GetDepartments = "IWEB.GET.DEPT";

        public static string GetCourse = "IWEB.GET.COURSE";

        public static string GetSections = "IWEB.GET.SECTION";

        public static string LoginUser = "IWEB.LOGIN";

        public static string SaltUser = "IWEB.GET.SALT";

        public static string CheckUser = "IWEB.CHECK.USERNAME";

        public static string CheckIsUConnActivated = "IWEB.UCONN.CHECK.MEMBERSHIP";

        public static string CheckIsUConnActivatedMemberId = "IWEB.UCONN.ACTIVATION";

        public static string AddUser = "IWEB.CUST.ADD";

        public static string UpdateUser = "IWEB.CUST.UPDATE";

        public static string ChangePassword = "IWEB.CHANGE.PASSWORD";

        public static string ResetPassword = "IWEB.SET.PWD";

        public static string GetBulkAddOption = "IWEB.BULK.ADOPTIONS";

        public static string GetCatalogs = "IWEB.GET.CATALOG";

        public static string GetCatalogItems = "IWEB.GET.CATALOG.ITEMS";

        public static string GetItems = "IWEB.GET.ITEM";

        public static string GetShippingMethodDetails = "IWEB.FREIGHT";

        public static string GetTax = "IWEB.GET.TAX";

        public static string GetOpenOrders = "IWEB.OPEN.ORDERS";

        public static string OrderPayment = "IWEB.PAYMENT";

        public static string SellBack = "IWEB.SELLBACK";

        public static string VerifyTerm = "IWEB.VERIFY.TERM";

        public static string CustomerList = "IWEB.GET.CUSTOMER.LIST";

        public static string GetCustomerDetail = "IWEB.GET.CUSTOMER.DETAIL";

        public static string GetOrderDetail = "IWEB.ORDER.DETAIL";

        public static string GetOrderHistory = "IWEB.ORDER.HISTORY";

        public static string OrderToClose = "IWEB.CLOSE.ORDERS";

        public static string IwebSetSettings = "IWEB.SET.GENPARMS";
        
        public static string GetTopCatalogs = "IWEB.INIT";

        public static string SellBackParams = "IWEB.GET.SELLBACKPARMS";

        public static string SetsellBackParams = "IWEB.SET.SELLBACK.PARMS";
       

        Constants()
        {

        }
        /// <summary>
        /// MD5 Method to salt string with cryptography....
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMd5Hash(string input)
        {
            var x = new System.Security.Cryptography.MD5CryptoServiceProvider();

            var bs = Encoding.UTF8.GetBytes(input);

            bs = x.ComputeHash(bs);

            var s = new StringBuilder();

            foreach (var b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }

            var password = s.ToString();

            return password;
        }
    }
}
