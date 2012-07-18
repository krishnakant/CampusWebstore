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
    public interface IOrderDaos
    {
        List<OrderModel> GetOpenOrders(string storeId, object myVars, string userName, string userPwd, string dbType,
                           string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                           string strd3PortNumber, string useEncryption, string d3PortNumber);
        /// <summary>
        /// Get the orders based on the userid and trasaction numbers
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
        OrderDetailModel GetOrderDetail(string storeId, object myVars, string userName, string userPwd, string dbType,
                             string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                             string strd3PortNumber, string useEncryption, string d3PortNumber);
        /// <summary>
        /// Get the order history based on the customer id
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
        List<OrderModel> GetOrderHistory(string storeId, object myVars, string userName, string userPwd, string dbType,
                           string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                           string strd3PortNumber, string useEncryption, string d3PortNumber);

        /// <summary>
        /// Close orders which are open...
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
        bool DoCloseOrder(string storeId, object myVars, string userName, string userPwd, string dbType,
                                 string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                                 string strd3PortNumber, string useEncryption, string d3PortNumber);
    }

    public class OrderDaos : IOrderDaos
    {
        #region  "Properties"

        [Dependency]
        public IDbAccess DbAccess { get; set; }

        #endregion

        /// <summary>
        /// Get All Open orders
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
        public List<OrderModel> GetOpenOrders(string storeId, object myVars, string userName, string userPwd, string dbType,
                           string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                           string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            var listOrders = new List<OrderModel>();

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.GetOpenOrders, myVars, userName, userPwd,
                                                             dbType, uvAddress, uvAccount, cacheTIme, dblCache,
                                                             strd3PortNumber, useEncryption, d3PortNumber);

            //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                //objTaxModel.ErrorMsg = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {
                listOrders = (from order in xmlResult.Descendants("OPENORDERITEM")
                              select new OrderModel
                                         {
                                             OrderId = order.Element("TRANSACTION").Value,
                                             UserName = order.Element("NAME").Value,
                                             ConfNumber = order.Element("CONFIRMATION").Value,
                                             DatePlaced = order.Element("DATE").Value,
                                             Amount = order.Element("AMOUNT").Value,
                                             Status = order.Element("STATUS").Value
                                         }).ToList();
            }

            return listOrders;
        }


        public OrderDetailModel GetOrderDetail(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.GetOrderDetail, myVars, userName, userPwd,
                                                           dbType, uvAddress, uvAccount, cacheTIme, dblCache,
                                                           strd3PortNumber, useEncryption, d3PortNumber);
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var orderDetail = new OrderDetailModel();

            //parse the xml
            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                orderDetail.ErrorMsg = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {
                //Header
                var orderHeader = (from order in xmlResult.Descendants("ORDERHEADER")
                               select new {
                                   OrderId = order.Element("ORDERID") != null ? order.Element("ORDERID").Value : "",
                                              UserId = order.Element("USERID") != null ? order.Element("USERID").Value : "",
                                   OrderDate = order.Element("ORDERDATE") != null ? order.Element("ORDERDATE").Value : "",
                                   OrderTime = order.Element("ORDERTIME") != null ? order.Element("ORDERTIME").Value : "",
                                              Status =order.Element("STATUS") != null ? order.Element("STATUS").Value : "",
                                              StatusDate =order.Element("STATUSDATE") != null? order.Element("STATUSDATE").Value: "",
                                              AllowSub = order.Element("ALLOWSUBS") != null ? order.Element("ALLOWSUBS").Value : "",
                                          

                                          }).FirstOrDefault();
                //Header
                var billingModel = (from order in xmlResult.Descendants("BILLING")
                                   select new
                                   {
                                       Name = order.Element("NAME") != null ? order.Element("NAME").Value : "",
                                       Address1 = order.Element("ADDRESS1") != null ? order.Element("ADDRESS1").Value : "",
                                       Address2 = order.Element("ADDRESS2") != null ? order.Element("ADDRESS2").Value : "",
                                       State = order.Element("STATE") != null ? order.Element("STATE").Value : "",
                                       City = order.Element("CITY") != null ? order.Element("CITY").Value : "",
                                       ZipCode = order.Element("ZIP") != null ? order.Element("ZIP").Value : "",
                                       Country = order.Element("COUNTRY") != null ? order.Element("COUNTRY").Value : "",

                                   }).FirstOrDefault();
                //SHIPPING
                var shippingModel = (from order in xmlResult.Descendants("SHIPPING")
                               select new
                               {
                                   Name = order.Element("NAME") != null ? order.Element("NAME").Value : "",
                                   Address1 = order.Element("ADDRESS1") != null ? order.Element("ADDRESS1").Value : "",
                                   Address2 = order.Element("ADDRESS2") != null ? order.Element("ADDRESS2").Value : "",
                                   State = order.Element("STATE") != null ? order.Element("STATE").Value : "",
                                   City = order.Element("CITY") != null ? order.Element("CITY").Value : "",
                                   ZipCode = order.Element("ZIP") != null ? order.Element("ZIP").Value : "",
                                   Country = order.Element("COUNTRY") != null ? order.Element("COUNTRY").Value : "",

                               }).FirstOrDefault();
                //ORDER DETAIL
                var orderDetailModel = (from order in xmlResult.Descendants("ORDERDETAIL")
                                     select new
                                     {
                                         ShippingMethod = order.Element("SHIPMETHOD") != null ? order.Element("SHIPMETHOD").Value : "",
                                         SubTotal = order.Element("SUBTOTAL") != null ? order.Element("SUBTOTAL").Value : "",
                                         Tax = order.Element("TAX") != null ? order.Element("TAX").Value : "",
                                         Freight = order.Element("FREIGHT") != null ? order.Element("FREIGHT").Value : "",
                                         Total = order.Element("TOTAL") != null ? order.Element("TOTAL").Value : "",

                                     }).FirstOrDefault();
                //informitems
                var infoModel = (from order in xmlResult.Descendants("INFODETAIL")
                                        select new
                                        {
                                            FreightType = order.Element("FREIGHTTYPE") != null ? order.Element("FREIGHTTYPE").Value : "",
                                            PromoCode = order.Element("PROMOCODE") != null ? order.Element("PROMOCODE").Value : "",
                                            PaymentDetail = order.Element("PAYMENTDETAILS") != null ? order.Element("PAYMENTDETAILS").Value : "",
                                            GiftCard = order.Element("GIFTCARD") != null ? order.Element("GIFTCARD").Value : "",


                                        }).FirstOrDefault();

                //Items detail
                var itemDetailModel = (from order in xmlResult.Descendants("ITEMDETAILS")
                                 select new OrderItemDetail
                                 {
                                     Title = order.Element("TITLE") != null ? order.Element("TITLE").Value : "",
                                     EbookUrl = order.Element("EBOOKURL") != null ? order.Element("EBOOKURL").Value : "",
                                     EbookInfo = order.Element("EBOOKINFO") != null ? order.Element("EBOOKINFO").Value : "",
                                     Author = order.Element("AUTHOR") != null ? order.Element("AUTHOR").Value : "",
                                     Memo = order.Element("MEMO") != null ? order.Element("MEMO").Value : "",
                                     Qty = order.Element("QTY") != null ? order.Element("QTY").Value : "",
                                     Price = order.Element("PRICE") != null ? order.Element("PRICE").Value : "",
                                     ExtPrice = order.Element("EXTPRICE") != null ? order.Element("EXTPRICE").Value : "",
                                     Type = order.Element("TYPE") != null ? order.Element("TYPE").Value : "",
                                     
                                 }).ToList();

                //Checking for the null 
                if(orderHeader!=null)
                {
                    orderDetail.UserId = orderHeader.UserId;
                    orderDetail.OrderId = orderHeader.OrderId;
                    orderDetail.OrderDate = orderHeader.OrderDate;
                    orderDetail.OrderTime = orderHeader.OrderTime;
                    orderDetail.Status = orderHeader.Status;
                    orderDetail.StatusDate = orderHeader.StatusDate;
                }

                //Checking for the null 
                if(billingModel!=null)
                {
                    orderDetail.Name = billingModel.Name;
                    orderDetail.Address1 = billingModel.Address1;
                    orderDetail.Address2 = billingModel.Address2;
                    orderDetail.City = billingModel.City;
                    orderDetail.State = billingModel.State;
                    orderDetail.Zip = billingModel.ZipCode;
                    orderDetail.Country = billingModel.Country;

                }

                //Checking for the null 
                if (shippingModel != null)
                {
                    orderDetail.ShipToName = shippingModel.Name;
                    orderDetail.ShipAddress1 = shippingModel.Address1;
                    orderDetail.ShipAddress2 = shippingModel.Address2;
                    orderDetail.ShipCity = shippingModel.City;
                    orderDetail.ShipState = shippingModel.State;
                    orderDetail.ShipZip = shippingModel.ZipCode;
                    orderDetail.ShipCountry = shippingModel.Country;

                }

                //Checking for the null 
                if (orderDetailModel != null)
                {
                    orderDetail.ShipMethod = orderDetailModel.ShippingMethod;
                    orderDetail.SubTotal = orderDetailModel.SubTotal;
                    orderDetail.Freight = orderDetailModel.Freight;
                    orderDetail.Total = orderDetailModel.Total;
                    orderDetail.Tax = orderDetailModel.Tax;

                }

                //Checking for the null 
                if (infoModel != null)
                {
                    orderDetail.FreightType = infoModel.FreightType;
                    orderDetail.PromoCode = infoModel.PromoCode;
                    orderDetail.GiftCard = infoModel.GiftCard;
                   
                }

                //Checking for the is there any item 
                if (itemDetailModel.Any())
                {
                    orderDetail.OrderItemDetail = itemDetailModel;

                }
            }

            return orderDetail;
        }

        /// <summary>
        /// Get the order history based on the customer id
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
        public List<OrderModel> GetOrderHistory(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            try
            {
                var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.GetOrderHistory, myVars, userName,
                                                                 userPwd,
                                                                 dbType, uvAddress, uvAccount, cacheTIme, dblCache,
                                                                 strd3PortNumber, useEncryption, d3PortNumber);
                var doc = new XmlDocument();

                doc.LoadXml(strPickDataReturn);

                //parse the xml
                var xmlResult = XElement.Parse(strPickDataReturn);

                //For open orders
                var openOrders = (from order in xmlResult.Descendants("OPENORDERSITEMS")
                                  select new OrderModel
                                             {
                                                 OrderId = order.Element("TRANSACTION") != null ? !order.Element("TRANSACTION").Value.Equals("NULL")? order.Element("TRANSACTION").Value : "NO OPEN ORDERS" : "NO OPEN ORDERS",
                                                 ConfNumber = order.Element("CONFIRMATION") != null ? order.Element("CONFIRMATION").Value != ""? order.Element("CONFIRMATION").Value : "-" : "",
                                                 DatePlaced = order.Element("DATE") != null ? order.Element("DATE").Value != "" ? order.Element("DATE").Value : "-" : "",
                                                 Amount = order.Element("AMOUNT") != null ? order.Element("AMOUNT").Value !=""? order.Element("AMOUNT").Value : "-" : "",
                                                 Status = order.Element("STATUS") != null ? order.Element("STATUS").Value!=""?order.Element("STATUS").Value:"-" : ""
                                             }).ToList();
                
                //Close orders status type "processed"
                var closeOrders = (from order in xmlResult.Descendants("CLOSEDORDERSITEMS")
                                   select new OrderModel
                                              {
                                                  OrderId = order.Element("TRANSACTION") != null ? !order.Element("TRANSACTION").Value.Equals("NULL") ? order.Element("TRANSACTION").Value : "NO CLOSED ORDERS" : "NO CLOSED ORDERS",
                                                  ConfNumber = order.Element("CONFIRMATION") != null ? order.Element("CONFIRMATION").Value != "" ? order.Element("CONFIRMATION").Value : "-" : "",
                                                  DateProcessed = order.Element("DATE") != null ? order.Element("DATE").Value != "" ? order.Element("DATE").Value : "-" : "",
                                                  Amount = order.Element("AMOUNT") != null ? order.Element("AMOUNT").Value != "" ? order.Element("AMOUNT").Value : "-" : "",
                                                  Status = order.Element("STATUS") != null ? order.Element("STATUS").Value != "" ? order.Element("STATUS").Value : "-" : ""
                                              }).ToList();


                var listOrders = openOrders.ToList();

                listOrders.AddRange(closeOrders);

                return listOrders;
            }catch(Exception x)
            {
                throw;
            }
        }


        /// <summary>
        /// To close selected order
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
        public bool DoCloseOrder(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {
            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.OrderToClose, myVars, userName,
                                                                 userPwd, dbType, uvAddress, uvAccount, cacheTIme,
                                                                 dblCache, strd3PortNumber, useEncryption, d3PortNumber);
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            //parse the xml
            var xmlResult = XElement.Parse(strPickDataReturn);

            return !xmlResult.Descendants("ERROR").Any();
        }
    }
}
