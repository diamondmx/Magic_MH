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
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

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
								defaults: new { controller = "Magic", action = "EditEvent" }
				);

			routes.MapRoute(
								name: "Default",
								url: "Magic/Index/{eventName}/{round}/{detailedMode}",
								defaults: new { controller = "Magic", action = "Index", eventName = "BFZ", round = 3, detailedMode = false }
			 );

			routes.MapRoute(
					name: "Details",
					url: "Magic/Match/{eventName}/{round}/{player1}/{player2}",
					defaults: new { controller = "Magic", action = "Details", eventName = "ORI", round = 3, player1wins = UrlParameter.Optional }
			);

			routes.MapRoute(
					name: "Generic",
					url: "{controller}/{action}/",
					defaults: new { controller = "Magic", action = "Index", eventName = "FRF", round = 2, detailedMode = false}
			);
		}
	}
}
