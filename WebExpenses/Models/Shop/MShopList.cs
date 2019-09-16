using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;
using WebExpenses.Models.Shop.Interfaces;

namespace WebExpenses.Models.Shop
{
    public class MShopList : IMShopList
    {
        public int? ShopId { get; set; } = null;

        public List<IShop> ShopList { get; set; }
    }
}