using System;
using System.Diagnostics;
using System.Threading;

namespace PHPGuard
{
    public class GuardClass
    {
        public static int TimerInterval { get; set; }
        public static ProcessStartInfo ProcessStartInfo { get; set; } = new ProcessStartInfo();
        
        private static Timer Timer { get; set; }

        public static bool StartGuard()
        {
            try
            {
                Timer = new Timer(Guard, null, 0, TimerInterval);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static bool StopGuard()
        {
            try
            {
                Timer?.Change(Timeout.Infinite, Timeout.Infinite);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void Guard(object state)
        {
            Process[] processes = Process.GetProcessesByName("php-cgi");
            if (processes.Length == 0)
            {
                Console.WriteLine("Process not exist. Restarting now.");
                Process.Start(ProcessStartInfo);
            }
            else
            {
                foreach (Process proc in processes)
                {
                    if (!proc.Responding)
                    {
                        proc.Kill();
                        Console.WriteLine("Process not responding. Restarting now.");
                        Process.Start(ProcessStartInfo);
                    }
                }
                Console.WriteLine("Process running stable.");
            }
        }
    }
}