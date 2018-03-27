using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Concrete;
using DomainExpenses.Abstract;

namespace WebExpenses.Models.Purchase
{
    public class MPeriodPurchaseSumByGroup
    {
        public MPeriodPurchaseSumByGroup()
        {

        }

        /// <summary>
        /// Промежуток времени
        /// </summary>
        public Period TimeSpan { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public IGroup Group { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public float Sum { get; set; }
    }
}