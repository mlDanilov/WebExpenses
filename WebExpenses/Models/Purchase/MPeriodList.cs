using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Purchase
{
    /// <summary>
    /// Список периодов(MM yyyy), где есть покупки
    /// </summary>
    public class MPeriodList
    {
        /// <summary>
        /// Текущий выбраный период
        /// </summary>
        public IPeriod Current { get; set; }
        /// <summary>
        /// Список периодов в которых есть покупки
        /// </summary>
        public List<IPeriod> PeriodList { get; set; }
    }
}