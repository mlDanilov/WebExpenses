using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;
using WebExpenses.Models.Group.Interface;

namespace WebExpenses.Models.Group
{
    /// <summary>
    /// Класс со списком групп для отображения на форме
    /// </summary>
    public class MGroupList : IMGroupList
    {

        public int? GroupId { get; set; }
        /// <summary>
        ///Список групп для отображения на форме
        /// </summary>
        public List<IMGroupCard> GroupList { get; set; }
    }
}