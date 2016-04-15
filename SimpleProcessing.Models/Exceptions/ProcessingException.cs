using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Models.Exceptions
{
	public class SimpleProcessingException : Exception
	{
		public SimpleProcessingException() : base(){}
		public SimpleProcessingException(string message) : base(message){ }
		public SimpleProcessingException(string message, System.Exception inner) : base(message, inner) { }
		protected SimpleProcessingException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
	}
}
