using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DomainExpenses.Abstract;
using DomainExpenses.Concrete;



namespace DomainExpenses.Concrete
{
    public class ExpensesRepository : IExpensesRepository
    {
        /*
        private ExpensesRepository()
        {

        }
        private static ExpensesRepository _instance = new ExpensesRepository();
        public static ExpensesRepository Get()
        {
            if (_instance == null)
                _instance = new ExpensesRepository();
            return _instance;
        }
        */
        public ExpensesRepository()
        {
            _purchaseModule = new PurchaseModule(_context.SelectWeeksOfCurrentPeriod);
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
                return _context.GroupExt.AsQueryable<Group>();
            }
        }
        /// <summary>
        /// Покупки
        /// </summary>
        public IQueryable<IPurchase> Purchase
        {
            get {
                return _context.Purchase;
            }
        }
        public int? CurrentPurchaseId { get; set; }

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
        {
            var item = _context.AddItem(name_, gId_).Result;
            _context.SaveChanges();
            return item;
        }
        /// <summary>
        /// Редактировать товар
        /// </summary>
        /// <param name="id_">Уникальный код товара</param>
        /// <param name="name_">новое название товара</param>
        /// <param name="gId_">новая код группы товаров</param>
        public void EditItem(int id_, string name_, int gId_)
        {
            _context.EditItem(id_, name_, gId_);
            var item = _context.Item.Where(it => it.Id == id_).FirstOrDefault();
            if (item != null)
            {
                item.Name = name_;
                item.GId = gId_;
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Удалить товар
        /// </summary>
        /// <param name="id_">Код товара</param>
        public void DeleteItem(int id_)
        {
            _context.DeleteItem(id_);
            _context.SaveChanges();
        }

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
        {
            var group = _context.AddNewGroup(name_, parentGroupId_).Result;
            _context.SaveChanges();
            return group;
        }
        /// <summary>
        /// Редактировать группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        /// <param name="name_">Новое название</param>
        /// <param name="parentGroupId_">Новый код родительской группы</param>
        public void EditGroup(int id_, string name_, int? parentGroupId_)
        {
            _context.EditGroup(id_, name_, parentGroupId_);
            var group = Group.Where(g => g.Id == id_).FirstOrDefault();
            if (group != null)
            {
                group.Name = name_;
                group.IdParent = parentGroupId_;
            }

            _context.SaveChanges();
        }
        /// <summary>
        /// Удалить группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        public void DeleteGroup(int id_)
        {
            _context.DeleteGroup(id_);
            _context.SaveChanges();
        }
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
        {
            var shop = _context.AddNewShop(name_, address_).Result;
            _context.SaveChanges();
            return shop;
        }
        /// <summary>
        /// Редактировать группу товаров
        /// </summary>
        /// <param name="id_">Код группы товаров</param>
        /// <param name="name_">Новое название</param>
        /// <param name="parentGroupId_">Новый код родительской группы</param>
        public void EditShop(int id_, string name_, string address_)
        {
            _context.EditShop(id_, name_, address_);
            _context.SaveChanges();
        }
        /// <summary>
        /// Удалить магазин
        /// </summary>
        /// <param name="id_">Код товара</param>
        public void DeleteShop(int id_)
        {
            _context.DeleteShop(id_);
            _context.SaveChanges();
        }


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
            int? shopId_,
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
            int? shopIdNew_,
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


        /// <summary>
        /// Загрузить периоды в которых есть покупки
        /// </summary>
        public IQueryable<IPeriod> SelectAllPeriods()
            => _context.SelectAllPeriods().AsQueryable<IPeriod>();
        /// <summary>
        /// Получить все недели текущего периода
        /// </summary>
        /// <returns></returns>
        public IQueryable<IWeek> SelectWeeksOfCurrentPeriod()
        => _purchaseModule.SelectWeeksByPeriod().AsQueryable();
        /// <summary>
        /// Получить все расходы за месяц
        /// </summary>
        /// <param name="period_"></param>
        /// <returns></returns>
        public IQueryable<IPurchase> SelectPurchasesByPeriod(IPeriod period_)
           => _context.SelectPurchasesByPeriod(period_).AsQueryable();
        /// <summary>
        /// Получить все расходы за неделю
        /// </summary>
        /// <param name="week_"></param>
        /// <returns></returns>
        public IQueryable<IPurchase> SelectPurchasesByWeek(IWeek week_)
                    => _context.SelectPurchasesByWeek(week_).AsQueryable();
        /// <summary>
        /// Получить все расходы за день
        /// </summary>
        /// <param name="date_"></param>
        /// <returns></returns>
        public IQueryable<IPurchase> SelectPurchaseByDate(DateTime date_)
                   => _context.SelectPurchasesByDay(date_).AsQueryable();

       
        public int? CurrentPurchaseGId { get; set; }
        public DateTime? CurrentDay
        {
            get {
                return _purchaseModule.CurrentDay;
            }
            set
            {
                _purchaseModule.CurrentDay = value;
            }
        }

        public IPeriod CurrentPeriod
        {
            get
            {
                return _purchaseModule.CurrentPeriod;
            }
            set
            {
                _purchaseModule.CurrentPeriod = value;
            }
        }

        public IWeek CurrentWeek
        {
            get
            {
                return _purchaseModule.CurrentWeek;
            }
            set
            {
                _purchaseModule.CurrentWeek = value;
            }
        }



        #endregion

        private ExpensesDBContext _context = new ExpensesDBContext();

        private PurchaseModule _purchaseModule = null;
    }
}
