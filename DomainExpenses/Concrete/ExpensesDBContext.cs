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


        #region Item
        /// <summary>
        /// Добавить новый товар
        /// </summary>
        /// <param name="name_">Название</param>
        /// <param name="gId_">родительская группа</param>
        /// <returns></returns>
        public Task<Item> AddItem(string name_, int gId_)
        {
            var pName = new SqlParameter("@Name", System.Data.SqlDbType.Char)
            { Value = name_ };

            var pGId = new SqlParameter("@GId", System.Data.SqlDbType.Int)
            { Value = gId_ };

            var rQuery = Database.SqlQuery<Item>("exec AddItem @Name, @GId", pName, pGId);
            return rQuery.FirstOrDefaultAsync();
        }
        /// <summary>
        /// Редактрировать товар 
        /// </summary>
        /// <param name="id_">Код товара</param>
        /// <param name="name_">Новое название товара</param>
        /// <param name="gId_">Новый код группы товаров</param>
        public void EditItem(int id_, string name_, int gId_)
        {
            var pId = new SqlParameter("@Id", System.Data.SqlDbType.Int)
            { Value = id_ };

            var pName = new SqlParameter("@Name", System.Data.SqlDbType.Char)
            { Value = name_ };

            var pGId = new SqlParameter("@GId", System.Data.SqlDbType.Int)
            { Value = gId_ };

            var rQuery = Database.ExecuteSqlCommand("exec EditItem @Id, @Name, @GId", pId, pName, pGId);
        }
        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id_">Код товара</param>
        public void DeleteItem(int id_)
        {
            var pId = new SqlParameter("@Id", System.Data.SqlDbType.Int)
            { Value = id_ };
            Database.ExecuteSqlCommand("exec DeleteItem @Id", pId);
        }

        #endregion

        #region Group
        /// <summary>
        /// Добавить новую группу
        /// </summary>
        /// <param name="name_">Название</param>
        /// <param name="gId_">Родительская группа</param>
        /// <returns></returns>
        public Task<Group> AddNewGroup(string name_, int gId_)
        {
            var pName = new SqlParameter("@Name", System.Data.SqlDbType.Char)
            { Value = name_ };

            var pGId = new SqlParameter("@IdParent", System.Data.SqlDbType.Int)
            { Value = gId_ };

            var rQuery = Database.SqlQuery<Group>("exec AddGroup @Name, @IdParent", pName, pGId);
            return rQuery.FirstOrDefaultAsync();
        }

      
        /// <summary>
        /// Редактировать группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        /// <param name="name_">Новое название</param>
        /// <param name="parentGroupId_">Новый код родительской группы</param>
        public void EditGroup(int id_, string name_, int parentGroupId_)
        {
            var pId = new SqlParameter("@Id", System.Data.SqlDbType.Int)
            { Value = id_ };

            var pName = new SqlParameter("@Name", System.Data.SqlDbType.Char)
            { Value = name_ };

            var pIdParent = new SqlParameter("@IdParent", System.Data.SqlDbType.Int)
            { Value = parentGroupId_ };

            var rQuery = Database.ExecuteSqlCommand("exec EditGroup @Id, @IdParent, @Name", pId, pIdParent, pName);
        }
        /// <summary>
        /// Удалить группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        public void DeleteGroup(int id_)
        {
            var pId = new SqlParameter("@Id", System.Data.SqlDbType.Int)
            { Value = id_ };
            Database.ExecuteSqlCommand("exec DeleteGroup @Id", pId);
        }

        #endregion

        #region Shop
        /// <summary>
        /// Добавить новый магазин
        /// </summary>
        /// <param name="name_">Название</param>
        /// <param name="address_">Адрес</param>
        /// <returns></returns>
        public Task<Shop> AddNewShop(string name_, string address_)
        {
            var pName = new SqlParameter("@Name", System.Data.SqlDbType.Char)
            { Value = name_ };

            var pAddress = new SqlParameter("@Address", System.Data.SqlDbType.Char)
            { Value = address_ };

            var rQuery = Database.SqlQuery<Shop>("exec AddShop @Name, @Address", pName, pAddress);
            return rQuery.FirstOrDefaultAsync();
        }
        /// <summary>
        /// Редактрировать магазин
        /// </summary>
        /// <param name="id_">Код магазина</param>
        /// <param name="name_">Новое название магазина</param>
        /// <param name="gId_">Новый адрес</param>
        public void EditShop(int id_, string name_, string address_)
        {
            var pId = new SqlParameter("@Id", System.Data.SqlDbType.Int)
            { Value = id_ };

            var pName = new SqlParameter("@Name", System.Data.SqlDbType.Char)
            { Value = name_ };

            var pAddress = new SqlParameter("@Address", System.Data.SqlDbType.Char)
            { Value = address_ };

            var rQuery = Database.ExecuteSqlCommand("exec EditShop @Id, @Name, @Address", pId, pName, pAddress);
        }
        /// <summary>
        /// Удалить магазин
        /// </summary>
        /// <param name="id_">Код магазина</param>
        public void DeleteShop(int id_)
        {
            var pId = new SqlParameter("@Id", System.Data.SqlDbType.Int)
            { Value = id_ };
            Database.ExecuteSqlCommand("exec DeleteShop @Id", pId);
        }
        #endregion

        #region Purchase
        /// <summary>
        /// Добавить покупку
        /// </summary>
        /// <param name="shopId_">код магазина</param>
        /// <param name="itemId_">код товара</param>
        /// <param name="price_">цена</param>
        /// <param name="count_">количество</param>
        /// <param name="date_">время покупки</param>
        /// <returns></returns>
        public Task<Purchase> AddNewPurchase(
            int shopId_, 
            int itemId_,
            float price_,
            float count_,
            DateTime date_
            )
        {
            var rQuery = Database.SqlQuery<Purchase>("AddPurchase", shopId_, itemId_, price_, count_, date_);
            return rQuery.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Редактрировать покупку
        /// </summary>
        /// <param name="id_">уникальный код покупки</param>
        /// <param name="shopIdNew_">новый код магазина</param>
        /// <param name="itemIdNew_">новы код товара</param>
        /// <param name="priceNew_">новая цена</param>
        /// <param name="countNew_">новое количество</param>
        /// <param name="dateNew_">новое время</param>
        public void EditPurchase(
            int id_, 
            int? shopIdNew_,
            int itemIdNew_,
            float priceNew_,
            float countNew_,
            DateTime dateNew_
            )
        {
            var purchase = Purchase.Where(p => p.Id == id_).FirstOrDefault();
            if (purchase == null)
                return;

            purchase.Shop_Id = shopIdNew_;
            purchase.Item_Id = itemIdNew_;
            purchase.Price = priceNew_;
            purchase.Count = countNew_;
            purchase.Date = dateNew_;

        }
       /// <summary>
       /// Удалить покупку
       /// </summary>
       /// <param name="id_">уникальный код покупки</param>
        public void DeletePurchase(int id_)
        {

        }

        /// <summary>
        /// Получить все периоды
        /// </summary>
        public DbRawSqlQuery<Period> SelectAllPeriods()
        {
            return Database.SqlQuery<Period>("SelectAllPeriods");
        }
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
        public DbRawSqlQuery<Purchase> SelectPurchasesByWeek(IWeek week_)
        {
            var pBDate = new SqlParameter("@BDate", SqlDbType.Date)
            { Value = week_.BDate };
            var pEDate = new SqlParameter("@EDate", SqlDbType.Date)
            { Value = week_.EDate };
            var rQuery = Database.SqlQuery<Purchase>("SelectPurchasesByWeek", pBDate, pEDate);
            return rQuery;
        }
        /// <summary>
        /// Получить все расходы за день
        /// </summary>
        /// <param name="date_"></param>
        /// <returns></returns>
        public DbRawSqlQuery<Purchase> SelectPurchasesByDay(DateTime date_)
        {
            var pDay = new SqlParameter("@Day", SqlDbType.Date)
            { Value = date_ };

            var rQuery = Database.SqlQuery<Purchase>("exec SelectPurchasesByDay @Day", pDay);
            return rQuery;
        }

        /// <summary>
        /// Получить все расходы за период
        /// </summary>
        /// <param name="period_"></param>
        /// <returns></returns>
        public DbRawSqlQuery<Purchase> SelectPurchasesByPeriod(IPeriod period_)
        {
            var pPeriod = new SqlParameter("@Period", SqlDbType.Date)
            { Value = period_.MonthYear };

            var rQuery = Database.SqlQuery<Purchase>("exec SelectPurchasesByPeriod @Period", pPeriod);
            return rQuery;
        }

        #endregion
    }
}
