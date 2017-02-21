
using System;using KCL.Data;

namespace JMS.Data.Services
{
    public abstract class ServiceBase
    {
        protected DbContext _db;

        public ServiceBase(DbContext db)
        {
            _db = db;
        }
    }
}
