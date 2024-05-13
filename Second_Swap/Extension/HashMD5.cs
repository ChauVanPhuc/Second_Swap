
using System.Security.Cryptography;
using System.Text;

namespace Second_Swap.Extension{
    public static class HashMD5
    {
        public static string ToMD5(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bHash )
            {
                sb.Append((String.Format("{0:x2}", b)));

                
            }
            return sb.ToString();
        }
    }
}
