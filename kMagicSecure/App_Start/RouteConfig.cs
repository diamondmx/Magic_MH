using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace kMagicSecure
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			//routes.MapRoute(
			//	name: "IP",
			//	url: "IP/GetSourceData",
			//	defaults: new { controller = "IP", action = "GetSourceData" });

			

			routes.MapRoute(
				name: "AdminDeleteAllMatchesInRound",
				url: "Magic/AdminDeleteAllMatchesInRound/",
				defaults: new { controller = "Magic", action = "AdminDeleteAllMatchesInRound" }
				);

			routes.MapRoute(
				name: "AdminDeleteAllInRound",
				url: "Magic/AdminDeleteAllInRound/",
				defaults: new { controller = "Magic", action = "AdminDeleteAllInRound" }
				);

			routes.MapRoute(
				name: "GetMatchCountInRound",
				url: "Magic/GetMatchCountInRound/",
				defaults: new { controller = "Magic", action = "GetMatchCountInRound" }
				);

			routes.MapRoute(
				name: "AdminDeleteMatch",
				url: "Magic/AdminDeleteMatch/",
				defaults: new { controller = "Magic", action = "AdminDeleteMatch" }
				);

			routes.MapRoute(
				name: "AdminInsertMatch",
				url: "Magic/AdminInsertMatch/",
				defaults: new { controller = "Magic", action = "AdminInsertMatch" }
				);

			routes.MapRoute(
				name: "AdminModifyMatch",
				url: "Magic/AdminModifyMatch/",
				defaults: new { controller = "Magic", action = "AdminModifyMatch" }
				);

			routes.MapRoute(
				name: "AdminModifyMatches",
				url: "Magic/AdminModifyMatches/",
				defaults: new { controller = "Magic", action = "AdminModifyMatches" });

			routes.MapRoute(
				name: "AdminMarkRecieved",
				url: "Magic/AdminMarkRecieved/",
				defaults: new { controller = "Magic", action = "AdminMarkRecieved" });

			routes.MapRoute(
				name: "ShowPrizes",
				url: "Magic/ShowPrizes/{unclaimedOnly}",
				defaults: new { controller = "Magic", action = "ShowPrizes", unclaimedOnly = false });

			routes.MapRoute(
				name: "AssignPrizesConfirmation",
				url: "Magic/AssignPrizesConfirmation/{prizeAssignmentTag}",
				defaults: new { controller = "Magic", action = "AssignPrizesConfirmation" });

			routes.MapRoute(
				name: "AssignPrizes",
				url: "Magic/AssignPrizes/{EventName}/{Round}",
				defaults: new { controller = "Magic", action = "AssignPrizes" });

			routes.MapRoute(
				name: "RecievedPrizes",
				url: "Magic/RecievedPrizes",
        defaults: new { controller = "Magic", action = "RecievedPrizes" });

			routes.MapRoute(
				name: "RSS",
				url: "Magic/RSS",
				defaults: new { controller = "MagicHttp", action = "RSS2" });

			routes.MapRoute(
				name: "PrizeSetup",
				url: "Magic/PrizeSetup",
				defaults: new { controller = "Magic", action = "PrizeSetup" });

			routes.MapRoute(
				name: "PlayerStats",
				url: "Magic/PlayerStats/{playerID}",
				defaults: new { controller = "Magic", action = "PlayerStats" }
			);

			routes.MapRoute(
				name: "PlayerStats2",
				url: "Magic/PlayerStats/",
				defaults: new { controller = "Magic", action = "PlayerStats" }
			);

			routes.MapRoute(
				name: "Register",
				url: "Account/Register",
				defaults: new { controller = "Account", action = "Register", id = "registerLink" }
			);

			routes.MapRoute(
				name: "Login",
        url: "Account/Login",
				defaults: new { controller = "Account", action = "Login", id = "loginLink" }
			);

			routes.MapRoute(
				name: "SaveMatches",
				url: "Magic/SaveMatches",
				defaults: new { controller = "Magic", action = "SaveMatches" }
				);

			routes.MapRoute(
				name: "GeneratePairings",
				url: "Magic/GeneratePairings",
				defaults: new { controller = "Magic", action = "GeneratePairings" }
				);

			routes.MapRoute(
				name: "AddPlayer",
				url: "Magic/AddPlayer",
				defaults: new { controller = "Magic", action = "AddPlayer" }
				);

			routes.MapRoute(
				name: "CreateEvent",
				url: "Magic/CreateEvent",
				defaults: new { controller = "Magic", action = "CreateEvent" }
				);

			routes.MapRoute(
				name: "ListPlayers",
				url: "Magic/ListPlayers",
				defaults: new { controller = "Magic", action = "ListPlayers" });

			routes.MapRoute(
								name: "ViewEvents",
								url: "Magic/ViewEvents",
								defaults: new { controller = "Magic", action = "ViewEvents" });

			routes.MapRoute(
								name: "EventArchiveList",
								url: "Magic/EventArchiveList",
								defaults: new { controller = "Magic", action = "EventArchiveList" });

			routes.MapRoute(
								name: "EditEvent",
								url: "Magic/EditEvent/{eventName}",
								defaults: new { controller = "Magic", action = "EditEvent", eventName = UrlParameter.Optional }
				);

			routes.MapRoute(
				name: "DefaultMatchList",
				url: "Magic/Default",
				defaults: new { controller = "Magic", action = "Default" });

			routes.MapRoute(
					name: "Default",
					url: "Magic/{eventName}/{round}/{detailMode}",
					defaults: new { controller = "Magic", action = "Index", eventName = "DEFAULT", round = -1, detailMode = 0}
					);

			routes.MapRoute(
					name: "Details",
					url: "Magic/Match/{eventName}/{round}/{player1ID}/{player2ID}",
					defaults: new { controller = "Magic", action = "Details"}
			);

			routes.MapRoute(
					name: "Generic",
					url: "{controller}/{action}/",
					defaults: new { controller = "Magic", action = "Default" });
		}
	}
}
