using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainExpenses.Abstract;

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
            return PartialView("PeriodSelect", periods);
        }

        private MPeriodList getPeriodModel()
        {
            var periodList = _repository.SelectAllPeriods.ToList();
            var mPeriods = new MPeriodList()
            {
                Current = periodList[0],
                PeriodList = periodList
            };
            return mPeriods;
        }

        private MPeriodList getWeekModel()
        {
            var periodList = _repository.SelectAllPeriods.ToList();
            var mPeriods = new MPeriodList()
            {
                Current = periodList[0],
                PeriodList = periodList
            };
            return mPeriods;
        }





        private IExpensesRepository _repository = null;
    }
}