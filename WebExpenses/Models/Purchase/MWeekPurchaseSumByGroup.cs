using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Purchase
{
    /// <summary>
    /// Сумма расходов за неделю по группе
    /// </summary>
    public class MWeekPurchaseSumByGroup
    {
        public MWeekPurchaseSumByGroup(IWeek week_, IGroup group_)
        {
            Week = week_;
            Group = group_;
        }
        /// <summary>
        /// Неделя
        /// </summary>
        public IWeek Week { get; private set; }
        /// <summary>
        /// Группа
        /// </summary>
        public IGroup Group { get; private set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public float Sum { get; set; }

    }
}