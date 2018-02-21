using Microsoft.EntityFrameworkCore;

namespace DotaAPI.Models
{
    public class DotaContext : DbContext
    {
        public DotaContext(DbContextOptions<DotaContext> options) : base(options) { }
    }
}