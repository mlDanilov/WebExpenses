using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Shop
{
    public class MShopList
    {
        public int? ShopId { get; set; } = null;

        public List<IShop> ShopList { get; set; }
    }
}