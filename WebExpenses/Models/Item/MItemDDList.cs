using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Item
{
    public class MItemDDList
    {
        /// <summary>
        /// Текущий товар
        /// </summary>
        public int? ItemId { get; set; } = null;
        /// <summary>
        /// Список товаров
        /// </summary>
        public List<IItem> ItemList { get; set; } = new List<IItem>();
    }
}