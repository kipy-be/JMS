using System;
using KCL.Data;
using System.Reflection;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using JMS.Common;

namespace JMS
{
    public class Application
    {
        public static readonly string Name = Assembly.GetEntryAssembly().GetName().Name;
        public static readonly string Version = Assembly.GetEntryAssembly().GetName().Version.ToString();

        private static readonly string CONF_FILENAME = "jms.conf";
        private static readonly string CONF_PATH_WIN = Path.Combine(Environment.GetEnvironmentVariable("ProgramData"), Name);
        private static readonly string CONF_PATH_UNIX = "/etc/jms/";

        private static Application _instance = null;

        private LogsManager _logsManager;

        public DbContext DbContext { get; private set; }
        public Config Config { get; private set; }

        private Application()
        {
            try
            {
                LoadConfig(GetConfigFile(CONF_FILENAME));

                if (Config.EnableLogs && !string.IsNullOrEmpty(Config.LogsPath) && Directory.Exists(Config.LogsPath))
                {
                    _logsManager = new LogsManager(Config.LogsPath, "jms.log");
                }

                DbContext = new DbContext(Config.DbHost, Config.DbPort, Config.DbName, Config.DbUser, Config.DbPassword);
                DbContext.Connect();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static Application GetInstance()
        {
            if (_instance == null)
                _instance = new Application();
            return _instance;
        }

        public static string AppDataPath()
        {
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
#if DEBUG
                Directory.CreateDirectory(CONF_PATH_WIN);
#endif
                return CONF_PATH_WIN;
            }
            else
                return CONF_PATH_UNIX;
        }

        public static string GetConfigFile(string fileName)
        {
            return Path.Combine(AppDataPath(), fileName);
        }

        public void LoadConfig(string fileUrl)
        {
            if (!File.Exists(fileUrl))
                throw new JMSException("Error : no config file found ({0})", fileUrl);
            else
            {
                try
                {
                    JObject jo;
                    using (StreamReader reader = new StreamReader(new FileStream(fileUrl, FileMode.Open, FileAccess.Read)))
                    {
                        string json = reader.ReadToEnd();
                        jo = JObject.Parse(json);
                        Config = jo.ToObject<Config>();

                        if (string.IsNullOrWhiteSpace(Config.DbHost)
                            || Config.DbPort == 0
                            || string.IsNullOrWhiteSpace(Config.DbName)
                            || string.IsNullOrWhiteSpace(Config.DbUser)
                            || string.IsNullOrWhiteSpace(Config.DbPassword))
                            throw new Exception();
                    }
                }
                catch(Exception ex)
                {
                    throw new JMSException("Error : invalid config file");
                }
            }
        }

        public void Log(object obj)
        {
            Debug(obj);
            _logsManager?.Log(obj);
        }

        public void Log(string message, params object[] args)
        {
            Debug(message, args);
            _logsManager?.Log(message, args);
        }

        public void Print(object obj)
        {
            Console.WriteLine(obj);
        }

        public void Print(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void Debug(object obj)
        {
#if DEBUG
            Console.WriteLine(obj);
#endif
        }

        public void Debug(string message, params object[] args)
        {
#if DEBUG
            Console.WriteLine(message, args);
#endif
        }
    }
}
