using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Models.Orders
{
	public enum OrderStatus
	{
		PROCESSING,
		COMPLETED,
		REFUNDED,
		AUDIT
	}
}
