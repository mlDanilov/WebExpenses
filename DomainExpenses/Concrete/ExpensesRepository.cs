using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;
using DomainExpenses.Abstract.Repositories;
using DomainExpenses.Concrete;
using DomainExpenses.Concrete.Repositories;
using DomainExpenses.Concrete.Repositories.Facotries;



namespace DomainExpenses.Concrete
{
    /// <summary>
    /// Репозиторий для работы с данными
    /// </summary>
    public class ExpensesRepository : IExpensesRepository
    {

        public ExpensesRepository()
        {

            ItemRep = _repFctry.GetItemRep();
            GroupRep = _repFctry.GetGroupRep();
            ShopRep = _repFctry.GetShopRep();
            PurchaseRep = _repFctry.GetPurchaseRep();
        }
        /// <summary>
        /// Фабрика, возвращающая репозитории
        /// </summary>
        private readonly IRepositoryFactory _repFctry = RepositoryFactory.Get();

        /// <summary>
        /// Репозиторий для работы с товарами
        /// </summary>
        public IItemRepository ItemRep { get; private set;}

        /// <summary>
        /// Репозиторий для работы с группами товаров
        /// </summary>
        public IGroupRepository GroupRep { get; private set; }

        /// <summary>
        /// Репозиторий для работы с магазинами
        /// </summary>
        public IShopRepository ShopRep { get; private set; }

        /// <summary>
        /// Репозиторий для работы с покупками
        /// </summary>
        public IPurchaseRepository PurchaseRep { get; private set; }
    }
}
