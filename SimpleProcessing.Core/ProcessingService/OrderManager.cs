using SimpleProcessing.Core.StorageService;
using SimpleProcessing.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Core.ProcessingService
{
	public class OrderManager
	{
		IOrderStorage _storage;

		public OrderManager(IOrderStorage storage)
		{
			_storage = storage;
		}

		//public void AddOrder(PayOrder order)
		//{
		//	if (_storage.GetById(order.ExternalOrderId) != null)
		//		throw new Models.Exceptions.SimpleProcessingException("duplicate order");

		//	_db.Add(order.ExternalOrderId, order);
		//}
	}
}
