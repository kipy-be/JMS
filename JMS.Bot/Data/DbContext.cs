using System;
using KCL.Data.Pgsql;
using JMS.Data.Services;

namespace KCL.Data
{
    public class DbContext : DbContextPgsql
    {
        public ChansService   Chans   { get; private set; }
        public ServersService Servers { get; private set; }
        public UsersService   Users   { get; private set; }

        public DbContext(string host, ushort port, string dbName, string user, string password)
            : base(host, port, dbName, user, password)
        {
            Chans   = new ChansService(this);
            Servers = new ServersService(this);
            Users   = new UsersService(this);
        }
    }
}
