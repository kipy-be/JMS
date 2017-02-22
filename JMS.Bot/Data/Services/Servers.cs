using System;
using System.Collections.Generic;
using KCL.Data;
using JMS.Data.Models;

namespace JMS.Data.Services
{
    public class ServersService : ServiceBase
    {
        public ServersService(DbContext db)
            : base(db)
        {}

        public Server GetFromId(int id)
        {
            return _db.ParseOne<Server>
            (
                @"SELECT 
                    s.server_id,
                    s.server_name,
                    s.server_host,
                    s.server_port,
                    s.server_password,
                    s.server_bot_nick,
                    s.server_bot_alternate_nick,
                    s.server_bot_real_name,
                    s.server_bot_password
                FROM servers s
                WHERE s.server_id = :p1",

                id
            );
        }

        public List<Server> GetAll(bool withChans = false)
        {
            return _db.ParseMany<Server>
            (
                @"SELECT 
                    s.server_id,
                    s.server_name,
                    s.server_host,
                    s.server_port,
                    s.server_password,
                    s.server_bot_nick,
                    s.server_bot_alternate_nick,
                    s.server_bot_real_name,
                    s.server_bot_password
                FROM servers s"
            );
        }
    }
}
