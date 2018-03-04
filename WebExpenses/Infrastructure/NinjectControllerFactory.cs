using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using DomainExpenses.Abstract;
using DomainExpenses.Concrete;
using DomainExpenses.Moq;

using Ninject;

namespace WebExpenses.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        public NinjectControllerFactory()
        {
            _ninjectKernel = new StandardKernel();
            addBindings();
        }

        protected override IController GetControllerInstance(
            RequestContext requestContext_,
            Type controllerType_)
        {
            return (controllerType_ == null) ? null : (IController)_ninjectKernel.Get(controllerType_);
        }


        private void addBindings()
        {
            //_ninjectKernel.Bind<IExpensesRepository>().To<ExpensesRepository>();
            //_ninjectKernel.Bind<IExpensesRepository>().To<ExpensesRepository>().InSingletonScope();
            _ninjectKernel.Bind<IExpensesRepository>().ToConstant(MockBus.Get().MockDbContext.Object);
        }




        private IKernel _ninjectKernel;
    }
}