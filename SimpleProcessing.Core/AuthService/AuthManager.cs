using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SimpleProcessing.Core.AuthService
{
	public class AuthManager : IAuthManager
	{
		const string SECRET_KEY = "SecretKey123";
		HttpContextBase _httpContext;

		string AuthKey
		{
			get { return _httpContext.Request.Headers["AuthorizationToken"]; }
		}

		string RequestIP
		{
			get { return _httpContext.Request.UserHostAddress; }
		}

		public AuthManager(HttpContextBase context)
		{
			_httpContext = context;
        }

		/// <summary>
		/// проверка корректности переданного кода
		/// для примера будем проверят простое соответствие заранее заданой константе
		/// </summary>
		/// <returns></returns>
		public bool CheckUserAuthorized()
		{
			if (!IsCodeCorrect())
			{
				NLog.LogManager.GetCurrentClassLogger().Error("unauthorized access from " + RequestIP);
				throw new SimpleProcessing.Models.Exceptions.SimpleProcessingException("unauthorized access");
			}
				
			return true;
		}

		bool IsCodeCorrect()
		{
			return AuthKey == SECRET_KEY;
		}
	}
}
