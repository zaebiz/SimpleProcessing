using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProcessing.Core.AuthService
{
	public interface IAuthManager
	{
		bool CheckUserAuthorized();
    }
}
