using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebExpenses.Models.Item.Interfaces;
using WebExpenses.Models.Purchase.Interfaces;
using WebExpenses.Models.Shop.Interfaces;

namespace WebExpenses.Models.Purchase
{
    /// <summary>
    /// Класс "Карточка покупки"(для отображения в виде)
    /// </summary>
    public class MPurchaseCard : IMPurchaseCard
    {
        /// <summary>
        /// Уникальный код покупки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public IMItemCard Item { get; set; }

        /// <summary>
        /// Магазин
        /// </summary>
        public IMShopCard Shop { get; set; }

        /// <summary>
        /// Цена товара
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// Кол-во купленного товара
        /// </summary>
        public float Count { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
    }
}