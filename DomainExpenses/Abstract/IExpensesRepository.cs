using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Concrete;
     

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
        /// Магазины
        /// </summary>
        IQueryable<IShop> Shop { get; }

        /// <summary>
        /// Покупки
        /// </summary>
        IQueryable<IPurchase> Purchase { get; }



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
        void EditGroup(int id_, string name_, int? parentGroupId_);
       
        /// <summary>
        /// Удалить группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        void DeleteGroup(int id_);

        /// <summary>
        /// Группы товаров, название содержит все родителские группы, кроме корневой
        /// </summary>
        IQueryable<IGroup> GroupExt { get; }

        #endregion


        #region Purchase
        /// <summary>
        /// Текущая покупка
        /// </summary>
        /// <param name="gId_"></param>
        int? CurrentPurchaseId { get; set; }

        /// <summary>
        /// Текущая группа товаров покупок
        /// </summary>
        int? CurrentPurchaseGId { get; set; }


        /// <summary>
        /// Текущий день
        /// </summary>
        DateTime? CurrentDay { get; set; }

        /// <summary>
        /// Добавить новую покупку
        /// </summary>
        /// <param name="shopId_">код магазина</param>
        /// <param name="itemId_">код товара</param>
        /// <param name="price_">цена</param>
        /// <param name="count_">количество</param>
        /// <param name="date_">дата</param>
        /// <returns></returns>
        IPurchase AddNewPurchase(
            int shopId_,
            int itemId_,
            float price_,
            float count_,
            DateTime date_
            );
        /// <summary>
        /// Редактировать
        /// </summary>
        /// <param name="id_">Уникальный код</param>
        /// <param name="shopIdNew_">Новый код магазина</param>
        /// <param name="itemIdNew_">Новый код товара</param>
        /// <param name="priceNew_">Новая цена</param>
        /// <param name="countNew_">Новое количество</param>
        /// <param name="dateNew_">Новая дата</param>
        void EditPurchase(
            int id_,
            int? shopIdNew_,
            int itemIdNew_,
            float priceNew_,
            float countNew_,
            DateTime dateNew_
            );
        /// <summary>
        /// Удалить покупку
        /// </summary>
        /// <param name="id_">уникальный код покупки</param>
        void DeletePurchase(int id_);

        /// <summary>
        /// Текущий период
        /// </summary>
        IPeriod CurrentPeriod { get; set; }
        /// <summary>
        /// Текущая неделя
        /// </summary>
        IWeek CurrentWeek { get; set; }
        /// <summary>
        /// Загрузить периоды в которых есть покупки
        /// </summary>
        IQueryable<IPeriod> SelectAllPeriods();
        /// <summary>
        /// Загрузить все недели текущего периода
        /// </summary>
        IQueryable<IWeek> SelectWeeksOfCurrentPeriod();


        IQueryable<IPurchase> SelectPurchasesByPeriod(IPeriod period_);

        IQueryable<IPurchase> SelectPurchasesByWeek(IWeek week_);

        IQueryable<IPurchase> SelectPurchaseByDate(DateTime day_);

        #endregion
    }
}
