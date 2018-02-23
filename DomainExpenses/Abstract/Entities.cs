using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainExpenses.Abstract
{

    public interface IItem
    {
        /// <summary>
        /// Уникальный код
        /// </summary>
        int Id { get; }
        /// <summary>
        /// Код родительской группы
        /// </summary>
        int GId { get; }
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

        float Price { get; set; }

        float Count { get; set; }

        DateTime Time { get; set; }
    }
}
