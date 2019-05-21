using Microsoft.EntityFrameworkCore;
using web.api.Properties.Models;

namespace web.api.Data

{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}


        public DbSet<Value> Values { get; set; }
    }
}