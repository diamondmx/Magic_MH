using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Magic.Domain;
using Magic.Core;
using Microsoft.AspNet.Identity.Owin;
using IdentitySample.Models;

namespace kMagicSecure.Controllers
{
	[RequireHttps]
	[Authorize]
	public class MagicController : Controller
	{
		private readonly IEventManager _eventManager;
		private readonly IMatchManager _matchManager;
		private readonly IPlayerManager _playerManager;
		private readonly IPrizeManager _prizeManager;

		private ApplicationUserManager _userManager;
		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		public MagicController()
		{
			var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
      var dataContext = new Magic.Data.DataContextWrapper(connectionString);
			var eventPlayerRepo = new Magic.Data.EventPlayerRepository(dataContext);
			var playerRepo = new Magic.Data.PlayerRepository(dataContext);
			var matchRepo = new Magic.Data.MatchRepository(dataContext);
			var roundPrizeRepo = new Magic.Data.RoundPrizeRepository(dataContext);
			var eventRepo = new Magic.Data.EventRepository(dataContext, eventPlayerRepo, matchRepo, playerRepo, roundPrizeRepo);
			_playerManager = new PlayerManager(playerRepo);
			_eventManager = new EventManager(eventRepo);
			_matchManager = new MatchManager(matchRepo);
			_prizeManager = new PrizeManager(roundPrizeRepo);
		}

		[AllowAnonymous]
		public ActionResult Default()
		{
			dbEvent currentEvent = _eventManager.GetCurrentEvent();
			return Index(currentEvent.Name, currentEvent.CurrentRound);
		}

		//
		// GET: /Magic/
		[AllowAnonymous]
		public ActionResult Index(string eventName, int round, int detailMode = 0)
		{
			try
			{
				Event thisEvent = _eventManager.LoadEvent(eventName);
				var userEmail = HttpContext.User.Identity.Name;
				if (!string.IsNullOrEmpty(userEmail))
				{
					ViewBag.CurrentUser = UserManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
        }
				else
				{
					ViewBag.CurrentUser = null;
				}
				
				ViewBag.Title = string.Format("{0}: Round {1}", eventName, round);
				ViewBag.Event = thisEvent;
				ViewBag.Round = round;
				ViewBag.DetailMode = detailMode;
				return View("Index");
			}
			catch(EventNotFoundException ex)
			{
				if(Session["LastError"]?.ToString().CompareTo(ex.Message)==0)
				{
					return RedirectToAction("Error");
				}
				else
				{
					Session["LastError"] = ex.Message;
					return RedirectToAction("Index");
				}
			}
			catch (Exception	ex)
			{
				Session["LastError"] = new Exception($"Failed to load event {eventName} - {round}", ex);
				return RedirectToAction("Index");
			}
		}

		private List<SelectListItem> GetDropdownWithSelected(int max, int selected)
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

			var match = thisEvent.Matches.FirstOrDefault(m => (m.Round == round) && ((m.Player1Name == player1 && m.Player2Name == player2) || (m.Player2Name == player1 && m.Player1Name == player2)));
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

		[Authorize(Roles = "Admin")]
		public ActionResult ViewEvents()
		{
			var eventList = _eventManager.LoadAllEvents();

			return View("ViewEvents", eventList);
		}

		public ActionResult EventArchiveList()
		{
			var eventList = _eventManager.LoadAllEvents();

			return View("EventArchiveList", eventList);
		}

		[Authorize(Roles = "Admin")]
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

		[Authorize(Roles = "Admin")]
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

		[Authorize(Roles = "Admin")]
		public ActionResult DeleteEvent(int id)
		{
			return View();
		}

		[Authorize(Roles = "Admin")]
		public ActionResult ListPlayers(string eventName)
		{
			var thisEvent = _eventManager.LoadEvent(eventName);

			return View("ListPlayers", thisEvent);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult AddPlayer(string eventName, string playerName)
		{
			var thisEvent = _eventManager.LoadEvent(eventName);

			var newPlayer = new Player(playerName);
			_eventManager.AddPlayer(thisEvent, newPlayer);

			return Redirect(Url.Action("ListPlayers", "Magic", new { eventName = eventName }));
		}

		[Authorize(Roles = "Admin")]
		public ActionResult GeneratePairings(string eventName)
		{
			var pairingsManager = new PairingsManager(_eventManager);

			var thisEvent = pairingsManager.LoadDatabase(eventName);
			pairingsManager.GeneratePairings(thisEvent);
			Session["pairedEvent"] = thisEvent;
			return View("PreviewPairings", thisEvent);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult SaveMatches()
		{
			Event eventToSave = Session["pairedEvent"] as Event;
			_matchManager.UpdateAllMatches(eventToSave.Matches, eventToSave.CurrentRound);

			return RedirectToAction("Index", new { eventName = eventToSave.name, round = eventToSave.CurrentRound });
		}

		[AllowAnonymous]
		public ActionResult PlayerStats(string playerName)
		{
			List<Player> allPlayers = _playerManager.GetAllPlayers();

			var playerList = allPlayers.Select(p => new SelectListItem { Text = p.Name, Value = p.Name });
			var currentPlayer = allPlayers.FirstOrDefault(p => p.Email == User.Identity.Name);

			if (playerName == "" || playerName == null)
			{
				playerName = currentPlayer?.Name ?? allPlayers.OrderBy(p=>p.Name).First().Name;
			}
			List<PlayerScoreSummary> playerStatistics = _matchManager.GetPlayerStatistics(playerName);

			ViewBag.playerName = playerList;
			ViewBag.CurrentUser = playerName;
			
			return View("PlayerStats", playerStatistics);
		}
		[AllowAnonymous]
		public ActionResult PrizeSetup(string eventName, int round)
		{
			ViewBag.EventName = eventName;
			ViewBag.Round = round;

			var eventNames = Request.Form["EventName"];
			var prizeList = new List<dbRoundPrize>();
			if (eventNames?.Length>0)
			{
				try
				{
					prizeList = ParsePrizeFormValues(Request.Form["EventName"], Request.Form["Round"], Request.Form["Position"], Request.Form["Packs"], Request.Form["Other"]);
					_prizeManager.SavePrizes(prizeList);
				}
				catch(InvalidOperationException ex)
				{
					Session["LastError"] = $"Not Saved: {ex.Message}\n{ex.StackTrace}";
				}
			}
			else
			{
				var thisEvent = _eventManager.LoadEvent(eventName);
				prizeList = thisEvent.RoundPrizes.Where(rp => rp.Round == round).ToList();
			}
			
			return View("PrizeSetup", prizeList);
		}

		private List<dbRoundPrize> ParsePrizeFormValues(string eventNames, string rounds, string position, string packs, string others)
		{
			char delimiter = ',';

			string[] eventNameList = eventNames.Split(delimiter);
				string[] roundStringList = rounds.Split(delimiter);
			List<int> roundList = roundStringList.ToList().Select(r => int.Parse(r)).ToList();
					string[] packStringList = packs.Split(delimiter);
			List<int> packList = packStringList.ToList().Select(r => int.Parse(r)).ToList();
				string[] positionStringList = position.Split(delimiter);
			List<int> positionList = positionStringList.ToList().Select(r => int.Parse(r)).ToList();
			string[] otherList = others.Split(delimiter);

			var counts = new List<int>
			{
				eventNameList.Length,
				roundList.Count(),
				positionList.Count(),
				packList.Count(),
				otherList.Length
			};

			var minCount = counts.Min();
			if (counts.Max() != minCount)
				throw new InvalidOperationException("Invalid number of elements recieved - do not use , in other or eventNameColumn");

			if (eventNameList.Count(en => en == eventNameList[0]) != minCount)
				throw new InvalidOperationException("EventNames do not match, all event names must be identical");

			if (roundList.Count(r => r == roundList.First()) != minCount)
				throw new InvalidOperationException("Rounds do not match, all rounds must be identical");

			if (positionList.Distinct().Count() != minCount)
				throw new InvalidOperationException("Positions are not distinct, all positions must be distinct");

			var roundPrizeList = new List<dbRoundPrize>();

			int round = roundList.First();
			string eventName = eventNameList.First();


			for(int i=0;i<minCount;i++)
			{
				roundPrizeList.Add(new dbRoundPrize
				{
					EventName = eventName,
					Round = round,
					Position = positionList[i],
					Packs = packList[i],
					Other = otherList[i]
				});
			}

			return roundPrizeList;
		}

	}
}