using System.Security.Cryptography;
using System.Text;

namespace SqlUpdateManager.Core.Common
{
	internal static class Hasher
    {
		internal static byte[] GetHash(string data) =>
            SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
    }
}
