using System;
using SimpleProcessing.Models.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace SimpleProcessing.Models.Cards
{
	public class CreditCard : CreditCardStandartInfo
	{
		public string CardId { get; set; }
		public CreditCardType CardType { get; set; }
		public decimal MoneyAmountKop { get; set; }
		public decimal CreditLimitKop { get; set; }
		public DateTime ExpireDate { get; set; }

		public override int ExpireMonth
		{
			get { return ExpireDate.Month; }
		}

		public override int ExpireYear
		{
			get { return ExpireDate.Year; }
		}

		public bool IsMoneyAvailable(decimal amount)
		{
			return 
				MoneyAmountKop + CreditLimitKop >= amount
				|| CardType == CreditCardType.UNLIMITED;
        }
	}
}
