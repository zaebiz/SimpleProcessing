using SimpleProcessing.Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Core.StorageService
{
	public interface ICardStorage : IEnumerable<CreditCard>
	{
		CreditCard this[string cardId] { get; }
		CreditCard GetByStandartInfo(CreditCardStandartInfo ccInfo);
	}
}
