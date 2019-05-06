using System.Linq;
using System.Net;

namespace Common.Converts
{
    /// <summary>
    /// IP 地址扩展
    /// </summary>
    public static class IPAddressExtend
    {
        public static int ToInt32Big(this IPAddress ip)
        {
            var bytes = ip.GetAddressBytes();
            return ByteConverter.ToInt32(bytes, 0, Endians.Big);
        }

        public static IPAddress Add(this IPAddress ip, int value)
        {
            var bytes = ByteConverter.ToBytes(ip.ToInt32Big() + value, Endians.Big);
            return new IPAddress(bytes);
        }

        public static bool IsValid(this IPAddress ip)
        {
            var last = ip.GetAddressBytes().LastOrDefault();
            return last != byte.MaxValue && last != byte.MinValue;
        }

        public static IPAddress GetNextValid(this IPAddress ip)
        {
            while (true)
            {
                ip = ip.Add(1);
                if (ip.IsValid() == true)
                {
                    return ip;
                }
            }
        }
        public static IPAddress ToIPAddress(this string ip)
        {
            if (!string.IsNullOrEmpty(ip))
            {
                return IPAddress.Parse(ip);
            }
            return null;
        }
    }
}
