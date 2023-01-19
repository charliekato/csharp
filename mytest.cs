using System;
using System.Text;

namespace convtest
{
    class Program
    {
        public static int convert_ascii_2_time(byte[] vs)
        {
            int minutes;
            int seconds;
            int tenmilseconds;
            minutes = Convert.ToInt32(Encoding.ASCII.GetString(vs, 0, 2));
            seconds = Convert.ToInt32(Encoding.ASCII.GetString(vs, 3, 2));
            tenmilseconds = Convert.ToInt32(Encoding.ASCII.GetString(vs, 6, 2));
            return tenmilseconds + (100 * seconds) + (10000 * minutes);
        }
        static void Main()
        {
            int i;
            byte[] receivedstr = new byte[] { 0x30, 0x31, 0x20, 0x33, 0x35, 0x20, 0x39, 0x39 };
            Console.WriteLine("01:35.99 -> {0}", convert_ascii_2_time(receivedstr));
         
        }
    }   

}