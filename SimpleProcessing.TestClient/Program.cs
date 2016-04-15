using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleProcessing.Models.Orders;
using SimpleProcessing.Models.Cards;
using System.Net.Http;
using Newtonsoft.Json;

namespace SimpleProcessing.TestClient
{
	class Program
	{
		static void Main(string[] args)
		{
			TestIncorrectRequestData().Wait();
			TestSameOrderNumbers().Wait();
			TestUnlimitedCardWithdrawal().Wait();
			TestDebetCardWithdrawal().Wait();
			TestRefundForDebitCard().Wait();

			Console.ReadKey();
		}

		#region Payment Objects
		static PayOrderDto CreateUnlimitedCardRequest()
		{
			return new PayOrderDto()
			{
				AmountKop = 60000,
				OrderId = "123",
				CardInfo = new CreditCardStandartInfo
				{
					CardholderName = "Nikita Titarenko",
					CVVCode = "333",
					CardNumber = "3333333333333333",
					ExpireMonth = 4,
					ExpireYear = 2017
				}
			};
		}

		static PayOrderDto CreateDebetCardRequest()
		{
			return new PayOrderDto()
			{
				AmountKop = 60000,
				OrderId = "456",
				CardInfo = new CreditCardStandartInfo
				{
					CardholderName = "Ivan Ivanov",
					CVVCode = "111",
					CardNumber = "1111111111111111",
					ExpireMonth = 4,
					ExpireYear = 2017
				}
			};
		}
		#endregion

		static async Task TestRefundForDebitCard()
		{
			Console.WriteLine("");
			Console.WriteLine("TEST DEBET CARD REFUND");
			Console.WriteLine("###############################");

			// первый платеж - ок
			PayOrderDto firstPayRequest = CreateDebetCardRequest();
			await PayRequest(firstPayRequest);

			// второй платеж - не хватает средств
			PayOrderDto secondPayRequest = CreateDebetCardRequest();
			secondPayRequest.OrderId = Guid.NewGuid().ToString();
			await PayRequest(secondPayRequest);

			// статус первого платежа
			await PaymentStatusRequest(firstPayRequest.OrderId);

			// рефанд первого платежа
			await PaymentRefundRequest(firstPayRequest.OrderId);

			// статус первого платежа
			await PaymentStatusRequest(firstPayRequest.OrderId);

			// теперь денег на карте хвататет на второй платеж
			await PayRequest(secondPayRequest);
		}

		// бесконечно можно снимать, лимита нет
		static async Task TestUnlimitedCardWithdrawal()
		{
			Console.WriteLine("");
			Console.WriteLine("TEST UNLIMITED CARD WITHDRAWAL ");
			Console.WriteLine("###############################");

			PayOrderDto request = CreateUnlimitedCardRequest();
			await PayRequest(request);

			request.OrderId = Guid.NewGuid().ToString();
			await PayRequest(request);

			request.OrderId = Guid.NewGuid().ToString();
			await PayRequest(request);
		}

		// баланс карты 1000. 
		// после первой транзакции (600р) вторая  и последующие с аналогичными суммами не проходят
		static async Task TestDebetCardWithdrawal()
		{
			Console.WriteLine("");
			Console.WriteLine("TEST DEBET CARD WITHDRAWAL ");
			Console.WriteLine("###############################");

			PayOrderDto request = CreateDebetCardRequest();
			await PayRequest(request);

			request.OrderId = Guid.NewGuid().ToString();
			await PayRequest(request);

			request.OrderId = Guid.NewGuid().ToString();
			await PayRequest(request);
		}

		// запросы с неверными данными по карте
		static async Task TestIncorrectRequestData()
		{
			Console.WriteLine("");
			Console.WriteLine("TEST INCORRECT REQUEST DATA");
			Console.WriteLine("###############################");

			PayOrderDto incorrectRequest = CreateUnlimitedCardRequest();
			incorrectRequest.CardInfo.CardholderName = "Incorrect Name";
			await PayRequest(incorrectRequest);

			incorrectRequest.CardInfo.CardholderName = "Nikita Titarenko";
			incorrectRequest.CardInfo.CardNumber = "wrong number";
			await PayRequest(incorrectRequest);

			incorrectRequest.CardInfo.CardNumber = "3333333333333333";
			incorrectRequest.CardInfo.CVVCode = null;
			await PayRequest(incorrectRequest);
		}

		// запрещены платежи с одинаковыми айди
		static async Task TestSameOrderNumbers()
		{
			Console.WriteLine("");
			Console.WriteLine("TEST ORDER ID UNIQUE CONTROL");
			Console.WriteLine("###############################");

			PayOrderDto incorrectRequest = CreateUnlimitedCardRequest();
			await PayRequest(incorrectRequest);
			await PayRequest(incorrectRequest);
		}

		#region Request Methods
		static async Task PayRequest(PayOrderDto req)
		{
			string reqContent = JsonConvert.SerializeObject(req);
			Console.WriteLine("REQUEST");
			Console.WriteLine(reqContent);

			using (HttpClient client = new HttpClient())
			{
				var request = new HttpRequestMessage()
				{
					RequestUri = new Uri("http://localhost:10393/api/processing/pay"),
					Method = HttpMethod.Post,
					Content = new StringContent(reqContent, Encoding.UTF8, "application/json")
				};

				request.Headers.Add("AuthorizationToken", "SecretKey123");
				var response = await client.SendAsync(request);

				string data = await response.Content.ReadAsStringAsync();
				Console.WriteLine("RESPONSE");
				Console.WriteLine(data);
			}
		}

		static async Task PaymentStatusRequest(string orderId)
		{
			Console.WriteLine("REQUEST");
			Console.WriteLine($"GetOrderStatus (id={orderId})");

			using (HttpClient client = new HttpClient())
			{
				var request = new HttpRequestMessage()
				{
					RequestUri = new Uri("http://localhost:10393/api/processing/status"),
					Method = HttpMethod.Post,
					Content = new StringContent(orderId, Encoding.UTF8, "application/json")
				};

				request.Headers.Add("AuthorizationToken", "SecretKey123");
				var response = await client.SendAsync(request);

				string data = await response.Content.ReadAsStringAsync();
				Console.WriteLine("RESPONSE");
				Console.WriteLine(data);
			}
		}

		static async Task PaymentRefundRequest(string orderId)
		{
			Console.WriteLine("REQUEST");
			Console.WriteLine($"Refund Payment (id={orderId})");

			using (HttpClient client = new HttpClient())
			{
				var request = new HttpRequestMessage()
				{
					RequestUri = new Uri("http://localhost:10393/api/processing/refund"),
					Method = HttpMethod.Post,
					Content = new StringContent(orderId, Encoding.UTF8, "application/json")
				};

				request.Headers.Add("AuthorizationToken", "SecretKey123");
				var response = await client.SendAsync(request);

				string data = await response.Content.ReadAsStringAsync();
				Console.WriteLine("RESPONSE");
				Console.WriteLine(data);
			}
		}
		#endregion

	}
}
