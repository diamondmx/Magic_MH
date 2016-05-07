using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Magic.Domain;
using Magic.Core;

namespace kMagicSecure.Controllers
{
	public class MagicController : Controller
	{
		private readonly IEventManager _eventManager;
		private readonly IMatchManager _matchManager;

		public MagicController()
		{
			var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
      var dataContext = new Magic.Data.DataContextWrapper(connectionString);
			var eventPlayerRepo = new Magic.Data.EventPlayerRepository(dataContext);
			var playerRepo = new Magic.Data.PlayerRepository(dataContext);
			var matchRepo = new Magic.Data.MatchRepository(dataContext);
			var eventRepo = new Magic.Data.EventRepository(dataContext, eventPlayerRepo, matchRepo, playerRepo);
			_eventManager = new EventManager(eventRepo);
			_matchManager = new MatchManager(matchRepo);
		}

		//
		// GET: /Magic/
		public ActionResult Index(string eventName, int round, int detailMode = 0)
		{
			try
			{
				Magic.Domain.Event thisEvent = _eventManager.LoadEvent(eventName);

				ViewBag.Title = String.Format("{0}: Round {1}", eventName, round);
				ViewBag.Players = thisEvent.Players;
				ViewBag.EventName = eventName;
				ViewBag.Round = round;
				ViewBag.Event = thisEvent;
				ViewBag.DetailMode = detailMode;
				return View("Index");
			}
			catch (Exception ex)
			{
				Session["LastError"] = new Exception($"Failed to load event {eventName} - {round}", ex);
				return RedirectToAction("ViewEvents");
			}
		}

		public List<SelectListItem> GetDropdownWithSelected(int max, int selected)
		{
			var output = new List<SelectListItem>();
			for (int i = 0; i <= max; i++)
			{
				output.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = selected == i });
			}

			return output;
		}

		public ActionResult Details(string eventName, int round, string player1, string player2, int? player1wins, int? player2wins, int? draws)
		{
			Magic.Domain.Event thisEvent = _eventManager.LoadEvent(eventName);

			var match = thisEvent.Matches.FirstOrDefault(m => (m.Round == round) && (m.Player1Name == player1 && m.Player2Name == player2) || (m.Player2Name == player1 && m.Player1Name == player2));
			ViewBag.Match = match;

			if (match == null)
			{
				Session["LastError"] = new Exception($"Match {player1} vs {player2} not found in {eventName}:{round}");
				return View("Index", new
				{
					eventName = eventName,
					round = round
				});
			}


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

					_matchManager.Update(match);

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
			var eventList = _eventManager.LoadAllEvents();

			return View("ViewEvents", eventList);
		}

		public ActionResult CreateEvent()
		{
			var newEvent = new Magic.Domain.Event();

			var currentRoundDropdown = GetDropdownWithSelected(4, 1);
			var roundMatchesDropdown = GetDropdownWithSelected(4, 4);

			@ViewBag.CurrentRound = currentRoundDropdown;
			@ViewBag.RoundMatches = roundMatchesDropdown;
			@ViewBag.NewEvent = true;

			return View("EditEvent", newEvent);
		}

		public ActionResult EditEvent(string eventName, bool NewEvent, string name, int? currentRound, int? roundMatches, DateTime? startDate, DateTime? roundEndDate)
		{
			Magic.Domain.Event thisEvent = null;
			if (NewEvent == false)
			{
				thisEvent = _eventManager.LoadEvent(eventName);
			}
			else
			{
				thisEvent = new Magic.Domain.Event();
			}

			if (currentRound.HasValue)
			{
				thisEvent.name = name;
				thisEvent.CurrentRound = currentRound.HasValue ? currentRound.Value : thisEvent.CurrentRound;
				thisEvent.RoundMatches = roundMatches.HasValue ? roundMatches.Value : thisEvent.RoundMatches;
				thisEvent.EventStartDate = startDate.HasValue ? startDate.Value : thisEvent.EventStartDate;
				thisEvent.RoundEndDate = roundEndDate.HasValue ? roundEndDate.Value : thisEvent.RoundEndDate;


				if (NewEvent == false)
				{
					_eventManager.SaveEvent(thisEvent);
				}
				else
				{
					_eventManager.CreateEvent(thisEvent);
				}


				return ViewEvents();
			}


			var currentRoundDropdown = GetDropdownWithSelected(4, thisEvent.CurrentRound);
			var roundMatchesDropdown = GetDropdownWithSelected(4, thisEvent.RoundMatches);

			@ViewBag.CurrentRound = currentRoundDropdown;
			@ViewBag.RoundMatches = roundMatchesDropdown;
			@ViewBag.NewEvent = false;

			return View("EditEvent", thisEvent);
		}

		public ActionResult DeleteEvent(int id)
		{
			return View();
		}

		public ActionResult ListPlayers(string eventName)
		{
			var thisEvent = _eventManager.LoadEvent(eventName);

			return View("ListPlayers", thisEvent);
		}

		public ActionResult AddPlayer(string eventName, string playerName)
		{
			var thisEvent = _eventManager.LoadEvent(eventName);

			var newPlayer = new Player(playerName);
			_eventManager.AddPlayer(thisEvent, newPlayer);

			return Redirect(Url.Action("ListPlayers", "Magic", new { eventName = eventName }));
		}

		public ActionResult GeneratePairings(string eventName)
		{
			var pairingsManager = new PairingsManager(_eventManager);

			var thisEvent = pairingsManager.LoadDatabase(eventName);
			pairingsManager.GeneratePairings(thisEvent);
			Session["pairedEvent"] = thisEvent;
			return View("PreviewPairings", thisEvent);
		}

		public ActionResult SaveMatches()
		{
			Event eventToSave = Session["pairedEvent"] as Event;
			_matchManager.UpdateAllMatches(eventToSave.Matches, eventToSave.CurrentRound);

			return RedirectToAction("Index", new { eventName = eventToSave.name, round = eventToSave.CurrentRound });
		}
	}
}