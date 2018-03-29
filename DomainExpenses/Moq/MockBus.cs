using System;
using System.Collections;
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
    public class MockBus
    {
        private MockBus()
        {
            defineMockDbContext();
        }
        /// <summary>
        /// Создать Moq с товарами/группами/магазинами
        /// </summary>
        private void defineMockDbContext()
        {
            var fBus = EntitiesFactory.Get();

            MockExpensesRep = new Mock<IExpensesRepository>();
            //репозиторий товаров
            MockExpensesRep.Setup(m => m.ItemRep).Returns(ItemRepMock.Get().ItRepMock.Object);
            //репозиторий групп товаров
            MockExpensesRep.Setup(m => m.GroupRep).Returns(GroupRepMock.Get().GpRepMock.Object);
            //репозиторий магазинов
            MockExpensesRep.Setup(m => m.ShopRep).Returns(ShopRepMock.Get().ShRepMock.Object);


            
            //Список покупок
            MockExpensesRep.Setup(m => m.Purchase).Returns(_purchaseList.AsQueryable());
            //Список покупок
            MockExpensesRep.Setup(m => m.SelectAllPeriods()).Returns(_periods.AsQueryable());
            //Список недель
            MockExpensesRep.Setup(m => m.SelectWeeksOfCurrentPeriod()).Returns(
                _weeks.Where(
                    week => 
                    ((_currentPeriod.MonthYear.Month == week.BDate.Month) && (_currentPeriod.MonthYear.Year == week.BDate.Year)) ||
                    ((_currentPeriod.MonthYear.Month == week.EDate.Month) && (_currentPeriod.MonthYear.Year == week.EDate.Year))
                    )
                .AsQueryable()
                );
            //Установить для мока поведение для работы с покупками
            setPurchaseBehavior();
        }
        private void setPurchaseBehavior()
        {
            var fBus = EntitiesFactory.Get();

            //Добавить новый магазин
            MockExpensesRep.Setup<IPurchase>(m => m.AddNewPurchase(
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<DateTime>()))
                .Returns(
            (int? shopId_, int itemId_, float price_, float count_, DateTime date_) =>
            {
                var purchase = fBus.CreatePurchase(_purchaseList.Max(p => p.Id) + 1,
                    shopId_, itemId_, price_, count_, date_);
                _purchaseList.Add(purchase);
                MockExpensesRep.Setup(m => m.Purchase).Returns(_purchaseList.AsQueryable());
                return purchase;
            });
            //Редактировать существующую покупку
            MockExpensesRep.Setup(m => m.EditPurchase(
                It.IsAny<int>(),
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<DateTime>())).Callback(
                 (int id_, int? shopId_, int itemId_, float price_, float count_, DateTime date_) =>
                {
                    var purchase = _purchaseList.Where(p => p.Id == id_).First();
                    if (purchase == null)
                        return;
                    purchase.Item_Id = itemId_;
                    purchase.Shop_Id = shopId_;
                    purchase.Price = price_;
                    purchase.Count = count_;
                    purchase.Date = date_;
                });
            //Удалить покупку
            MockExpensesRep.Setup(m => m.DeletePurchase(It.IsAny<int>()))
               .Callback(
               (int id_) =>
               {
                   _purchaseList.RemoveAll(p => p.Id == id_);
                   MockExpensesRep.Setup(m => m.Purchase).Returns(_purchaseList.AsQueryable());
               });

            //Текущий магазин(get)
            MockExpensesRep.SetupGet(m => m.CurrentPurchaseId).Returns(
                () => _currentPurchaseId);
            //Текущий магазин(set)
            MockExpensesRep.SetupSet(m => m.CurrentPurchaseId = It.IsAny<int?>()).Callback(
                (int? purchaseId_) =>
                {
                    _currentPurchaseId = purchaseId_;
                    MockExpensesRep.SetupGet(m => m.CurrentPurchaseId).Returns(
                      () => _currentPurchaseId);
                });

            //Текущий период(set)
            MockExpensesRep.SetupSet(m => m.CurrentPeriod = It.IsAny<IPeriod>()).Callback(
                (IPeriod period_) =>
                {
                    _currentPeriod = period_;
                    MockExpensesRep.SetupGet(m => m.CurrentPeriod).Returns(
                      () => _currentPeriod);
                }
                );

            //Текущий период(get)
            MockExpensesRep.SetupGet(m => m.CurrentPeriod).Returns(
                      () => _currentPeriod);

            //Текущая неделя(set)
            MockExpensesRep.SetupSet(m => m.CurrentWeek = It.IsAny<IWeek>()).Callback(
                (IWeek week_) =>
                {
                    _currentWeek = week_;
                    MockExpensesRep.SetupGet(m => m.CurrentWeek).Returns(
                      () => _currentWeek);
                }
                );

            //Текущая неделя(get)
            MockExpensesRep.SetupGet(m => m.CurrentWeek).Returns(
                      () => _currentWeek);

            //Текущий день (get)
            MockExpensesRep.SetupGet(m => m.CurrentDay).Returns(
                   () => _currentDay);

            //Текущий день (set)
            MockExpensesRep.SetupSet(m => m.CurrentDay = It.IsAny<DateTime?>()).Callback(
                   (DateTime? day_) =>
                   {
                       _currentDay = day_;
                       MockExpensesRep.SetupGet(m => m.CurrentDay).Returns(
                    () => _currentDay);
                   }
                   );

            //Получить все покупки за период
            MockExpensesRep.Setup(m => m.SelectPurchasesByPeriod(It.IsAny<IPeriod>())).Returns(
                ((IPeriod period_) => { return SelectPurchaseByPeriod(period_); })
                );
            

            //Получить все покупки за неделю
            MockExpensesRep.Setup(m => m.SelectPurchasesByWeek(It.IsAny<IWeek>())).Returns
                ((IWeek week_) => { return SelectPurchaseByWeek(week_); });
            //Получить все покупки за день
            MockExpensesRep.Setup(m => m.SelectPurchaseByDate(It.IsAny<DateTime>())).Returns(
                (DateTime day_) => { return SelectPurchaseByDay(day_); });

            //Текущая группа покупок(get)
            MockExpensesRep.SetupGet(m => m.CurrentPurchaseGId).Returns(
                   () => _currentPurchaseGId);

            //Текущая группа покупок(set)
            MockExpensesRep.SetupSet(m => m.CurrentPurchaseGId = It.IsAny<int?>()).Callback(
                   (int? currentPurchaseGId_) =>
                   {
                       _currentPurchaseGId = currentPurchaseGId_;
                       MockExpensesRep.SetupGet(m => m.CurrentPurchaseGId).Returns(
                    () => _currentPurchaseGId);
                   });

        }

        public static MockBus Get()
        {
            if (_mBus == null)
                _mBus = new MockBus();
            return _mBus;

        }

        /// <summary>
        /// Получить все подгруппы выбранной группы
        /// </summary>
        /// <param name="parent_">родительская группа</param>
        /// <returns></returns>
        private void selectSubGroupByParent(IGroup parent_, ref List<IGroup> subGroupList_)
        {
            var gEnum = _groupsList.Where(g => g.IdParent == parent_.Id).ToList();
            subGroupList_.AddRange(gEnum);

            foreach (IGroup g in _groupsList)
                selectSubGroupByParent(g, ref subGroupList_);
        }

        /// <summary>
        /// Получить все покупки за месяц
        /// </summary>
        /// <param name="week_"></param>
        /// <returns></returns>
        // private List<IPurchase> SelectPurchasesBy
        private IQueryable<IPurchase> SelectPurchaseByPeriod(IPeriod period_)
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
        private IQueryable<IPurchase> SelectPurchaseByWeek(IWeek week_)
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
        public IQueryable<IPurchase> SelectPurchaseByDay(DateTime day_)
        {
            var purchases =
                _purchaseList.Where(p => (p.Date == day_)).AsQueryable();
            return purchases;
        }

        #region eintity lists
        

        


        #endregion


        private int? _currentGroup = null;
        private int? _currentItem = null;

        private int? _currentPurchaseId = null;
        private int? _currentPurchaseGId = null;

        private IPeriod _currentPeriod = null;
        private IWeek _currentWeek = null;
        private DateTime? _currentDay = null;

        public Mock<IExpensesRepository> MockExpensesRep { get; private set; }

        private static MockBus _mBus;
            
    }
}
