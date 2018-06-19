using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

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
        public MPurchase(IPurchase purch_)
        {
            Id = purch_.Id;
            Shop_Id = purch_.Shop_Id;
            Item_Id  = purch_.Item_Id;
            Price = purch_.Price;
            Count = purch_.Count;
            Date = purch_.Date;
        }
        public int Id { get; set; }
        /// <summary>
        /// Код магазина
        /// </summary>
        public int? Shop_Id { get; set; }
        /// <summary>
        /// Название магазина
        /// </summary>
        public string ShopName { get; set; }

        /// <summary>
        /// Адрес магазина
        /// </summary>
        public string ShopAddress { get; set; }
        /// <summary>
        /// Код товара
        /// </summary>
        //[Required(ErrorMessage = "Выберите товар")]
        [Range(0, int.MaxValue, ErrorMessage = "Выберите товар")]
        public int Item_Id { get; set; } = -1;

        /// <summary>
        /// Название товара
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// Код группы
        /// </summary>
        [Required(ErrorMessage = "Выберите группу товара")]
        public int GroupId { get; set; }

        /// <summary>
        /// Название группы с родительскими группами кроме корневой
        /// </summary>
        public string GroupExtName { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        //[Required(ErrorMessage = "Введите цену товара")]
        [Range(float.Epsilon, float.MaxValue, ErrorMessage = "Цена должна быть больше нуля")]
        public float Price { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        [Range(float.Epsilon, float.MaxValue, ErrorMessage = "Количество должно быть больше нуля")]
        public float Count { get; set; }
        /// <summary>
        /// Время покупки
        /// </summary>
        [DataType(DataType.Date, ErrorMessage ="Некорректный формат. Введите в формате: дд.мм.гг")]
        [Remote("ValidateDate", "Purchase")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Введите дату покупки")]
        public DateTime Date { get; set; }
    }
}