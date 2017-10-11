using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IdentityUsersManager.Logic
{
    class PasswordManager
    {
        public string ProtectPassword(string password, out string entropy)
        {
            byte[] plaintext = Encoding.UTF8.GetBytes(password);
            
            byte[] ent = new byte[20];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(ent);
            }

            byte[] ciphertext = ProtectedData.Protect(plaintext, ent,
                DataProtectionScope.CurrentUser);
            entropy = BitConverter.ToString(ent);
            return BitConverter.ToString(ciphertext);
        }

        public string UnProtectData(string text, string entropy)
        {
            byte[] plaintext = ProtectedData.Unprotect(text.Split('-').Select(x => byte.Parse(x, NumberStyles.HexNumber))
    .ToArray(), entropy.Split('-').Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToArray(),
    DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(plaintext);
        }
    }
}
