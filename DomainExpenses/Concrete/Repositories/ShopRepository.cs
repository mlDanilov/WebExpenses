using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;
using DomainExpenses.Abstract.Repositories;

namespace DomainExpenses.Concrete.Repositories
{
    /// <summary>
    /// Репозиторий для работы с магазинами
    /// </summary>
    internal class ShopRepository : IShopRepository
    {

        public ShopRepository(ExpensesDBContext dbContext_)
        {
            _dbContext = dbContext_;
        }

        /// <summary>
        /// Магазины
        /// </summary>
        public IQueryable<Shop> Entities
        {
            get
            {
                return _dbContext.Shop;
            }
        }
        /// <summary>
        /// Добавить магазин в БД
        /// </summary>
        /// <param name="shop_"></param>
        public Shop Create(IShop shop_)
        {
            var shop = EntitiesFactory.Get().CreateShopC(shop_.Id, shop_.Name, shop_.Address);
            var sh = _dbContext.Shop.Add(shop);
            _dbContext.SaveChanges();
            return sh;
        }
        /// <summary>
        /// Обновить магазин в БД
        /// </summary>
        /// <param name="shop_"></param>
        public void Update(IShop shop_)
        {
            var shop = _dbContext.Shop.Where(sh => sh.Id == shop_.Id).First();
            shop.Name = shop_.Name;
            shop.Address = shop_.Address;

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Удалить магазин из БД
        /// </summary>
        /// <param name="shop_"></param>
        public void Delete(IShop shop_)
        {
            var shop = _dbContext.Shop.Where(sh => sh.Id == shop_.Id).First();
            _dbContext.Shop.Remove(shop);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Текущий магазин
        /// </summary>
        public int? CurrentShopId { get; set; }

        private ExpensesDBContext _dbContext;
    }
}
