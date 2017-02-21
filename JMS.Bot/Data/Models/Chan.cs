using System;
using KCL.Data.Entity;

namespace JMS.Data.Models
{
    [DbTable("chans")]
    public class Chan : DbEntity<Chan>
    {
        [DbKey("chan_id")]
        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [DbField("chan_name")]
        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        [DbParentRelation("servers", "server_id")]
        public Server Server
        {
            get { return GetValue<Server>(); }
            set { SetValue(value); }
        }
    }
}