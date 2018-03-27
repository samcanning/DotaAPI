using Microsoft.EntityFrameworkCore;

namespace DotaAPI.Models
{
    public class DotaContext : DbContext
    {
        public DotaContext(DbContextOptions<DotaContext> options) : base(options) { }
        public DbSet<Hero> Heroes {get;set;}
        public DbSet<Spell> Spells {get;set;}
        public DbSet<Admin> Admins {get;set;}
        public DbSet<New_Hero> New_Heroes {get;set;}
        public DbSet<User> Users {get;set;}
        public DbSet<Vote> Votes {get;set;}
    }
}