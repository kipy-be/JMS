using System;
using System.Collections.Generic;
using System.Threading;
using JMS.Core;
using KCL.Data;

namespace JMS
{
    class Program
    {
        private static List<Bot> _bots;
        private static Application _app;
        private static DbContext _db;

        private static bool IsBotsRunning => _bots != null && _bots.TrueForAll(b => b.IsRunning);

        static void Main(string[] args)
        {
            Console.CancelKeyPress += ProcessExit;

            try
            {
                _app = Application.GetInstance();
                _db = _app.DbContext;

                _app.Log("Launching JMS");

                _bots = new List<Bot>();
                var servers = _db.Servers.GetAll();

                foreach (var server in servers)
                {
                    server.Chans = _db.Chans.GetListFromServerId(server.Id);
                    var bot = new Bot(server);

                    _bots.Add(bot);

                    _app.Log("Starting bot on {0} server...", server.Name);

                    bot.Start();
                }

                _app.Print("Running...");

                while(IsBotsRunning)
                {
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                _app.Log(ex.Message);
            }
            finally
            {
                Exit();
            }
        }

        private static void Exit()
        {
            if (IsBotsRunning)
            {
                _app.Log("Stopping bots...");
                _bots.ForEach(b => b.Stop());
            }

            _app.Log("Terminated");

#if DEBUG
            Console.ReadLine();
#endif
        }

        private static void ProcessExit(object sender, EventArgs e)
        {
            Exit();
        }
    }
}