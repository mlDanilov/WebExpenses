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

            // Установить для мока поведения для работы с группами товаров
            setGroupBehavior();
            // Установить для мока поведение для работы с товарами
            setItemBehavior();
            //Установить для мока поведения для работы с магазинами
            setShopBehavior();
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
        /// Установить для мока поведения для работы с группами товаров
        /// </summary>
        private void setGroupBehavior()
        {
            var fBus = EntitiesFactory.Get();
            //Добавить новую группу
            MockDbContext.Setup<IGroup>(m => m.AddNewGroup(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(
            (string name_, int gId_) =>
            {
                var group = fBus.CreateGroup(_groupsList.Max(g => g.Id) + 1, gId_, name_);

                _groupsExtList.Add(group);
                _groupsList.Add(group);

                MockDbContext.Setup(m => m.Group).Returns(_groupsList.AsQueryable());
                MockDbContext.Setup(m => m.GroupExt).Returns(_groupsExtList.AsQueryable());
                return group;
            });
            //Редактировать существующую группу товаров
            MockDbContext.Setup(m => m.EditGroup(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Callback(
                (int id_, string name_, int parentGId_) =>
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


        public static MockBus Get()
        {
            if (_mBus == null)
                _mBus = new MockBus();
            return _mBus;

        }
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
                EntitiesFactory.Get().CreateGroup(4, 1, @"Остальное")
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
                EntitiesFactory.Get().CreateGroup(4, 1, @"Остальное")
            };

        private int? _currentGroup = null;
        private int? _currentItem = null;
        private int? _currentShop= null;

        public Mock<IExpensesRepository> MockDbContext { get; private set; }

        private static MockBus _mBus;
            
    }
}
