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
    /// Мок-синглтон IShopRepository
    /// </summary>
    public class ShopRepMock 
    {
        private ShopRepMock()
        {
            setShopBehavior();
        }
        /// <summary>
        /// Установить для мока поведение для работы с магазинами
        /// </summary>
        private void setShopBehavior()
        {
            var fBus = EntitiesFactory.Get();

            _shRepMock.SetupGet(m => m.Entities).Returns
                (
                () => _shopsList.AsQueryable()
                );
            //Текущий магазин(get)
            _shRepMock.SetupGet(m => m.CurrentShopId).Returns(
                () => _currentShop);
            //Текущий магазин(set)
            _shRepMock.SetupSet(m => m.CurrentShopId = It.IsAny<int?>()).Callback(
                (int? shopId_) =>
                {
                    _currentShop = shopId_;
                    _shRepMock.SetupGet(m => m.CurrentShopId).Returns(
                      () => _currentShop);
                });
            //Добавить новый магазин
            _shRepMock.Setup<Shop>(m => m.Create(It.IsAny<IShop>()))
                .Returns((IShop shop_) =>
                {
                    var shop = fBus.CreateShop(_shopsList.Max(sh => sh.Id) + 1,
                        shop_.Name, shop_.Address);

                    _shopsList.Add(shop);
                    _shRepMock.Setup(m => m.Entities).Returns(_shopsList.AsQueryable());
                    return shop;
                });

            //Редактировать существующий магазин
            _shRepMock.Setup(m => m.Update(It.IsAny<IShop>()))
                .Callback((IShop shop_) =>
                {
                    var shop = _shopsList.Where(sh => sh.Id == shop_.Id).FirstOrDefault();
                    if (shop == null)
                        return;
                    shop.Name = shop_.Name;
                    shop.Address = shop_.Address;
                });
            //Удалить магазин
            _shRepMock.Setup(m => m.Delete(It.IsAny<IShop>()))
                    .Callback(
                    (IShop shop_) =>
                    {
                        _shopsList.RemoveAll(sh => sh.Id == shop_.Id);
                        _shRepMock.Setup(m => m.Entities).Returns(_shopsList.AsQueryable());
                    });
        }


        /// <summary>
        /// Получить эксземпляр класса-синглтона
        /// </summary>
        /// <returns></returns>
        public static ShopRepMock Get()
        {
            if (_instance == null)
                _instance = new ShopRepMock();

            return _instance;
        }

        private static ShopRepMock _instance = new ShopRepMock();


        public Mock<IShopRepository> Mock
        {
            get
            {
                return _shRepMock;
            }
        }
        private Mock<IShopRepository> _shRepMock = new Mock<IShopRepository>();

      
        private int? _currentShop = null;
        /// <summary>
        /// Список магазинов
        /// </summary>
        private List<Shop> _shopsList = new List<Shop>
            {
                EntitiesFactory.Get().CreateShop(0, "Кировский", "На июльской"),
                EntitiesFactory.Get().CreateShop(1, "Райт","На сулимова"),
                EntitiesFactory.Get().CreateShop(2, "Пятёрочка", "Baky plaza"),
                EntitiesFactory.Get().CreateShop(3, "Аптека", "На июльской"),
                EntitiesFactory.Get().CreateShop(4, "Аптека", "На сулимова")
            };
    }
}
