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

        //костыль(ибо у ApiController'а должен быть конструктор по-умолчанию)
        private readonly IExpensesRepository _repository = MockBus.Get().MockExpensesRep.Object;

        public PeriodController()
          : this(GlobalConfiguration.Configuration)
        {
        }

        public PeriodController(HttpConfiguration config)
        {
            Configuration = config;
        }

        /// <summary>
        /// Получить все годы, за которые есть покупки
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetYears()
        {
            try
            {
                var apiDescr = Configuration.Services.GetApiExplorer().ApiDescriptions;
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
