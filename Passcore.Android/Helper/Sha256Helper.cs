using System.Security.Cryptography;
using System.Text;

namespace Passcore.Android.Helper
{
    public class Sha256Helper
    {
        private static SHA256 _sha256 = SHA256.Create();

        public static byte[] Compute(string rawData)
        {
            return _sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        }
    }
}