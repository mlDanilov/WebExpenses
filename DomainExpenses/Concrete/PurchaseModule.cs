using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;

namespace DomainExpenses.Concrete
{
    /// <summary>
    /// Модуль для работы с покупками
    /// </summary>
    internal class PurchaseModule
    {
        public PurchaseModule(SelectWeeksDelegate selectWeeksDel_)
        {
            _selectWeeksDel = selectWeeksDel_;
        }
        /// <summary>
        /// Текущая покупка
        /// </summary>
        public int? CurrentPurchaseId { get; set; } = null;

        /// <summary>
        /// Текущий выбранный период
        /// </summary>
        public IPeriod CurrentPeriod { get; set; } = null;

        /// <summary>
        /// Текущая выбранная неделя
        /// </summary>
        public IWeek CurrentWeek { get; set; } = null;

        /// <summary>
        /// Текущий день
        /// </summary>
        public DateTime? CurrentDay { get; set; } = null;

        /// <summary>
        /// Получить все недели текущего периода месяца
        /// </summary>
        /// <param name="month_">Месяц</param>
        /// <returns></returns>
        public DbRawSqlQuery<Week> SelectWeeksByPeriod() => _selectWeeksDel(CurrentPeriod.Period);
        private SelectWeeksDelegate _selectWeeksDel = null;


        public delegate DbRawSqlQuery<Week> SelectWeeksDelegate(DateTime month_);
    }
}
