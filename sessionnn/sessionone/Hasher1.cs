using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace sessionone
{
   static class Hasher
    {
       static public String hashToMD5(String text)
        {
            var md5 = MD5.Create();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(text);
            byte[] hash = md5.ComputeHash(passwordBytes);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var stroke in hash)
            {
                stringBuilder.Append(stroke.ToString("X2"));
            }
            return Convert.ToString(stringBuilder);
        }
    }
}
