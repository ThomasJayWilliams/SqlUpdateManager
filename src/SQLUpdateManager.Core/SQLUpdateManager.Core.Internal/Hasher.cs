using System.Security.Cryptography;
using System.Text;

namespace SQLUpdateManager.Core.Internal
{
    public static class Hasher
    {
        public static byte[] GetHash(string data) =>
            SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
    }
}
