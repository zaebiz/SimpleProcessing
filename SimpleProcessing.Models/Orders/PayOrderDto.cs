using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleProcessing.Models.Cards;

namespace SimpleProcessing.Models.Orders
{
	public class PayOrderDto
	{
		public string OrderId { get; set; }
		public decimal AmountKop { get; set; }
		public CreditCardStandartInfo CardInfo { get; set; }
	}
}
