using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;
using WebExpenses.Models.Shop.Interfaces;

namespace WebExpenses.Models.Shop
{
    /// <summary>
    /// Список магазинов + текущий выбранный 
    /// для отображения в виде
    /// </summary>
    public class MShopList : IMShopList
    {
        /// <summary>
        /// Текущий выбранный магазин
        /// </summary>
        public IMShopCard Current { get; set; }

        /// <summary>
        /// Список магазинов
        /// </summary>
        public List<IMShopCard> Shops { get; private set; } = new List<IMShopCard>();
    }
}