using System.Text;

namespace firmware
{
    internal class Util
    {
        public static string GetHexStringFromBytes(byte[] bytes)
        {
            StringBuilder sb = new();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
