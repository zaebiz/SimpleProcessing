using SimpleProcessing.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Core
{
	public class SimpleProcessingConfig
	{
		public static Dictionary<OrderStatus, string> OrderStatusList;

		static SimpleProcessingConfig()
		{
			OrderStatusList = new Dictionary<OrderStatus, string>();
			OrderStatusList.Add(OrderStatus.AUDIT, "Платеж находится на проверке");
			OrderStatusList.Add(OrderStatus.COMPLETED, "Платеж успешно проведен");
			OrderStatusList.Add(OrderStatus.PROCESSING, "Платеж выполняется");
			OrderStatusList.Add(OrderStatus.REFUNDED, "Платеж отменен");
		}
	}
}
