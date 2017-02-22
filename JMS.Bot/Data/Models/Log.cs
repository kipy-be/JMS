using System;
using KCL.Data.Entity;

namespace JMS.Data.Models
{
    [DbTable("logs")]
    public class Log : DbEntity<Log>
    {
        [DbKey("log_id")]
        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [DbField("log_from")]
        public string From
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        [DbField("log_content")]
        public string Content
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        [DbField("log_is_action")]
        public bool IsAction
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        [DbField("log_date")]
        public DateTime Date
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }
    }
}