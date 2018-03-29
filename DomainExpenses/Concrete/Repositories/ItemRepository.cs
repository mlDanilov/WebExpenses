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
    /// Репозиторий для работы с товарами
    /// </summary>
    internal class ItemRepository : IItemRepository
    {
        public ItemRepository(ExpensesDBContext dbContext_)
        {
            _dbContext = dbContext_;
        }

        /// <summary>
        /// Товары
        /// </summary>
        public IQueryable<Item> Entities
        {
            get {
                return _dbContext.Item;
            }
        }
        /// <summary>
        /// Добавить товар в БД
        /// </summary>
        /// <param name="item_"></param>
        public Item Create(IItem item_)
        {
            var item = EntitiesFactory.Get().CreateItemC(item_.Id, item_.GId, item_.Name);
            var it = _dbContext.Item.Add(item);
            _dbContext.SaveChanges();

            return it;
        }
        /// <summary>
        /// Обновить товар в БД
        /// </summary>
        /// <param name="item_"></param>
        public void Update(IItem item_)
        {
            var item = _dbContext.Item.Where(it => it.Id == item_.Id).First();
            item.Name = item_.Name;
            item.GId = item_.GId;

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Удалить товар из БД
        /// </summary>
        /// <param name="entity_"></param>
        public void Delete(IItem entity_)
        {
            var item = _dbContext.Item.Where(it => it.Id == entity_.Id).First();
            _dbContext.Item.Remove(item);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Текущий товар
        /// </summary>
        public int? CurrentIId { get; set; }

        private ExpensesDBContext _dbContext;
    }
}
