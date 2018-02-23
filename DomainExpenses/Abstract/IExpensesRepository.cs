using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainExpenses.Abstract
{
    public interface IExpensesRepository
    {
        IQueryable<IItem> Item { get; }

        IQueryable<IGroup> Group { get; }

        IQueryable<IShop> Shop { get; }

        IQueryable<IPurchase> Purchase { get; }
    }
}
