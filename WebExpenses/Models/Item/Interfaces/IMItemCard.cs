using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Item.Interfaces
{
    /// <summary>
    /// Класс для отображения в виде "Карточка товара"
    /// </summary>
    public interface IMItemCard : IItem
    {
        new int Id { get; }

        new int GId { get; set; }
        new string Name { get; set; }
    }
}