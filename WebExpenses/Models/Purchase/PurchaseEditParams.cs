using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebExpenses.Models.Purchase.Interfaces;
using DomainExpenses.Abstract;
namespace WebExpenses.Models.Purchase
{
    /// <summary>
    /// Аргументы, передаваемые на изменение покупки
    /// </summary>
    public class PurchaseEditParams : IPurchaseEditParams
    {
        /// <summary>
        /// Уникальный код покупки
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Уникальный код товара
        /// </summary>
        public int Item_Id{ get; set; }
        /// <summary>
        /// Уникальный код магазина
        /// </summary>
        public int? Shop_Id { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public float Count { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
    }
}