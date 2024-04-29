using System;
using System.Diagnostics;
using System.Linq;

namespace PHPGuard
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: PHPGuard.exe Interval php-cgi.exe arguments \n e.g.  PHPGuard.exe 10 php-cgi.exe -b 127.0.0.1:9000 -c php.ini");
                return;
            }

            if (!int.TryParse(args[0], out int timerInterval))
            {
                Console.WriteLine("Invalid timer interval value.");
                return;
            }

            string programPath = args[1];
            if (!programPath.Contains("php-cgi.exe"))
            {
                Console.WriteLine("The target program name should be php-cgi.exe.");
                return;
            }
            
            string arguments = string.Join(" ", args.Skip(2));
            GuardClass.ProcessStartInfo.Arguments = arguments;
            GuardClass.ProcessStartInfo.FileName = programPath;
            GuardClass.ProcessStartInfo.CreateNoWindow = true;
            GuardClass.ProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            
            int timerIntervalInMilliseconds = timerInterval * 1000;

            GuardClass.TimerInterval = timerIntervalInMilliseconds; 
            
            GuardClass.StartGuard();

            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();
        }
    }
}