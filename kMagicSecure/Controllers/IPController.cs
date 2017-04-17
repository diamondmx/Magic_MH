//using kMagicSecure.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.ServiceModel.Channels;
//using System.Web;
//using System.Web.Http;

//namespace kMagicSecure.Controllers
//{
//	public class IPController : ApiController
//	{
//		// GET: api/IP
//		//public IEnumerable<string> Get()
//		//{
//		//	return new string[] { "value1", "value2" };
//		//}

//		// GET: api/IP/GetSourceData
//		[Route("api/IP/GetSourceData")]
//		public SourceData GetSourceData()
//		{
//			return new SourceData()
//			{
//				IP = GetClientIp()
//			};
//		}

//		private string GetClientIp(HttpRequestMessage request = null)
//		{
//			request = request ?? Request;

//			if (request.Properties.ContainsKey("MS_HttpContext"))
//			{
//				return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
//			}
//			else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
//			{
//				RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
//				return prop.Address;
//			}
//			else if (HttpContext.Current != null)
//			{
//				return HttpContext.Current.Request.UserHostAddress;
//			}
//			else
//			{
//				return null;
//			}
//		}

//		//// POST: api/IP
//		//public void Post([FromBody]string value)
//		//{
//		//}

//		//// PUT: api/IP/5
//		//public void Put(int id, [FromBody]string value)
//		//{
//		//}

//		//// DELETE: api/IP/5
//		//public void Delete(int id)
//		//{
//		//}
//	}
//}
