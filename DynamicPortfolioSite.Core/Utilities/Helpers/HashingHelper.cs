using System.Security.Cryptography;
using System.Text;

namespace DynamicPortfolioSite.Core.Utilities.Helpers
{
    public static class HashingHelper
    {
        public static string CreateMD5Hash(string input)
        {
            using (var md5Hasher = MD5.Create())
            {
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

        public static bool VerifyMD5Hash(string inputHash, string verifyInput)
        {
            string verifyInputHash = CreateMD5Hash(verifyInput);//İlk önce md5 e çeviriyoruz
            return inputHash.Equals(verifyInputHash);
        }
    }
}
