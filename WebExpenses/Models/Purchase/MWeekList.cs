using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Purchase
{
    public class MWeekList
    {
        public IWeek Current { get; set; }

        public List<IWeek> WeekList { get; set; }
    }
}