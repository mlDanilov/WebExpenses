using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;

namespace DomainExpenses.Concrete
{
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
        public int GId { get; private set; }
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

    public class GroupExt : IGroup
    {
        public GroupExt() { }
        public GroupExt(int id_)
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

    public class Purchase : IPurchase
    {
        public Purchase() { }
        public Purchase(int id_)
        {
            Id = id_;
        }

        public int Id { get; private set; }
        public int Shop_Id { get; set; }

        public int Item_Id { get; set; }

        public float Price { get; set; }

        public float Count { get; set; }

        public DateTime Time { get; set; }
    }
}


