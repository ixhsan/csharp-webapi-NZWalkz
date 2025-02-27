using Microsoft.EntityFrameworkCore;
using NZWalkz.API.Models.Domain;

namespace NZWalkz.API.Data
{
    public class NZWalkzDbContext: DbContext
    {
        public NZWalkzDbContext(DbContextOptions dbContextOptions): base(dbContextOptions)
        {
            
        }

        // this will create tables in Db
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }

    }
}
