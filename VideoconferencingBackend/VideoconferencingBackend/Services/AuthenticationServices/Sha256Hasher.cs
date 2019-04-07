using System.Security.Cryptography;
using System.Text;
using VideoconferencingBackend.Interfaces.Services.Authentication;

namespace VideoconferencingBackend.Services.AuthenticationServices
{
    public class Sha256Hasher : IHasherService
    {
        public string Hash(string toHash)
        {
            if (toHash == null)
                return null;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(toHash));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
