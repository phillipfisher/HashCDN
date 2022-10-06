using System;

namespace HashCDN
{
    public static class ByteExtensions
    {
        public static string Base64UrlEncode(this byte[] data)
        {
            var str = Convert.ToBase64String(data);
            return str.Replace("=", "").Replace("/", "_").Replace("+", "-");
        }
    }
}
