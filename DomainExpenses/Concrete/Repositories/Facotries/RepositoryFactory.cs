using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;
using DomainExpenses.Abstract.Repositories;
using DomainExpenses.Concrete.Repositories;

namespace DomainExpenses.Concrete.Repositories.Facotries
{
    class RepositoryFactory : IRepositoryFactory
    {
        private RepositoryFactory()
        {

        }

        public static IRepositoryFactory Get()
        {
            if (_repFctry == null)
                _repFctry = new RepositoryFactory();
            return _repFctry;
        }
        private static IRepositoryFactory _repFctry = null;

        /// <summary>
        /// Репозиторий для работы с товарами
        /// </summary>
        public IItemRepository GetItemRep() {
            return new ItemRepository(_dbCntxt);
        }

        /// <summary>
        /// Репозиторий для работы с группами товаров
        /// </summary>
        public IGroupRepository GetGroupRep() {
            return new GroupRepository(_dbCntxt);
        }

        /// <summary>
        /// Репозиторий для работы с магазинами
        /// </summary>
        public IShopRepository GetShopRep() {
            return new ShopRepository(_dbCntxt);
        }

        /// <summary>
        /// Репозиторий для работы с покупками
        /// </summary>
        public IPurchaseRepository GetPurchaseRep() {
            return new PurchaseRepository(_dbCntxt);
        }

        private readonly ExpensesDBContext _dbCntxt = new ExpensesDBContext();
    }
}
