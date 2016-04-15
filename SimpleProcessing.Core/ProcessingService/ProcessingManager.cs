using SimpleProcessing.Core.StorageService;
using SimpleProcessing.Models.Orders;
using SimpleProcessing.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleProcessing.Core.Infrastructure;
using SimpleProcessing.Models.Cards;
using System.ComponentModel.DataAnnotations;

namespace SimpleProcessing.Core.ProcessingService
{
	public class ProcessingManager : IProcessingManager
	{
		ICardManager _cardManager;
		IOrderStorage _orderStorage;

        public ProcessingManager(ICardManager cardManager, IOrderStorage orderStorage)
		{
			_cardManager = cardManager;
			_orderStorage = orderStorage;
        }

		public void Pay(PayOrderDto order)
		{
			Contract.Requires<ArgumentNullException>(order != null && order.CardInfo != null);			

			var usedCard = _cardManager.GetCardByStandartInfo(order.CardInfo);
			_cardManager.CardOperation(usedCard.CardId, order.AmountKop);

			try
			{
				_orderStorage.AddOrder(new PayOrder()
				{
					OrderId = Guid.NewGuid().ToString(),
					AmountKop = order.AmountKop,
					ExternalOrderId = order.OrderId,
					OrderDate = DateTime.Now,
					CardId = usedCard.CardId,
					OrderStatus = OrderStatus.PROCESSING
				});
			}
			catch (Exception ex)
			{
				// "транзакционность" создания платежа
				// при ошибке создания плаьежа в БД, отменим предыдущую операцию с деньгами на карте				
				_cardManager.CardOperation(usedCard.CardId, order.AmountKop, true);
				throw ex;
			}
			
		}

		public async Task<OrderStatus> GetOrderStatus(string orderId)
		{
			var order = await GetOrder(orderId).ConfigureAwait(false);
			return order.OrderStatus;
		}

		public async Task Refund(string orderId)
		{
			var order = await GetOrder(orderId).ConfigureAwait(false);
			if (order.OrderStatus == OrderStatus.REFUNDED)
				throw new SimpleProcessingException($"payment #{orderId} already refunded");

			var amountToRefund = order.AmountKop;
			_cardManager.CardOperation(order.CardId, amountToRefund, true);

			// транзакция рефанда платежа, при ошибке отменяем предыдущую операцию с деньгами на карте
			try
			{
				_orderStorage.UpdateOrderStatus(order.ExternalOrderId, OrderStatus.REFUNDED);
			}
			catch (Exception ex)
			{
				_cardManager.CardOperation(order.CardId, amountToRefund, false);
				throw ex;
			}
			
		}		

		/// <summary>
		/// Task.FromResult() имитирует асинхронный запрос к удаленному хранилищу
		/// для демонстрации концепции использвоания async / await, 
		/// которые в ASP.NET имеет смысл использовать для продолжительных операций
		/// </summary>
		/// <param name="orderId"></param>
		/// <returns></returns>
		Task<PayOrder> GetOrder(string orderId)
		{
			Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(orderId));

			var order = _orderStorage[orderId];
			if (order == null)
			{
				string msg = $"order id #{orderId} not found";
				NLog.LogManager.GetCurrentClassLogger().Error(msg);
				throw new SimpleProcessingException(msg);
			}

			return Task.FromResult(order);
		}


	}
}
