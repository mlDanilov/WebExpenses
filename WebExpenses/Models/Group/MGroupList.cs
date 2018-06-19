using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Group
{
    public class MGroupList
    {
        public int? GroupId { get; set; } = null;

        public List<IGroup> GroupList { get; set; }
    }
}