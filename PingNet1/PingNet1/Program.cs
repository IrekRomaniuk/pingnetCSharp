﻿/* Source: https://stackoverflow.com/a/4042887
 * Original author: Tim Coker
 */

using System;
using System.Diagnostics;
using System.Threading;
using System.Net.NetworkInformation;

namespace PingNet1
{
    interface IListAllInterface
    {
        System.Collections.Generic.List<string> Ip(string ipbase, string second, string thrid, string fourth);
        System.Collections.Generic.List<string> Ip(string ipcidr);
    }
    class Program
    {
        static CountdownEvent countdown;
        static int upCount = 0;
        static int totalCount = 0;
        static object lockObj = new object();
        const bool resolveNames = true;

        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Need IP range as argument i.e. 192, 208, 2 and 2000 for 10.192-207.1-255.1 with 2s tiemout");
                return;
            }
            Console.WriteLine("Pinging 10.{0}-{1}.1-255.1-{2}", args[0], Int32.Parse(args[1]) - 1, Int32.Parse(args[2]) - 1);
            countdown = new CountdownEvent(1);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string ipBase = "10.";
            IListAllInterface ipList = new ListAll();
            foreach ( var ip in ipList.Ip(ipBase, args[0], args[1], args[2])) {
                totalCount++;
                //Console.WriteLine(ip);
                Ping p = new Ping();
                p.PingCompleted += new PingCompletedEventHandler(p_PingCompleted);
                countdown.AddCount();
                p.SendAsync(ip, Int32.Parse(args[3]), ip);
            }
                  
            countdown.Signal();
            countdown.Wait();
            sw.Stop();
            TimeSpan span = new TimeSpan(sw.ElapsedTicks);
            Console.WriteLine("Took {0} milliseconds. {1}/{2} hosts active.", sw.ElapsedMilliseconds, upCount, totalCount);
            Console.ReadLine();
        }

        static void p_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            string ip = (string)e.UserState;
            if (e.Reply != null && e.Reply.Status == IPStatus.Success)
            {
                Console.WriteLine("{0} is up: ({1} ms)", ip, e.Reply.RoundtripTime);
                lock (lockObj)
                {
                    upCount++;
                }
            }
            else if (e.Reply == null)
            {
                Console.WriteLine("Pinging {0} failed. (Null Reply object?)", ip);
            }
            countdown.Signal();
        }
    }
}