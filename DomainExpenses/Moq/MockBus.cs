using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

using Moq;
using DomainExpenses.Abstract;
using DomainExpenses.Abstract.Repositories;
using DomainExpenses.Concrete;

namespace DomainExpenses.Moq
{
    public class MockBus
    {
        private MockBus()
        {
            defineMockDbContext();
        }
        /// <summary>
        /// Создать Moq с товарами/группами/магазинами
        /// </summary>
        private void defineMockDbContext()
        {
            var fBus = EntitiesFactory.Get();

            MockExpensesRep = new Mock<IExpensesRepository>();
            //репозиторий товаров
            MockExpensesRep.Setup(m => m.ItemRep).Returns(ItemRepMock.Get().Mock.Object);
            //репозиторий групп товаров
            MockExpensesRep.Setup(m => m.GroupRep).Returns(GroupRepMock.Get().GpRepMock.Object);
            //репозиторий магазинов
            MockExpensesRep.Setup(m => m.ShopRep).Returns(ShopRepMock.Get().Mock.Object);
            //репозиторий покупок
            MockExpensesRep.Setup(m => m.PurchaseRep).Returns(PurchaseRepMock.Get().Mock.Object);
        }
        

        public static MockBus Get()
        {
            if (_mBus == null)
                _mBus = new MockBus();
            return _mBus;

        }

        

        #region eintity lists
        

        


        #endregion


       

        public Mock<IExpensesRepository> MockExpensesRep { get; private set; }

        private static MockBus _mBus;
            
    }
}
