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
            var bDate = _repository.CurrentWeek.BDate;
            var eDate = _repository.CurrentWeek.EDate;
            var days = new List<DateTime>();

            while (bDate != eDate)
            {
                days.Add(bDate);
                bDate = bDate.AddDays(1);
            }
            days.Add(eDate);
            return PartialView("DaysOfWeekSelect", days);
        }

        public void SetCurrentWeekByBDate(object bDate_)
        {
            object[] array = bDate_ as object[];
            DateTime bDate = DateTime.Parse(array[0].ToString());
            var week = _repository.SelectWeeksOfCurrentPeriod().Where(w => w.BDate == bDate).FirstOrDefault();
            _repository.CurrentWeek = week;
        }

        public void SetCurrentPeriod(object period_)
        {
            object[] array = period_ as object[];
            DateTime periodMonth = DateTime.Parse(array[0].ToString());
            var period = _repository.SelectAllPeriods().Where(p => p.Period == periodMonth).FirstOrDefault();
            _repository.CurrentPeriod = period;

        }

        public void SetCurrentDay(object dayOfWeek_)
        {
            object[] array = dayOfWeek_ as object[];
            int dayOfWeek = Convert.ToInt32(array[0]);
            //За все дни недели
            if (dayOfWeek == 0)
                _repository.CurrentDay = null;

            DateTime day = _repository.CurrentWeek.BDate;
            while ((int)day.DayOfWeek != dayOfWeek)
            {
                day = day.AddDays(1);
            }

            _repository.CurrentDay = day;
        }

        /// <summary>
        /// Получить список периодов, где есть покупки
        /// </summary>
        /// <returns></returns>
        private MPeriodList getPeriodModel()
        {
            var periodList = _repository.SelectAllPeriods().OrderByDescending(p => p.Period).ToList();
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
            var periodList = _repository.SelectAllPeriods().OrderByDescending(p => p.Period).ToList();
            if (_repository.CurrentPeriod == null)
                _repository.CurrentPeriod = periodList[0];
            var weekList = _repository.SelectWeeksOfCurrentPeriod().ToList();
            if (_repository.CurrentWeek == null)
                _repository.CurrentWeek = weekList[0];
            var week = _repository.CurrentWeek;
            var mWeeks = new MWeekList()
            {
                Current = week,
                WeekList = weekList
            };
            return mWeeks;
        }

        #endregion


        /// <summary>
        /// Вид, отображающий суммы расходов, сгруппированных по группам товаров
        /// </summary>
        /// <returns></returns>
        public PartialViewResult WeekPurchaseSumByGroupTotal()
        {
            var weekPurchSumList = new List<MWeekPurchaseSumByGroup>();

            IWeek week = _repository.CurrentWeek;
            var purchases = _repository.SelectPurchaseByWeek(week);

            var res =
                (from g in _repository.GroupExt
                 join gSum in
                (from p in purchases
                 join it in _repository.Item on p.Item_Id equals it.Id
                 join g in _repository.Group on it.GId equals g.Id
                 group p by it.GId into pG
                 select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
                       ) on g.Id equals gSum.GroupId
                 select new { GroupId = gSum.GroupId, GroupName = g.Name, Sum = gSum.Sum });
            /*
            var res = (from p in purchases
                           join it in _repository.Item on p.Item_Id equals it.Id
                           join g in _repository.Group on it.GId equals g.Id
                       select p
                      );
            */
            var list = res.ToList();
            foreach (var p in list)
            {

            }
            return PartialView("WeekPurchaseSumByGroupTotal");
        }

        private IExpensesRepository _repository = null;
    }
}