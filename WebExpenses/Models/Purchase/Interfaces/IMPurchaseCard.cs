using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebExpenses.Models.Shop.Interfaces;
using WebExpenses.Models.Item.Interfaces;

namespace WebExpenses.Models.Purchase.Interfaces
{
    /// <summary>
    /// Интерфейс карточки покупки(для отображения на форме)
    /// </summary>
    public interface IMPurchaseCard
    {
        /// <summary>
        /// Уникальный код покупки
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Магазин
        /// </summary>
        IMShopCard  Shop { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        IMItemCard Item { get; set; }
        
        /// <summary>
        /// Цена
        /// </summary>
        float Price { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        float Count { get; set; }
        /// <summary>
        /// Дата покупки
        /// </summary>
        DateTime Date { get; set; }
    }
}
