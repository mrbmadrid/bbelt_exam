
using Microsoft.EntityFrameworkCore;

namespace bbelt_exam.Models
{
    public class DojoActivityContext : DbContext
    {
        public DojoActivityContext(DbContextOptions<DojoActivityContext> options) : base(options) { }
        public DbSet<User> Users {get; set;}
        public DbSet<Activity> Activities {get; set;}
        public DbSet<Participate> Participate {get; set;}
    }
}