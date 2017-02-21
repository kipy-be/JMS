using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMS.Common
{
    public class Config
    {
        public string DbHost { get; set; } = "localhost";
        public ushort DbPort { get; set; } = 5656;
        public string DbName { get; set; }
        public string DbUser { get; set; }
        public string DbPassword { get; set; }

        public bool EnableLogs { get; set; } = false;
        public string LogsPath { get; set; } = "";

        public string BotNick { get; set; } = "JMS";
        public string BotAlternateNick { get; set; } = "~JMS";
        public string BotHostName { get; set; } = "jms.kipy.be";
        public string BotUserName { get; set; } = "JeanMichelScribe";
        public string BotPassword { get; set; }
    }
}
