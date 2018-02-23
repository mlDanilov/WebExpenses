using DomainExpenses.Abstract;
using System.Data.Entity;

namespace DomainExpenses.Concrete
{
    internal class ExpensesDBContext : DbContext
    {

        public ExpensesDBContext()
        {
            
        }
        public DbSet<Item> Item {
            get; set;
        }

        public DbSet<Group> Group { get; set; }

        public DbSet<Group> GroupExt
        {
            get; set;
        }

        public DbSet<Shop> Shop { get; set; }

        public DbSet<Purchase> Purchase { get; set; }
    }
}
