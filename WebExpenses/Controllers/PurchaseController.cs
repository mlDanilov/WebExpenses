using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainExpenses.Abstract;
using DomainExpenses.Concrete;

using WebExpenses.Models.Item;
using WebExpenses.Models.Group;
using WebExpenses.Models.Purchase;


namespace WebExpenses.Controllers
{

    /// <summary>
    /// Контроллер для работы с покупками
    /// </summary>
    public class PurchaseController : Controller
    {
        public PurchaseController(IExpensesRepository rep_)
        {
            _repository = rep_;
        }

        #region Period, Weeks, Days
        public ActionResult Table()
        {
            ViewData["Title"] = "Покупки";
            return View();

        }
        public PartialViewResult PeriodSelect()
        {
            var periods = getPeriodModel();
            ViewData["OptionName"] = "period";
            return PartialView("PeriodSelect", periods);
        }

        public PartialViewResult WeekSelect()
        {
            var weeks = getWeekModel();
            ViewData["OptionName"] = "week";
            return PartialView("WeekSelect", weeks);
        }

        public PartialViewResult WeekOptions()
        {
            var weeks = getWeekModel();
            ViewData["OptionName"] = "week";
            return PartialView("WeekOptions", weeks);
        }

        public PartialViewResult DaysOfWeekSelect()
        {
            var days = new List<DateTime>();
            if (_repository.CurrentWeek != null)
            {
                var bDate = _repository.CurrentWeek.BDate;
                var eDate = _repository.CurrentWeek.EDate;
                while (bDate != eDate)
                {
                    days.Add(bDate);
                    bDate = bDate.AddDays(1);
                }
                days.Add(eDate);
            }
            
            return PartialView("DaysOfWeekSelect", days);
        }

        public void SetCurrentWeekByBDate(object bDate_)
        {          
            object[] array = bDate_ as object[];
            if ((array[0].ToString() == "-1"))
            {
                _repository.CurrentWeek = null;
                return;
            }

            DateTime bDate = DateTime.Parse(array[0].ToString());
            var week = _repository.SelectWeeksOfCurrentPeriod().Where(w => w.BDate == bDate).FirstOrDefault();
            _repository.CurrentWeek = week;
        }

        public void SetCurrentPeriod(object period_)
        {
            object[] array = period_ as object[];
            DateTime periodMonth = DateTime.Parse(array[0].ToString());
            var period = _repository.SelectAllPeriods().Where(p => p.MonthYear == periodMonth).FirstOrDefault();
            _repository.CurrentPeriod = period;

        }

        public void SetCurrentDay(object dayOfWeek_)
        {
            object[] array = dayOfWeek_ as object[];
            int dayOfWeek = Convert.ToInt32(array[0]);
            //За все дни недели
            if (dayOfWeek == -1)
            {
                _repository.CurrentDay = null;
                return;
            }

            DateTime day = _repository.CurrentWeek.BDate;
            while ((int)day.DayOfWeek != dayOfWeek)
            {
                day = day.AddDays(1);
            }

            _repository.CurrentDay = day;
        }

        public void SetCurrentPurchaseGId(int gId_)
        {
            _repository.CurrentPurchaseGId = gId_;
        }

        /// <summary>
        /// Получить список периодов, где есть покупки
        /// </summary>
        /// <returns></returns>
        private MPeriodList getPeriodModel()
        {
            var periodList = _repository.SelectAllPeriods().OrderByDescending(p => p.MonthYear).ToList();
            if (_repository.CurrentPeriod == null)
                _repository.CurrentPeriod = periodList[0];
            var period = _repository.CurrentPeriod;
            var mPeriods = new MPeriodList()
            {
                Current = period,
                PeriodList = periodList
            };
            return mPeriods;
        }
        /// <summary>
        /// Получить список недель, текущего периода
        /// </summary>
        /// <returns></returns>
        private MWeekList getWeekModel()
        {
            var periodList = _repository.SelectAllPeriods().OrderByDescending(p => p.MonthYear).ToList();
            if (_repository.CurrentPeriod == null)
                _repository.CurrentPeriod = periodList[0];
            var weekList = _repository.SelectWeeksOfCurrentPeriod().ToList();
          
            var week = _repository.CurrentWeek;
            var mWeeks = new MWeekList()
            {
                Current = week,
                WeekList = weekList
            };
            return mWeeks;
        }

        #endregion

        #region Purchases
        /// <summary>
        /// Сумма расходов по группам за текущую выбранный месяц
        /// </summary>
        /// <returns></returns>
        private IQueryable<MPeriodPurchaseSumByGroup> periodPurchSumByGroupTotal2()
        {
            var periodPurchSumList = new List<MPeriodPurchaseSumByGroup>();

            var period = _repository.CurrentPeriod;
            var pp = new Period(period);
            var purchases = _repository.SelectPurchasesByPeriod(period);
            //var pList = purchases.ToList();
            //Суммы затрат по группам за месяц
            var groupSum = (from p in purchases
                            join it in _repository.Item on p.Item_Id equals it.Id
                            join g in _repository.Group on it.GId equals g.Id
                            group p by it.GId into pG
                            select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
                       );
            var gList = groupSum.ToList();
            //результат
            var res =
                (from g in _repository.GroupExt
                 join gSum in groupSum
                 on g.Id equals gSum.GroupId
                 select
                 new
                   MPeriodPurchaseSumByGroup()
                 {
                     Group = g,
                     TimeSpan = pp,
                     Sum = gSum.Sum
                 });
            var list = res.ToList();

            return res;
        }
        private IQueryable<MPeriodPurchaseSumByGroup> periodPurchSumByGroupTotal()
        {
            var period = _repository.CurrentPeriod;
            var pp = new Period(period);
            //var purchases = _repository.SelectPurchasesByPeriod(period);
            //var pList = purchases.ToList();
            //Суммы затрат по группам за месяц
            var groupSum = (from p in  
                                _repository.Purchase
                 
                            join it in _repository.Item on p.Item_Id equals it.Id
                            join g in _repository.Group on it.GId equals g.Id
                            where (p.Date.Month == period.MonthYear.Month) && (p.Date.Year == period.MonthYear.Year)
                            group p by it.GId into pG
                            select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
                       );
            var gList = groupSum.ToList();
            //результат
            var res =
                (from g in _repository.Group
                 join gSum in groupSum
                 on g.Id equals gSum.GroupId
                 select 
                 new
                   MPeriodPurchaseSumByGroup()
                 {
                     Group = g,
                     TimeSpan = pp,
                     Sum = gSum.Sum
                 }
                 );
            
            return res;
        }
        /// <summary>
        /// Сумма расходов по группам за текущую выбранную неделю
        /// </summary>
        /// <returns></returns>
        private IQueryable<MWeekPurchaseSumByGroup> weekPurchSumByGroupTotal()
        {
            var weekPurchSumList = new List<MWeekPurchaseSumByGroup>();

            IWeek week = _repository.CurrentWeek;
            var purchases = _repository.SelectPurchasesByWeek(week);
            //Суммы затрат по группам за неделю
            var groupSum = (from p in purchases
                            join it in _repository.Item on p.Item_Id equals it.Id
                            join g in _repository.Group on it.GId equals g.Id
                            group p by it.GId into pG
                            select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
                       );
            //результат
            var res =
                (from g in _repository.GroupExt
                 join gSum in groupSum
                 on g.Id equals gSum.GroupId
                 select
                 new MWeekPurchaseSumByGroup()
                 {
                     Group = g,
                     Week = week,
                     Sum = gSum.Sum
                 });
            return res;
        }
        private IQueryable<MDayPurchaseSumByGroup> dayPurchSumByGroupTotal()
        {
            var date = _repository.CurrentDay;
            if (date == null)
                throw new Exception("такого не может быть");

            var purchases = _repository.SelectPurchaseByDate(date.Value);
            //Суммы затрат по группам за неделю
            var groupSum = (from p in purchases
                            join it in _repository.Item on p.Item_Id equals it.Id
                            join g in _repository.Group on it.GId equals g.Id
                            group p by it.GId into pG
                            select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
                       );
            //результат
            var res =
                (from g in _repository.GroupExt
                 join gSum in groupSum
                 on g.Id equals gSum.GroupId
                 select
                 new
                   MDayPurchaseSumByGroup()
                 {
                     Group = g,
                     Day = date.Value,
                     Sum = gSum.Sum
                 });
            return res;
        }
        /// <summary>
        /// Вид, отображающий суммы расходов, сгруппированных по группам товаров
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PurchaseSumByGroupTotal()
        {
            if (_repository.CurrentWeek == null)
            {
                IQueryable<MPeriodPurchaseSumByGroup> purchTotal = periodPurchSumByGroupTotal();
                var list = purchTotal.ToList();
                return PartialView("PeriodPurchaseSumByGroupTotal", list);
            }
            else if (_repository.CurrentDay == null)
            {
                IQueryable<MWeekPurchaseSumByGroup> purchTotal = weekPurchSumByGroupTotal();
                var list = purchTotal.ToList();
                return PartialView("WeekPurchaseSumByGroupTotal", list);
            }
            else
            {
                IQueryable<MDayPurchaseSumByGroup> purchTotal = dayPurchSumByGroupTotal();
                var list = purchTotal.ToList();
                return PartialView("DayPurchaseSumByGroupTotal", list);
            }

            
        }

        public PartialViewResult PurchaseItem(int itemId_)
        {
            var item = _repository.Item.Where(it => it.Id == itemId_).FirstOrDefault();

            MItemCard mItem = null;
            if (item != null)
                mItem = new MItemCard(item);
            else
                mItem = new MItemCard();

            return PartialView("PurchaseItem", mItem);
        }

        public PartialViewResult InnerPurchases()
        {
            var purchGId = _repository.CurrentPurchaseGId;
            
            IQueryable<IPurchase> purchases = null;
            if (_repository.CurrentDay != null)
                purchases = _repository.SelectPurchaseByDate(_repository.CurrentDay.Value);
            else if (_repository.CurrentWeek != null)
                purchases = _repository.SelectPurchasesByWeek(_repository.CurrentWeek);
            else
                purchases = _repository.SelectPurchasesByPeriod(_repository.CurrentPeriod);

            var res = purchases.ToList();
            
            //Суммы затрат с группой
            var purchasesDetail = (from p in purchases
                            join it in _repository.Item on p.Item_Id equals it.Id
                            join sh in _repository.Shop on p.Shop_Id equals sh.Id
                            join g in _repository.GroupExt on it.GId equals g.Id
                            where it.GId == purchGId
                            select
                            new MPurchase(p.Id) {
                                Item_Id = p.Item_Id,
                                ItemName = it.Name,
                                Date = p.Date,
                                Price = p.Price,
                                GroupId = it.GId,
                                GroupExtName = g.Name,
                                Shop_Id = p.Shop_Id,
                                ShopName = sh.Name,
                                ShopAddress = sh.Address,
                                Count = p.Count
                            }
                       );

            var purchDetailList = purchasesDetail.ToList();

            return PartialView("PurchaseDetail", purchDetailList);

        }

        public PartialViewResult InnerPurchasesByGId(int gId_)
        {
            var purchGId = gId_;
            ViewData["PurchGId"] = gId_;
            IQueryable<IPurchase> purchases = null;
            if (_repository.CurrentDay != null)
                purchases = _repository.SelectPurchaseByDate(_repository.CurrentDay.Value);
            else if (_repository.CurrentWeek != null)
                purchases = _repository.SelectPurchasesByWeek(_repository.CurrentWeek);
            else
                purchases = _repository.SelectPurchasesByPeriod(_repository.CurrentPeriod);

            var res = purchases.ToList();

            //Суммы затрат с группой
            var purchasesDetail = (from p in purchases
                                   join it in _repository.Item on p.Item_Id equals it.Id
                                   join sh in _repository.Shop on p.Shop_Id equals sh.Id
                                   join g in _repository.GroupExt on it.GId equals g.Id
                                   where it.GId == purchGId
                                   select
                                   new MPurchase(p.Id)
                                   {
                                       Item_Id = p.Item_Id,
                                       ItemName = it.Name,
                                       Date = p.Date,
                                       Price = p.Price,
                                       GroupId = it.GId,
                                       GroupExtName = g.Name,
                                       Shop_Id = p.Shop_Id,
                                       ShopName = sh.Name,
                                       ShopAddress = sh.Address,
                                       Count = p.Count
                                   }
                       );

            var purchDetailList = purchasesDetail.ToList();

            return PartialView("PurchaseDetail", purchDetailList);

        }

        public void SetCurrentPurchaseId(int purchaseId_) => _repository.CurrentPurchaseId = purchaseId_;

        public ViewResult CreatePurchase(int? gId_)
        {
            var mPurch = new MPurchase();
            ViewData["Title"] = "Добавить покупку";
            ViewData["Head"] = "Добавить";
            return View("PurchaseCard", mPurch);
        }
        [HttpPost]
        public ActionResult CreatePurchase(MPurchase purchase_)
        {
            if (ModelState.IsValid)
            {
                var purchase = _repository.AddNewPurchase(
                purchase_.Shop_Id, purchase_.Item_Id,
                purchase_.Price, purchase_.Count, purchase_.Date);
                return RedirectToAction("Table");
            }
            else
                return CreatePurchase(_repository.CurrentPurchaseGId);
        }

        public ViewResult EditPurchase()
        {
            var purchase =
                (from p in _repository.Purchase
                 join it in _repository.Item on p.Item_Id equals it.Id
                 join g in _repository.GroupExt on it.GId equals g.Id
                 join sh in _repository.Shop on p.Shop_Id equals sh.Id
                 where p.Id == _repository.CurrentPurchaseId
                 select new MPurchase(p.Id) {
                     Item_Id = p.Item_Id,
                     ItemName = it.Name,
                     GroupId = it.GId,
                     GroupExtName = g.Name,
                     Shop_Id = p.Shop_Id,
                     ShopName = sh.Name,
                      ShopAddress = sh.Address,
                       Price = p.Price,
                       Count = p.Count,
                        Date = p.Date,
                 }).FirstOrDefault();
                //.Where(p => p.Id == _repository.CurrentPurchaseId).FirstOrDefault();
            

            ViewData["Title"] = "Редактировать покупку";
            ViewData["Head"] = "Редактировать";
            
            return View("PurchaseCard",purchase);
        }


        [HttpPost]
        public ActionResult EditPurchase(int id, int shopId, int itemId, float count, float price, DateTime date)
        {
            _repository.EditPurchase(id, shopId, itemId, price, count, date);
            return RedirectToAction("Table");
        }

        public ActionResult DeletePurchase()
        {
            int? purchId = _repository.CurrentPurchaseId;
            if (purchId.HasValue)
                _repository.DeletePurchase(purchId.Value);
            return RedirectToAction("Table");
            //return RedirectToAction("GroupsAndItems", new { gId_ = item.GId });
        }
        [HttpPost]
        public ActionResult DeletePurchaseAjax()
        {
            int? purchId = _repository.CurrentPurchaseId;
            if (purchId != null)
                _repository.DeletePurchase(purchId.Value);

            return PurchaseSumByGroupTotal();
        }

        #endregion

        private IExpensesRepository _repository = null;
    }
}