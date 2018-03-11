using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;
using DomainExpenses.Concrete;

namespace WebExpenses.Models.Purchase
{
    public class MPurchaseFactory
    {
        private MPurchaseFactory()
        {

        }
       

        public MPeriodPurchaseSumByGroup CreatePeriodPurchaseSumByGroup(
            IPeriod period_, IGroup group_, float sum_)
        {
            return new MPeriodPurchaseSumByGroup()
            {
                Group = new DomainExpenses.Concrete.Group()
                {
                    Id = group_.Id,
                    Name = group_.Name,
                    IdParent = group_.IdParent
                },
                TimeSpan = new Period(period_),
                Sum = sum_
            };
        }


        public static MPurchaseFactory Get()
        {
            if (_instance == null)
                _instance = new MPurchaseFactory();
            return _instance;
        }

        private static MPurchaseFactory _instance = null;
    }
}