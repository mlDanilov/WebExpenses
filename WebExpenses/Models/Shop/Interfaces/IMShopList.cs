using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Shop.Interfaces
{
    public interface IMShopList
    {
        int? ShopId { get; set; }

        List<IShop> ShopList { get; set; }
    }
}