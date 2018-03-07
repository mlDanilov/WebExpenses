﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Purchase
{
    /// <summary>
    /// Покупка
    /// </summary>
    public class MPurchase : IPurchase
    {
        public MPurchase()
        {

        }
        public MPurchase(int id_)
        {
            Id = id_;
        }
        public int Id { get; private set; }
        /// <summary>
        /// Код магазина
        /// </summary>
        public int Shop_Id { get; set; }
        /// <summary>
        /// Название магазина
        /// </summary>
        public string ShopName { get; set; }
        /// <summary>
        /// Код товара
        /// </summary>
        public int Item_Id { get; set; }

        /// <summary>
        /// Название товара
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// Код группы
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Название группы с родительскими группами кроме корневой
        /// </summary>
        public int GroupExtName { get; set; }
       
        /// <summary>
        /// Цена
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public float Count { get; set; }
        /// <summary>
        /// Время покупки
        /// </summary>
        public DateTime Date { get; set; }
    }
}