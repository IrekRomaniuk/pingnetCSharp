using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace PingNet2
{
    interface IListAllInterface
    {
        System.Collections.Generic.List<string> Ip(string ipbase, string second, string thrid, string fourth);
        System.Collections.Generic.List<string> Ip(string ipcidr);
    }
    public class PingSweep
    {
        private static int upCount = 0;

        static object lockObj = new object();
        static Stopwatch stopWatch = new Stopwatch();
        static TimeSpan ts;

        public static async void RunPingSweep_Async(string[] args)
        {
            upCount = 0;

            var tasks = new List<Task>();
            stopWatch.Start();
            IListAllInterface ipList = new ListAll();
            string ipBase = "10.";
            foreach (var ip in ipList.Ip(ipBase, args[0], args[1], args[2]))
            {
                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                //Console.WriteLine(ip.ToString());
                var task = PingAndUpdateAsync(p, ip.ToString(), Int32.Parse(args[3]));
                tasks.Add(task);
            }

            await Task.WhenAll(tasks).ContinueWith(t =>
            {
                stopWatch.Stop();
                ts = stopWatch.Elapsed;
                Console.WriteLine(upCount.ToString() + " devices found! Elapsed time: " + ts.ToString(), "Asynchronous");
                //MessageBox.Show(upCount.ToString() + " devices found! Elapsed time: " + ts.ToString(), "Asynchronous");
                Console.WriteLine("Completed");
                Console.ReadLine();
            });
        }

        private static async Task PingAndUpdateAsync(System.Net.NetworkInformation.Ping ping, string ip, int timeout)
        {
            var reply = await ping.SendPingAsync(ip, timeout);

            //Console.WriteLine($"ip: {ip} - reply: {reply.Status}");

            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                Console.WriteLine(ip);
                lock (lockObj)
                {
                    upCount++;
                }
            }
        }
    }
}
