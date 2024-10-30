namespace BCSH2BDAS2.Helpers;

public class OurCryptography
{
	public static string Encrypt(string text)
	{
		byte[] data = System.Text.Encoding.UTF8.GetBytes(text);
		byte[] hash = System.Security.Cryptography.SHA256.HashData(data);
		return BitConverter.ToString(hash).Replace("-", "");
	}
}