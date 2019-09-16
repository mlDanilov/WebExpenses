using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using DomainExpenses.Abstract;
using WebExpenses.Models.Shop.Interfaces;

namespace WebExpenses.Models.Shop
{
    /// <summary>
    /// Класс для отображения в виде "Карточка магазина"
    /// </summary>
    public class MShopCard : IMShopCard
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
        public int Id { get; set; } = -1;

        [Required(ErrorMessage = "Введите название магазина")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Введите адрес магазина")]
        public string Address { get; set; }
    }
}