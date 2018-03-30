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
    /// <summary>
    /// Мок-синглтон IPurchaseRepository
    /// </summary>
    public class PurchaseRepMock
    {
        private PurchaseRepMock()
        {
            setPurchaseBehavior();
        }

        private void setPurchaseBehavior()
        {
            var fBus = EntitiesFactory.Get();
            //= new Mock<IExpensesRepository>();

            //Список покупок
            _purchRepMock.Setup(m => m.Entities).Returns(_purchaseList.AsQueryable());
            //Список покупок
            _purchRepMock.Setup(m => m.SelectAllPeriods()).Returns(_periods.AsQueryable());
            //Список недель
            _purchRepMock.Setup(m => m.SelectWeeksByPeriod(It.IsAny<IPeriod>())
            ).Returns(
                (IPeriod period_) =>
                {
                    return _weeks.Where(
                        week =>
                        ((period_.MonthYear.Month == week.BDate.Month) && (period_.MonthYear.Year == week.BDate.Year)) ||
                        ((period_.MonthYear.Month == week.EDate.Month) && (period_.MonthYear.Year == week.EDate.Year))
                        ).AsQueryable();
                }
                );
            //Добавить новую покупку
            _purchRepMock.Setup<Purchase>(m => m.Create(
                It.IsAny<IPurchase>())).Returns(
                (IPurchase purchase_) =>
            {
                var purchase = fBus.CreatePurchaseC(
                    _purchaseList.Max(p => p.Id) + 1,
                    purchase_.Shop_Id, purchase_.Item_Id, purchase_.Price, purchase_.Count, purchase_.Date);

                _purchaseList.Add(purchase);
                _purchRepMock.Setup(m => m.Entities).Returns(_purchaseList.AsQueryable());
                return purchase;
            });
            //Редактировать существующую покупку
            _purchRepMock.Setup(m => m.Update(
                It.IsAny<IPurchase>())).Callback(
                 (IPurchase purchase_) =>
                 {
                     var purchase = _purchaseList.Where(p => p.Id == purchase_.Id).First();
                     if (purchase == null)
                         return;
                     purchase.Item_Id = purchase_.Item_Id;
                     purchase.Shop_Id = purchase_.Shop_Id;
                     purchase.Price = purchase_.Price;
                     purchase.Count = purchase_.Count;
                     purchase.Date = purchase_.Date;
                 });
            //Удалить покупку
            _purchRepMock.Setup(m => m.Delete(It.IsAny<IPurchase>()))
               .Callback(
               (IPurchase purchase_) =>
               {
                   _purchaseList.RemoveAll(p => p.Id == purchase_.Id);
                   _purchRepMock.Setup(m => m.Entities).Returns(_purchaseList.AsQueryable());
               });

            //Текущий магазин(get)
            _purchRepMock.SetupGet(m => m.CurrentPurchaseId).Returns(
                () => _currentPurchaseId);

            //Текущий магазин(set)
            _purchRepMock.SetupSet(m => m.CurrentPurchaseId = It.IsAny<int?>()).Callback(
                (int? purchaseId_) =>
                {
                    _currentPurchaseId = purchaseId_;
                    _purchRepMock.SetupGet(m => m.CurrentPurchaseId).Returns(
                      () => _currentPurchaseId);
                });

            //Текущий период(set)
            _purchRepMock.SetupSet(m => m.CurrentPeriod = It.IsAny<IPeriod>()).Callback(
                (IPeriod period_) =>
                {
                    _currentPeriod = period_;
                    _purchRepMock.SetupGet(m => m.CurrentPeriod).Returns(
                      () => _currentPeriod);
                }
                );

            //Текущий период(get)
            _purchRepMock.SetupGet(m => m.CurrentPeriod).Returns(
                      () => _currentPeriod);

            //Текущая неделя(set)
            _purchRepMock.SetupSet(m => m.CurrentWeek = It.IsAny<IWeek>()).Callback(
                (IWeek week_) =>
                {
                    _currentWeek = week_;
                    _purchRepMock.SetupGet(m => m.CurrentWeek).Returns(
                      () => _currentWeek);
                });

            //Текущая неделя(get)
            _purchRepMock.SetupGet(m => m.CurrentWeek).Returns(
                      () => _currentWeek);

            //Текущий день (get)
            _purchRepMock.SetupGet(m => m.CurrentDay).Returns(
                   () => _currentDay);

            //Текущий день (set)
            _purchRepMock.SetupSet(m => m.CurrentDay = It.IsAny<DateTime?>()).Callback(
                   (DateTime? day_) =>
                   {
                       _currentDay = day_;
                       _purchRepMock.SetupGet(m => m.CurrentDay).Returns(
                    () => _currentDay);
                   }
                   );

            //Получить все покупки за период
            _purchRepMock.Setup(m => m.SelectPurchasesByPeriod(It.IsAny<IPeriod>())).Returns(
                ((IPeriod period_) => { return SelectPurchaseByPeriod(period_); })
                );


            //Получить все покупки за неделю
            _purchRepMock.Setup(m => m.SelectPurchasesByWeek(It.IsAny<IWeek>())).Returns
                ((IWeek week_) => { return SelectPurchaseByWeek(week_); });
            //Получить все покупки за день
            _purchRepMock.Setup(m => m.SelectPurchaseByDate(It.IsAny<DateTime>())).Returns(
                (DateTime day_) => { return SelectPurchaseByDay(day_); });

            //Текущая группа покупок(get)
            _purchRepMock.SetupGet(m => m.CurrentPurchaseGId).Returns(
                   () => _currentPurchaseGId);

            //Текущая группа покупок(set)
            _purchRepMock.SetupSet(m => m.CurrentPurchaseGId = It.IsAny<int?>()).Callback(
                   (int? currentPurchaseGId_) =>
                   {
                       _currentPurchaseGId = currentPurchaseGId_;
                       _purchRepMock.SetupGet(m => m.CurrentPurchaseGId).Returns(
                    () => _currentPurchaseGId);
                   });

        }

        /// <summary>
        /// Получить все покупки за месяц
        /// </summary>
        /// <param name="week_"></param>
        /// <returns></returns>
        // private List<IPurchase> SelectPurchasesBy
        private IQueryable<Purchase> SelectPurchaseByPeriod(IPeriod period_)
        {
            var purchases =
                _purchaseList.Where(
                    p => (p.Date.Month == period_.MonthYear.Month) &&
                    (p.Date.Year == period_.MonthYear.Year)).AsQueryable();
            return purchases;
        }
        /// <summary>
        /// Получить все покупки за неделю
        /// </summary>
        /// <param name="week_"></param>
        /// <returns></returns>
        // private List<IPurchase> SelectPurchasesBy
        private IQueryable<Purchase> SelectPurchaseByWeek(IWeek week_)
        {
            var purchases =
                _purchaseList.Where(
                    p => (p.Date >= week_.BDate) &&
                    (p.Date <= week_.EDate)).AsQueryable();
            return purchases;
        }

        /// <summary>
        /// Получить все покупки за день
        /// </summary>
        /// <param name="day_"></param>
        /// <returns></returns>
        public IQueryable<Purchase> SelectPurchaseByDay(DateTime day_)
        {
            var purchases =
                _purchaseList.Where(p => (p.Date == day_)).AsQueryable();
            return purchases;
        }

        private int? _currentGroup = null;
        private int? _currentItem = null;

        private int? _currentPurchaseId = null;
        private int? _currentPurchaseGId = null;

        private IPeriod _currentPeriod = null;
        private IWeek _currentWeek = null;
        private DateTime? _currentDay = null;

        /// <summary>
        /// Список покупок
        /// </summary>
        private List<Purchase> _purchaseList = new List<Purchase>
        {
            //Декабрь 2018 1 неделя
            EntitiesFactory.Get().CreatePurchaseC(0, null, 2, 10, 5, new DateTime(2018, 1, 1)),
            EntitiesFactory.Get().CreatePurchaseC(1, 2, 7, 20, 7, new DateTime(2018, 1, 1)),
            EntitiesFactory.Get().CreatePurchaseC(2, 2, 5, 15, 1, new DateTime(2018, 1, 2)),
            EntitiesFactory.Get().CreatePurchaseC(3, null, 3, 54, 1, new DateTime(2018, 1, 2)),
            EntitiesFactory.Get().CreatePurchaseC(4, 3, 1, 20, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchaseC(5, 1, 8, 10, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchaseC(6, null, 9, 8,  6, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchaseC(7, 2, 5, 61, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchaseC(8, 1, 6, 48, 3, new DateTime(2018, 1, 5)),
            EntitiesFactory.Get().CreatePurchaseC(9, null, 7, 48, 3, new DateTime(2018, 1, 5)),


            //Ноябрь 2017 1 неделя
            EntitiesFactory.Get().CreatePurchaseC(10, 4, 7, 33, 5, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchaseC(11, null, 8, 20, 7, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchaseC(12, 3, 8, 27, 1, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchaseC(13, null, 9, 73, 1, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchaseC(14, 2, 2, 23, 2, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchaseC(15, null, 1, 56, 2, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchaseC(16, 4, 3, 43,  6, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchaseC(17, 1, 4, 61, 2, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchaseC(18, null, 5, 21, 3, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchaseC(19, 3, 5, 55, 3, new DateTime(2018, 11, 4)),

            //Ноябрь 2017 2-3 недели
             EntitiesFactory.Get().CreatePurchaseC(20, null, 7, 45, 3, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchaseC(21, 1, 8, 43, 4, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchaseC(22, 3, 8, 28, 8, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchaseC(23, null, 9, 12, 9, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchaseC(24, 2, 2, 76, 4, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchaseC(25, 2, 1,20, 5, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchaseC(26, null, 3, 65,  7, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchaseC(27, 4, 4, 36, 3, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchaseC(28, 2, 5, 87, 3, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchaseC(29, null, 5, 29, 1, new DateTime(2018, 11, 19))
        };

        private List<Period> _periods = new List<Period>
        {
            EntitiesFactory.Get().CreatePeriodC(new DateTime(2017, 12, 1)),
            EntitiesFactory.Get().CreatePeriodC(new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePeriodC(new DateTime(2018, 01, 1))
        };

        private List<Week> _weeks = new List<Week>
        {
            //Ноябрь 2017
            EntitiesFactory.Get().CreateWeekC(new DateTime(2017, 10, 30), new DateTime(2017, 11, 5)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2017, 11, 6), new DateTime(2017, 11, 12)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2017, 11, 13), new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2017, 11, 20), new DateTime(2017, 11, 26)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2017, 11, 27), new DateTime(2017, 12, 3)),

            //Декабрь 2017
            EntitiesFactory.Get().CreateWeekC(new DateTime(2017, 12, 4), new DateTime(2017, 12, 10)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2017, 12, 11), new DateTime(2017, 12, 17)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2017, 12, 18), new DateTime(2017, 12, 24)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2017, 12, 25), new DateTime(2017, 12, 31)),

            //Январь 2018
            EntitiesFactory.Get().CreateWeekC(new DateTime(2018, 1, 1), new DateTime(2018, 1, 7)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2018, 1, 8), new DateTime(2018, 1, 14)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2018, 1, 15), new DateTime(2018, 1, 21)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2018, 1, 22), new DateTime(2018, 1, 28)),
            EntitiesFactory.Get().CreateWeekC(new DateTime(2018, 1, 29), new DateTime(2018, 2, 4))

        };

        public Mock<IPurchaseRepository> Mock
        {
            get
            {
                return _purchRepMock;
            }
        }
        private Mock<IPurchaseRepository> _purchRepMock = new Mock<IPurchaseRepository>();

        public static PurchaseRepMock Get()
        {
            if (_instance == null)
                _instance = new PurchaseRepMock();
            return _instance;
        }
        private static PurchaseRepMock _instance = new PurchaseRepMock();
    }
}
