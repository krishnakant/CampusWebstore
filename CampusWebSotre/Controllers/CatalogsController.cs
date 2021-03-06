﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampusWebStore.Models;
using CampusWebStore.Business.Services;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;
using ItemLookUpModel = CampusWebStore.Models.ItemLookUpViewModel;

namespace CampusWebStore.Controllers
{
    public class CatalogsController : WebController
    {
        //
        // GET: /Catalogs/


        public ActionResult Index()
        {


            return View();
        }

        //
        // GET: /Catalogs/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Catalogs/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Catalogs/Create

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
        // GET: /Catalogs/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Catalogs/Edit/5

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
        // GET: /Catalogs/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Catalogs/Delete/5

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

        /// <summary>
        /// Get the Catalog Product ITems
        /// </summary>
        /// <returns></returns>
        public ActionResult CatalogProductItems()
        {
            var delc = Request["Delc"] ?? "-";

            var title = Request["Title"] ?? "Catalog";

            var sku = Request["Sku"] ?? "";

            var source = Request["Source"] ?? "";

            try
            {
                var myObject = new
                  {
                      STOREID = 1,
                      SKU = sku,
                      SOURCE = source,
                      SEARCHTYPE = "SKU"
                  };

                var lstCatalogProductLookUpItems = CatalogsServices.GetCataLogProductLookUpItems(StoreNumber, myObject,source,
                                                             UvUsername, UvPassword,
                                                             DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                             Strd3PortNumber, UseEncryption, Strd3PortNumber);

                IEnumerable<ItemLookUpViewModel> itemLookUpViewModels = (from itemLookUp in lstCatalogProductLookUpItems
                                                                         select new ItemLookUpViewModel
                                                                                    {
                                                                                       Binding=itemLookUp.Binding,
                                                                                       Comments=itemLookUp.Comments,
                                                                                       Description=itemLookUp.Description,
                                                                                       Edition=itemLookUp.Edition,
                                                                                       FlatFreight = itemLookUp.FlatFreight,
                                                                                       FreeShipping=itemLookUp.FreeShipping,
                                                                                       Image=itemLookUp.Image,
                                                                                       IsMatrix=itemLookUp.IsMatrix,
                                                                                       Manf = itemLookUp.Manf,
                                                                                       NewPrice=itemLookUp.NewPrice,
                                                                                       NewQoh=itemLookUp.NewQoh,
                                                                                       Price=itemLookUp.Price,
                                                                                       Publisher=itemLookUp.Publisher,
                                                                                       Qoh=itemLookUp.Qoh,
                                                                                       SalePrice=itemLookUp.SalePrice,
                                                                                       SeoSku=itemLookUp.SeoSku,
                                                                                       SeoTitle=itemLookUp.SeoTitle,
                                                                                       Sku=itemLookUp.Sku,
                                                                                       Source=itemLookUp.Source,
                                                                                       StockMsg=itemLookUp.StockMsg,
                                                                                       Style=itemLookUp.Style,
                                                                                       UsedPrice=itemLookUp.UsedPrice,
                                                                                       UsedQoh=itemLookUp.UsedQoh,
                                                                                       Vendor=itemLookUp.Vendor,
                                                                                       Vol=itemLookUp.Vol,
                                                                                       MatrixItem=(from mItem in itemLookUp.MatrixItem select new MatrixItemViewModel
                                                                                                                                                  {
                                                                                                                                                      Color=mItem.Color,
                                                                                                                                                      Desc=mItem.Desc,
                                                                                                                                                      DLdsc=mItem.DLdsc,
                                                                                                                                                      MatrixId=mItem.MatrixId,
                                                                                                                                                      MatrixLine=mItem.MatrixLine,
                                                                                                                                                      Price=mItem.Price,
                                                                                                                                                      Qqh=mItem.Qqh,
                                                                                                                                                      Size=mItem.Size
                                                                                                                                                  })

                                                                                    }).ToList();

                var itemModels = new List<ItemLookUpViewModel>();

                if (Session["GetItemDetail"] != null)
                {
                    itemModels = (List<ItemLookUpViewModel>)Session["GetItemDetail"];
                    itemModels.Add(itemLookUpViewModels.FirstOrDefault());
                    //Create session 
                   
                }
                else
                {
                   itemModels.Add(itemLookUpViewModels.FirstOrDefault());

                }
                Session["GetItemDetail"] = itemModels;
                //*****

                ViewBag.Title = title;

                //put into the delc to get on the page
                ViewBag.Delc = delc;

                return View(itemLookUpViewModels);

            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }
        /// <summary>
        /// Get the catalog products
        /// </summary>
        /// <returns></returns>
        public ActionResult CatalogProduct()
        {
            var catId = Request["CatId"] ?? "0";

            var title = Request["Title"] ?? "Catalog";

            try
            {
                var myObject = new { STOREID = 1, CATALOGID = catId };
                //Call the funnction to get the catalogs product
                var lstCatalogProduct = CatalogsServices.CataLogProducts(StoreNumber, myObject,
                                                             UvUsername, UvPassword,
                                                             DbType, UvAddress, UvAccount, CacheTime, CacheTime,
                                                             Strd3PortNumber, UseEncryption, Strd3PortNumber);
                var catalogItemViewModel=new List<CatalogItemViewModel>() ;

                foreach (var lst in lstCatalogProduct)
                {
                    catalogItemViewModel.Add(new CatalogItemViewModel
                                                 {
                                                     SeoUrl=title,
                                                     Author=lst.Author,
                                                     CatalogPrice=lst.CatalogPrice,
                                                     CatId=lst.CatId,
                                                     Comments=lst.Comments,
                                                     Desc=lst.Desc,
                                                     DisplayText=lst.DisplayText,
                                                     Images=lst.Images,
                                                     Price=lst.Price,
                                                     RealPrice=lst.RealPrice,
                                                     SeoSku=lst.SeoSku,
                                                     SeoTitle=lst.SeoTitle,
                                                     Sku=lst.Sku,
                                                     Source=lst.Source,
                                                     Title=lst.Title,
                                                            
                                                 });
                   
                }

                ViewBag.Title = title;
                return View(catalogItemViewModel);

            }
            catch (Exception x)
            {
                return Json(x.Message);
            }
        }
        /// <summary>
        /// Add the catalog items to the cart
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCatalogItemsToCart()
        {
           
            var source = Request["Source"] ?? "";

            var qty = Convert.ToInt32(Request["Quantity"] ?? "1");

            var id = Request["MatrixId"] ?? "";

            id = id.Replace("-", "*");

            var rdo = (Request["NewUsed"] ?? "");
            var seosku = Request["Sku"] ?? "";

            seosku = seosku.Replace("-", "*");

            var lstCatalogProductLookUpItems = new ItemLookUpViewModel();

            //Get the items from the sessions
            if (Session["GetItemDetail"]!=null)
            {

                var itemModels = (List<ItemLookUpViewModel>)Session["GetItemDetail"];
                lstCatalogProductLookUpItems = itemModels.FirstOrDefault(m => m.Sku.Equals(seosku));
            }

           
            try
            {
                if (lstCatalogProductLookUpItems != null)
                {
                    //Check for the items
                    string color = "";
                    string size = "";
                    string itemoos = "";
                    string price, sku = "", title = "", newused = "", type = "";
                    string desc = "-";
                    double fprice = 0.00;
                    //Check if source type is gm then add to cart 
                    if (source.ToLower().Equals("gm"))
                    {
                       
                        try
                        {
                            if (string.IsNullOrEmpty(id))
                            {
                                id = lstCatalogProductLookUpItems.Sku;
                            }
                            sku = lstCatalogProductLookUpItems.Sku;

                            size =
                                lstCatalogProductLookUpItems.MatrixItem.FirstOrDefault(m => m.MatrixId.Equals(id)).Size;

                            price =
                                lstCatalogProductLookUpItems.MatrixItem.FirstOrDefault(m => m.MatrixId.Equals(id)).Price;

                            itemoos =
                                lstCatalogProductLookUpItems.MatrixItem.FirstOrDefault(m => m.MatrixId.Equals(id)).Qqh;

                            color =
                                lstCatalogProductLookUpItems.MatrixItem.FirstOrDefault(m => m.MatrixId.Equals(id)).Color;

                            title =
                                lstCatalogProductLookUpItems.MatrixItem.FirstOrDefault(m => m.MatrixId.Equals(id)).Desc;
                        }
                            //If Exception is came then there is no matrix items so catch block code will be executed
                        catch (Exception)
                        {
                            string saleprice = lstCatalogProductLookUpItems.SalePrice;

                            if (saleprice != "$0.00")
                            {
                                price = saleprice;

                            }
                            else
                            {
                                price = lstCatalogProductLookUpItems.Price;
                            }

                            sku = lstCatalogProductLookUpItems.Sku;

                            title = lstCatalogProductLookUpItems.Description;

                        }

                        fprice = Convert.ToDouble(price.Replace("$", ""));

                        newused = "N/A";

                        type = "GM";

                        desc = color + " - " + size;

                        if (!itemoos.Contains("Out"))
                        {
                        }

                    }
                    if (source.ToLower().Equals("tr"))
                    {
                       
                        // made changes 18-06-2012
                        //  id = sku;

                        sku = lstCatalogProductLookUpItems.Sku;

                        // made changes 18-06-2012
                        id = sku;

                        title = lstCatalogProductLookUpItems.Description;

                        price = lstCatalogProductLookUpItems.Price;

                        newused = "N/A";

                        type = "TR";

                        desc = "Trade book";

                        fprice = Convert.ToDouble(price.Replace("$", ""));


                    }
                    if (source.ToLower().Equals("tx"))
                    {

                        string usedprice, newprice;

                        id = sku;

                        sku = lstCatalogProductLookUpItems.Sku;

                        title = lstCatalogProductLookUpItems.Description;

                        usedprice = lstCatalogProductLookUpItems.UsedPrice;

                        newprice = lstCatalogProductLookUpItems.NewPrice;
                        type = "TX";
                        desc = "Textbook";

                        if (rdo.Equals("1"))
                        {
                            fprice = Convert.ToDouble(newprice.Replace("$", ""));

                            newused = "New";
                        }
                        if (rdo.Equals("0"))
                        {
                            fprice = Convert.ToDouble(usedprice.Replace("$", ""));
                            newused = "Used";
                        }

                    }

                    var lstCartItems = new List<ShoppingCartModel>();

                    var model = lstCatalogProductLookUpItems;

                    //Create the object
                    var shoppingCartModel = new ShoppingCartModel
                                                {
                                                    Id = id,
                                                    ActualPrice = fprice,
                                                    Qty = qty,
                                                    Total = qty*fprice,
                                                    Type = source,
                                                    Title = title,
                                                    Detail = desc,
                                                    Isbn = sku,
                                                    Course = desc
                                                };

                    //Check for the null if nulll then create the object and assign 
                    if (Session["CARTITEMS"] != null)
                    {
                        lstCartItems = (List<ShoppingCartModel>) Session["CARTITEMS"];

                        var alreadyExist = lstCartItems.FirstOrDefault(lst => lst.Id.Equals(id));

                        //Checking for already exist
                        if (alreadyExist == null)
                        {
                            lstCartItems.Add(shoppingCartModel);

                            //Latest item added to the cart
                            Session["LASTITEMADDED"] = shoppingCartModel.Title;
                        }
                        //else
                        //{
                        //    lstCartItems.Add(shoppingCartModel);

                        //    //Latest item added to the cart
                        //    Session["LASTITEMADDED"] = shoppingCartModel.Title;

                        //}

                    }
                    else
                    {
                        //Add to the list
                        lstCartItems.Add(shoppingCartModel);

                        //Latest item added to the cart
                        Session["LASTITEMADDED"] = shoppingCartModel.Title;
                    }

                    //Put the list to the session
                    Session["CARTITEMS"] = lstCartItems;


                    return Json(new {success = true});
                }
                return Json(new { success = false });
            }
            catch (Exception x)
            {
                return Json(new { errMsg = x.Message });
            }
        }

        /// <summary>
        /// Add the catalog items to the cart
        /// </summary>
        /// <returns></returns>
        public ActionResult Catalog()
        {
          try
          {
             
          }
            catch(Exception x)
            {
                
            }
          return View();
        }

      


    }
}
