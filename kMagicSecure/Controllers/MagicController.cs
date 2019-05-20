using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Magic.Domain;
using Magic.Core;
using Microsoft.AspNet.Identity.Owin;
using IdentitySample.Models;
using System.ServiceModel.Syndication;
using kMagicSecure._3rdParty;
using System.Threading;
using System.Diagnostics;

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
		private readonly Magic.Data.IGameLog _gameLog;

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
			var playerPrizeRepo = new Magic.Data.PlayerPrizeRepository(dataContext);
			_gameLog = new Magic.Data.GameLog(dataContext);
			var matchRepo = new Magic.Data.MatchRepository(dataContext, _gameLog);
			var roundPrizeRepo = new Magic.Data.RoundPrizeRepository(dataContext);
			var eventRepo = new Magic.Data.EventRepository(dataContext, eventPlayerRepo, matchRepo, playerRepo, roundPrizeRepo);
			_playerManager = new PlayerManager(playerRepo);
			_eventManager = new EventManager(eventRepo, roundPrizeRepo);
			_matchManager = new MatchManager(matchRepo, eventRepo, playerRepo);
			_prizeManager = new PrizeManager(roundPrizeRepo, playerPrizeRepo);
		}

		private void SetPlayerContext()
		{
			_gameLog.SetPlayerContext(HttpContext?.User?.Identity?.Name);
		}

		private void Setup()
		{
			var falsevar = false;

			if (falsevar)
			{
				System.Threading.Tasks.Task.Run(() => OverrideAdminUser());
			}

			ViewBag.CurrentEvents = new List<dbEvent>();
			var grn = new dbEvent()
			{
				Name = "GRN"
			};
			var rna = new dbEvent()
			{
				Name = "RNA"
			};


			ViewBag.CurrentEvents.Add(grn);
			ViewBag.CurrentEvents.Add(rna);

			SetPlayerContext();
		}

		private void OverrideAdminUser()
		{
			//ApplicationUser adminUser = UserManager.FindByNameAsync("mhill@kcura.com").Result;
			//var resetToken = UserManager.GeneratePasswordResetTokenAsync(adminUser.Id).Result;
			//var resetResult = UserManager.ResetPasswordAsync(adminUser.Id, resetToken, "TESTpassword9956$").Result;
		}

		[AllowAnonymous]
		public ActionResult Default()
		{
			Setup();
			dbEvent currentEvent = _eventManager.GetCurrentEvent();
			return Index(currentEvent.Name, currentEvent.CurrentRound);
		}

		//
		// GET: /Magic/
		[AllowAnonymous]
		public ActionResult Index(string eventName, int round, int detailMode = 0)
		{
			Setup();
			if (eventName == "DEFAULT")
			{
				return Default();
			}

			try
			{
				Event thisEvent = _eventManager.LoadEvent(eventName);
				if (round == -1)
				{
					round = thisEvent.CurrentRound;
				}

				var userEmail = HttpContext.User.Identity.Name;
				if (!string.IsNullOrEmpty(userEmail))
				{
					ApplicationUser currentUser = UserManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
					ViewBag.CurrentUser = currentUser;

					 ViewBag.CurrentPlayer = _playerManager.GetPlayerByEmail(currentUser.Email);
					if((ViewBag.CurrentPlayer != null) && (_prizeManager != null))
					{
						ViewBag.PlayerPrizeInfo = _prizeManager.GetUncollectedPlayerPrizes(ViewBag.CurrentPlayer.ID);
					}
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
			catch (EventNotFoundException ex)
			{
				throw;
			//	if (Session["LastError"]?.ToString().CompareTo(ex.Message) == 0)
			//	{
			//		return RedirectToAction("Error");
			//	}
			//	else
			//	{
			//		Session["LastError"] = ex;
			//		return RedirectToAction("Index");
			//	}
			//}
			//catch (Exception ex)
			//{
			//	Session["LastError"] = new Exception($"Failed to load event {eventName} - {round}", ex);
			//	return Default();
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

		public ActionResult RecievedPrizes()
		{
			ApplicationUser currentUser = UserManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
			var currentPlayer = _playerManager.GetPlayerByEmail(currentUser.Email);
			List<dbPlayerPrize> acknowledgedList;

			try
			{
				acknowledgedList = ParsePlayerPrizeForm(Request.Form["playerID"], Request.Form["prizeEvent"], Request.Form["prizeRound"], Request.Form["prizePosition"], Request.Form["prizePacks"], Request.Form["prizeRecieved"], currentPlayer.Name);
			}
			catch (Exception ex)
			{
				throw new Exception("Error: Failed to parse prize reciept. Try again.");
			}

			try
			{
				_prizeManager.AcknowledgeRecievedAll(currentPlayer.ID, acknowledgedList);
			}
			catch(Exception ex)
			{
				throw new Exception("Error: Failed to save prize reciept. Try again.");
			}
			
			return Default();
		}

		private List<dbPlayerPrize> ParsePlayerPrizeForm(string playerID, string eventName, string round, string position, string packs, string recieved, string player)
		{
			var prizeList = new List<dbPlayerPrize>();

			var eventNameList = eventName.Split(new char[]{','});
			var playerIDList = playerID.Split(new char[] { ',' }).Select(item => int.Parse(item)).ToList();
			var roundList = round.Split(new char[] {',' }).Select(item=>int.Parse(item)).ToList();
			var positionList = position.Split(new char[] { ',' }).Select(item => int.Parse(item)).ToList();
			var packsList = packs.Split(new char[] { ',' }).Select(item => int.Parse(item)).ToList();
			var recievedList = recieved.Split(new char[] { ',' }).Select(item => int.Parse(item)).ToList();
			
			for (int i = 0; i < eventNameList.Count(); i++)
			{
				prizeList.Add(new dbPlayerPrize()
				{
					PlayerID = playerIDList[i],
					EventName = eventNameList[i],
					Round = roundList[i],
					Position = positionList[i],
					Packs = packsList[i],
					Recieved = recievedList[i],
					Player = player
				});
			}
			

		return prizeList;
		}

		public ActionResult Details(string eventName, int round, int player1ID, int player2ID, int? player1wins, int? player2wins, int? draws)
		{
			Setup();
			Magic.Domain.Event thisEvent = _eventManager.LoadEvent(eventName);

			var match = thisEvent.Matches.FirstOrDefault(m => (m.Round == round) && ((m.Player1ID == player1ID && m.Player2ID == player2ID) || (m.Player2ID == player1ID && m.Player1ID == player2ID)));
			ViewBag.Match = match;

			if (match == null)
			{
				Session["LastError"] = new Exception($"Match {player1ID} vs {player2ID} not found in {eventName}:{round}");
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


		//public ActionResult Details(string eventName, int round, int player1, int player2, int? player1wins, int? player2wins, int? draws)
		//{
		//	Setup();
  //    Magic.Domain.Event thisEvent = _eventManager.LoadEvent(eventName);

		//	var match = thisEvent.Matches.FirstOrDefault(m => (m.Round == round) && ((m.Player1.ID == player1 && m.Player2.ID == player2) || (m.Player2.ID == player1 && m.Player1.ID == player2)));
		//	ViewBag.Match = match;

		//	if (match == null)
		//	{
		//		Session["LastError"] = new Exception($"Match {player1} vs {player2} not found in {eventName}:{round}");
		//		return View("Index", new
		//		{
		//			eventName = eventName,
		//			round = round
		//		});
		//	}


		//	if (player1wins.HasValue && player2wins.HasValue && draws.HasValue)
		//	{
		//		if (thisEvent.Locked(round))
		//		{
		//			ModelState.AddModelError("CustomError", "This match is Locked");
		//		}
		//		else
		//		{
		//			match.Player1Wins = player1wins.Value;
		//			match.Player2Wins = player2wins.Value;
		//			match.Draws = draws.Value;

		//			_matchManager.Update(match);

		//			return RedirectToAction("Index", new { controller = "Magic", eventName = eventName, round = round });
		//		}
		//	}

		//	var p1Dropdown = GetDropdownWithSelected(2, match.Player1Wins);
		//	var p2Dropdown = GetDropdownWithSelected(2, match.Player2Wins);
		//	var drawsDropdown = GetDropdownWithSelected(3, match.Draws);

		//	ViewBag.player1wins = p1Dropdown;
		//	ViewBag.player2wins = p2Dropdown;
		//	ViewBag.draws = drawsDropdown;

		//	return View("MagicMatch");
		//}

		[Authorize(Roles = "Admin")]
		public ActionResult AssignPrizes(string eventName, int round)
		{
			ViewBag.Round = round;
			ViewBag.EventName = eventName;
			var prizeAssignmentTag = "playerPrizes" + eventName + round;
			ViewBag.PrizeAssignmentTag = prizeAssignmentTag;
      var thisEvent =  _eventManager.LoadEvent(eventName);
			List<dbPlayerPrize> prizeAssignments = _eventManager.GetPrizeAssignments(thisEvent, round);

			Session.Add(prizeAssignmentTag, prizeAssignments);

			return View("AssignPrizes", prizeAssignments);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult AdminMarkRecieved(int playerID, string eventName, int round, int position, int packs, int recieved)
		{
			dbPlayerPrize prize = new dbPlayerPrize()
			{
				PlayerID = playerID,
				EventName = eventName,
				Round = round,
				Position = position,
				Packs = packs,
				Recieved = recieved
			};

			_prizeManager.AcknowledgeRecievedAll(playerID, new List<dbPlayerPrize> { prize });

			return Default();
		}

		[Authorize(Roles = "Admin")]
		public ActionResult AssignPrizesConfirmation(string prizeAssignmentTag)
		{
			try
			{
				var prizeAssignments = Session[prizeAssignmentTag] as List<dbPlayerPrize>;
				_prizeManager.AssignPrizes(prizeAssignments);

				return Default();

			}
			catch(Exception ex)
			{
				//Session["LastError"] = new Exception($"Failed to assign prizes for {prizeAssignmentTag}", ex);
				return Default();
			}
		}

		[Authorize(Roles = "Admin")]
		public ActionResult ShowPrizes(bool unclaimedOnly = true)
		{
			var model = _prizeManager.GetAllPlayerPrizes();
			return View("ViewAssignedPrizes", model);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult ViewEvents()
		{
			Setup();
      var eventList = _eventManager.LoadAllEvents();

			return View("ViewEvents", eventList);
		}

		public ActionResult EventArchiveList()
		{
			Setup();
			var eventList = _eventManager.LoadAllEvents();

			return View("EventArchiveList", eventList);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult CreateEvent()
		{
			Setup();
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
			Setup();
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
			Setup();
			return View();
		}

		[Authorize(Roles = "Admin")]
		public ActionResult ListPlayers(string eventName)
		{
			Setup();
			var thisEvent = _eventManager.LoadEvent(eventName);
			var allPlayers = _playerManager.GetAllPlayers();

			ViewBag.AllPlayers = allPlayers;

			return View("ListPlayers", thisEvent);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult AddPlayer(string eventName, string playerName)
		{
			Setup();
			var thisEvent = _eventManager.LoadEvent(eventName);

			var newPlayer = new Player(playerName, null, 0);
			_eventManager.AddPlayer(thisEvent, newPlayer);

			return Redirect(Url.Action("ListPlayers", "Magic", new { eventName = eventName }));
		}

		[Authorize(Roles = "Admin")]
		public ActionResult GeneratePairings(string eventName)
		{
			Setup();
			var pairingsManager = new PairingsManager(_eventManager);

			var thisEvent = pairingsManager.LoadDatabase(eventName);
			pairingsManager.GeneratePairings(thisEvent);
			Session["pairedEvent"] = thisEvent;
			return View("PreviewPairings", thisEvent);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult SaveMatches()
		{
			Setup();
			Event eventToSave = Session["pairedEvent"] as Event;
			_matchManager.UpdateAllMatches(eventToSave.Matches, eventToSave.CurrentRound);

			return RedirectToAction("Index", new { eventName = eventToSave.name, round = eventToSave.CurrentRound });
		}

		[AllowAnonymous]
		public ActionResult PlayerStats(int playerID)
		{
			Setup();
			List<Player> allPlayers = _playerManager.GetAllPlayers().OrderBy(p=>p.Name).ToList();

			var playerList = allPlayers.Select(p => new SelectListItem { Text = p.Name, Value = p.ID.ToString() }); // CHANGED THIS to ID FROM Name
			var currentPlayer = allPlayers.FirstOrDefault(p => p.Email == User.Identity.Name);

			if (playerID <= 0 && currentPlayer?.ID > 0)
			{
				playerID = currentPlayer.ID;
			}
			else if (playerID <= 0 && currentPlayer?.ID <= 0)
			{
				playerID = allPlayers.FirstOrDefault()?.ID ?? 0;
			}

			PlayerScoreSummary playerStatistics = _matchManager.GetPlayerStatistics(playerID);

			ViewBag.playerID = playerList;
			ViewBag.CurrentUser = allPlayers.First(p => p.ID == playerID).Name;
			
			return View("PlayerStats", playerStatistics);
		}

		[Authorize(Roles="Admin")]
		public ActionResult PrizeSetup(string eventName, int round)
		{
			Setup();
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
					Session["LastError"] = new Exception($"Not Saved: {ex.Message}\n{ex.StackTrace}");
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