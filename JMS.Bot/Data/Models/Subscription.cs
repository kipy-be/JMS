using System;
using KCL.Data.Entity;

namespace JMS.Data.Models
{
    [DbTable("subscriptions")]
    public class Subscription : DbEntity<Subscription>
    {
        [DbKey("subscription_registered_date")]
        public DateTime RegisteredDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        [DbField("subscription_last_seen_date")]
        public DateTime LastSeenDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        [DbField("subscription_last_activity_date")]
        public DateTime LastActivityDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        [DbField("subscription_is_protected")]
        public bool IsProtected
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        [DbField("subscription_can_add")]
        public bool CanAdd
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        [DbField("subscription_can_delete")]
        public bool CanDelete
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        [DbParentRelation("users", "user_id")]
        public User User
        {
            get { return GetValue<User>(); }
            set { SetValue(value); }
        }

        [DbParentRelation("chans", "chan_id")]
        public Chan Chan
        {
            get { return GetValue<Chan>(); }
            set { SetValue(value); }
        }

        [DbParentRelation("users", "user_parent_id", "user_id", "parent")]
        public User ParentUser
        {
            get { return GetValue<User>(); }
            set { SetValue(value); }
        }
    }
}