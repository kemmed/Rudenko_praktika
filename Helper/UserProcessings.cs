using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Rudenko_praktika.Helper
{
    public class UserProcessings
    {
        /// <summary>
        /// Хеширует пароль с использованием MD5.
        /// </summary>
        /// <param name="password">Пароль, который требуется захешировать.</param>
        /// <returns>Хеш переданного пароля.</returns>
        public static string GetPasswordHash(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(password);
                byte[] hash = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
