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
    /// Репозиторий для работы с покупками
    /// </summary>
    internal class PurchaseRepository : IPurchaseRepository
    {

        public PurchaseRepository(ExpensesDBContext dbContext_)
        {
            _dbContext = dbContext_;
        }
        /// <summary>
        /// Товары
        /// </summary>
        public IQueryable<Purchase> Entities
        {
            get
            {
                return _dbContext.Purchase;
            }
        }
        /// <summary>
        /// Добавить покупку в БД
        /// </summary>
        /// <param name="purchase_"></param>
        public Purchase Create(IPurchase purchase_)
        {
            var purchase = EntitiesFactory.Get().CreatePurchase(
                purchase_.Id, purchase_.Shop_Id, purchase_.Item_Id, purchase_.Price, purchase_.Count, purchase_.Date
                );
            var purch = _dbContext.Purchase.Add(purchase);
            _dbContext.SaveChanges();

            return purch;
        }
        /// <summary>
        /// Обновить покупку в БД
        /// </summary>
        /// <param name="purchase_"></param>
        public void Update(IPurchase purchase_)
        {
            var purchase = _dbContext.Purchase.Where(p => p.Id == purchase_.Id).First();
            purchase.Item_Id = purchase_.Item_Id;
            purchase.Price = purchase_.Price;
            purchase.Shop_Id = purchase_.Shop_Id;
            purchase.Count = purchase_.Count;
            purchase.Date = purchase_.Date;

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Удалить покупку из БД
        /// </summary>
        /// <param name="entity_"></param>
        public void Delete(IPurchase purchase_)
        {
            var purchase = _dbContext.Purchase.Where(p => p.Id == purchase_.Id).First();
            _dbContext.Purchase.Remove(purchase);
            _dbContext.SaveChanges();
        }


        public IQueryable<Period> SelectAllPeriods()
           => _dbContext.SelectAllPeriods().AsQueryable<Period>();
        /// <summary>
        /// Получить все недели текущего периода
        /// </summary>
        /// <returns></returns>
        public IQueryable<Week> SelectWeeksByPeriod(IPeriod period_)
        => _dbContext.SelectWeeksOfCurrentPeriod(period_).AsQueryable<Week>();
        /// <summary>
        /// Получить все расходы за месяц
        /// </summary>
        /// <param name="period_"></param>
        /// <returns></returns>
        public IQueryable<Purchase> SelectPurchasesByPeriod(IPeriod period_)
           => _dbContext.SelectPurchasesByPeriod(period_);
        /// <summary>
        /// Получить все расходы за неделю
        /// </summary>
        /// <param name="week_"></param>
        /// <returns></returns>
        public IQueryable<Purchase> SelectPurchasesByWeek(IWeek week_)
                    => _dbContext.SelectPurchasesByWeek(week_);
        /// <summary>
        /// Получить все расходы за день
        /// </summary>
        /// <param name="date_"></param>
        /// <returns></returns>
        public IQueryable<Purchase> SelectPurchaseByDate(DateTime date_)
                   => _dbContext.SelectPurchasesByDay(date_);
        /// <summary>
        /// Текущая покупка
        /// </summary>
        public int? CurrentPurchaseId { get; set; }
        /// <summary>
        /// Текущая выбранная группа покупок
        /// </summary>
        public int? CurrentPurchaseGId { get; set; }
        /// <summary>
        /// Текущий выбраный день
        /// </summary>
        public DateTime? CurrentDay { get; set; }
        /// <summary>
        /// Текущий период(месяц)
        /// </summary>
        public IPeriod CurrentPeriod { get; set; }
        /// <summary>
        /// Текущая неделя
        /// </summary>
        public IWeek CurrentWeek { get; set; }

        private ExpensesDBContext _dbContext;
    }
}
