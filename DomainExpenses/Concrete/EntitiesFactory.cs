using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;
using DomainExpenses.Concrete;

namespace DomainExpenses.Concrete
{
    /// <summary>
    /// фабрика порождающая объекты + сингтон
    /// </summary>
    public class EntitiesFactory
    {
        private EntitiesFactory()
        {

        }


        /// <summary>
        /// Создать товар
        /// </summary>
        /// <param name="id_">уникальный код</param>
        /// <param name="gId_">код родительской группы</param>
        /// <param name="name_">Название товара</param>
        /// <returns></returns>
        public Item CreateItem(int id_, int gId_, string name_)
          => new Item(id_, gId_) { Name = name_ };

        /// <summary>
        /// Создать группу товаров
        /// </summary>
        /// <param name="id_">уникальный код</param>
        /// <param name="idParent_">код родительской группы</param>
        /// <param name="name_">название</param>
        /// <returns></returns>
        public Group CreateGroup(int id_, int? idParent_, string name_)
          => new Group(id_) { IdParent = idParent_, Name = name_ };
        /// <summary>
        /// Создать магазин
        /// </summary>
        /// <param name="id_">уникальный код</param>
        /// <param name="name_">Название</param>
        /// <returns></returns>
        public Shop CreateShop(int id_, string name_, string address_)
            => new Shop(id_) { Name = name_, Address = address_ };

        /// <summary>
        /// Создать покупку
        /// </summary>
        /// <param name="id_">уникальный код</param>
        /// <param name="shopId_">код магазина</param>
        /// <param name="itemId_">код товара</param>
        /// <param name="price_">Цена товара(за штуку/килограмм)</param>
        /// <param name="count_">Количество купленного товара(штук/килограммов)</param>
        /// <param name="date_">дата покупки</param>
        /// <returns></returns>
        public Purchase CreatePurchase(int id_, int? shopId_, int itemId_, float price_, float count_, DateTime date_)
            => new Purchase(id_)
            {
                Item_Id = itemId_,
                Shop_Id = shopId_,
                Count = count_,
                Price = price_,
                Date = date_
            };

        /// <summary>
        /// Создать период
        /// </summary>
        /// <param name="period_">yyyy-MM-01</param>
        /// <returns></returns>
        public Period CreatePeriod(DateTime period_)
           => new Period()
           { MonthYear = new DateTime(period_.Year, period_.Month, 1) };


        /// <summary>
        /// Создать неделю
        /// </summary>
        /// <param name="bDate_">Начало недели</param>
        /// <param name="eDate_">Конец недели</param>
        /// <returns></returns>
        public Week CreateWeek(DateTime bDate_, DateTime eDate_)
          => new Week()
          { BDate = bDate_, EDate = eDate_ };


        public static EntitiesFactory Get()
        {
            if (_fBus == null)
                _fBus = new EntitiesFactory();
            return _fBus;
        }
        
        private static EntitiesFactory _fBus = null;
    }
}
