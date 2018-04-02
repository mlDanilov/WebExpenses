using System;
using System.Globalization;
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

        public ActionResult List()
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
            if (_repository.PurchaseRep.CurrentWeek != null)
            {
                var bDate = _repository.PurchaseRep.CurrentWeek.BDate;
                var eDate = _repository.PurchaseRep.CurrentWeek.EDate;
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
                _repository.PurchaseRep.CurrentWeek = null;
                return;
            }

            DateTime bDate = DateTime.Parse(array[0].ToString());

            var period = _repository.PurchaseRep.CurrentPeriod;
            var week = _repository.PurchaseRep.SelectWeeksByPeriod(period).Where(w => w.BDate == bDate).FirstOrDefault();

            _repository.PurchaseRep.CurrentWeek = week;
        }

        public void SetCurrentPeriod(object period_)
        {
            object[] array = period_ as object[];
            DateTime periodMonth = DateTime.Parse(array[0].ToString());
            var period = _repository.PurchaseRep.SelectAllPeriods().Where(p => p.MonthYear == periodMonth).FirstOrDefault();
            _repository.PurchaseRep.CurrentPeriod = period;

        }

        public void SetCurrentDay(object dayOfWeek_)
        {
            object[] array = dayOfWeek_ as object[];
            int dayOfWeek = Convert.ToInt32(array[0]);
            //За все дни недели
            if (dayOfWeek == -1)
            {
                _repository.PurchaseRep.CurrentDay = null;
                return;
            }

            DateTime day = _repository.PurchaseRep.CurrentWeek.BDate;
            while ((int)day.DayOfWeek != dayOfWeek)
            {
                day = day.AddDays(1);
            }

            _repository.PurchaseRep.CurrentDay = day;
        }

        public void SetCurrentPurchaseGId(int gId_) =>_repository.PurchaseRep.CurrentPurchaseGId = gId_;

        /// <summary>
        /// Получить список периодов, где есть покупки
        /// </summary>
        /// <returns></returns>
        private MPeriodList getPeriodModel()
        {
            // Получить все периоды(месяц-год) в которых есть покупки
            var periodList = getAllPeriods();
            //Установить первый в списке период, если такой не установлен
            if (_repository.PurchaseRep.CurrentPeriod == null)
                _repository.PurchaseRep.CurrentPeriod = periodList[0];

            var period = _repository.PurchaseRep.CurrentPeriod;
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
            var periodList = _repository.PurchaseRep.SelectAllPeriods().OrderByDescending(p => p.MonthYear).ToList();
            if (_repository.PurchaseRep.CurrentPeriod == null)
                _repository.PurchaseRep.CurrentPeriod = periodList[0];

            var week = _repository.PurchaseRep.CurrentWeek;
            var mWeeks = new MWeekList()
            {
                Current = week,
                //WeekList = weekList
            };
            _repository.PurchaseRep.SelectWeeksByPeriod(_repository.PurchaseRep.CurrentPeriod)
                .ToList().ForEach(w => mWeeks.WeekList.Add(w));

            return mWeeks;
        }


        /// <summary>
        /// Получить все периоды(месяц-год) в которых есть покупки
        /// </summary>
        /// <returns></returns>
        private List<IPeriod> getAllPeriods()
        {
            var periodList = _repository.PurchaseRep.SelectAllPeriods().OrderByDescending(p => p.MonthYear).ToList();
            var pList = new List<IPeriod>();

            periodList.ForEach(p => pList.Add(p));
            return pList;
        }

        /// <summary>
        /// Сумма расходов по группам за текущую выбранный месяц
        /// </summary>
        /// <returns></returns>
        private IQueryable<MPeriodPurchaseSumByGroup> periodPurchSumByGroupTotal2()
        {
            var periodPurchSumList = new List<MPeriodPurchaseSumByGroup>();

            var period = _repository.PurchaseRep.CurrentPeriod;
            var pp = new Period(period);
            var purchases = _repository.PurchaseRep.SelectPurchasesByPeriod(period);

            //Суммы затрат по группам за месяц
            var groupSum = (from p in purchases
                            join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
                            join g in _repository.GroupRep.Entities on it.GId equals g.Id
                            group p by it.GId into pG
                            select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
                       );
            var gList = groupSum.ToList();
            //результат
            var res =
                (from g in _repository.GroupRep.GroupExt
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
            var period = _repository.PurchaseRep.CurrentPeriod;
            var pp = new Period(period);
            //var purchases = _repository.SelectPurchasesByPeriod(period);
            //var pList = purchases.ToList();
            //Суммы затрат по группам за месяц
            var groupSum = (from p in  _repository.PurchaseRep.Entities
                            join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
                            join g in _repository.GroupRep.Entities on it.GId equals g.Id
                            where (p.Date.Month == period.MonthYear.Month) && (p.Date.Year == period.MonthYear.Year)
                            group p by it.GId into pG
                            select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
                       );
            var gList = groupSum.ToList();
            //результат
            var res =
                (from g in _repository.GroupRep.Entities
                 join gSum in groupSum
                 on g.Id equals gSum.GroupId
                 select 
                 new
                   MPeriodPurchaseSumByGroup()
                 {
                     Group = g,
                     //TimeSpan = pp,
                     Sum = gSum.Sum
                 }
                 );
            var list = res.ToList();
            list.ForEach(mp => mp.TimeSpan = pp);


            return res;
        }
        /// <summary>
        /// Сумма расходов по группам за текущую выбранную неделю
        /// </summary>
        /// <returns></returns>
        private IQueryable<MWeekPurchaseSumByGroup> weekPurchSumByGroupTotal()
        {
            var weekPurchSumList = new List<MWeekPurchaseSumByGroup>();

            IWeek week = _repository.PurchaseRep.CurrentWeek;
            var ww = new Week() { BDate = week.BDate, EDate = week.EDate };
            var purchases = _repository.PurchaseRep.SelectPurchasesByWeek(week);
            //Суммы затрат по группам за неделю
            var groupSum = (from p in purchases
                            join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
                            join g in _repository.GroupRep.Entities on it.GId equals g.Id
                            group p by it.GId into pG
                            select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
                       );

            //var t = purchases.ToList();
            //результат
            var res =
                (
                 //from g in _repository.GroupRep.GroupExt
                 from g in _repository.GroupRep.Entities
                 join gSum in groupSum
                 on g.Id equals gSum.GroupId
                 select
                 new MWeekPurchaseSumByGroup()
                 {
                     Group = g,
                     //Week = week,
                     Sum = gSum.Sum
                 });
            var list = res.ToList();
            list.ForEach(mp => mp.Week = ww);


            return res;
        }
        private IQueryable<MDayPurchaseSumByGroup> dayPurchSumByGroupTotal()
        {
            var date = _repository.PurchaseRep.CurrentDay;
            if (date == null)
                throw new Exception("такого не может быть");

            var purchases = _repository.PurchaseRep.SelectPurchaseByDate(date.Value);
            //Суммы затрат по группам за неделю
            var groupSum = (from p in purchases
                            join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
                            join g in _repository.GroupRep.Entities on it.GId equals g.Id
                            group p by it.GId into pG
                            select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
                       );
            //результат
            var res =
                (
                //from g in _repository.GroupRep.GroupExt
                from g in _repository.GroupRep.Entities
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
            if (_repository.PurchaseRep.CurrentDay != null)
            {
                IQueryable<MDayPurchaseSumByGroup> purchTotal = dayPurchSumByGroupTotal();
                var list = purchTotal.ToList();
                return PartialView("DayPurchaseSumByGroupTotal", list);
            }
            else if (_repository.PurchaseRep.CurrentWeek != null)
            {
                IQueryable<MWeekPurchaseSumByGroup> purchTotal = weekPurchSumByGroupTotal();
                var list = purchTotal.ToList();
                return PartialView("WeekPurchaseSumByGroupTotal", list);
            }
            else
            {
                IQueryable<MPeriodPurchaseSumByGroup> purchTotal = periodPurchSumByGroupTotal();
                var list = purchTotal.ToList();
                return PartialView("PeriodPurchaseSumByGroupTotal", list);
            }

            
        }

        public JsonResult PurchaseTotalSum()
        {
            IQueryable<Purchase> purchases = null;
            if (_repository.PurchaseRep.CurrentDay != null)
            {
                var day = _repository.PurchaseRep.CurrentDay;
                purchases = _repository.PurchaseRep.SelectPurchaseByDate(day.Value);
            }
            else if (_repository.PurchaseRep.CurrentWeek != null)
            {
                var week = _repository.PurchaseRep.CurrentWeek;
                purchases = _repository.PurchaseRep.SelectPurchasesByWeek(week);
            }
            else
            {
                var period = _repository.PurchaseRep.CurrentPeriod;
                purchases = _repository.PurchaseRep.SelectPurchasesByPeriod(period);
            }
            var sum = purchases.Sum(p => p.Count * p.Price).ToString("### ##0.00");


            return Json(sum, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult PurchaseItem(int itemId_)
        {
            var item = _repository.ItemRep.Entities.Where(it => it.Id == itemId_).FirstOrDefault();

            MItemCard mItem = null;
            if (item != null)
                mItem = new MItemCard(item);
            else
                mItem = new MItemCard();

            return PartialView("PurchaseItem", mItem);
        }

        public PartialViewResult InnerPurchases()
        {
            var purchGId = _repository.PurchaseRep.CurrentPurchaseGId;
            
            IQueryable<IPurchase> purchases = null;
            if (_repository.PurchaseRep.CurrentDay != null)
                purchases = _repository.PurchaseRep.SelectPurchaseByDate(_repository.PurchaseRep.CurrentDay.Value);
            else if (_repository.PurchaseRep.CurrentWeek != null)
                purchases = _repository.PurchaseRep.SelectPurchasesByWeek(_repository.PurchaseRep.CurrentWeek);
            else
                purchases = _repository.PurchaseRep.SelectPurchasesByPeriod(_repository.PurchaseRep.CurrentPeriod);

            var res = purchases.ToList();
            
            //Суммы затрат с группой
            var purchasesDetail = (from p in purchases
                            join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
                            join sh in _repository.ShopRep.Entities on p.Shop_Id equals sh.Id into p_sh
                            from pSh in p_sh.DefaultIfEmpty()
                            join g in _repository.GroupRep.GroupExt on it.GId equals g.Id
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
                                ShopName = (pSh == null) ? string.Empty : pSh.Name,
                                ShopAddress = (pSh == null) ? string.Empty : pSh.Address,
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
            if (_repository.PurchaseRep.CurrentDay != null)
                purchases = _repository.PurchaseRep.SelectPurchaseByDate(_repository.PurchaseRep.CurrentDay.Value);
            else if (_repository.PurchaseRep.CurrentWeek != null)
                purchases = _repository.PurchaseRep.SelectPurchasesByWeek(_repository.PurchaseRep.CurrentWeek);
            else
                purchases = _repository.PurchaseRep.SelectPurchasesByPeriod(_repository.PurchaseRep.CurrentPeriod);

            var res = purchases.ToList();

            //Суммы затрат с группой
            var purchasesDetail = (from p in purchases
                                   join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
                                   join sh in _repository.ShopRep.Entities on p.Shop_Id equals sh.Id into p_sh
                                   from pSh in p_sh.DefaultIfEmpty()
                                       //join g in _repository.GroupRep.GroupExt on it.GId equals g.Id
                                   join g in _repository.GroupRep.Entities on it.GId equals g.Id
                                   where it.GId == purchGId
                                   orderby p.Date descending
                                   select
                                   new MPurchase()
                                   {
                                       Id = p.Id,
                                       Item_Id = p.Item_Id,
                                       ItemName = it.Name,
                                       Date = p.Date,
                                       Price = p.Price,
                                       GroupId = it.GId,
                                       GroupExtName = g.Name,
                                       Shop_Id = p.Shop_Id,
                                       ShopName = (pSh == null) ? string.Empty : pSh.Name,
                                       ShopAddress = (pSh == null) ? string.Empty : pSh.Address,
                                       Count = p.Count
                                   }
                       );

            var purchDetailList = purchasesDetail.ToList();

            return PartialView("PurchaseDetail", purchDetailList);

        }

        public PartialViewResult InnerPurchasesByGId_сrutchVersion(int gId_)
        {
            var purchGId = gId_;
            ViewData["PurchGId"] = gId_;
            IQueryable<IPurchase> purchasesTemp = null;
            if (_repository.PurchaseRep.CurrentDay != null)
                purchasesTemp = _repository.PurchaseRep.SelectPurchaseByDate(_repository.PurchaseRep.CurrentDay.Value);
            else if (_repository.PurchaseRep.CurrentWeek != null)
                purchasesTemp = _repository.PurchaseRep.SelectPurchasesByWeek(_repository.PurchaseRep.CurrentWeek);
            else
                purchasesTemp = _repository.PurchaseRep.SelectPurchasesByPeriod(_repository.PurchaseRep.CurrentPeriod);

            var res = purchasesTemp.ToList();

            //Суммы затрат с группой
            var purchasesDetail = (from p in purchasesTemp
                                   join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
                                   join sh in _repository.ShopRep.Entities on p.Shop_Id equals sh.Id into p_sh
                                   from pSh in p_sh.DefaultIfEmpty()
                                   //join g in _repository.Group on it.GId equals g.Id
                                   join g in _repository.GroupRep.GroupExt on it.GId equals g.Id
                                   where it.GId == purchGId
                                   orderby p.Date descending
                                   select
                                   new MPurchase()
                                   {
                                       Id = p.Id,
                                       Item_Id = p.Item_Id,
                                       ItemName = it.Name,
                                       Date = p.Date,
                                       Price = p.Price,
                                       GroupId = it.GId,
                                       GroupExtName = g.Name,
                                       Shop_Id = p.Shop_Id,
                                      // ShopName = sh.Name,
                                      // ShopAddress = sh.Address,
                                       ShopName = (p_sh == null) ? string.Empty : pSh.Name,
                                       ShopAddress = (p_sh == null) ? string.Empty : pSh.Address,
                                       Count = p.Count
                                   }
                       );

            var purchDetailList = purchasesDetail.ToList();

            return PartialView("PurchaseDetail", purchDetailList);

        }

        public void SetCurrentPurchaseId(int purchaseId_) => _repository.PurchaseRep.CurrentPurchaseId = purchaseId_;

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
                if (purchase_.Shop_Id == -1) purchase_.Shop_Id = null;
                var purchase = _repository.PurchaseRep.Create(purchase_);
                return RedirectToAction("List");
            }
            else
                return CreatePurchase(_repository.PurchaseRep.CurrentPurchaseGId);
        }

        public ViewResult EditPurchase()
        {
            var purchase =
                (from p in _repository.PurchaseRep.Entities
                 join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
                 join g in _repository.GroupRep.Entities on it.GId equals g.Id
                 join sh in _repository.ShopRep.Entities on p.Shop_Id equals sh.Id into p_sh
                 from pSh in p_sh.DefaultIfEmpty()
                 where p.Id == _repository.PurchaseRep.CurrentPurchaseId
                 select new MPurchase()
                 {
                     Id = p.Id,
                     Item_Id = p.Item_Id,
                     ItemName = it.Name,
                     GroupId = it.GId,
                     GroupExtName = g.Name,
                     Shop_Id = p.Shop_Id,
                     ShopName = (pSh == null) ? string.Empty : pSh.Name,
                     ShopAddress = (pSh == null) ? string.Empty : pSh.Address,
                     Price = p.Price,
                     Count = p.Count,
                     Date = p.Date,
                 }).FirstOrDefault();
                //.Where(p => p.Id == _repository.CurrentPurchaseId).FirstOrDefault();
            

            ViewData["Title"] = "Редактировать покупку";
            ViewData["Head"] = "Редактировать";
            
            return View("PurchaseCard",purchase);
        }

        public JsonResult ValidateDate(string Date)
        {
            DateTime parsedDate;
            if (!DateTime.TryParse(Date, out parsedDate))
            {
                return Json($"Введите допустимое значение даты", JsonRequestBehavior.AllowGet);
            }
            else if (DateTime.Now <= parsedDate)
            {
                return Json($"Дата покупки должна быть раньше чем {DateTime.Now}", JsonRequestBehavior.AllowGet);
            }
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult EditPurchase(MPurchase purchase_)
        {
            if (ModelState.IsValid)
            {
                _repository.PurchaseRep.Update(purchase_);
                return RedirectToAction("List");
            }
            else
                return CreatePurchase(_repository.PurchaseRep.CurrentPurchaseGId);

        }

        public ActionResult DeletePurchase()
        {
            int? purchId = _repository.PurchaseRep.CurrentPurchaseId;
            if (purchId.HasValue)
                _repository.PurchaseRep.Delete(getPurchaseById(purchId));
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult DeletePurchaseAjax()
        {
            int? purchId = _repository.PurchaseRep.CurrentPurchaseId;
            if (purchId != null)
                _repository.PurchaseRep.Delete(getPurchaseById(purchId));

            return PurchaseSumByGroupTotal();
        }
        private Purchase getPurchaseById(int? purchId_)
        {
            var purch = _repository.PurchaseRep.Entities.Where(p => p.Id == purchId_).FirstOrDefault();
            return purch;
        }


        private IExpensesRepository _repository = null;
    }
}