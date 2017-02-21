using System;
using System.Collections.Generic;
using KCL.Data;
using JMS.Data.Models;

namespace JMS.Data.Services
{
    public class ChansService : ServiceBase
    {
        public ChansService(DbContext db)
            : base(db)
        {}

        public Chan GetFromId(int id)
        {
            return _db.ParseOne<Chan>
            (
                @"SELECT 
                    c.chan_id,
                    c.chan_name,
                    s.server_id,
                    s.server_name,
                    s.server_host,
                    s.server_port,
                    s.server_password
                FROM chans c
                INNER JOIN servers s ON c.server_id = s.server_id
                WHERE c.chan_id = :p1",

                id
            );
        }

        public List<Chan> GetAll()
        {
            return _db.ParseMany<Chan>
            (
                @"SELECT 
                    c.chan_id,
                    c.chan_name,
                    s.server_id,
                    s.server_name,
                    s.server_host,
                    s.server_port,
                    s.server_password
                FROM chans c
                INNER JOIN servers s ON c.server_id = s.server_id"
            );
        }

        public List<Chan> GetListFromServerId(int serverId)
        {
            return _db.ParseMany<Chan>
            (
                @"SELECT 
                    c.chan_id,
                    c.chan_name
                FROM chans c
                WHERE c.server_id = :p1",
                serverId
            );
        }
    }
}
