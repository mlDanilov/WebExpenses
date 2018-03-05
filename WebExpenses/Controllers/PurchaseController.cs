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
            DateTime bDate = _repository.CurrentWeek.BDate;
            DateTime eDate = _repository.CurrentWeek.EDate;
            List<DateTime> days = new List<DateTime>();

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

        public void SetCurrentDay(int dayOfWeek_)
        {
            //За все дни недели
            if (dayOfWeek_ == 0)
                _repository.CurrentDay = null;

            DateTime day = _repository.CurrentWeek.BDate;
            while ((int)day.DayOfWeek != dayOfWeek_)
            {
                day = day.AddDays(1);
            }

            //_repository.CurrentWeek
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

        private IExpensesRepository _repository = null;
    }
}