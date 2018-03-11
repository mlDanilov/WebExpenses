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
       /* public MPeriodPurchaseSumByGroup(IPeriod timeSpan_, IGroup group_)
        {
            TimeSpan = new Period(timeSpan_);
            Group = group_;
        }*/
        /*public MPeriodPurchaseSumByGroup(Period timeSpan_, IGroup group_)
        {
            TimeSpan = timeSpan_;
            Group = new DomainExpenses.Concrete.Group(group_);
        }
        */
        /// <summary>
        /// Промежуток времени
        /// </summary>
        public Period TimeSpan { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public DomainExpenses.Concrete.Group Group { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public float Sum { get; set; }
    }
}