using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Concrete;

namespace DomainExpenses.Abstract.Repositories
{
    /// <summary>
    /// Репозиторий для работы с покупками
    /// (Single Responsibility Principle)
    /// </summary>
    public interface IPurchaseRepository : IEntityRepositiory<IPurchase, Purchase>
    {

        /// <summary>
        /// Получить все периоды, в которых есть покупки
        /// </summary>
        /// <returns></returns>
        IQueryable<Period> SelectAllPeriods();

        /// <summary>
        /// Получить все годы, за которые есть покупки
        /// </summary>
        /// <returns></returns>
        IQueryable<int> SelectAllYears();



        /// <summary>
        /// Получить все расходы за месяц
        /// </summary>
        /// <param name="period_"></param>
        /// <returns></returns>
        [Obsolete("Используй SelectPurchaseByBeginAndEndDates")]
        IQueryable<Purchase> SelectPurchasesByPeriod(IPeriod period_);
        /// <summary>
        /// Получить все расходы за неделю
        /// </summary>
        /// <param name="week_"></param>
        /// <returns></returns>
        [Obsolete("Используй SelectPurchaseByBeginAndEndDates")]
        IQueryable<Purchase> SelectPurchasesByWeek(IWeek week_);
        /// <summary>
        /// Получить все расходы за день
        /// </summary>
        /// <param name="date_"></param>
        /// <returns></returns>
        [Obsolete("Используй SelectPurchaseByBeginAndEndDates")]
        IQueryable<Purchase> SelectPurchaseByDate(DateTime date_);

        /// <summary>
        /// Получить все расходы за диапазон
        /// </summary>
        /// <param name="date_"></param>
        /// <returns></returns>
        IQueryable<Purchase> SelectPurchaseByBeginAndEndDates(DateTime bDate_, DateTime eDate_);

        ///// <summary>
        ///// Текущая выбранная покупка
        ///// </summary>
        //int? CurrentPurchaseId { get; set; }
        ///// <summary>
        ///// Текущая выбранная группа покупок
        ///// </summary>
        //int? CurrentPurchaseGId { get; set; }
        ///// <summary>
        ///// Текущий выбраный день
        ///// </summary>
        //DateTime? CurrentDay { get; set; }
        ///// <summary>
        ///// Текущий период(месяц)
        ///// </summary>
        //IPeriod CurrentPeriod { get; set; }
        ///// <summary>
        ///// Текущая неделя
        ///// </summary>
        //IWeek CurrentWeek { get; set; }
    }
}
