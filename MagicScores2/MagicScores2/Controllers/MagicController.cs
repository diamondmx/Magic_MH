using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Magic;

namespace MagicScores2.Controllers
{
    public class MagicController : Controller
    {
        //
        // GET: /Magic/
        public ActionResult Index(string eventName="FRF Sealed", int round=1)
        {
            var thisEvent = new Magic.Core.Event();
            thisEvent.LoadEvent(eventName);
            
            ViewBag.Players = thisEvent.Players;
            ViewBag.EventName = eventName;
            ViewBag.Round = round;
            return View();
        }

        public ActionResult Matches(string mtgevent)
        {
            return View();
        }

        //
        // GET: /Magic/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Magic/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Magic/Create
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
        // GET: /Magic/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Magic/Edit/5
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
        // GET: /Magic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Magic/Delete/5
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
