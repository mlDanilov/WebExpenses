using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;

namespace DomainExpenses.Concrete
{
    public class ExpensesRepository : IExpensesRepository
    {
        public IQueryable<IItem> Item
        {
            get {
                return _context.Item;
            }
        }
        public IQueryable<IShop> Shop
        {
            get
            {
                return _context.Shop;
            }
        }

        public IQueryable<IGroup> Group
        {
            get
            {
                return _context.Group;
            }
        }

        public IQueryable<IGroup> GroupExt
        {
            get
            {
                return _context;
            }
        }

        public IQueryable<IPurchase> Purchase
        {
            get
            {
                return _context.Purchase;
            }
        }

        private readonly ExpensesDBContext _context = new ExpensesDBContext();
    }
}
