using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CampusWebStore.Data.Daos;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;

namespace CampusWebStore.Business.Services
{
    public interface IOrderService
    {
        List<OrderModel> GetOpenOrders(string storeId, string sortby, string sortdirection, string userName, string userPwd, string dbType,
                          string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                          string strd3PortNumber, string useEncryption, string d3PortNumber);
        /// <summary>
        /// Get the order deatil based on the userid and transaction Number
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="userId"></param>
        /// <param name="transactionNumber"></param>
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
        OrderDetailModel GetOrderDetail(string storeId, string userId, string transactionNumber, string userName, string userPwd, string dbType,
                         string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                         string strd3PortNumber, string useEncryption, string d3PortNumber);


        bool CloseOrder(string storeId, string orders, string userName, string userPwd,
                       string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber);



        /// <summary>
        /// Get the Order history by user id
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="userId"></param>
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
        IEnumerable<OrderModel> GetOrderHistory(string storeId, string userId, string userName, string userPwd, string dbType,
                       string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber);

    }

    public class OrderService : IOrderService
    {
        #region Properties
        [Dependency]
        public IOrderDaos OrderDaos { get; set; }
        #endregion



        public List<OrderModel> GetOpenOrders(string storeId, string sortby, string sortdirection, string userName, string userPwd, string dbType,
                          string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                          string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            var myObject = new
            {
                SORTBY = sortby,
                DIRECCTION = sortdirection
            };

            return OrderDaos.GetOpenOrders(storeId, myObject, userName, userPwd, dbType, uvAddress,
                                     uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption,
                                     d3PortNumber);
        }

        /// <summary>
        /// Get the order deatil based on the userid and transaction Number
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="userId"></param>
        /// <param name="transactionNumber"></param>
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
        public OrderDetailModel GetOrderDetail(string storeId, string userId, string transactionNumber, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            var myObject = new
            {
                USERID = userId,
                ORDERID = transactionNumber
            };

            var orderDetail = OrderDaos.GetOrderDetail(storeId, myObject, userName, userPwd, dbType, uvAddress,
                                     uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption,
                                     d3PortNumber);

            return orderDetail;
        }


        /// <summary>
        /// To Close the selected open orders...
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="orders"></param>
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
        public bool CloseOrder(string storeId, string orders, string userName, string userPwd,
                       string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                       string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            var myObject = new
            {
                STDIDS = orders 
            };

            return OrderDaos.DoCloseOrder(storeId, myObject, userName, userPwd, dbType, uvAddress,
                                     uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption,
                                     d3PortNumber);
        }


        /// <summary>
        /// Get the Order history by user id
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="userId"></param>
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
        public IEnumerable<OrderModel> GetOrderHistory(string storeId, string userId, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            var myObject = new
            {
                USERID = userId,

            };

            var orderModels = OrderDaos.GetOrderHistory(storeId, myObject, userName, userPwd, dbType, uvAddress,
                                     uvAccount, cacheTIme, dblCache, strd3PortNumber, useEncryption,
                                     d3PortNumber);

            return orderModels;
        }

    }
}
