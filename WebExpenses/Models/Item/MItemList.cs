using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Item
{
    /// <summary>
    /// Список товаров + текущая группа
    /// </summary>
    public class MItemList
    {
        /// <summary>
        /// Текущая группа
        /// </summary>
        public int? GroupId { get; set; } = null;
        /// <summary>
        /// Список товаров
        /// </summary>
        public List<IItem> ItemList { get; set; } = new List<IItem>();
    }

    
}