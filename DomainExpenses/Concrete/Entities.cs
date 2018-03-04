using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;

namespace DomainExpenses.Concrete
{
    /// <summary>
    /// Товар
    /// </summary>
    public class Item : IItem
    {
        public Item() { }
        public Item(int id_, int gId_)
        {
            Id = id_;
            GId = gId_;
        }
        /// <summary>
        /// Уникальный код
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Код родительской группы
        /// </summary>
        public int GId { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
    }
    /// <summary>
    /// Группа товаров
    /// </summary>
    public class Group : IGroup
    {
        public Group() { }
        public Group(int id_)
        {
            Id = id_;
        }
        /// <summary>
        /// Уникальный код
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Код родительской группы
        /// </summary>
        public int? IdParent { get; set; }
        /// <summary>
        /// Название 
        /// </summary>
        public string Name { get; set; }
    }
    /// <summary>
    /// Магазин
    /// </summary>
    public class Shop : IShop
    {
        public Shop() { }
        public Shop(int id_)
        {
            Id = id_;
        }
        /// <summary>
        /// Уникальный код
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }
    }
    /// <summary>
    /// Покупка
    /// </summary>
    public class Purchase : IPurchase
    {
        public Purchase() { }
        public Purchase(int id_)
        {
            Id = id_;
        }
        /// <summary>
        /// Уникальный код
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Код магазина
        /// </summary>
        public int Shop_Id { get; set; }
        /// <summary>
        /// Код товара
        /// </summary>
        public int Item_Id { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public float Price { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public float Count { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
    }
    /// <summary>
    /// Период "Месяц-Год"
    /// </summary>
    public class Periond :  IPeriod
    {
        /// <summary>
        ///  yyyy-MM-01
        /// </summary>
        public DateTime Period { get; set; }
    }
    /// <summary>
    /// Неделя
    /// </summary>
    public class Week : IWeek
    {
        /// <summary>
        /// Начальная дата
        /// </summary>
        public DateTime BDate { get; set; }
        /// <summary>
        /// Конечная дата
        /// </summary>
        public DateTime EDate { get; set; }
    }
}


