using Ip_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ip_Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<IpRecord> IpRecords { get; set; }
    }
}