using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebExpenses.Models.Group.Interface;
using WebExpenses.Models.Purchase.Interfaces;

namespace WebExpenses.Models.Purchase
{
    /// <summary>
    /// Покупки, объединенные одной группой товаров за период
    /// </summary>
    public class MGroupOfPurchases : IMGroupOfPurchases
    {
        /// <summary>
        /// Наименование группы
        /// </summary>
        public IMGroupCard Group { get; set; }
        /// <summary>
        /// Покупки
        /// </summary>
        public IMPurchaseCard[] Purchases { get; set; }
    }
}