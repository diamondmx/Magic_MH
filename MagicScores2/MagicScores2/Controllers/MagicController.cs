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
        public ActionResult Index(string eventName, int round)
        {
            var thisEvent = new Magic.Core.Event();
            thisEvent.LoadEvent(eventName);

	        ViewBag.Title = String.Format("{0}: Round {1}", eventName, round);
          ViewBag.Players = thisEvent.Players;
          ViewBag.EventName = eventName;
          ViewBag.Round = round;
          return View();
        }

        public List<SelectListItem> GetGameWinsDropdownWithSelected(int winsSelected)
        {
            var output = new List<SelectListItem>();
            output.Add(new SelectListItem { Text = "0", Value = "0", Selected = winsSelected == 0 });
            output.Add(new SelectListItem { Text = "1", Value = "1", Selected = winsSelected == 1 });
            output.Add(new SelectListItem { Text = "2", Value = "2", Selected = winsSelected == 2 });

            return output;
        }

        //
        // GET: /Magic/Details/5
        public ActionResult Details(string eventName, int round, string player1, string player2, int? player1wins, int? player2wins)
        {
            var thisEvent = new Magic.Core.Event();
            thisEvent.LoadEvent(eventName);

            var match = thisEvent.Matches.Where(m => (m.Player1Name == player1 && m.Player2Name == player2) || (m.Player2Name == player1 && m.Player1Name == player2)).First();
            ViewBag.Match = match;
            
            if (player1wins.HasValue && player2wins.HasValue)
            {
                match.Player1Wins = player1wins.Value;
                match.Player2Wins = player2wins.Value;

                match.Update();

                return RedirectToAction("Index", new { controller = "Magic", eventName = eventName, round = round });
            }

            var p1dropdown = GetGameWinsDropdownWithSelected(match.Player1Wins);
            var p2dropdown = GetGameWinsDropdownWithSelected(match.Player2Wins);

            ViewBag.player1wins = p1dropdown;
            ViewBag.player2wins = p2dropdown;

            return View("MagicMatch");
        }

        //
        // GET: /Magic/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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
