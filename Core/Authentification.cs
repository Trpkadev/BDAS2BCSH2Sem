using BCSH2BDAS2.Models;

namespace BCSH2BDAS2.Core
{
	public class Authentification
	{
		public static Uzivatel? LoggedUser { get; private set; }
		public static bool IsLoggedIn { get; private set; }

		public static bool Login(string username, string password)
		{
			return true;
		}
		public static bool Logout()
		{
			return true;
		}
	}
}
