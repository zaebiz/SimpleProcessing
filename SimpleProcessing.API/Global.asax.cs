using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleProcessing.Core.StorageService;
using SimpleProcessing.Models.Cards;

namespace SimpleProcessing.API
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			AutofacConfig.ConfigureDependencyResolver();
			InitializeDB();
        }

		void InitializeDB()
		{
			var debetCard = new CreditCard
			{
				CardholderName = "Ivan Ivanov",
				CardId = "1",
				CardNumber = "1111111111111111",
				CardType = CreditCardType.DEBET,
				MoneyAmountKop = 100000,
				CreditLimitKop = 0,
				CVVCode = "111",
				ExpireDate = DateTime.Now.AddYears(1)
			};

			var creditCard = new CreditCard
			{
				CardholderName = "Petr Petrov",
				CardId = "2",
				CardNumber = "2222222222222222",
				CardType = CreditCardType.CREDIT,
				MoneyAmountKop = 100000,
				CreditLimitKop = 100000,
				CVVCode = "222",
				ExpireDate = DateTime.Now.AddYears(1)
			};

			var unlimitedCard = new CreditCard
			{
				CardholderName = "Nikita Titarenko",
				CardId = "3",
				CardNumber = "3333333333333333",
				CardType = CreditCardType.UNLIMITED,
				MoneyAmountKop = 100000,
				CreditLimitKop = 0,
				CVVCode = "333",
				ExpireDate = DateTime.Now.AddYears(1)
			};

			CardStorage.AddNewCreditCard(debetCard);
			CardStorage.AddNewCreditCard(creditCard);
			CardStorage.AddNewCreditCard(unlimitedCard);
		}
	}
}
