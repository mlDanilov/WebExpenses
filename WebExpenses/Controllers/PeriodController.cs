using DomainExpenses.Abstract;
using DomainExpenses.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebExpenses.Controllers
{
    public class PeriodController : ApiController
    {

        //private readonly IExpensesRepository _repository = new DomainExpenses.Concrete.ExpensesRepository();
        private readonly IExpensesRepository _repository = MockBus.Get().MockExpensesRep.Object;

        public PeriodController()
        {

        }
        //public PeriodController(IExpensesRepository rep_)
        //{
        //    _repository = rep_;
        //}
        /// <summary>
        /// Получить все годы, за которые есть покупки
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetYears()
        {
            try
            {
                var years = _repository.PurchaseRep.SelectAllYears().ToArray();
                return Request.CreateResponse(HttpStatusCode.OK, years);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        public HttpResponseMessage GetMonthesByYear([FromBody] int year_)
        {
            try
            {
                var years = _repository.PurchaseRep.SelectAllYears().ToArray();
                return Request.CreateResponse(HttpStatusCode.OK, years);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

    }
}
