using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebExpenses.Models.Purchase.Interfaces;
using DomainExpenses.Abstract;
using DomainExpenses.Concrete;
namespace WebExpenses.Models.Purchase
{
    /// <summary>
    /// Аргументы, передаваемые на создании покупки
    /// </summary>
    public class PurchaseCreateParams : IPurchaseCreateParams
    {
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

        public IPurchase Convert()
        {
            return new DomainExpenses.Concrete.Purchase()
            {
                Count = Count,
                Price = Price,
                Date = Date,
                 Item_Id = Item_Id,
                  Shop_Id = Shop_Id
            };
        }
    }
}