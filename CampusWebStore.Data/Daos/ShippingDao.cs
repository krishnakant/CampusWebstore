using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;
using MAX.USPS;

namespace CampusWebStore.Data.Daos
{
    public interface IShippingDao
    {
        #region "Public Methods"

        ShippingRateModel GetShippingMethodDetails(string storeId, object myVars, string userName, string userPwd, string dbType,
                                      string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                                      string strd3PortNumber, string useEncryption, string d3PortNumber, bool useLiveRates, string shipzip);



        TaxModel GetShippingRate(string storeId, object myVars, string userName, string userPwd, string dbType,
                             string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                             string strd3PortNumber, string useEncryption, string d3PortNumber);

        CheckoutMessageModel DoPayment(string storeId, object myVars, string userName, string userPwd, string dbType,
                             string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                             string strd3PortNumber, string useEncryption, string d3PortNumber);

        #endregion
    }

    public class ShippingDao : IShippingDao
    {
        #region  "Properties"

        [Dependency]
        public IDbAccess DbAccess { get; set; }

        RateResponse _reply;
        #endregion

        /// <summary>
        /// Get Shipping Methods Details...
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
        public ShippingRateModel GetShippingMethodDetails(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme, string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber, bool useLiveRates, string shipzip)
        {

            var objShippingRateModel = new ShippingRateModel();


            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.GetShippingMethodDetails, myVars, userName, userPwd, dbType,
                                                              uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                              useEncryption, d3PortNumber);


             //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                objShippingRateModel.ErrorMsg = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {

                var listShippingMehods = (from shipping in xmlResult.Descendants("OPTION")
                                          select shipping).ToList();

                var objWeight = (from weight in xmlResult.Descendants("WEIGHT")
                                 select new
                                            {
                                                ORIGINATIONZIP = weight.Element("ORIGINATIONZIP").Value,
                                                TOTALDECIMAL = weight.Element("TOTALDECIMAL").Value,
                                                TOTALOUNCES = weight.Element("TOTALOUNCES").Value,
                                                FREEWEIGHTOZ = weight.Element("FREEWEIGHTOZ").Value,
                                            }).SingleOrDefault();


                var flatRateFreightAmount =
                    Convert.ToDecimal(xmlResult.Element("FLATFREIGHTAMOUNT").Value);

                var freeflatRateFreightAmount =
                    Convert.ToDecimal(xmlResult.Element("FREEFLATAMOUNT").Value);

                var totalFreeOunces = objWeight.FREEWEIGHTOZ;

                var freeWeightInOz = Convert.ToDouble(totalFreeOunces) / 100;

                var orgZipCode = objWeight.ORIGINATIONZIP;

                var totalDecimal = objWeight.TOTALDECIMAL;

                var totalOunces = objWeight.TOTALOUNCES;

                var confnumber = xmlResult.Element("CONFIRMATION").Value;

                //Set in model object...
                objShippingRateModel.ConfNumber = confnumber;

                var weightInOz = Convert.ToDouble(totalOunces) / 100;

                foreach (var objShippingMethod in listShippingMehods)
                {
                    var isFreeShipping = Convert.ToBoolean(objShippingMethod.Element("ISFREE").Value);

                    var tcsTableCode = objShippingMethod.Element("TCSCODE").Value;

                    var strLiveRate = objShippingMethod.Element("LIVERATE").Value;

                    var boolLiveRate = Convert.ToBoolean(strLiveRate); //must pass True/False

                    var isLiveRate = boolLiveRate && weightInOz != 0;

                    //this is the display text for the user to see
                    var displayText = objShippingMethod.Element("DISPLAY").Value;

                    //this is USPS,FEDEX,UPS,DHL, etc..
                    var shipperCode = objShippingMethod.Element("SHIPPERCODE").Value;

                    //this is the USPS,FEDEX,UPS name eqivilent
                    var tableType = objShippingMethod.Element("TABLETYPE").Value;

                    //this is the USPS class (could be more later)
                    var tableClassCode = objShippingMethod.Element("TABLECLASS").Value;

                    //this is the extra amount they might want to add to the rate
                    var addAmount = objShippingMethod.Element("ADDAMOUNT").Value;

                    //this is the amount for manual rates
                    var strAmount = objShippingMethod.Element("AMOUNT").Value;

                    var dAmount = Convert.ToDecimal(addAmount);

                    //useLiveRates = true;

                    if (isLiveRate && useLiveRates )
                    {
                            //we have to go get a rate from somewhere
                            decimal liveRate = 0;
                            decimal newCalulatedRate = 0;
                            if (isFreeShipping)
                            {
                                if (freeWeightInOz == 0)
                                {
                                    //the shipping is 100% free for weight, no need to get rate
                                    newCalulatedRate = 0;
                                }
                                else
                                {
                                    liveRate = GetLiveRate(shipperCode, tableType, tableClassCode, shipzip, orgZipCode, freeWeightInOz);
                                }
                                if (liveRate == 0 && freeWeightInOz != 0)
                                {
                                    newCalulatedRate = Convert.ToDecimal(strAmount);
                                    //live rate failed
                                }
                                else
                                {
                                    newCalulatedRate = liveRate + dAmount + freeflatRateFreightAmount;
                                }
                            }
                            else
                            {
                                liveRate = GetLiveRate(shipperCode, tableType, tableClassCode, shipzip, orgZipCode, weightInOz);
                                if (liveRate == 0)
                                {
                                    newCalulatedRate = Convert.ToDecimal(strAmount);
                                }
                                else
                                {
                                    newCalulatedRate = liveRate + dAmount + flatRateFreightAmount;
                                }
                            }

                            objShippingRateModel.ShippingMethods.Add(new ShippingMethod()
                                                          {
                                                              Display = displayText,
                                                              Amount = newCalulatedRate.ToString("C", new CultureInfo("en-us")),
                                                              Table = tcsTableCode
                                                          });
                    }
                    else
                    {
                        //its not a live rate, a manual rate
                        decimal decAmount = Convert.ToDecimal(strAmount);// +dAmount + flatRateFreightAmount;

                        objShippingRateModel.ShippingMethods.Add(new ShippingMethod()
                        {
                            Display = displayText,
                            Amount = decAmount.ToString("C", new CultureInfo("en-us")),
                            Table = tcsTableCode
                        });
                    }

              
                    //its not a live rate, a manual rate
                    //var decAmount = Convert.ToDecimal(strAmount);// +dAmount + flatRateFreightAmount;

                    //objShippingRateModel.ShippingMethods.Add(new ShippingMethod()
                    //                              {
                    //                                  Display = displayText,
                    //                                  Amount = decAmount.ToString("c"),
                    //                                  Table = tcsTableCode
                    //                              });

                    var weight = objWeight.TOTALOUNCES;

                    var zipcode = objWeight.ORIGINATIONZIP;

                    var iweight = Convert.ToInt32(weight);

                }

            }
            return objShippingRateModel;
        }

        private decimal GetLiveRate(string shipperCode, string tableType, string tableClassCode, string destZip, string orgZip, double weightInOz)
        {
            decimal returnRate = 0;
            switch (shipperCode)
            {
                case "USPS":
                    {
                        try
                        {
                            if (_reply == null)
                            {

                                USPSManager m = new USPSManager("065TOTAL8347", false);

                                Package pack = new Package();

                                pack.ServiceType = ServiceType.All;
                                pack.PackageSize = PackageSize.Regular;
                                pack.FromAddress.Zip = orgZip;
                                pack.ToAddress.Zip = destZip;
                                pack.WeightInOunces = Convert.ToInt32(weightInOz);
                                pack.Machinable = true;
                                _reply = new RateResponse();
                                _reply = m.GetRate(pack);

                            }

                            foreach (MAX.USPS.Postage postage in _reply.Postage)
                            {

                                if (postage.Class.ToString() == tableClassCode)
                                {
                                    decimal postageAmount = 0;
                                    postageAmount = postage.Amount;
                                    return postageAmount;
                                }



                            }
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                    break;
                case "FEDEX":
                    {
                        try
                        {
                            FedExRates.RateService.ServiceType myService = FedExRates.RateService.ServiceType.FEDEX_GROUND;

                            switch (tableType)
                            {
                                case "FEDEX_GROUND":
                                    {
                                        myService = FedExRates.RateService.ServiceType.FEDEX_GROUND;
                                        break;
                                    }
                                case "EUROPE_FIRST_INTERNATIONAL_PRIORITY":
                                    {
                                        myService = FedExRates.RateService.ServiceType.EUROPE_FIRST_INTERNATIONAL_PRIORITY;
                                        break;
                                    }
                                case "FEDEX_1_DAY_FREIGHT":
                                    {
                                        myService = FedExRates.RateService.ServiceType.FEDEX_1_DAY_FREIGHT;
                                        break;
                                    }
                                case "FEDEX_2_DAY":
                                    {
                                        myService = FedExRates.RateService.ServiceType.FEDEX_2_DAY;
                                        break;
                                    }
                                case "FEDEX_2_DAY_FREIGHT":
                                    {
                                        myService = FedExRates.RateService.ServiceType.FEDEX_2_DAY_FREIGHT;
                                        break;
                                    }
                                case "FEDEX_3_DAY_FREIGHT":
                                    {
                                        myService = FedExRates.RateService.ServiceType.FEDEX_3_DAY_FREIGHT;
                                        break;
                                    }
                                case "FEDEX_EXPRESS_SAVER":
                                    {
                                        myService = FedExRates.RateService.ServiceType.FEDEX_EXPRESS_SAVER;
                                        break;
                                    }
                                case "FIRST_OVERNIGHT":
                                    {
                                        myService = FedExRates.RateService.ServiceType.FIRST_OVERNIGHT;
                                        break;
                                    }
                                case "GROUND_HOME_DELIVERY":
                                    {
                                        myService = FedExRates.RateService.ServiceType.GROUND_HOME_DELIVERY;
                                        break;
                                    }
                                case "INTERNATIONAL_ECONOMY":
                                    {
                                        myService = FedExRates.RateService.ServiceType.INTERNATIONAL_ECONOMY;
                                        break;
                                    }
                                case "INTERNATIONAL_ECONOMY_FREIGHT":
                                    {
                                        myService = FedExRates.RateService.ServiceType.INTERNATIONAL_ECONOMY_FREIGHT;
                                        break;
                                    }
                                case "INTERNATIONAL_FIRST":
                                    {
                                        myService = FedExRates.RateService.ServiceType.INTERNATIONAL_FIRST;
                                        break;
                                    }
                                case "INTERNATIONAL_PRIORITY":
                                    {
                                        myService = FedExRates.RateService.ServiceType.INTERNATIONAL_PRIORITY;
                                        break;

                                    }
                                case "INTERNATIONAL_PRIORITY_FREIGHT":
                                    {
                                        myService = FedExRates.RateService.ServiceType.INTERNATIONAL_PRIORITY_FREIGHT;
                                        break;
                                    }
                                case "PRIORITY_OVERNIGHT":
                                    {
                                        myService = FedExRates.RateService.ServiceType.PRIORITY_OVERNIGHT;
                                        break;
                                    }
                                case "STANDARD_OVERNIGHT":
                                    {
                                        myService = FedExRates.RateService.ServiceType.STANDARD_OVERNIGHT;
                                        break;
                                    }
                                default:
                                    break;


                            }
                            IWEB.FedEx.Rate myRate = new IWEB.FedEx.Rate();
                            myRate.ServiceType = myService;
                            myRate.DestinationCountryCode = "US";
                            myRate.DestinationZip = destZip;
                            myRate.OriginCountryCode = "US";
                            myRate.OriginZip = orgZip;
                            decimal dweightInOz = Convert.ToDecimal(weightInOz);
                            myRate.PackageWeightInOunces = dweightInOz;

                            decimal rate = myRate.GetRate();
                            if (rate != 0)
                            {
                                return rate;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                    break;
                case "UPS":
                    {

                    }
                    break;
                case "DHL":
                    {

                    }
                    break;
                default:
                    //there wasent something we know about, return null values
                    break;
            }

            return returnRate;
        }

        /// <summary>
        /// Get Shipping Rate...
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
        public TaxModel GetShippingRate(string storeId, object myVars, string userName, string userPwd, string dbType, string uvAddress, string uvAccount, string cacheTIme,
            string dblCache, string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            var objTaxModel = new TaxModel();


            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.GetTax, myVars, userName, userPwd,
                                                             dbType,
                                                             uvAddress, uvAccount, cacheTIme, dblCache, strd3PortNumber,
                                                             useEncryption, d3PortNumber);

            //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                objTaxModel.ErrorMsg = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {
                var objTax = (from tax in xmlResult.Descendants("TAXFREIGHT")
                              select new
                                         {
                                             AMOUNT = tax.Element("AMOUNT").Value,
                                             TOTALFREIGHT = tax.Element("TOTALFREIGHT").Value
                                         }).SingleOrDefault();

                objTaxModel.Tax = objTax.AMOUNT;


                objTaxModel.PaymentMethods = (from payment in xmlResult.Descendants("PAYMENTINFO")
                                              select new PaymentMethod
                                                         {
                                                             Type = payment.Element("TYPE").Value,
                                                             Tender = payment.Element("TENDER").Value
                                                         }).ToList();
            }

            return objTaxModel;
        }

        /// <summary>
        /// Make payment for an order...
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
        public CheckoutMessageModel DoPayment(string storeId, object myVars, string userName, string userPwd, string dbType,
                             string uvAddress, string uvAccount, string cacheTIme, string dblCache,
                             string strd3PortNumber, string useEncryption, string d3PortNumber)
        {

            var strPickDataReturn = DbAccess.GetStringResult(storeId, Constants.OrderPayment, myVars, userName, userPwd,
                                                             dbType, uvAddress, uvAccount, cacheTIme, dblCache,
                                                             strd3PortNumber, useEncryption, d3PortNumber);



            //Read XML data
            var doc = new XmlDocument();

            doc.LoadXml(strPickDataReturn);

            var checkoutMessageModel = new CheckoutMessageModel();

            var xmlResult = XElement.Parse(strPickDataReturn);

            if (xmlResult.Descendants("ERROR").Any())
            {
                //If not set error message
                //objTaxModel.ErrorMsg = xmlResult.Descendants("ERROR").SingleOrDefault().Value;
            }
            else
            {


                checkoutMessageModel.Line1 = xmlResult.Descendants("LINE1").FirstOrDefault().Value;
                checkoutMessageModel.Line2 = xmlResult.Descendants("LINE2").FirstOrDefault().Value;
                checkoutMessageModel.Line3 = xmlResult.Descendants("LINE3").FirstOrDefault().Value;
                checkoutMessageModel.TrasactionNumber = xmlResult.Descendants("DATA").FirstOrDefault().Value;
                checkoutMessageModel.Emails = xmlResult.Descendants("EMAILS").FirstOrDefault().Value;

            }
            return checkoutMessageModel;
        }
    }


}
