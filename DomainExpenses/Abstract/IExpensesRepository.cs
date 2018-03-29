using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Concrete;
using DomainExpenses.Abstract.Repositories;
     

namespace DomainExpenses.Abstract
{
    public interface IExpensesRepository
    {
       

        /// <summary>
        /// Репозиторий для работы с товарами
        /// </summary>
        IItemRepository ItemRep { get; }

        /// <summary>
        /// Репозиторий для работы с группами товаров
        /// </summary>
        IGroupRepository GroupRep { get; }

        /// <summary>
        /// Репозиторий для работы с магазинами
        /// </summary>
        IShopRepository ShopRep { get; }
        /// <summary>
        /// Репозиторий для работы с покупками
        /// </summary>
        IPurchaseRepository PurchaseRep { get; }
    }
}
