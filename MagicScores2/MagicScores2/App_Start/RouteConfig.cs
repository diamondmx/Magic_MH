using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MagicScores2
{
	public class RouteConfig
	{
        public static int currentRound = 2;
        public static string currentEvent = "OGW";
        public static int defaultDetail = 0;

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

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
								name: "EditEvent",
								url: "Magic/EditEvent/{eventName}",
								defaults: new { controller = "Magic", action = "EditEvent", eventName = UrlParameter.Optional }
				);

            routes.MapRoute(
                name: "Default",
                url: "{eventName}/{round}/{detailMode}",
                defaults: new { controller="Magic", action = "Index", eventName = currentEvent, round=currentRound, detailMode=defaultDetail }
                );

			routes.MapRoute(
					name: "Details",
					url: "Magic/Match/{eventName}/{round}/{player1}/{player2}",
					defaults: new { controller = "Magic", action = "Details", eventName = currentEvent, round = currentRound, player1wins = UrlParameter.Optional }
			);

			routes.MapRoute(
					name: "Generic",
					url: "{controller}/{action}/",
					defaults: new { controller = "Magic", action = "Index", eventName = currentEvent, round = currentRound, detailedMode = defaultDetail}
			);
		}
	}
}
