using System;

namespace SixthTask
{
    class Program
    {
        static void Main(string[] args)
        {
            LogBuffer buf = new LogBuffer();
            for (var i = 0; i < 152; i++)
            {
                buf.Add(i.ToString());
            }
            buf.Close();
        }
    }
}