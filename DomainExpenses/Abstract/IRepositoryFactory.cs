using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract.Repositories;

namespace DomainExpenses.Abstract
{
    /// <summary>
    /// Фабрика, возвращающая репозитории
    /// </summary>
    interface IRepositoryFactory
    {
        /// <summary>
        /// Репозиторий для работы с товарами
        /// </summary>
        IItemRepository GetItemRep();

        /// <summary>
        /// Репозиторий для работы с группами товаров
        /// </summary>
        IGroupRepository GetGroupRep();

        /// <summary>
        /// Репозиторий для работы с магазинами
        /// </summary>
        IShopRepository GetShopRep();

        /// <summary>
        /// Репозиторий для работы с покупками
        /// </summary>
        IPurchaseRepository GetPurchaseRep();
    }
}
