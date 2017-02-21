using System;
using KCL.Data.Entity;
using System.Collections.Generic;

namespace JMS.Data.Models
{
    [DbTable("servers")]
    public class Server : DbEntity<Server>
    {
        [DbKey("server_id")]
        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [DbField("server_name")]
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        [DbField("server_host")]
        public string Host
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        [DbField("server_port")]
        public int Port
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [DbField("server_password")]
        public string Password
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public List<Chan> Chans { get; set; }
    }
}