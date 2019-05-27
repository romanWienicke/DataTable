using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Connexion.DataExtensions
{
    public class CachingDbContext : DbContext
    {
        public CachingDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbContext FromCache<T>(Func<DbSet<T>, DbSet<T>> getSet) where T: class
        {
            var x = getSet(a);

            return this;

        }
    }
}
