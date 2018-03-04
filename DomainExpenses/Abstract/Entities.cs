using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainExpenses.Abstract
{
    /// <summary>
    /// Товар
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Уникальный код
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Код родительской группы
        /// </summary>
        int GId { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; set; }
    }
    /// <summary>
    /// Группа товаров
    /// </summary>
    public interface IGroup
    {
        /// <summary>
        /// Уникальный код
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Код родительской группы
        /// </summary>
        int? IdParent { get; set; }
        /// <summary>
        /// Название 
        /// </summary>
        string Name { get; set; }
    }

    /// <summary>
    /// Магазин
    /// </summary>
    public interface IShop
    {
        /// <summary>
        /// Уникальный код
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Название
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        string Address { get; set; }
    }

    /// <summary>
    /// Покупка
    /// </summary>
    public interface IPurchase
    {
        /// <summary>
        /// Уникальный код покупки
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Код магазина
        /// </summary>
        int Shop_Id { get; set; }
        /// <summary>
        /// Код товара
        /// </summary>
        int Item_Id { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        float Price { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        float Count { get; set; }
        /// <summary>
        /// Время покупки
        /// </summary>
        DateTime Date { get; set; }
    }

    /// <summary>
    /// Период "Месяц-Год"
    /// </summary>
    public interface IPeriod
    {
        /// <summary>
        ///  yyyy-MM-01
        /// </summary>
        DateTime Period { get; set; }
    }
    /// <summary>
    /// Неделя 
    /// </summary>
    public interface IWeek
    {
        /// <summary>
        /// Начальная дата
        /// </summary>
        DateTime BDate { get; set; }
        /// <summary>
        /// Конечная дата
        /// </summary>
        DateTime EDate { get; set; }
    }
}
