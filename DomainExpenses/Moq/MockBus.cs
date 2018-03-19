using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

using Moq;
using DomainExpenses.Abstract;
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

            MockDbContext = new Mock<IExpensesRepository>();
            //Список товаров
            MockDbContext.Setup(m => m.Item).Returns(_itemList.AsQueryable());
            //Список групп
            MockDbContext.Setup(m => m.Group).Returns(_groupsList.AsQueryable());
            //Список групп, название указывается с родительскими группми, кроме корневой
            MockDbContext.Setup(m => m.GroupExt).Returns(_groupsExtList.AsQueryable());
            //Список магазинов
            MockDbContext.Setup(m => m.Shop).Returns(_shopsList.AsQueryable());
            //Список покупок
            MockDbContext.Setup(m => m.Purchase).Returns(_purchaseList.AsQueryable());
            //Список покупок
            MockDbContext.Setup(m => m.SelectAllPeriods()).Returns(_periods.AsQueryable());
            //Список недель
            MockDbContext.Setup(m => m.SelectWeeksOfCurrentPeriod()).Returns(
                _weeks.Where(
                    week => 
                    ((_currentPeriod.MonthYear.Month == week.BDate.Month) && (_currentPeriod.MonthYear.Year == week.BDate.Year)) ||
                    ((_currentPeriod.MonthYear.Month == week.EDate.Month) && (_currentPeriod.MonthYear.Year == week.EDate.Year))
                    )
                .AsQueryable()
                );
          



            // Установить для мока поведения для работы с группами товаров
            setGroupBehavior();
            // Установить для мока поведение для работы с товарами
            setItemBehavior();
            //Установить для мока поведение для работы с магазинами
            setShopBehavior();
            //Установить для мока поведение для работы с покупками
            setPurchaseBehavior();
        }
        /// <summary>
        /// Установить для мока поведение для работы с товарами
        /// </summary>
        private void setItemBehavior()
        {
            var fBus = EntitiesFactory.Get();
            //Добавить новый товар
            MockDbContext.Setup<IItem>(m => m.AddNewItem(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(
            (string name_, int gId_) =>
            {
                var item = fBus.CreateItem(_itemList.Max(it => it.Id) + 1, gId_, name_);
                _itemList.Add(item);
                MockDbContext.Setup(m => m.Item).Returns(_itemList.AsQueryable());
                return item;
            });
            //Редактировать существующий товар
            MockDbContext.Setup(m => m.EditItem(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Callback(
                (int id_, string name_, int gId_) =>
                {
                    var item = _itemList.Where(it => it.Id == id_).FirstOrDefault();
                    if (item == null) return;
                    item.Name = name_;
                    item.GId = gId_;
                });
            //Удалить товар
            MockDbContext.Setup(m => m.DeleteItem(It.IsAny<int>()))
               .Callback(
               (int id_) =>
               {
                   _itemList.RemoveAll(it => it.Id == id_);
                    MockDbContext.Setup(m => m.Item).Returns(_itemList.AsQueryable()); 
               });

            //Текущий товар(get)
            MockDbContext.SetupGet(m => m.CurrentIId).Returns(
                () =>
                {
                    return _currentItem;
                }
                );
            //Текущий товар(set)
            MockDbContext.SetupSet(m => m.CurrentIId = It.IsAny<int?>()).Callback(
                (int? iid_) =>
                {
                    _currentItem = iid_;
                    MockDbContext.SetupGet(m => m.CurrentIId).Returns(
                      () =>
                      {
                          return _currentItem;
                      }
                      );
                });
        }
        /// <summary>
        /// Установить для мока поведение для работы с группами товаров
        /// </summary>
        private void setGroupBehavior()
        {
            var fBus = EntitiesFactory.Get();
            //Добавить новую группу
            MockDbContext.Setup<IGroup>(m => m.AddNewGroup(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(
            (string name_, int gId_) =>
            {
                var group = fBus.CreateGroup(
                    _itemList.Max(it => it.GId) + 1, 
                    gId_, name_);

                _groupsExtList.Add(group);
                _groupsList.Add(group);

                MockDbContext.Setup(m => m.Group).Returns(_groupsList.AsQueryable());
                MockDbContext.Setup(m => m.GroupExt).Returns(_groupsExtList.AsQueryable());
                return group;
            });
            //Редактировать существующую группу товаров
            MockDbContext.Setup(m => m.EditGroup(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int?>()))
                .Callback(
                (int id_, string name_, int? parentGId_) =>
                {
                    var group = _groupsList.Where(g => g.Id == id_).FirstOrDefault();
                    var groupExt = _groupsExtList.Where(g => g.Id == id_).FirstOrDefault();
                    if (group == null) return;
                    group.Name = name_;
                    group.IdParent = parentGId_;
                    if (groupExt == null) return;
                    groupExt.Name = name_;
                    groupExt.IdParent = parentGId_;
                });
            //Удалить группу товаров
            MockDbContext.Setup(m => m.DeleteGroup(It.IsAny<int>()))
               .Callback(
               (int id_) =>
               {
                   _groupsList.RemoveAll(g => g.Id == id_);
                   _groupsExtList.RemoveAll(g => g.Id == id_);
                   MockDbContext.Setup(m => m.Group).Returns(_groupsList.AsQueryable());
                   MockDbContext.Setup(m => m.GroupExt).Returns(_groupsExtList.AsQueryable());
               });

            //Текущая группа(get)
            MockDbContext.SetupGet(m => m.CurrentGId).Returns(
                () => _currentGroup );
            //Текущая группа(set)
            MockDbContext.SetupSet(m=>m.CurrentGId = It.IsAny<int?>()).Callback(
                (int? gId_) => 
                {
                    _currentGroup = gId_;
                    MockDbContext.SetupGet(m => m.CurrentGId).Returns(
                      () => _currentGroup );
                });

            
        }
        /// <summary>
        /// Установить для мока поведение для работы с магазинами
        /// </summary>
        private void setShopBehavior()
        {
            var fBus = EntitiesFactory.Get();

            //Добавить новый магазин
            MockDbContext.Setup<IShop>(m => m.AddNewShop(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(
            (string name_, string address_) =>
            {
                var shop = fBus.CreateShop(_shopsList.Max(sh => sh.Id) + 1, name_, address_);
                _shopsList.Add(shop);
                MockDbContext.Setup(m => m.Shop).Returns(_shopsList.AsQueryable());
                return shop;
            });
            //Редактировать существующий магазин
            MockDbContext.Setup(m => m.EditShop(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback(
                (int id_, string name_, string address_) =>
                {
                    var shop = _shopsList.Where(sh => sh.Id == id_).FirstOrDefault();
                    if (shop == null)
                        return;
                    shop.Name = name_;
                    shop.Address = address_;
                });
            //Удалить магазин
            MockDbContext.Setup(m => m.DeleteShop(It.IsAny<int>()))
               .Callback(
               (int id_) =>
               {
                   _shopsList.RemoveAll(sh => sh.Id == id_);
                   MockDbContext.Setup(m => m.Shop).Returns(_shopsList.AsQueryable());
               });

            //Текущий магазин(get)
            MockDbContext.SetupGet(m => m.CurrentShopId).Returns(
                () => _currentShop );
            //Текущий магазин(set)
            MockDbContext.SetupSet(m => m.CurrentShopId = It.IsAny<int?>()).Callback(
                (int? shopId_) =>
                {
                    _currentShop = shopId_;
                    MockDbContext.SetupGet(m => m.CurrentShopId).Returns(
                      () => _currentShop );
                });


        }
        /// <summary>
        /// Установить для мока поведение для работы с покупками
        /// </summary>
        private void setPurchaseBehavior()
        {
            var fBus = EntitiesFactory.Get();

            //Добавить новый магазин
            MockDbContext.Setup<IPurchase>(m => m.AddNewPurchase(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<DateTime>()))
                .Returns(
            (int shopId_, int itemId_, float price_, float count_, DateTime date_) =>
            {
                var purchase = fBus.CreatePurchase(_purchaseList.Max(p => p.Id) + 1,
                    shopId_, itemId_, price_, count_, date_);
                _purchaseList.Add(purchase);
                MockDbContext.Setup(m => m.Purchase).Returns(_purchaseList.AsQueryable());
                return purchase;
            });
            //Редактировать существующую покупку
            MockDbContext.Setup(m => m.EditPurchase(
                It.IsAny<int>(),
                It.IsAny<int?>(),
                It.IsAny<int>(),
                It.IsAny<float>(),
                It.IsAny<float>(),
                It.IsAny<DateTime>())).Callback(
                 (int id_, int? shopId_, int itemId_, float price_, float count_, DateTime date_) =>
                {
                    var purchase = _purchaseList.Where(p => p.Id == id_).FirstOrDefault();
                    if (purchase == null)
                        return;
                    purchase.Item_Id = itemId_;
                    purchase.Shop_Id = shopId_;
                    purchase.Price = price_;
                    purchase.Count = count_;
                    purchase.Date = date_;
                });
            //Удалить покупку
            MockDbContext.Setup(m => m.DeletePurchase(It.IsAny<int>()))
               .Callback(
               (int id_) =>
               {
                   _purchaseList.RemoveAll(p => p.Id == id_);
                   MockDbContext.Setup(m => m.Purchase).Returns(_purchaseList.AsQueryable());
               });

            //Текущий магазин(get)
            MockDbContext.SetupGet(m => m.CurrentPurchaseId).Returns(
                () => _currentPurchaseId);
            //Текущий магазин(set)
            MockDbContext.SetupSet(m => m.CurrentPurchaseId = It.IsAny<int?>()).Callback(
                (int? purchaseId_) =>
                {
                    _currentPurchaseId = purchaseId_;
                    MockDbContext.SetupGet(m => m.CurrentPurchaseId).Returns(
                      () => _currentPurchaseId);
                });

            //Текущий период(set)
            MockDbContext.SetupSet(m => m.CurrentPeriod = It.IsAny<IPeriod>()).Callback(
                (IPeriod period_) =>
                {
                    _currentPeriod = period_;
                    MockDbContext.SetupGet(m => m.CurrentPeriod).Returns(
                      () => _currentPeriod);
                }
                );

            //Текущий период(get)
            MockDbContext.SetupGet(m => m.CurrentPeriod).Returns(
                      () => _currentPeriod);

            //Текущая неделя(set)
            MockDbContext.SetupSet(m => m.CurrentWeek = It.IsAny<IWeek>()).Callback(
                (IWeek week_) =>
                {
                    _currentWeek = week_;
                    MockDbContext.SetupGet(m => m.CurrentWeek).Returns(
                      () => _currentWeek);
                }
                );

            //Текущая неделя(get)
            MockDbContext.SetupGet(m => m.CurrentWeek).Returns(
                      () => _currentWeek);

            //Текущий день (get)
            MockDbContext.SetupGet(m => m.CurrentDay).Returns(
                   () => _currentDay);

            //Текущий день (set)
            MockDbContext.SetupSet(m => m.CurrentDay = It.IsAny<DateTime?>()).Callback(
                   (DateTime? day_) =>
                   {
                       _currentDay = day_;
                       MockDbContext.SetupGet(m => m.CurrentDay).Returns(
                    () => _currentDay);
                   }
                   );

            //Получить все покупки за период
            MockDbContext.Setup(m => m.SelectPurchasesByPeriod(It.IsAny<IPeriod>())).Returns(
                ((IPeriod period_) => { return SelectPurchaseByPeriod(period_); })
                );
            

            //Получить все покупки за неделю
            MockDbContext.Setup(m => m.SelectPurchasesByWeek(It.IsAny<IWeek>())).Returns
                ((IWeek week_) => { return SelectPurchaseByWeek(week_); });
            //Получить все покупки за день
            MockDbContext.Setup(m => m.SelectPurchaseByDate(It.IsAny<DateTime>())).Returns(
                (DateTime day_) => { return SelectPurchaseByDay(day_); });

            //Текущая группа покупок(get)
            MockDbContext.SetupGet(m => m.CurrentPurchaseGId).Returns(
                   () => _currentPurchaseGId);

            //Текущая группа покупок(set)
            MockDbContext.SetupSet(m => m.CurrentPurchaseGId = It.IsAny<int?>()).Callback(
                   (int? currentPurchaseGId_) =>
                   {
                       _currentPurchaseGId = currentPurchaseGId_;
                       MockDbContext.SetupGet(m => m.CurrentPurchaseGId).Returns(
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
        /// <summary>
        /// Список товаров
        /// </summary>
        private List<IItem> _itemList = new List<IItem>
            {
                //Главная группа\Мясо\Птица
                 EntitiesFactory.Get().CreateItem(0, 2,  "Куриные бёдрышки"),
                 EntitiesFactory.Get().CreateItem(1, 2,  "Куриные крылышки"),
                EntitiesFactory.Get().CreateItem(3, 2,  "Куриные шейки"),
                //Главная группа\Мясо\Говядина
                EntitiesFactory.Get().CreateItem(4, 3,  "Говяжья вырезка"),
                EntitiesFactory.Get().CreateItem(5, 3,  "Рёбрышки"),
                EntitiesFactory.Get().CreateItem(6, 3,  "Смесь для шашлыка"),
                EntitiesFactory.Get().CreateItem(7, 3,  "Рога и копыта"),
                //Главная группа\Остальное
                EntitiesFactory.Get().CreateItem(8, 4,  "Мыло"),
                EntitiesFactory.Get().CreateItem(9, 4,  "Шампунь"),
                EntitiesFactory.Get().CreateItem(10, 4,  "Туалетная бумага"),
                EntitiesFactory.Get().CreateItem(11, 4,  "Зубная паста"),
                EntitiesFactory.Get().CreateItem(12, 4,  "Чистящее средство")
        };

        /// <summary>
        /// Список магазинов
        /// </summary>
        private List<IShop> _shopsList = new List<IShop>
            {
                EntitiesFactory.Get().CreateShop(0, "Кировский", "На июльской"),
                EntitiesFactory.Get().CreateShop(1, "Райт","На сулимова"),
                EntitiesFactory.Get().CreateShop(2, "Пятёрочка", "Baky plaza"),
                EntitiesFactory.Get().CreateShop(3, "Аптека", "На июльской"),
                EntitiesFactory.Get().CreateShop(4, "Аптека", "На сулимова")
            };
        /// <summary>
        /// Список групп
        /// </summary>
        private List<IGroup> _groupsList = new List<IGroup>
            {
                EntitiesFactory.Get().CreateGroup(0, null,"Главная группа"),
                EntitiesFactory.Get().CreateGroup(1, 0,"Мясо"),
                EntitiesFactory.Get().CreateGroup(2, 1, @"Птица"),
                EntitiesFactory.Get().CreateGroup(3, 1, @"Говядина"),
                EntitiesFactory.Get().CreateGroup(4, 0, @"Остальное")
            };
        /// <summary>
        /// Список групп, название указывается с родительскими группами, кроме корневой
        /// </summary>
        private List<IGroup> _groupsExtList = new List<IGroup>
            {
                EntitiesFactory.Get().CreateGroup(0, null,"Главная группа"),
                EntitiesFactory.Get().CreateGroup(1, 0,"Мясо"),
                EntitiesFactory.Get().CreateGroup(2, 1, @"Мясо\Птица"),
                EntitiesFactory.Get().CreateGroup(3, 1, @"Мясо\Говядина"),
                EntitiesFactory.Get().CreateGroup(4, 0, @"Остальное")
            };
        /// <summary>
        /// Список покупок
        /// </summary>
        private List<IPurchase> _purchaseList = new List<IPurchase>
        {
            //Декабрь 2018 1 неделя
            EntitiesFactory.Get().CreatePurchase(0, 1, 2, 10, 5, new DateTime(2018, 1, 1)),
            EntitiesFactory.Get().CreatePurchase(1, 2, 7, 20, 7, new DateTime(2018, 1, 1)),
            EntitiesFactory.Get().CreatePurchase(2, 2, 5, 15, 1, new DateTime(2018, 1, 2)),
            EntitiesFactory.Get().CreatePurchase(3, 1, 3, 54, 1, new DateTime(2018, 1, 2)),
            EntitiesFactory.Get().CreatePurchase(4, 3, 1, 20, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(5, 1, 8, 10, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(6, 2, 9, 8,  6, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(7, 2, 5, 61, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(8, 1, 6, 48, 3, new DateTime(2018, 1, 5)),
            EntitiesFactory.Get().CreatePurchase(9, 3, 7, 48, 3, new DateTime(2018, 1, 5)),


            //Ноябрь 2017 1 неделя
            EntitiesFactory.Get().CreatePurchase(10, 5, 7, 33, 5, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchase(11, 6, 8, 20, 7, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchase(12, 3, 8, 27, 1, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchase(13, 4, 9, 73, 1, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchase(14, 2, 2, 23, 2, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchase(15, 2, 1, 56, 2, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchase(16, 4, 3, 43,  6, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchase(17, 8, 4, 61, 2, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchase(18, 7, 5, 21, 3, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchase(19, 3, 5, 55, 3, new DateTime(2018, 11, 4)),

            //Ноябрь 2017 2-3 недели
             EntitiesFactory.Get().CreatePurchase(20, 5, 7, 45, 3, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchase(21, 6, 8, 43, 4, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchase(22, 3, 8, 28, 8, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchase(23, 4, 9, 12, 9, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchase(24, 2, 2, 76, 4, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchase(25, 2, 1,20, 5, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchase(26, 4, 3, 65,  7, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchase(27, 8, 4, 36, 3, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchase(28, 7, 5, 87, 3, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchase(29, 3, 5, 29, 1, new DateTime(2018, 11, 19))
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

        #endregion


        private int? _currentGroup = null;
        private int? _currentItem = null;
        private int? _currentShop= null;
        private int? _currentPurchaseId = null;
        private int? _currentPurchaseGId = null;

        private IPeriod _currentPeriod = null;
        private IWeek _currentWeek = null;
        private DateTime? _currentDay = null;

        public Mock<IExpensesRepository> MockDbContext { get; private set; }

        private static MockBus _mBus;
            
    }
}
