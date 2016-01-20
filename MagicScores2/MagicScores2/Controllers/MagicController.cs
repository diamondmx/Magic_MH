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
		public ActionResult Index(string eventName, int round, bool detailedMode)
		{
			var thisEvent = new Magic.Core.Event();
			thisEvent.LoadEvent(eventName);

			ViewBag.Title = String.Format("{0}: Round {1}", eventName, round);
			ViewBag.Players = thisEvent.Players;
			ViewBag.EventName = eventName;
			ViewBag.Round = round;
			ViewBag.Event = thisEvent;
			return View();
		}

		public List<SelectListItem> GetDropdownWithSelected(int max, int selected)
		{
			var output = new List<SelectListItem>();
			for(int i=0; i<=max;i++)
			{
				output.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = selected == i });
			}

			return output;
		}

		//
		// GET: /Magic/Details/5
		public ActionResult Details(string eventName, int round, string player1, string player2, int? player1wins, int? player2wins, int? draws)
		{
			var thisEvent = new Magic.Core.Event();
			thisEvent.LoadEvent(eventName);

			var match = thisEvent.Matches.Where(m => (m.Round == round) && (m.Player1Name == player1 && m.Player2Name == player2) || (m.Player2Name == player1 && m.Player1Name == player2)).First();
			ViewBag.Match = match;

			if (player1wins.HasValue && player2wins.HasValue && draws.HasValue)
			{
				if (thisEvent.Locked(round))
				{
					ModelState.AddModelError("CustomError", "This match is Locked");
				}
				else
				{
					match.Player1Wins = player1wins.Value;
					match.Player2Wins = player2wins.Value;
					match.Draws = draws.Value;

					match.Update();

					return RedirectToAction("Index", new { controller = "Magic", eventName = eventName, round = round });
				}
			}

			var p1Dropdown = GetDropdownWithSelected(2, match.Player1Wins);
			var p2Dropdown = GetDropdownWithSelected(2, match.Player2Wins);
			var drawsDropdown = GetDropdownWithSelected(3, match.Draws);

			ViewBag.player1wins = p1Dropdown;
			ViewBag.player2wins = p2Dropdown;
			ViewBag.draws = drawsDropdown;

			return View("MagicMatch");
		}

		public ActionResult ViewEvents()
		{
			var dummyEvent = new Magic.Core.Event();
			var eventList = dummyEvent.LoadAllEvents();

			return View("ViewEvents", eventList);
		}

		//
		// GET: /Magic/Edit/5
		public ActionResult EditEvent(string eventName, string name, int? currentRound, int? roundMatches, DateTime? startDate, DateTime? roundEndDate)
		{

			var thisEvent = new Magic.Core.Event();
			thisEvent.LoadEvent(eventName);

			if (currentRound.HasValue)
			{
				thisEvent.name = name;
				thisEvent.CurrentRound = currentRound.HasValue? currentRound.Value: thisEvent.CurrentRound;
				thisEvent.RoundMatches = roundMatches.HasValue ? roundMatches.Value: thisEvent.RoundMatches;
				thisEvent.EventStartDate = startDate.HasValue? startDate.Value: thisEvent.EventStartDate;
				thisEvent.RoundEndDate = roundEndDate.HasValue? roundEndDate.Value: thisEvent.RoundEndDate;

				thisEvent.SaveEvent(saveMatches: false);

				return ViewEvents();
			}


			var currentRoundDropdown = GetDropdownWithSelected(4, thisEvent.CurrentRound);
			var roundMatchesDropdown = GetDropdownWithSelected(4, thisEvent.RoundMatches);

			@ViewBag.CurrentRound = currentRoundDropdown;
			@ViewBag.RoundMatches = roundMatchesDropdown;

			return View("EditEvent", thisEvent);
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
