using DomainExpenses.Abstract;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

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
        public Task<Item> AddNewItem(string name_, int gId_)
        {
            var rQuery = Database.SqlQuery<Item>("AddItem", name_, gId_);
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

        }
        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id_">Код товара</param>
        public void DeleteItem(int id_)
        {

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
            var rQuery = Database.SqlQuery<Group>("AddGroup", name_, gId_);
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
        }
        /// <summary>
        /// Удалить группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        public void DeleteGroup(int id_)
        {
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
            var rQuery = Database.SqlQuery<Shop>("AddShop", name_, address_);
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

        }
        /// <summary>
        /// Удалить магазин
        /// </summary>
        /// <param name="id_">Код магазина</param>
        public void DeleteShop(int id_)
        {

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
            int shopIdNew_,
            int itemIdNew_,
            float priceNew_,
            float countNew_,
            DateTime dateNew_
            )
        {

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
        public DbRawSqlQuery<Periond> SelectAllPeriods()
        {
            return Database.SqlQuery<Periond>("SelectAllPeriods");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="month_"></param>
        /// <returns></returns>
        public DbRawSqlQuery<Week> SelectWeeksOfCurrentPeriod(IPeriod period_)
        => Database.SqlQuery<Week>("SelectWeeksByMonth", period_.Period);

        public DbRawSqlQuery<Purchase> SelectPurchaseByWeek(IWeek week_)
        => Database.SqlQuery<Purchase>("SelectPurchaseByWeek", week_);


        #endregion
    }
}
