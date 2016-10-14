using kMagicSecure._3rdParty;
using Magic.Data;
using Magic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;

namespace kMagicSecure.Controllers
{
	public class MagicHttpController : Controller
	{
		private GameLog _gameLog;

		public MagicHttpController()
		{
			var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			var dataContext = new Magic.Data.DataContextWrapper(connectionString);
			_gameLog = new Magic.Data.GameLog(dataContext);
		}

		public FeedResult RSS2()
		{
			var data = _gameLog.GetAll();

			var feedItems = data.OrderByDescending(gle=>gle.Timestamp).Select(gle => {
				var item = new SyndicationItem() { Title = new TextSyndicationContent(gle.Description) };
				item.Authors.Add(new SyndicationPerson(gle.User));
				item.PublishDate = gle.Timestamp;
				//var link = HtmlHelper.GenerateLink
				//item.Links.Add()

				return item;
			}
			);

			


			var feed = new SyndicationFeed("kMagic Activity Feed", "Match Results", null, feedItems);
			var feedResult = new FeedResult(new Rss20FeedFormatter(feed));
			feedResult.ContentType = "text/rss";
			return feedResult;
		}
	}
}