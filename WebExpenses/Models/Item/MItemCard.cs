﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using DomainExpenses.Abstract;

namespace WebExpenses.Models.Item
{
    using Interfaces;
        
    /// <summary>
    /// Класс для отображения в виде "Карточка товара"
    /// </summary>
    public class MItemCard : IMItemCard
    {
        public MItemCard()
        {

        }
        public MItemCard(IItem item_)
        {
            Id = item_.Id;
            GId = item_.GId;
            Name = item_.Name;
        }
        public int Id { get; set; } = -1;

        [Required(ErrorMessage = "Выберите группу товара")]
        public int GId { get; set; }

        [Required(ErrorMessage = "Выберите название товара")]
        public string Name { get; set; }
    }
}