using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using DomainExpenses.Abstract;
using WebExpenses.Models.Group.Interface;

namespace WebExpenses.Models.Group
{
    /// <summary>
    /// Класс карточки группы, передаваемой в вид
    /// </summary>
    public class MGroupCard : IMGroupCard
    {

        public MGroupCard()
        {

        }

        public MGroupCard(IGroup group_)
        {
            Id = group_.Id;
            IdParent = group_.IdParent;
            Name = group_.Name;
            NameExt = group_.Name;
        }
        public MGroupCard(IGroup group_, string nameExt_)
        {
            Id = group_.Id;
            IdParent = group_.IdParent;
            Name = group_.Name;
            NameExt = nameExt_;
        }
        /// <summary>
        /// Уникальный код
        /// </summary>
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

        /// <summary>
        /// Наименование + путь к головной группе
        /// </summary>
        public string NameExt { get; set; }
    }
}