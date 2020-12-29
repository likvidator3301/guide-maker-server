using System.Security.Cryptography;
using System.Text;

namespace GuideMaker.Helpers
{
    internal static class PasswordHelper
    {
        public static string GetHash(string password, string salt)
        {
            var passwordWithSalt = password + salt;

            var sha256CryptoServiceProvider = new SHA256CryptoServiceProvider();
            sha256CryptoServiceProvider.Initialize();

            var bytes = Encoding.UTF8.GetBytes(passwordWithSalt.ToCharArray());
            var sha256 = sha256CryptoServiceProvider.ComputeHash(bytes);

            var sha256String = Encoding.UTF8.GetString(sha256);

            return sha256String;
        }
    }
}
