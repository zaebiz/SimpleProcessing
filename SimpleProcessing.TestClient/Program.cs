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
			var task = TestIncorrectRequestData();
			task.Wait();
			Console.ReadKey();
		}

		static async Task TestIncorrectRequestData()
		{
			PayOrderDto incorrectRequest = new PayOrderDto()
			{
				AmountKop = 100000,
				OrderId = "123",
				CardInfo = new CreditCardStandartInfo
				{
					CardholderName = "Nikita Titarenko Error",
					CVVCode = "333",
					CardNumber = "3333333333333333",
					ExpireMonth = 4,
					ExpireYear = 4
				}
			};

			await PayRequest(incorrectRequest);
		}

		static async Task PayRequest(PayOrderDto req)
		{
			string reqContent = JsonConvert.SerializeObject(req);
			Console.WriteLine(reqContent);

			using (HttpClient client = new HttpClient())
			{
				//StringContent content = 
				var request = new HttpRequestMessage()
				{
					RequestUri = new Uri("http://localhost:10393/api/processing/pay"),
					Method = HttpMethod.Post,
					Content = new StringContent(reqContent, Encoding.UTF8, "application/json")
				};

				request.Headers.Add("AuthorizationToken", "SecretKey123");
				var response = await client.SendAsync(request);

				//var response = await client.PostAsync("http://localhost:10393/api/processing/pay", content);
				//response.EnsureSuccessStatusCode();

				string data = await response.Content.ReadAsStringAsync();
				Console.WriteLine(data);
			}
		}
	}
}
