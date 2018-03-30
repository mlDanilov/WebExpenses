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
    /// Мок-синглтон IItemRepository
    /// </summary>
    public class ItemRepMock
    {

        private ItemRepMock()
        {
            setItemBehavior();
        }
        private void setItemBehavior()
        {
            var fBus = EntitiesFactory.Get();
            //Текущий товар(get)
            _itRepMock.SetupGet(m => m.CurrentIId).Returns(
                () => _currentItemId);
            //Текущий товар(set)
            _itRepMock.SetupSet(m => m.CurrentIId = It.IsAny<int?>()).Callback(
                (int? itemId_) =>
                {
                    _currentItemId = itemId_;
                    _itRepMock.SetupGet(m => m.CurrentIId).Returns(
                      () => _currentItemId);
                });
            //Товары
            _itRepMock.SetupGet(m => m.Entities).Returns(_itemList.AsQueryable());


            //Добавить новый товар
            _itRepMock.Setup<Item>(m => m.Create(It.IsAny<IItem>()))
                .Returns((IItem item_) =>
                {
                    var item = fBus.CreateItemC(_itemList.Max(it => it.Id) + 1,
                        item_.GId, item_.Name);

                    _itemList.Add(item);
                    _itRepMock.Setup(m => m.Entities).Returns(_itemList.AsQueryable());
                    return item;
                });

            //Редактировать существующий товар
            _itRepMock.Setup(m => m.Update(It.IsAny<IItem>()))
                .Callback((IItem shop_) =>
                {
                    var item = _itemList.Where(it => it.Id == shop_.Id).FirstOrDefault();
                    if (item == null)
                        return;
                    item.GId = shop_.GId;
                    item.Name = shop_.Name;
                });
            //Удалить товар
            _itRepMock.Setup(m => m.Delete(It.IsAny<IItem>()))
                    .Callback(
                    (IItem item_) =>
                    {
                        _itemList.RemoveAll(it => it.Id == item_.Id);
                        _itRepMock.Setup(m => m.Entities).Returns(_itemList.AsQueryable());
                    });
        }


       

        public Mock<IItemRepository> Mock
        {
            get
            {
                return _itRepMock;
            }
        }
        private Mock<IItemRepository> _itRepMock = new Mock<IItemRepository>();


        /// <summary>
        /// Текущий товар
        /// </summary>
        private int? _currentItemId = null;
        /// <summary>
        /// Список товаров
        /// </summary>
        private List<Item> _itemList = new List<Item>
            {
                //Главная группа\Мясо\Птица
                 EntitiesFactory.Get().CreateItemC(0, 2,  "Куриные бёдрышки"),
                 EntitiesFactory.Get().CreateItemC(1, 2,  "Куриные крылышки"),
                EntitiesFactory.Get().CreateItemC(3, 2,  "Куриные шейки"),
                //Главная группа\Мясо\Говядина
                EntitiesFactory.Get().CreateItemC(4, 3,  "Говяжья вырезка"),
                EntitiesFactory.Get().CreateItemC(5, 3,  "Рёбрышки"),
                EntitiesFactory.Get().CreateItemC(6, 3,  "Смесь для шашлыка"),
                EntitiesFactory.Get().CreateItemC(7, 3,  "Рога и копыта"),
                //Главная группа\Остальное
                EntitiesFactory.Get().CreateItemC(8, 4,  "Мыло"),
                EntitiesFactory.Get().CreateItemC(9, 4,  "Шампунь"),
                EntitiesFactory.Get().CreateItemC(10, 4,  "Туалетная бумага"),
                EntitiesFactory.Get().CreateItemC(11, 4,  "Зубная паста"),
                EntitiesFactory.Get().CreateItemC(12, 4,  "Чистящее средство")
        };

        public static ItemRepMock Get()
        {
            if (_instance == null)
                _instance = new ItemRepMock();
            return _instance;
        }
        private static ItemRepMock _instance = new ItemRepMock();

    }
}
