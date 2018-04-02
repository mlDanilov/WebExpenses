using DomainExpenses.Abstract;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;




namespace DomainExpenses.Concrete
{
    internal class ExpensesDBContext : DbContext
    {

        public ExpensesDBContext()
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder_)
        {
            modelBuilder_.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Item> Item { get; set; }

        public DbSet<Group> Group { get; set; }

        public DbRawSqlQuery<Group> GroupExt {
            get {
                return Database.SqlQuery<Group>("SelectAllGroupsNew");
            }
        }

        public DbSet<Shop> Shop { get; set; }

        public DbSet<Purchase> Purchase { get; set; }

        /// <summary>
        /// Получить все периоды
        /// </summary>
        public DbRawSqlQuery<Period> SelectAllPeriods() 
            => Database.SqlQuery<Period>("SelectAllPeriods");
        /// <summary>
        /// Получить все недели текущего периода
        /// </summary>
        /// <param name="period_"></param>
        /// <returns></returns>
        public DbRawSqlQuery<Week> SelectWeeksOfCurrentPeriod(IPeriod period_)
        {
            //Database.SqlQuery<Week>("SelectWeeksByMonth", period_.Period);
            var pMonth = new SqlParameter("@Month", System.Data.SqlDbType.Date)
            { Value = period_.MonthYear };

            var rQuery = Database.SqlQuery<Week>("exec SelectWeeksByMonth @Month", pMonth);
            return rQuery;
           
        }

        /// <summary>
        /// Получить все расходы за неделю
        /// </summary>
        /// <param name="week_"></param>
        /// <returns></returns>
        public IQueryable<Purchase> SelectPurchasesByWeek(IWeek week_)
        {
            var res = (from p in Purchase
                       where ((p.Date >= week_.BDate) && (p.Date <= week_.EDate))
                       select p);
            return res;
        }
        /// <summary>
        /// Получить все расходы за день
        /// </summary>
        /// <param name="date_"></param>
        /// <returns></returns>
        public IQueryable<Purchase> SelectPurchasesByDay(DateTime date_)
        {
            var res = (from p in Purchase
                       where (p.Date == date_) select p);
            return res;
        }

        public IQueryable<Purchase> SelectPurchasesByPeriod(IPeriod period_)
        {
            var res = (from p in Purchase
                       where
                       (p.Date.Month == period_.MonthYear.Month) &&
                       (p.Date.Year == period_.MonthYear.Year)
                       select p);
            return res;
        }
    }
}
