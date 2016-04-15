using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SimpleProcessing.Models.Cards
{

	public class CreditCardStandartInfo
	{
		[Required]
		[RegularExpression("^[0-9]{16}$")]
		public string CardNumber { get; set; }

		[Required]
		[MinLength(6)]
		public string CardholderName { get; set; }

		[Required]
		[RegularExpression("^[0-9]{3}$")]
		public string CVVCode { get; set; }

		[Required]
		public virtual int ExpireMonth { get; set; }

		[Required]
		public virtual int ExpireYear { get; set; }
	}
}
