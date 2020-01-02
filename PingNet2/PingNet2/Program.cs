using System;

namespace PingNet2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Pinging 10.{0}-{1}.1-255.1-{2}", args[0], Int32.Parse(args[1]) - 1, Int32.Parse(args[2]) - 1);
            //Console.WriteLine(args);
            //Console.ReadLine();
            PingSweep.RunPingSweep_Async(args);
        }
    }
}
