using Microsoft.EntityFrameworkCore;
using web.api.Models;

namespace web.api.Data

{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}


        public DbSet<Value> Values { get; set; }
        public DbSet<User> Users { get; set; }
    }
}