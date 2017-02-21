using System;
using System.Collections.Generic;
using KCL.Data;
using JMS.Data.Models;

namespace JMS.Data.Services
{
    public class UsersService : ServiceBase
    {
        public UsersService(DbContext db)
            : base(db)
        {}

        public User GetFromId(int id)
        {
            return _db.ParseOne<User>
            (
                @"SELECT 
                    u.user_id,
                    u.user_nick,
                    u.user_alternate_nick
                FROM users u
                WHERE u.user_id = :p1",

                id
            );
        }
    }
}
