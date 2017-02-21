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
                    s.server_password
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
                    s.server_password
                FROM servers s"
            );
        }
    }
}
