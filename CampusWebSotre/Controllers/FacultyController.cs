using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CampusWebStore.Models;
using CampusWebStore.Business.Services;
using CampusWebStore.Shared;
using CampusWebStore.Shared.Models;
using Microsoft.Practices.Unity;

namespace CampusWebStore.Controllers
{
    public class FacultyController : WebController
    {
        #region Properties

        [Dependency]
        public IStoreServices StoreServices { get; set; }

        [Dependency]
        public ITermServices TermServices { get; set; }
        #endregion

        //
        // GET: /Faculty/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Faculty/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Faculty/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Faculty/Create

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
        // GET: /Faculty/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Faculty/Edit/5

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
        // GET: /Faculty/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Faculty/Delete/5

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
