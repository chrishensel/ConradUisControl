using System;
using System.Threading;

namespace ConradUisControl
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintHelp();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
                else
                {
                    HandleKey(key.Key);
                }
                Thread.Sleep(1);
            }
        }

        private static void HandleKey(ConsoleKey consoleKey)
        {
            switch (consoleKey)
            {
                case ConsoleKey.C:
                    Console.Clear();
                    break;

                case ConsoleKey.H:
                    PrintHelp();
                    break;

                // Get outlet status
                case ConsoleKey.NumPad0:
                    HandleGetOutletStatus();
                    break;

                // Outlet #1
                case ConsoleKey.NumPad1:
                    SetOutletStatus(1, false);
                    break;
                case ConsoleKey.NumPad2:
                    SetOutletStatus(1, true);
                    break;

                // Outlet #2
                case ConsoleKey.NumPad4:
                    SetOutletStatus(2, false);
                    break;
                case ConsoleKey.NumPad5:
                    SetOutletStatus(2, true);
                    break;
            }

            Console.WriteLine();
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Configured device URI: {0}", DeviceCommunication.GetConfiguredDeviceHostUri());
            Console.WriteLine("");
            Console.WriteLine(Properties.Resources.HelpText);
            Console.WriteLine("");
        }

        private static void HandleGetOutletStatus()
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Reading outlet status...");
            Console.WriteLine("");

            bool[] status = DeviceCommunication.GetOutletStatus();
            if (status == null)
            {
                Console.WriteLine("Could not retrieve outlet status! Please try again in a few seconds!");
            }
            else
            {
                Console.WriteLine("Status (#1 / #2):");
                Console.Write("{0} / {1}", status[0], status[1]);
            }
        }

        private static void SetOutletStatus(int outletIndex, bool enabled)
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Setting outlet {0} to {1}...", outletIndex, enabled);
            Console.WriteLine("");

            DeviceCommunication.SetOutletStatus(outletIndex, enabled);

            Console.WriteLine("Done.");
            Console.WriteLine("");
        }
    }
}
