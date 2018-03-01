using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Shop
{
    /// <summary>
    /// Класс для отображения в виде "Карточка магазина"
    /// </summary>
    public class MShopCard : IShop
    {
        public MShopCard()
        {

        }
        public MShopCard(IShop shop_)
        {
            Id = shop_.Id;
            Name = shop_.Name;
            Address = shop_.Address;
        }
        public int Id { get; } = -1;

        public string Name { get; set; }

        public string Address { get; set; }
    }
}