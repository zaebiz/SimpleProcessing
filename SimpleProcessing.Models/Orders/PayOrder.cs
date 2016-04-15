using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Models.Orders
{
	public class PayOrder
	{
		public string OrderId { get; set; }
		public string ExternalOrderId { get; set; }
		public string CardId { get; set; }
		public decimal AmountKop { get; set; }
		public DateTime OrderDate { get; set; }
		public OrderStatus OrderStatus { get; set; }
	}
}
