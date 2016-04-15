using SimpleProcessing.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics.Contracts;

namespace SimpleProcessing.Core.StorageService
{
	public class OrderStorage : IOrderStorage
	{
		static readonly object _syncItem;
		static Dictionary<string, PayOrder> _db;

		static OrderStorage()
		{
			_db = new Dictionary<string, PayOrder>();
			_syncItem = new object();
		}

		public IEnumerator<PayOrder> GetEnumerator()
		{
			return _db.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public PayOrder this[string orderId]
		{
			get { return GetById(orderId); }
		}

		PayOrder GetById(string orderId)
		{
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(orderId));

			if (_db.ContainsKey(orderId))
				return _db[orderId];

			return null;
		}

		public void AddOrder(PayOrder order)
		{
			if (GetById(order.ExternalOrderId) != null)
				throw new Models.Exceptions.SimpleProcessingException("duplicate order");
			
			_db.Add(order.ExternalOrderId, order);
			NLog.LogManager.GetCurrentClassLogger().Info($"order #{order.OrderId} for card #{order.CardId} created");
		}

		public void UpdateOrderStatus(string orderId, OrderStatus status)
		{
			Contract.Requires<ArgumentException>(String.IsNullOrEmpty(orderId));

			var order = _db[orderId];
			order.OrderStatus = status;

			NLog.LogManager.GetCurrentClassLogger().Info($"order #{order.OrderId} change status to {SimpleProcessingConfig.OrderStatusList[status]} ");
		}

		public void RemoveOrder(string orderId)
		{
			Contract.Requires<ArgumentException>(String.IsNullOrEmpty(orderId));

			_db.Remove(orderId);
			NLog.LogManager.GetCurrentClassLogger().Info($"order #{orderId} refunded");
		}
	}
}
