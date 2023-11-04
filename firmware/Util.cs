using System.Text;

namespace firmware
{
    internal class Util
    {
        public static void PrintBytes(byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                Console.Write(b.ToString("x2") + " ");
            }
            Console.WriteLine();
        }

        public static string GetHexStringFromBytes(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
