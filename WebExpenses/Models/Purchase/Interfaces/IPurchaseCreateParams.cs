using DomainExpenses.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebExpenses.Models.Purchase.Interfaces
{
    /// <summary>
    /// Аргументы, передаваемые на создание покупки
    /// </summary>
    public interface IPurchaseCreateParams 
    {
        /// <summary>
        /// Код магазина
        /// </summary>
        int? Shop_Id { get; set; }
        /// <summary>
        /// Код товара
        /// </summary>
        int Item_Id { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        float Price { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        float Count { get; set; }
        /// <summary>
        /// Время покупки
        /// </summary>
        DateTime Date { get; set; }

        IPurchase Convert();
    }
}
