using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebExpenses.Models.Group.Interface
{
    /// <summary>
    /// Интерфейс списка групп для отображения на форме
    /// </summary>
    public interface IMGroupList
    {
        int? GroupId { get; set; }
        /// <summary>
        /// Список групп для отображения на форме
        /// </summary>
        List<IMGroupCard> GroupList { get; set; }
    }
}