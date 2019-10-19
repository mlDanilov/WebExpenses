using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebExpenses.Models.Group.Interface;

namespace WebExpenses.Models.Purchase.Interfaces
{
    /// <summary>
    /// Покупки, объединенные одной группой товаров за период
    /// </summary>
    public interface IMGroupOfPurchases
    {
        /// <summary>
        /// Наименование группы
        /// </summary>
        IMGroupCard Group { get; }
        /// <summary>
        /// Покупки
        /// </summary>
        IMPurchaseCard[] Purchases { get; }
    }
}
