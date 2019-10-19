using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
        //[DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        /// <summary>
        /// Костыль
        /// </summary>
        [Obsolete("Костыль для сериализации в JSON")]
        public string DateStr
        {
            get
            {
                return Date.ToString("yyyy-MM-dd");
            }
        }
    }
}