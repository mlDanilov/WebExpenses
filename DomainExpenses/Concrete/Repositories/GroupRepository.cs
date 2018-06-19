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
    /// Репозиторий для работы с группами товаров
    /// </summary>
    internal class GroupRepository : IGroupRepository
    {
        internal GroupRepository(ExpensesDBContext dbContext_)
        {
            _dbContext = dbContext_;
        }

        /// <summary>
        /// Товары
        /// </summary>
        public IQueryable<Group> Entities
        {
            get
            {
                return _dbContext.Group;
            }
        }
        /// <summary>
        /// Добавить группу в БД
        /// </summary>
        /// <param name="group_"></param>
        public Group Create(IGroup group_)
        {
            var group = EntitiesFactory.Get().CreateGroup(group_.Id, group_.IdParent, group_.Name);
            var g = _dbContext.Group.Add(group);

            _dbContext.SaveChanges();
            return g;
        }
        /// <summary>
        /// Обновить группу в БД
        /// </summary>
        /// <param name="group_"></param>
        public void Update(IGroup group_)
        {
            var group = _dbContext.Group.Where(g => g.Id == group_.Id).First();
            group.Name = group_.Name;
            group.IdParent = group_.IdParent;

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Удалить группу из БД
        /// </summary>
        /// <param name="group_"></param>
        public void Delete(IGroup group_)
        {
            var group = _dbContext.Group.Where(g => g.Id == group_.Id).First();
            _dbContext.Group.Remove(group);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Текущая группа товаров
        /// </summary>
        public int? CurrentGId { get; set; }


        public IQueryable<Group> GroupExt
        {
            get
            {
                return _dbContext.GroupExt.AsQueryable<Group>();
            }
        }

        private ExpensesDBContext _dbContext;
    }
}
