using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Concrete;

namespace DomainExpenses.Abstract.Repositories
{
    /// <summary>
    /// Репозиторий для работы с группами товаров
    /// </summary>
    public interface IGroupRepository : IEntityRepositiory<IGroup, Group>
    {
        /// <summary>
        /// Текущая группа товаров
        /// </summary>
        int? CurrentGId { get; set; }


        IQueryable<Group> GroupExt { get; }
    }
}
