using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Models.Errors
{
	public class ErrorDefination
	{
		public ErrorDefination(int code, string msg)
		{
			Code = code;
			Message = msg;
		}

		public int Code { get; set; } 
		public string Message { get; set; }
	}
}
