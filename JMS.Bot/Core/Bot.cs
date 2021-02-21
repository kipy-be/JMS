using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using JMS.Common;
using KCL.Data;
using System.Threading.Tasks;
using JMS.Data.Models;

namespace JMS.Core
{
    public class Bot
    {
        private Thread         _workingtThread, _inputThread;
        private TcpClient      _tcpClient;
        private NetworkStream  _stream;
        private StreamReader   _reader;
        private StreamWriter   _writer;
        private AutoResetEvent _getData, _gotData;
        private string         _data;

        private Server _server;

        private volatile bool _running;

        private Application _app;
        private DbContext   _db;

    // Constructors

        public Bot(Server server)
        {
            _server = server;

            _app = Application.GetInstance();
            _db  = _app.DbContext;
        }

    // Methods

        public void Start()
        {
            if (_running)
                return;

            _running = true;

            _tcpClient = new TcpClient();
            _tcpClient.ConnectAsync(_server.Host, _server.Port).ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    StartThread();
                }
                else
                    _running = false;
            });
        }

        private void StartThread()
        {
            _workingtThread = new Thread(new ThreadStart(Working));
            _workingtThread.IsBackground = true;
            _workingtThread.Start();

            _getData = new AutoResetEvent(false);
            _gotData = new AutoResetEvent(false);
            _inputThread = new Thread(new ThreadStart(Reading));

            _inputThread.IsBackground = true;
            _inputThread.Start();
        }

        public void Stop()
        {
            if (!_running)
                return;

            _workingtThread.Join();
            _inputThread.Join();

            _running = false;
        }

        private void Reading()
        {
            try
            {
                while (_running)
                {
                    _getData.WaitOne();
                    _data = _reader.ReadLine();
                    _gotData.Set();
                }
            }
            catch (Exception)
            {}
            finally
            {
                _app.Debug("Reading thread terminated");
            }
        }

        private string ReadLine(int timeout)
        {
            _getData.Set();
            bool success = _gotData.WaitOne(timeout);
            if (success)
                return _data;
            else
                return "NOP";
        }

        public bool IsRunning
        {
            get { return _running; }
        }

        private void Working()
        {
            try
            {
                _stream = _tcpClient.GetStream();
                _reader = new StreamReader(_stream);
                _writer = new StreamWriter(_stream);

                SendCommand("USER", _server.BotRealName, "jms.kipy.be", _server.BotNick, _server.BotNick);
                SendCommand("NICK", _server.BotNick);

                string line;
                string[] data;
                char[] sep = new char[] { ' ' };
                bool isIdentified = false;

                while (_running)
                {
                    line = ReadLine(2000);

                    if (line == "NOP")
                        continue;

                    _app.Debug(line);
                    data = line.Split(sep, 4);

                // Ping pong handling
                    if (data[0] == "PING")
                    {
                        SendCommand("PONG", data[1]);
                        continue;
                    }

                // Identification
                    if (!isIdentified)
                    {
                        if (line.Contains("This nickname is registered"))
                        {
                            SendCommand("PRIVMSG", "NICKSERV IDENTIFY", _server.BotPassword);
                            isIdentified = true;
                            continue;
                        }
                    }

                    // Chans joins

                    if (data.Length == 4 && data[2] == _server.BotNick)
                    {
                        // Interactions

                        if (data[1] == "PRIVMSG")
                        {
                            switch (data[3].ToLower())
                            {
                                case ":ping":
                                    SendCommand("PRIVMSG", GetNick(data[0]), "pong");
                                    continue;

                                case ":test":
                                    SendCommand("NS STATUS", GetNick(data[0]));
                                    continue;
                            }
                        }
                        else if (data[1] == "NOTICE")
                        {
                            switch (data[3].ToLower())
                            {
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new JMSException("Error in bot : {0}", ex.Message);
            }
            finally
            {
                _reader.Dispose();
                _writer.Dispose();
                _stream.Dispose();

                _running = false;
                _app.Debug("Working thread terminated");
            }
        }

        private void SendCommand(string command, params string[] args)
        {
            string cmdLine = String.Format("{0} {1}", command, String.Join(" ", args));
            _writer.WriteLine(cmdLine);
            _writer.Flush();
            _app.Debug(cmdLine);
        }

        private string GetNick(string identifier)
        {
            return identifier.Substring(1, identifier.IndexOf("!") - 1);
        }
    }
}
