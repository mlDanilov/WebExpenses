using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Group
{
    public class MGroupCard : IGroup
    {

        public MGroupCard()
        {

        }

        public MGroupCard(IGroup group_)
        {
            Id = group_.Id;
            IdParent = group_.IdParent;
            Name = group_.Name;
        }
        public int Id { get; set; } = -1;

        /// <summary>
        /// Код родительской группы
        /// </summary>
        [Required(ErrorMessage = "Выберите родительску группу")]
        public int? IdParent { get; set; }

        /// <summary>
        /// Имя продительской группы
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// Название 
        /// </summary>
        [Required(ErrorMessage = "Введине название группы товаров")]
        public string Name { get; set; }
    }
}