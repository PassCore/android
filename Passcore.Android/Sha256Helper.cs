using System.Security.Cryptography;
using System.Text;

namespace Passcore.Android
{
    public class Sha256Helper
    {
        private static SHA256 _sha256 = SHA256.Create();

        public static string Compute(string rawData)
        {
            byte[] bytes = _sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
 
            StringBuilder builder = new StringBuilder();
            foreach (var i in bytes)
            {
                builder.Append(i.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}