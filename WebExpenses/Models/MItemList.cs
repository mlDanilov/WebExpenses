using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models
{
    public class MItemList
    {
        public int? GroupId { get; set; } = null;

        public List<IItem> ItemList { get; set; }
    }
}