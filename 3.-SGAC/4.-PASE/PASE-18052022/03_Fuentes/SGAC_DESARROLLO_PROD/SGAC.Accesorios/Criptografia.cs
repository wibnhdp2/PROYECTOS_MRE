using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace SGAC.Accesorios
{
    public class Criptografia
    {
        public static string Encriptar(string strCadena)
        {
            UnicodeEncoding parser = new UnicodeEncoding();
            byte[] OriginalValue = parser.GetBytes(strCadena);
            MD5CryptoServiceProvider Hash = new MD5CryptoServiceProvider();
            byte[] EncryptValue = Hash.ComputeHash(OriginalValue);
            return Convert.ToBase64String(EncryptValue);
        }
    }
}
