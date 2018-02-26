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

            // Установить для мока поведения для работы с группами товаров
            AddEditDeleteGroupBehavior();
            // Установить для мока поведение для работы с товарами
            AddEditDeleteItemBehavior();
        }
        /// <summary>
        /// Установить для мока поведение для работы с товарами
        /// </summary>
        private void AddEditDeleteItemBehavior()
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
            MockDbContext.Setup<int>(m => m.EditItem(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(
                (int id_, string name_, int gId_) =>
                {
                    var item = _itemList.Where(it => it.Id == id_).FirstOrDefault();
                    if (item == null) return -1;
                    item.Name = name_;
                    item.GId = gId_;

                    return 1;
                });
            //Удалить товар
            MockDbContext.Setup<int>(m => m.DeleteItem(It.IsAny<int>()))
               .Returns(
               (int id_) =>
               {
                   _itemList.RemoveAll(it => it.Id == id_);
                    MockDbContext.Setup(m => m.Item).Returns(_itemList.AsQueryable()); 
                   return 1;
               });
        }
        /// <summary>
        /// Установить для мока поведения для работы с группами товаров
        /// </summary>
        private void AddEditDeleteGroupBehavior()
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
            MockDbContext.Setup<int>(m => m.EditGroup(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(
                (int id_, string name_, int parentGId_) =>
                {
                    var group = _groupsList.Where(g => g.Id == id_).FirstOrDefault();
                    var groupExt = _groupsExtList.Where(g => g.Id == id_).FirstOrDefault();
                    if (group == null) return -1;
                    group.Name = name_;
                    group.IdParent = parentGId_;
                    if (groupExt == null) return -1;
                    groupExt.Name = name_;
                    groupExt.IdParent = parentGId_;

                    return 1;
                });
            //Удалить группу товаров
            MockDbContext.Setup<int>(m => m.DeleteGroup(It.IsAny<int>()))
               .Returns(
               (int id_) =>
               {
                   _groupsList.RemoveAll(g => g.Id == id_);
                   _groupsExtList.RemoveAll(g => g.Id == id_);
                   MockDbContext.Setup(m => m.Group).Returns(_groupsList.AsQueryable());
                   MockDbContext.Setup(m => m.GroupExt).Returns(_groupsExtList.AsQueryable());
                   return 1;
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

        public Mock<IExpensesRepository> MockDbContext { get; private set; }

        private static MockBus _mBus;
            
    }
}
