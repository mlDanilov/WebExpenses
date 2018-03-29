using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;
using DomainExpenses.Abstract.Repositories;
using DomainExpenses.Concrete;
using DomainExpenses.Concrete.Repositories;



namespace DomainExpenses.Concrete
{
    public class ExpensesRepository : IExpensesRepository
    {

        public ExpensesRepository()
        {
            //_purchaseModule = new PurchaseModule(_context.SelectWeeksOfCurrentPeriod);

            _itemRep = new ItemRepository(_context);
            _groupRep = new GroupRepository(_context);
            _shopRep = new ShopRepository(_context);
            _purchRep = new PurchaseRepository(_context);

        }

        /// <summary>
        /// Репозиторий для работы с товарами
        /// </summary>
        public IItemRepository ItemRep
        {
            get { return _itemRep; }
        }

        /// <summary>
        /// Репозиторий для работы с группами товаров
        /// </summary>
        public IGroupRepository GroupRep
        {
            get { return _groupRep; }
        }

        /// <summary>
        /// Репозиторий для работы с магазинами
        /// </summary>
        public IShopRepository ShopRep
        {
            get { return _shopRep; }
        }

        /// <summary>
        /// Репозиторий для работы с покупками
        /// </summary>
        public IPurchaseRepository PurchaseRep
        {
            get { return _purchRep; }
        }

        private ExpensesDBContext _context = new ExpensesDBContext();

        /// <summary>
        /// Репозиторий для работы с товарами
        /// </summary>
        private ItemRepository _itemRep = null;
        /// <summary>
        /// Репозиторий для работы с группами товаров
        /// </summary>
        private GroupRepository _groupRep = null;
        /// <summary>
        /// Репозиторий для работы с магазинами
        /// </summary>
        private ShopRepository _shopRep = null;
        /// <summary>
        /// Репозиторий для работы с магазинами
        /// </summary>
        private PurchaseRepository _purchRep = null;
    }
}
