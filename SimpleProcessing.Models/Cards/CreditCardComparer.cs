using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Models.Cards
{
	public class CreditCardComparer : IEqualityComparer<CreditCardStandartInfo>
	{
		public bool Equals(CreditCardStandartInfo x, CreditCardStandartInfo y)
		{
			return
				x.CardholderName.ToUpper() == y.CardholderName.ToUpper()
				&& x.CardNumber == y.CardNumber
				&& x.CVVCode == y.CVVCode
				&& x.ExpireMonth == y.ExpireMonth
				&& x.ExpireYear == y.ExpireYear;
		}

		public int GetHashCode(CreditCardStandartInfo obj)
		{
			string uniqueId = $"{obj.CardholderName}_{obj.CardNumber}_{obj.CVVCode}";
            return uniqueId.GetHashCode();
		}
	}
}
