using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SimpleProcessing.Core.AuthService;
using SimpleProcessing.Models.Orders;
using System.Web;
using System.Diagnostics.Contracts;
using SimpleProcessing.Core.ProcessingService;
using SimpleProcessing.Models.Exceptions;
using Newtonsoft.Json;
using SimpleProcessing.Models.Errors;
using System.Web.Http.Results;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace SimpleProcessing.API.Controllers
{
	[RoutePrefix("api/processing")]
	public class ProcessingController : ApiController
	{
		HttpContextBase _ctx;
		IAuthManager _auth;
		IProcessingManager _bank;

		public ProcessingController()
		{}

		public ProcessingController(IAuthManager auth, IProcessingManager bank)
		{
			_auth = auth;
			_bank = bank;
		}

		#region Response Wrappers
		[NonAction]
		IHttpActionResult CreateJsonResponse(string result, ErrorDefination error = null)
		{
			var httpCode = (error == null) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
			var resultObj = (result != null) ? JsonConvert.DeserializeObject(result) : null;

			var response = new
			{
				Time = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
				Result = resultObj,
				Error = error
			};

			return new ResponseMessageResult(new HttpResponseMessage(httpCode)
			{
				Content = new ObjectContent(response.GetType(), response, new JsonMediaTypeFormatter())
			});
		}

		[NonAction]
		IHttpActionResult OkResponse()
		{
			var responseObj = new { Status = "ok" };
			string strObj = JsonConvert.SerializeObject(responseObj);
			return CreateJsonResponse(strObj);
		}
		#endregion

		[Route("online")]
		[HttpPost]
		public IHttpActionResult GetTest()
		{
			return OkResponse();
		}

		[Route("pay")]
		[HttpPost]
		public IHttpActionResult Pay(PayOrderDto order)
		{
			try
			{
				_auth.CheckUserAuthorized();
				_bank.Pay(order);

				return OkResponse();
			}
			catch (SimpleProcessingException spex)
			{
				return CreateJsonResponse(null, new ErrorDefination(400, spex.Message));
			}
			catch (Exception ex)
			{
				return CreateJsonResponse(null, new ErrorDefination(500, ex.Message));
			}
		}		

		[Route("status")]
		[HttpPost]
		public async Task<IHttpActionResult> GetOrderStatus([FromBody]string orderId)
		//public IHttpActionResult GetOrderStatus()
		{
			try
			{
				_auth.CheckUserAuthorized();

				var statusCode = await _bank.GetOrderStatus(orderId);
				var response = new
				{
					code = (int)statusCode,
					name = Core.SimpleProcessingConfig.OrderStatusList[statusCode]
				};

				return CreateJsonResponse(JsonConvert.SerializeObject(response));
			}
			catch (SimpleProcessingException spex)
			{
				return CreateJsonResponse(null, new ErrorDefination(400, spex.Message));
			}
			catch (Exception ex)
			{
				return CreateJsonResponse(null, new ErrorDefination(500, ex.Message));
			}
		}

		[Route("refund")]
		[HttpPost]
		public async Task<IHttpActionResult> Refund([FromBody]string orderId)
		{
			try
			{
				_auth.CheckUserAuthorized();
				await _bank.Refund(orderId);

				return OkResponse();
			}
			catch (SimpleProcessingException spex)
			{
				return CreateJsonResponse(null, new ErrorDefination(400, spex.Message));
			}
			catch (Exception ex)
			{
				return CreateJsonResponse(null, new ErrorDefination(500, ex.Message));
			}
		}

	}
}
