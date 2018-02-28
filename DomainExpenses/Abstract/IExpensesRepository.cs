using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainExpenses.Abstract
{
    public interface IExpensesRepository
    {
        /// <summary>
        /// Товары
        /// </summary>
        IQueryable<IItem> Item { get; }
        /// <summary>
        /// Группы товаров
        /// </summary>
        IQueryable<IGroup> Group { get; }
        /// <summary>
        /// Группы товаров, название содержит все родителские группы, кроме корневой
        /// </summary>
        IQueryable<IGroup> GroupExt { get; }
        /// <summary>
        /// Магазины
        /// </summary>
        IQueryable<IShop> Shop { get; }
        /// <summary>
        /// Текущая группа товаров
        /// </summary>
        /// <param name="gId_"></param>
        int? CurrentGId { get; set; }

        int? CurrentIId { get; set; }

        /// <summary>
        /// Добавить новый товар
        /// </summary>
        /// <param name="name_">Название</param>
        /// <param name="gId_">Код группы товаров</param>
        /// <returns></returns>
        IItem AddNewItem(string name_, int gId_);
        /// <summary>
        /// Добавить новую группу
        /// </summary>
        /// <param name="name_">Название</param>
        /// <param name="parentGroupId_">Код родительской группы</param>
        /// <returns></returns>
        IGroup AddNewGroup(string name_, int parentGroupId_);
        /// <summary>
        /// Редактрировать товар 
        /// </summary>
        /// <param name="id_">Код товара</param>
        /// <param name="name_">Новое название товара</param>
        /// <param name="gId_">Новый код группы товаров</param>
        int EditItem(int id_, string name_, int gId_);
        /// <summary>
        /// Редактировать группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        /// <param name="name_">Новое название</param>
        /// <param name="parentGroupId_">Новый код родительской группы</param>
        int EditGroup(int id_, string name_, int parentGroupId_);
        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id_">Код товара</param>
        int DeleteItem(int id_);
        /// <summary>
        /// Удалить группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        int DeleteGroup(int id_);
    }
}
