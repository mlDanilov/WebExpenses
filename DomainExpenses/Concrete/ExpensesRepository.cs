using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;

namespace DomainExpenses.Concrete
{
    public class ExpensesRepository : IExpensesRepository
    {

        public ExpensesRepository()
        {
           
        }

        public IQueryable<IItem> Item
        {
            get {
                return _context.Item;
            }
        }
        public IQueryable<IShop> Shop
        {
            get
            {
                return _context.Shop;
            }
        }

        public IQueryable<IGroup> Group
        {
            get
            {
                return _context.Group;
            }
        }

        public IQueryable<IGroup> GroupExt {
            get {
                return _context.GroupExt.AsQueryable<IGroup>();
            }
        }


        public IItem AddNewItem(string name_, int gId_) 
            => _context.AddNewItem(name_, gId_).Result;

        public IGroup AddNewGroup(string name_, int gId_)
            => _context.AddNewGroup(name_, gId_).Result;

        public int EditItem(int id_, string name_, int gId_) 
            => _context.EditItem(id_, name_, gId_);
        /// <summary>
        /// Редактировать группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        /// <param name="name_">Новое название</param>
        /// <param name="parentGroupId_">Новый код родительской группы</param>
        public int EditGroup(int id_, string name_, int parentGroupId_) 
            => _context.EditGroup(id_, name_, parentGroupId_);
        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id_">Код товара</param>
        public int DeleteItem(int id_) 
            => _context.DeleteItem(id_);
        /// <summary>
        /// Удалить группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        public int DeleteGroup(int id_)
            => _context.DeleteGroup(id_);
        private ExpensesDBContext _context = new ExpensesDBContext();
    }
}
