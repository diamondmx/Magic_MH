using System.Collections.Generic;
using System.Web.Optimization;

namespace IdentitySample
{
	public class NonOrderingBundleOrderer : IBundleOrderer
	{
		public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
		{
			return files;
		}
	}

	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			//bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
			//						));


			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
												"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
									"~/Scripts/modernizr-*"));

			var jqueryBootstrapAndUIBundle = new ScriptBundle("~/bundles/jquerybootstrapandui").Include(
								"~/Scripts/jquery-{version}.js",
								"~/Scripts/bootstrap.js",
								"~/Scripts/jquery-ui-{version}.js",
								"~/Scripts/respond.js");
			jqueryBootstrapAndUIBundle.Orderer = new NonOrderingBundleOrderer();

			bundles.Add(jqueryBootstrapAndUIBundle);

			bundles.Add(new StyleBundle("~/Content/css").Include(
								"~/Content/bootstrap.css",
								"~/Content/site.css",
								"~/Content/jquery-ui-{version}.css"));

			bundles.Add(new ScriptBundle("~/bundles/site").Include(
								"~/Scripts/site.js"));
		}
	}
}
