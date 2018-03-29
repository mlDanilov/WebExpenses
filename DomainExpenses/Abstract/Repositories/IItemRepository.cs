using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using DomainExpenses.Concrete;

namespace DomainExpenses.Abstract.Repositories
{
    /// <summary>
    /// Репозиторий для работы с товарами
    /// </summary>
    public interface IItemRepository : IEntityRepositiory<IItem, Item>
    {
        /// <summary>
        /// Текущий товар
        /// </summary>
        int? CurrentIId { get; set; }
    }
}
