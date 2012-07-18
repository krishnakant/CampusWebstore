using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using CampusWebStore.Models;
using CampusWebStore.Business.Services;
using CampusWebStore.Shared;
using Microsoft.Practices.Unity;
using CampusWebStore.Shared.Models;
using IWEB.Connector;
 
namespace CampusWebStore.Controllers
{

    public class ShippingController : WebController
    {
        #region "Properties"

        [Dependency]
        public IShippingService ShippingService { get; set; }

        #endregion
        //
        // GET: /Shipping/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get Shipping method list...
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonGetShippingMethodDetails()
        {

            var objUserModel = (UserModel) Session["USERINFO"];

            var listCartItems = ((List<ShoppingCartModel>) Session["CARTITEMS"]);

            var allowSub = Convert.ToString(Convert.ToBoolean(Request["IsAllowSubscription"]));

            var isShippingThisTime = Convert.ToBoolean(Request["IsShippingThisTime"]);

            var shiptoname = Request["ShipToName"];

            var shipaddress = Request["ShipAddress"];

            var shipaddress2 = Request["ShipAddress2"];

            var shipcity = Request["ShipCity"];

            var shipstate = Request["ShipState"];

            var shipzip = Request["ShipZip"];

            var shipcountry = Request["ShipCountry"];

            var shipinstruc = Request["ShipInstructions"];

            if (!isShippingThisTime)
            {
                shiptoname = objUserModel.ShipToName;

                shipaddress = objUserModel.ShipAddress;

                shipaddress2 = objUserModel.ShipAddress2;

                shipcity = objUserModel.ShipCity;

                shipstate = objUserModel.ShipState;

                shipzip = objUserModel.ShipZip;

                shipcountry = objUserModel.ShipCountry;

                shipinstruc = objUserModel.ShipInstruc;
            }
           
            var eisbn = string.Empty;

            var enewused = string.Empty;

            var eprice = string.Empty;

            var eqty = string.Empty;

            var ecourse = string.Empty;

            var etype = string.Empty;

            foreach (var objCartItem in listCartItems)
            {

                etype = etype + objCartItem.Type + "]";

                eisbn = eisbn + objCartItem.Isbn + "]";

                enewused = enewused + objCartItem.NewUsed + "]";

                eprice = eprice + objCartItem.ActualPrice.ToString("n") + "]";

                eqty = eqty + Convert.ToString(objCartItem.Qty) + "]";

                ecourse = ecourse + objCartItem.Course + "]";
            }

            //***** Change Krishna 29-5-2012 *****
            bool useLiveRates  = objBaseMainConfigAdmin.IwebConfigAdmin.UseLiveShippingRates;

            var objShippingRateModel = ShippingService.GetShippingMethodDetails(StoreNumber,
                                                                                 objUserModel.UserName, ecourse, etype,
                                                                                 eqty, eprice, eisbn, allowSub,
                                                                                 shiptoname, shipaddress, shipaddress2,
                                                                                 shipcity, shipstate, shipzip,
                                                                                 shipcountry, shipinstruc, UvUsername,
                                                                                 UvPassword, DbType, UvAddress,
                                                                                 UvAccount, CacheTime, CacheTime,
                                                                                 Strd3PortNumber, UseEncryption,
                                                                                 Strd3PortNumber, useLiveRates);
            //Check if there is no error

           
            if(!string.IsNullOrEmpty(objShippingRateModel.ErrorMsg))
            {
                return Json(new {success = false, error = objShippingRateModel.ErrorMsg});
            }

            Session["CONFNUMBER"] = objShippingRateModel.ConfNumber;

            return Json(new {success = true, objShippingRateModel.ShippingMethods});
        }

        /// <summary>
        /// Get tax of shopping cart accoding to selected shipping...
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonGetShippingTax()
        {
            var shipname = Request["ShipName"];

            var amount = Request["Amount"];

            var table = Request["Table"];

            var confnumber = Convert.ToString(Session["CONFNUMBER"]);

            var objTaxModel = ShippingService.GetShippingTax(StoreNumber, shipname, confnumber, table, amount,
                                                             UvUsername, UvPassword, DbType, UvAddress, UvAccount,
                                                             CacheTime, CacheTime, Strd3PortNumber, UseEncryption,
                                                             Strd3PortNumber);


            //Check if there is no error
            if (!string.IsNullOrEmpty(objTaxModel.ErrorMsg))
            {
                return Json(new { success = false, error = objTaxModel.ErrorMsg });
            }

            return Json(new {success = true, objTaxModel});
        }

        /// <summary>
        /// Do payment of shopping cart
        /// </summary>
        /// <returns></returns>
        public JsonResult JsonDoPayment()
        {
            var dtPayment = new DataTable();
            
            var cardtype = Request["CardType"];

            var expdate = Request["ExpiryDate"];

            var cardno = Request["CardNo"];

            var cid = Request["CIDNo"];

            var promocode = Request["PromoCode"];

            var subtotal = Request["SubTotal"];
            
             subtotal = "$0.1";

            var objUserModel = (UserModel)Session["USERINFO"];

            var confnumber = Convert.ToString(Session["CONFNUMBER"]);

            var checkoutMessageModel = new CheckoutMessageModel();

            switch (cardtype)
            {
                case "CC":
                    {
                        if (!string.IsNullOrEmpty(cardno) && !string.IsNullOrEmpty(cid))
                        {
                            dtPayment = TryPayment(cardno, expdate, cid, subtotal);
                        }
                    }
                    break;
                case "GC":
                case "FA":
                case "CA":
                    dtPayment = TryPayment(cardno, expdate, cid, subtotal);
                    break;
                case "NA":
                    {
                        return Json(new {success = false, errorMsg = "Please select method of Payment."});
                    }
                default:
                    {
                        return Json(new {success = false, errorMsg = "Please select method of Payment."});
                    }
            }

            if (dtPayment !=null &&dtPayment.Rows.Count>0)
            {

                var verified = Convert.ToInt16(dtPayment.Rows[0].ItemArray.GetValue(0).ToString());

                if (verified == 1)
                {
                    //credit card is valid then do payment
                    checkoutMessageModel = ShippingService.DoPayment(StoreNumber, confnumber, objUserModel.UserName, cardno, cid, expdate,
                                              promocode, cardtype, UvUsername, UvPassword, DbType, UvAddress, UvAccount,
                                              CacheTime, CacheTime, Strd3PortNumber, UseEncryption,
                                              Strd3PortNumber);
                }
                else
                {
                    //credit card is no good!
                    var errorcode = dtPayment.Rows[0]["MessageCode"].ToString();

                    switch (errorcode)
                    {
                        case "D101":
                            {
                                // Still do payment...
                              checkoutMessageModel=  ShippingService.DoPayment(StoreNumber, confnumber, objUserModel.UserName, cardno, cid, expdate,
                                             promocode, cardtype, UvUsername, UvPassword, DbType, UvAddress, UvAccount,
                                             CacheTime, CacheTime, Strd3PortNumber, UseEncryption,
                                             Strd3PortNumber);
                            }
                            break;

                        default:
                            {
                                return
                                    Json(new { success = false, errorMsg = dtPayment.Rows[0].ItemArray.GetValue(1).ToString() });
                            }
                    }
                }
            }
            else
            {
                
              checkoutMessageModel=  ShippingService.DoPayment(StoreNumber, confnumber, objUserModel.UserName, cardno, cid, expdate,
                                             promocode, cardtype, UvUsername, UvPassword, DbType, UvAddress, UvAccount,
                                             CacheTime, CacheTime, Strd3PortNumber, UseEncryption,
                                             Strd3PortNumber);
            }

            Session["ADMINEMAILADDRESSLIST"] = checkoutMessageModel.Emails;
            Session["TMPORDERNUMBER"] = checkoutMessageModel.TrasactionNumber;
            Session["SHIPSELECTED"] = null;
           
            return Json(new {success = true});
        }

        protected DataTable TryPayment(string cardno,string expirydate,string cid, string subtotal)
        {
            var dtValidate = new DataTable();

            var tmpDoc1 = new XmlDocument();

            var objUserModel = (UserModel) Session["USERINFO"];

            var bill1 = objUserModel.Address;

            var bill2 = objUserModel.Address2;

            var billcity = objUserModel.City;

            var billstate = objUserModel.State;

            var billzip = objUserModel.Zip;

            var merchid = ConfigurationManager.AppSettings["MERCHID"];

            var merchuser = ConfigurationManager.AppSettings["MERCHUSERID"];

            var custcode = ConfigurationManager.AppSettings["MERCHCUSTCODE"];

            var merchpass = ConfigurationManager.AppSettings["MERCHPASS"];

            var appsurl = ConfigurationManager.AppSettings["APPSURL"];

            try
            {

                //***** Change Krishna 28-5-2012 *****
                //...Need to build xml to validate...
                if (objBaseMainConfigAdmin.IwebConfigAdmin.ValidateCard)
                {
                    dtValidate = TryToValidate(tmpDoc1, bill1, bill2, billzip, billstate, billcity, cardno, expirydate, cid, merchuser, custcode, merchpass, appsurl, merchid);
                }

            }
            catch (Exception)
            {
                return null;
            }
           
            return dtValidate;
        }

       

        protected DataTable TryToValidate(XmlDocument tmpDoc1, string bill1, string bill2, string billzip, string billstate, string billcity,string cardno,string expdate,
           string cid, string merchuser, string custcode, string merchpass, string appsurl, string merchid)
        {

            tmpDoc1.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?><TLTransaction><TLRequest><UserCode>" +
                                merchuser + "</UserCode><Password>" + merchpass + "</Password><CustCode>" + custcode +
                                "</CustCode><MerchantID>" + merchid + "</MerchantID><AccountNumber>" + cardno +
                                "</AccountNumber><ExpireDate>" + expdate +
                                "</ExpireDate><Amount>1.00</Amount><ValidAddressOnly>1</ValidAddressOnly><Date>" +
                                DateTime.Today.ToShortDateString() + "</Date><BillAddress1>" + bill1 +
                                "</BillAddress1><BillAddress2>" + bill2 + "</BillAddress2><BillCity>" + billcity +
                                "</BillCity><BillState>" + billstate + "</BillState><BillZip>" + billzip +
                                "</BillZip><CardID>" + cid + "</CardID><Method>A</Method></TLRequest></TLTransaction>");

            var ccdoc = GetCcAuth(tmpDoc1, appsurl);

            var tmpDS = new DataSet();

            var stream = new StringReader(ccdoc.InnerXml);

            tmpDS.ReadXml(stream);

            return tmpDS.Tables["TLResponse"];
        }

        protected XmlDocument GetCcAuth(XmlDocument xDoc, string URL)
        {
            try
            {
                // get the data from the xml document into a byte stream
                var bdata = System.Text.Encoding.UTF8.GetBytes(xDoc.OuterXml);

                // instantiate a web client
                var wc = new System.Net.WebClient();

                // add appropriate headers
                wc.Headers.Add("Content-Type", "text/xml");

                // send data to server, and wait for a response
                var bresp = wc.UploadData(URL, bdata);

                // read the response
                var resp = System.Text.Encoding.ASCII.GetString(bresp);

                var xresp = new XmlDocument();

                xresp.LoadXml(resp);

                // return the xml document response from the server
                return xresp;
            }
            catch
            {
                // your error handler
                return null;
            }

        }
        
        //
        // GET: /Shipping/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Shipping/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Shipping/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Shipping/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Shipping/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Shipping/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Shipping/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
