using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model
{
    public class PasswordHasher
    {
        /// <summary>
        /// This method is used to generate a random salt with length equal to the parameter saltSize, it
        /// generates an array of bytes, and fills them with random values, and then converts it into a string.
        /// With ToBase64String conversion the byte array is split into groups of 3 bytes and then every 6 bits is converted into a character resulting in 4 characters every 3 bytes,
        /// this means that if the saltSize is not divisible by 3 there will be some added characters to the end of the string as padding.
        /// It also means that the length of the string will be longer than the saltSize
        /// In order to get a salt with a length of 16 you have to use saltSize 12 as (12/3)*4=16 characters.
        /// If you use a saltSize of 16 you would get (16/3)*4=21,34 and it would be padded to be 24 characters,
        /// given that 16%3=1 the last byte of the array will still correspond to 4 characters
        /// </summary>
        /// <param name="saltSize"></param>
        /// <returns></returns>
        public static string SaltMaker(int saltSize)
        {
            var saltBytes = new byte[saltSize];
            var randomSaltGenerator = new RNGCryptoServiceProvider();
            randomSaltGenerator.GetNonZeroBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
        /// <summary>
        /// This method returns a hash based on the parameters password and salt, it also takes the parameters nIterations which is the amount of iterations the random number generator uses.
        /// nHash is the size of the byte array that the random number generator creates, it is then converted to a string using Convert.ToBase64String.
        /// With ToBase64String conversion the byte array is split into groups of 3 bytes and then every 6 bits is converted into a character resulting in 4 characters every 3 bytes,
        /// this means that if the nHash is not divisible by 3 there will be some added characters to the end of the string as padding.
        /// It also means that the length of the string will be longer than the nHash
        /// In order to get a hash with a length of 16 you have to use nHash 12 as (12/3)*4=16 characters.
        /// If you use a nHash of 16 you would get (16/3)*4=21,34 and it would be padded to be 24 characters,
        /// given that 16%3=1 the last byte of the array will still correspond to 4 characters
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="nIterations"></param>
        /// <param name="nHash"></param>
        /// <returns></returns>
        public static string HashPasswordAndSalt(string password, string salt, int nIterations, int nHash)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var rfc2898DeriveBytes= new Rfc2898DeriveBytes(password, saltBytes, nIterations);
            string hashedSaltAndPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
            return hashedSaltAndPassword;
        }
    }
}
