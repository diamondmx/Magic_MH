using System.Web.Mvc;
using System.Web.Routing;

namespace IdentitySample
{
	public class RouteConfig
	{
		public static int currentRound = 1;
		public static string currentEvent = "SOI";
		public static int defaultDetail = 0;


		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
					name: "HomeDefault",
					url: "{controller}/{action}/{id}",
					defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

			routes.MapRoute(
					name: "Default",
					url: "{eventName}/{round}/{detailMode}",
					defaults: new { controller = "Magic", action = "Index", eventName = currentEvent, round = currentRound, detailMode = defaultDetail }
			);
		}
	}
}