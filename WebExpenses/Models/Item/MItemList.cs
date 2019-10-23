using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;
using WebExpenses.Models.Item.Interfaces;

namespace WebExpenses.Models.Item
{
    /// <summary>
    /// Список товаров + текущая группа
    /// </summary>
    public class MItemList : IMItemList
    {
        /// <summary>
        /// Текущая группа
        /// </summary>
        public int? GroupId { get; set; } = null;
        /// <summary>
        /// Список товаров
        /// </summary>
        public List<IMItemCard> ItemList { get; set; } = new List<IMItemCard>();
    }

    
}