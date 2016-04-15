using SimpleProcessing.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Core.StorageService
{
	public interface IOrderStorage : IEnumerable<PayOrder>
	{		
		PayOrder this[string orderId] { get; }
		void AddOrder(PayOrder order);
		void UpdateOrderStatus(string orderId, OrderStatus status);
		void RemoveOrder(string orderId);
	}
}
