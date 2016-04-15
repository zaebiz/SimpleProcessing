using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleProcessing.Models.Orders;
using SimpleProcessing.Models.Cards;

namespace SimpleProcessing.TestClient
{
	class Program
	{
		static void Main(string[] args)
		{
		}

		static void TestIncorrectRequestData()
		{
			PayOrderDto incorrectName = new PayOrderDto()
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
			}
		}
	}
}
