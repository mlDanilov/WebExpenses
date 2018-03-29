using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Moq;
using DomainExpenses.Abstract;
using DomainExpenses.Abstract.Repositories;
using DomainExpenses.Concrete;

namespace DomainExpenses.Moq
{
    internal class PurchRepMock
    {






        /// <summary>
        /// Список покупок
        /// </summary>
        private List<IPurchase> _purchaseList = new List<IPurchase>
        {
            //Декабрь 2018 1 неделя
            EntitiesFactory.Get().CreatePurchase(0, null, 2, 10, 5, new DateTime(2018, 1, 1)),
            EntitiesFactory.Get().CreatePurchase(1, 2, 7, 20, 7, new DateTime(2018, 1, 1)),
            EntitiesFactory.Get().CreatePurchase(2, 2, 5, 15, 1, new DateTime(2018, 1, 2)),
            EntitiesFactory.Get().CreatePurchase(3, null, 3, 54, 1, new DateTime(2018, 1, 2)),
            EntitiesFactory.Get().CreatePurchase(4, 3, 1, 20, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(5, 1, 8, 10, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(6, null, 9, 8,  6, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(7, 2, 5, 61, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(8, 1, 6, 48, 3, new DateTime(2018, 1, 5)),
            EntitiesFactory.Get().CreatePurchase(9, null, 7, 48, 3, new DateTime(2018, 1, 5)),


            //Ноябрь 2017 1 неделя
            EntitiesFactory.Get().CreatePurchase(10, 4, 7, 33, 5, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchase(11, null, 8, 20, 7, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchase(12, 3, 8, 27, 1, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchase(13, null, 9, 73, 1, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchase(14, 2, 2, 23, 2, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchase(15, null, 1, 56, 2, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchase(16, 4, 3, 43,  6, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchase(17, 1, 4, 61, 2, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchase(18, null, 5, 21, 3, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchase(19, 3, 5, 55, 3, new DateTime(2018, 11, 4)),

            //Ноябрь 2017 2-3 недели
             EntitiesFactory.Get().CreatePurchase(20, null, 7, 45, 3, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchase(21, 1, 8, 43, 4, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchase(22, 3, 8, 28, 8, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchase(23, null, 9, 12, 9, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchase(24, 2, 2, 76, 4, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchase(25, 2, 1,20, 5, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchase(26, null, 3, 65,  7, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchase(27, 4, 4, 36, 3, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchase(28, 2, 5, 87, 3, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchase(29, null, 5, 29, 1, new DateTime(2018, 11, 19))
        };

        private List<IPeriod> _periods = new List<IPeriod>
        {
            EntitiesFactory.Get().CreatePeriod(new DateTime(2017, 12, 1)),
            EntitiesFactory.Get().CreatePeriod(new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePeriod(new DateTime(2018, 01, 1))
        };

        private List<IWeek> _weeks = new List<IWeek>
        {
            //Ноябрь 2017
            EntitiesFactory.Get().CreateWeek(new DateTime(2017, 10, 30), new DateTime(2017, 11, 5)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2017, 11, 6), new DateTime(2017, 11, 12)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2017, 11, 13), new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2017, 11, 20), new DateTime(2017, 11, 26)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2017, 11, 27), new DateTime(2017, 12, 3)),

            //Декабрь 2017
            EntitiesFactory.Get().CreateWeek(new DateTime(2017, 12, 4), new DateTime(2017, 12, 10)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2017, 12, 11), new DateTime(2017, 12, 17)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2017, 12, 18), new DateTime(2017, 12, 24)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2017, 12, 25), new DateTime(2017, 12, 31)),

            //Январь 2018
            EntitiesFactory.Get().CreateWeek(new DateTime(2018, 1, 1), new DateTime(2018, 1, 7)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2018, 1, 8), new DateTime(2018, 1, 14)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2018, 1, 15), new DateTime(2018, 1, 21)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2018, 1, 22), new DateTime(2018, 1, 28)),
            EntitiesFactory.Get().CreateWeek(new DateTime(2018, 1, 29), new DateTime(2018, 2, 4))

        };
        public static PurchRepMock Get()
        {
            if (_instance == null)
                _instance = new PurchRepMock();
            return _instance;
        }
        private static PurchRepMock _instance = new PurchRepMock();
    }
}
