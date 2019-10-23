using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Group.Interface
{
    /// <summary>
    /// Интерфейс карточки группы, передаваемой в вид
    /// </summary>
    public interface IMGroupCard : IGroup
    {
        /// <summary>
        /// Уникальный код
        /// </summary>
        new int Id { get; }

        /// <summary>
        /// Код родительской группы
        /// </summary>
        new int? IdParent { get; set; }
        /// <summary>
        /// Название 
        /// </summary>
        new string Name { get; set; }

        /// <summary>
        /// Наименование + путь к головной группе
        /// </summary>
        string NameExt { get; set; }

    }
}
