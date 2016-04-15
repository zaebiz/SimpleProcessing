using SimpleProcessing.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Core.ProcessingService
{
	public interface ICardManager
	{
		CreditCard GetCardByStandartInfo(CreditCardStandartInfo ccInfo);
		string CardOperation(string cardId, decimal amount, bool isRefund = false);
	}
}
