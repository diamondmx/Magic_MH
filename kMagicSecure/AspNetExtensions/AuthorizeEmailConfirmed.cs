//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace System.Web.Mvc
//{
//	public class AuthorizeEmailConfirmed : AuthorizeAttribute
//	{
//		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
//		{
//			filterContext.Result = new HttpUnauthorizedResult();
//		}

//		public override bool AuthorizeCore(HttpContextBase httpContext)
//		{
//			var baseResult = base.AuthorizeCore(httpContext);
//			return baseResult;

//		}

//	}
//}