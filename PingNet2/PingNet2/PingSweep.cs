using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace PingNet2
{
    public class PingSweep
    {
        private static string BaseIP = "10.4.4.";
        private static int StartIP = 1;
        private static int StopIP = 255;
        private static string ip;

        private static int timeout = 100;
        private static int nFound = 0;

        static object lockObj = new object();
        static Stopwatch stopWatch = new Stopwatch();
        static TimeSpan ts;

        public static async void RunPingSweep_Async()
        {
            nFound = 0;

            var tasks = new List<Task>();

            stopWatch.Start();

            for (int i = StartIP; i <= StopIP; i++)
            {
                ip = BaseIP + i.ToString();

                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                var task = PingAndUpdateAsync(p, ip);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks).ContinueWith(t =>
            {
                stopWatch.Stop();
                ts = stopWatch.Elapsed;
                Console.WriteLine(nFound.ToString() + " devices found! Elapsed time: " + ts.ToString(), "Asynchronous");
                //MessageBox.Show(nFound.ToString() + " devices found! Elapsed time: " + ts.ToString(), "Asynchronous");
            });
        }

        private static async Task PingAndUpdateAsync(System.Net.NetworkInformation.Ping ping, string ip)
        {
            var reply = await ping.SendPingAsync(ip, timeout);

            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                lock (lockObj)
                {
                    nFound++;
                }
            }
        }
    }
}
