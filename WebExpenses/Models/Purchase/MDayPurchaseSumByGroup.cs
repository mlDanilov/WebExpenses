using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Purchase
{
    public class MDayPurchaseSumByGroup
    {
        public MDayPurchaseSumByGroup()
        {

        }
        public MDayPurchaseSumByGroup(DateTime day_, IGroup group_)
        {
            Day = day_;
            Group = group_;
        }
        /// <summary>
        /// Неделя
        /// </summary>
        public DateTime Day { get; set; }
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