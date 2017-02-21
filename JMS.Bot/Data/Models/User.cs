using System;
using KCL.Data.Entity;

namespace JMS.Data.Models
{
    [DbTable("users")]
    public class User : DbEntity<User>
    {
        [DbKey("user_id")]
        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        [DbField("user_nick")]
        public string NickName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        [DbField("user_alternate_nick")]
        public string AlternateNickName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
    }
}