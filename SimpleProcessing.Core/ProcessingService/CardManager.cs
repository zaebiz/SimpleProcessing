using SimpleProcessing.Core.Infrastructure;
using SimpleProcessing.Core.StorageService;
using SimpleProcessing.Models.Cards;
using SimpleProcessing.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Core.ProcessingService
{
	public class CardManager : ICardManager
	{
		static readonly object _syncItem;
		ICardStorage _storage;

		static CardManager()
		{
			_syncItem = new object();
		}

		public CardManager(ICardStorage storage)
		{
			_storage = storage;
		}

		public CreditCard GetCardByStandartInfo(CreditCardStandartInfo ccInfo)
		{
			Contract.Requires<ArgumentNullException>(ccInfo != null);
			ValidateCreditCardInfo(ccInfo);

			var existingCard = _storage.GetByStandartInfo(ccInfo);
			if (existingCard == null)
			{
				string msg = $"requested credit card #{ccInfo.CardNumber} not found";
				NLog.LogManager.GetCurrentClassLogger().Error(msg);
				throw new SimpleProcessingException(msg);
			}

			return existingCard;
		}

		public string CardOperation(string cardId, decimal amount, bool isRefundOperation = false)
		{
			Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(cardId));
			Contract.Requires<ArgumentException>(amount > 0);

			var card = _storage[cardId];
			lock (_syncItem)
			{
				if (!card.IsMoneyAvailable(amount))
				{
					string msg = $"requested amount for credit card #{card.CardNumber} not available";
					NLog.LogManager.GetCurrentClassLogger().Error(msg);
					throw new SimpleProcessingException(msg);
				}
					
				if (isRefundOperation) amount *= -1;
				card.MoneyAmountKop -= amount;
				return card.CardId;
			}
		}

		void ValidateCreditCardInfo(CreditCardStandartInfo ccInfo)
		{
			ICollection<ValidationResult> result;
			bool isValid = DataAnnotationsValidator.TryValidate(ccInfo, out result);
			if (!isValid)
			{
				string msg = new String(result.SelectMany(x => x.ErrorMessage + " \n").ToArray());
				throw new SimpleProcessingException(msg);
			}
		}
	}
}
