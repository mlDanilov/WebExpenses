﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Concrete;

namespace DomainExpenses.Abstract.Repositories
{
    /// <summary>
    /// Репозиторий для работы с магазинами
    /// (Single Responsibility Principle)
    /// </summary>
    public interface IShopRepository : IEntityRepositiory<IShop, Shop>
    {

        /// <summary>
        /// Текущая группа товаров
        /// </summary>
        int? CurrentShopId { get; set; }
    }
}
