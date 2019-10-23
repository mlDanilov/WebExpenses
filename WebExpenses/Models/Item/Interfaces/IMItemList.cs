using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebExpenses.Models.Item.Interfaces;

namespace WebExpenses.Models.Item.Interfaces
{
    /// <summary>
    /// Список товаров для отображения
    /// </summary>
    public interface IMItemList
    {
        /// <summary>
        /// Текущая группа
        /// </summary>
        int? GroupId { get;  }
        /// <summary>
        /// Список товаров
        /// </summary>
        List<IMItemCard> ItemList { get; } 
    }
}

