using SimpleProcessing.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Core.ProcessingService
{
	public interface IProcessingManager
	{
		void Pay(PayOrderDto dto);
		Task<OrderStatus> GetOrderStatus(string orderId);
		Task Refund(string orderId);
	}
}
