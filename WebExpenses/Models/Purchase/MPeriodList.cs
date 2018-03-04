using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Purchase
{
    public class MPeriodList
    {
        public IPeriod Current { get; set; }

        public List<IPeriod> PeriodList { get; set; }
    }
}