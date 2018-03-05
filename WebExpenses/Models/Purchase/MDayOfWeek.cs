using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebExpenses.Models.Purchase
{
    public struct MDayOfWeek
    {
        /// <summary>
        /// День недели
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// День
        /// </summary>
        public DateTime Day { get; set; }

    }
}