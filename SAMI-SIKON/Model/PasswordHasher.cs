using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model
{
    public class PasswordHasher
    {
        public static string SaltMaker(int saltSize)
        {
            var saltBytes = new byte[saltSize];
            var randomSaltGenerator = new RNGCryptoServiceProvider();
            randomSaltGenerator.GetNonZeroBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }

        public static string HashPasswordAndSalt(string password, string salt, int nIterations, int nHash)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var rfc2898DeriveBytes= new Rfc2898DeriveBytes(password, saltBytes, nIterations);
            string hashedSaltAndPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
            return hashedSaltAndPassword;
        }
    }
}
