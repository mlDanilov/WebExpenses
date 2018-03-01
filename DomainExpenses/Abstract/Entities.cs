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


    public interface IPurchase
    {
        int Id { get; }

        int Shop_Id { get; set; }

        int Item_Id { get; set; }

        float price { get; set; }

        float count { get; set; }

        DateTime Time { get; set; }
    }
}
