using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Shop.Interfaces
{
    /// <summary>
    /// Список магазинов + текущий выбранный 
    /// для отображения в виде
    /// </summary>
    public interface IMShopList
    {
        /// <summary>
        /// Текущий магазин
        /// </summary>
        IMShopCard Current { get; set; }

        /// <summary>
        /// Список магазинов
        /// </summary>
        List<IMShopCard> Shops { get;  }
    }
}