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
       
        

        #region Shop
        /// <summary>
        /// Текущий магазин
        /// </summary>
        int? CurrentShopId { get; set; }

        /// <summary>
        /// Добавить новый магазин
        /// </summary>
        /// <param name="">название</param>
        /// <returns></returns>
        IShop AddNewShop(string name_, string address_);

        /// <summary>
        /// Редактировать текущий магазин
        /// </summary>
        /// <param name="id_">уникальный код</param>
        /// <param name="name_">Новое название</param>
        /// /// <param name="address_">Новый адрес</param>
        void EditShop(int id_, string name_, string address_);
      
        /// <summary>
        /// Удалить магазин
        /// </summary>
        /// <param name="id_">Уникальный код</param>
        void DeleteShop(int id_);


        #endregion


        #region Item

        /// <summary>
        /// Текущий товар
        /// </summary>
        int? CurrentIId { get; set; }
        /// <summary>
        /// Добавить новый товар
        /// </summary>
        /// <param name="name_">Название</param>
        /// <param name="gId_">Код группы товаров</param>
        /// <returns></returns>
        IItem AddNewItem(string name_, int gId_);
        /// <summary>
        /// Редактрировать товар 
        /// </summary>
        /// <param name="id_">Код товара</param>
        /// <param name="name_">Новое название товара</param>
        /// <param name="gId_">Новый код группы товаров</param>
        void EditItem(int id_, string name_, int gId_);

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id_">Код товара</param>
        void DeleteItem(int id_);

        #endregion


        #region Group

        /// <summary>
        /// Текущая группа товаров
        /// </summary>
        /// <param name="gId_"></param>
        int? CurrentGId { get; set; }
        /// <summary>
        /// Добавить новую группу
        /// </summary>
        /// <param name="name_">Название</param>
        /// <param name="parentGroupId_">Код родительской группы</param>
        /// <returns></returns>
        IGroup AddNewGroup(string name_, int parentGroupId_);

        /// <summary>
        /// Редактировать группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        /// <param name="name_">Новое название</param>
        /// <param name="parentGroupId_">Новый код родительской группы</param>
        void EditGroup(int id_, string name_, int parentGroupId_);
       
        /// <summary>
        /// Удалить группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        void DeleteGroup(int id_);

        #endregion
    }
}
