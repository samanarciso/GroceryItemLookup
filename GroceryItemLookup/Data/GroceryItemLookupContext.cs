using Microsoft.EntityFrameworkCore;
using GroceryItemLookup.Models;

namespace GroceryItemLookup.Data
{
    public class GroceryItemLookupContext : DbContext
    {
        public GroceryItemLookupContext (DbContextOptions<GroceryItemLookupContext> options)
            : base(options)
        {
        }
        public DbSet<GroceryItemLookup.Models.Product> Product { get; set; } = default!;
        public DbSet<GroceryItemLookup.Models.Employee> Employee { get; set; } = default!;
        public DbSet<GroceryItemLookup.Models.Department> Department { get; set; } = default!;
        public DbSet<GroceryItemLookup.Models.Supervisor> Supervisor { get; set; } = default!;
    }
}
