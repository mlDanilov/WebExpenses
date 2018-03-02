using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;

namespace DomainExpenses.Concrete
{
    public class ExpensesRepository : IExpensesRepository
    {

        public ExpensesRepository()
        {
           
        }
        /// <summary>
        /// Товары
        /// </summary>
        public IQueryable<IItem> Item
        {
            get {
                return _context.Item;
            }
        }
        /// <summary>
        /// Магазины
        /// </summary>
        public IQueryable<IShop> Shop
        {
            get
            {
                return _context.Shop;
            }
        }
        /// <summary>
        /// Группы
        /// </summary>
        public IQueryable<IGroup> Group
        {
            get
            {
                return _context.Group;
            }
        }
        /// <summary>
        /// Группы с указанием в названии родительских групп, кроме корневой
        /// </summary>
        public IQueryable<IGroup> GroupExt {
            get {
                return _context.GroupExt.AsQueryable<IGroup>();
            }
        }
        /// <summary>
        /// Покупки
        /// </summary>
        public IQueryable<IPurchase> Purchases
        {
            get {
                return _context.Purchase;
            }
        }
       

        #region Item
        /// <summary>
        /// Текущий товар
        /// </summary>
        public int? CurrentIId { get; set; }
        /// <summary>
        /// Добавить новый товар
        /// </summary>
        /// <param name="name_">название товара</param>
        /// <param name="gId_">код группы товаров</param>
        /// <returns></returns>
        public IItem AddNewItem(string name_, int gId_) 
            => _context.AddNewItem(name_, gId_).Result;
        /// <summary>
        /// Редактировать товар
        /// </summary>
        /// <param name="id_">Уникальный код товара</param>
        /// <param name="name_">новое название товара</param>
        /// <param name="gId_">новая код группы товаров</param>
        public void EditItem(int id_, string name_, int gId_) 
            => _context.EditItem(id_, name_, gId_);

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id_">Код товара</param>
        public void DeleteItem(int id_)
            => _context.DeleteItem(id_);

        #endregion

        #region Group
        /// <summary>
        /// Текущая группа товаров
        /// </summary>
        public int? CurrentGId { get; set; }
        /// <summary>
        /// Добавить новую группу товаров
        /// </summary>
        /// <param name="name_">Название группы</param>
        /// <param name="parentGroupId_">Код родительской группы</param>
        /// <returns></returns>
        public IGroup AddNewGroup(string name_, int parentGroupId_)
         => _context.AddNewGroup(name_, parentGroupId_).Result;
        /// <summary>
        /// Редактировать группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        /// <param name="name_">Новое название</param>
        /// <param name="parentGroupId_">Новый код родительской группы</param>
        public void EditGroup(int id_, string name_, int parentGroupId_) 
            => _context.EditGroup(id_, name_, parentGroupId_);
        /// <summary>
        /// Удалить группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        public void DeleteGroup(int id_)
            => _context.DeleteGroup(id_);
        #endregion

        #region Shop
        /// <summary>
        /// Текущая группа товаров
        /// </summary>
        public int? CurrentShopId { get; set; }

        /// <summary>
        /// Добавить новый магазин
        /// </summary>
        /// <param name="name_">Название магазина</param>
        /// <param name="address_">Адрес магазина</param>
        public IShop AddNewShop(string name_, string address_)
            => _context.AddNewShop(name_, address_).Result;
        /// <summary>
        /// Редактировать группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        /// <param name="name_">Новое название</param>
        /// <param name="parentGroupId_">Новый код родительской группы</param>
        public void EditShop(int id_, string name_, string address_)
            => _context.EditShop(id_, name_, address_);
        /// <summary>
        /// Удалить магазин
        /// </summary>
        /// <param name="id_">Код товара</param>
        public void DeleteShop(int id_)
            => _context.DeleteShop(id_);


        #endregion

        #region Purchase
        /// <summary>
        /// Добавить покупку
        /// </summary>
        /// <param name="shopId_">код магазина</param>
        /// <param name="itemId_">код товара</param>
        /// <param name="price_">цена</param>
        /// <param name="count_">количество</param>
        /// <param name="date_">время покупки</param>
        /// <returns></returns>
        public IPurchase AddNewPurchase(
            int shopId_,
            int itemId_,
            float price_,
            float count_,
            DateTime date_
            ) => _context.AddNewPurchase(shopId_, itemId_, price_, count_, date_).Result;

        /// <summary>
        /// Редактрировать покупку
        /// </summary>
        /// <param name="id_">уникальный код покупки</param>
        /// <param name="shopIdNew_">новый код магазина</param>
        /// <param name="itemIdNew_">новы код товара</param>
        /// <param name="priceNew_">новая цена</param>
        /// <param name="countNew_">новое количество</param>
        /// <param name="dateNew_">новое время</param>
        public void EditPurchase(
            int id_,
            int shopIdNew_,
            int itemIdNew_,
            float priceNew_,
            float countNew_,
            DateTime dateNew_
            ) => _context.EditPurchase(id_, shopIdNew_, itemIdNew_, priceNew_, countNew_, dateNew_);
        /// <summary>
        /// Удалить покупку
        /// </summary>
        /// <param name="id_">уникальный код покупки</param>
        public void DeletePurchase(int id_) => _context.DeletePurchase(id_);

        #endregion

        private ExpensesDBContext _context = new ExpensesDBContext();
    }
}
