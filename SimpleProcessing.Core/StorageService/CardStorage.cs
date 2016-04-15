using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleProcessing.Models.Cards;
using System.Diagnostics.Contracts;
using SimpleProcessing.Models.Exceptions;

namespace SimpleProcessing.Core.StorageService
{
	public class CardStorage : ICardStorage
	{		
		static List<CreditCard> _db;

		#region static members
		static CardStorage()
		{
			_db = new List<CreditCard>();
		}

		public static void AddNewCreditCard(CreditCard card)
		{
			_db.Add(card);
		}
		#endregion

		public CardStorage() {}

		public IEnumerator<CreditCard> GetEnumerator()
		{
			return _db.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public CreditCard this[string cardId]
		{
			get { return GetById(cardId); }
        }

		CreditCard GetById(string cardId)
		{
			return _db.FirstOrDefault(c => c.CardId == cardId);
		}

		public CreditCard GetByStandartInfo(CreditCardStandartInfo ccInfo)
		{
			CreditCardComparer _ccComparer = new CreditCardComparer();

			return _db.FirstOrDefault(c =>
				_ccComparer.Equals(c, ccInfo)
			);
		}
		
	}
}
