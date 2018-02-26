using DomainExpenses.Abstract;
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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            
        }

        public DbSet<Item> Item { get; set; }

        public DbSet<Group> Group { get; set; }

        public DbRawSqlQuery<Group> GroupExt {
            get {
                return Database.SqlQuery<Group>("SelectAllGroupsNew");
            }
        }

        public DbSet<Shop> Shop { get; set; }

        public Task<Item> AddNewItem(string name_, int gId_)
        {
            var rQuery = Database.SqlQuery<Item>("AddItem", name_, gId_);
            return rQuery.FirstOrDefaultAsync();
        }

        public Task<Group> AddNewGroup(string name_, int gId_)
        {
            var rQuery = Database.SqlQuery<Group>("AddGroup", name_, gId_);
            return rQuery.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Редактрировать товар 
        /// </summary>
        /// <param name="id_">Код товара</param>
        /// <param name="name_">Новое название товара</param>
        /// <param name="gId_">Новый код группы товаров</param>
        public int EditItem(int id_, string name_, int gId_)
        {
            return -1;
        }
        /// <summary>
        /// Редактировать группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        /// <param name="name_">Новое название</param>
        /// <param name="parentGroupId_">Новый код родительской группы</param>
        public int EditGroup(int id_, string name_, int parentGroupId_)
        {
            return -1;
        }
        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id_">Код товара</param>
        public int DeleteItem(int id_)
        {
            return -1;
        }
        /// <summary>
        /// Удалить группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        public int DeleteGroup(int id_)
        {
            return -1;
        }
    }
}
