using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainExpenses;
using DomainExpenses.Abstract;

namespace WebExpenses.Controllers
{


    public class ExpensesController : Controller
    {
        public ExpensesController(IExpensesRepository rep_)
        {
            _repository = rep_;
        }

        public ViewResult Items()
        {
            var purchase = _repository.Purchase.ToList();

            return View(_repository.Item);
        }

        private IExpensesRepository _repository = null;
    }
}