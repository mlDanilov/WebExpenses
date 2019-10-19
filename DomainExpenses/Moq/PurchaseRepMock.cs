using System;
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
    /// <summary>
    /// Мок-синглтон IPurchaseRepository
    /// </summary>
    public class PurchaseRepMock
    {
        private PurchaseRepMock()
        {
            setPurchaseBehavior();
        }
        /// <summary>
        /// Задать поведение моку
        /// </summary>
        private void setPurchaseBehavior()
        {
            var fBus = EntitiesFactory.Get();
            //= new Mock<IExpensesRepository>();

            //Список покупок
            _purchRepMock.Setup(m => m.Entities).Returns(_purchaseList.AsQueryable());
           
            //Добавить новую покупку
            _purchRepMock.Setup<Purchase>(m => m.Create(
                It.IsAny<IPurchase>())).Returns(
                (IPurchase purchase_) =>
            {
                var purchase = fBus.CreatePurchase(
                    _purchaseList.Max(p => p.Id) + 1,
                    purchase_.Shop_Id, purchase_.Item_Id, purchase_.Price, purchase_.Count, purchase_.Date);

                _purchaseList.Add(purchase);
                _purchRepMock.Setup(m => m.Entities).Returns(_purchaseList.AsQueryable());
                return purchase;
            });
            //Редактировать существующую покупку
            _purchRepMock.Setup(m => m.Update(
                It.IsAny<IPurchase>())).Callback(
                 (IPurchase purchase_) =>
                 {
                     var purchase = _purchaseList.Where(p => p.Id == purchase_.Id).First();
                     if (purchase == null)
                         return;
                     purchase.Item_Id = purchase_.Item_Id;
                     purchase.Shop_Id = purchase_.Shop_Id;
                     purchase.Price = purchase_.Price;
                     purchase.Count = purchase_.Count;
                     purchase.Date = purchase_.Date;
                 });
            //Удалить покупку
            _purchRepMock.Setup(m => m.Delete(It.IsAny<IPurchase>()))
               .Callback(
               (IPurchase purchase_) =>
               {
                   _purchaseList.RemoveAll(p => p.Id == purchase_.Id);
                   _purchRepMock.Setup(m => m.Entities).Returns(_purchaseList.AsQueryable());
               });

           
            
            //Получить все покупки за день
            _purchRepMock.Setup(m => m.SelectPurchaseByBeginAndEndDates(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(
                (DateTime bDate_, DateTime eDate_) => { return SelectPurchasesByBeginAndEndDates(bDate_, eDate_); });

            //Вернуть все доступные годы покупок
            _purchRepMock.Setup(m => m.SelectAllYears()).Returns(() => SelectAllYears());

           

        }

        /// <summary>
        /// Получить покупки по начальной и конечной дате
        /// </summary>
        /// <param name="bDate_"></param>
        /// <param name="eDate_"></param>
        /// <returns></returns>
        public IQueryable<Purchase> SelectPurchasesByBeginAndEndDates(DateTime bDate_, DateTime eDate_)
        {
            var purchases =
                _purchaseList.Where(p => (p.Date >= bDate_ && p.Date <= eDate_)).AsQueryable();
            return purchases;
        }


        /// <summary>
        /// Получить все годы, за которые есть покупи
        /// </summary>
        /// <returns></returns>
        public IQueryable<int> SelectAllYears() {
            HashSet<int> years = new HashSet<int>();

            _purchaseList.ForEach(p =>
            {
                int year = p.Date.Year;
                if (!years.Contains(year))
                    years.Add(year);

            });

            return years.AsQueryable();
        }

        /// <summary>
        /// Список покупок
        /// </summary>
        private List<Purchase> _purchaseList = new List<Purchase>
        {
            //Декабрь 2018 1 неделя
            EntitiesFactory.Get().CreatePurchase(0, null, 2, 10, 5, new DateTime(2018, 1, 1)),
            EntitiesFactory.Get().CreatePurchase(1, 2, 7, 20, 7, new DateTime(2018, 1, 1)),
            EntitiesFactory.Get().CreatePurchase(2, 2, 5, 15, 1, new DateTime(2018, 1, 2)),
            EntitiesFactory.Get().CreatePurchase(3, null, 3, 54, 1, new DateTime(2018, 1, 2)),
            EntitiesFactory.Get().CreatePurchase(4, 3, 1, 20, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(5, 1, 8, 10, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(6, null, 9, 8,  6, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(7, 2, 5, 61, 2, new DateTime(2018, 1, 3)),
            EntitiesFactory.Get().CreatePurchase(8, 1, 6, 48, 3, new DateTime(2018, 1, 5)),
            EntitiesFactory.Get().CreatePurchase(9, null, 7, 48, 3, new DateTime(2018, 1, 5)),


            //Ноябрь 2017 1 неделя
            EntitiesFactory.Get().CreatePurchase(10, 4, 7, 33, 5, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchase(11, null, 8, 20, 7, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchase(12, 3, 8, 27, 1, new DateTime(2017, 11, 1)),
            EntitiesFactory.Get().CreatePurchase(13, null, 9, 73, 1, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchase(14, 2, 2, 23, 2, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchase(15, null, 1, 56, 2, new DateTime(2017, 11, 3)),
            EntitiesFactory.Get().CreatePurchase(16, 4, 3, 43,  6, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchase(17, 1, 4, 61, 2, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchase(18, null, 5, 21, 3, new DateTime(2017, 11, 4)),
            EntitiesFactory.Get().CreatePurchase(19, 3, 5, 55, 3, new DateTime(2018, 11, 4)),

            //Ноябрь 2017 2-3 недели
             EntitiesFactory.Get().CreatePurchase(20, null, 7, 45, 3, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchase(21, 1, 8, 43, 4, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchase(22, 3, 8, 28, 8, new DateTime(2017, 11, 16)),
            EntitiesFactory.Get().CreatePurchase(23, null, 9, 12, 9, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchase(24, 2, 2, 76, 4, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchase(25, 2, 1,20, 5, new DateTime(2017, 11, 18)),
            EntitiesFactory.Get().CreatePurchase(26, null, 3, 65,  7, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchase(27, 4, 4, 36, 3, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchase(28, 2, 5, 87, 3, new DateTime(2017, 11, 19)),
            EntitiesFactory.Get().CreatePurchase(29, null, 5, 29, 1, new DateTime(2018, 11, 19)),

            //Октябрь 2019 1 неделя
             EntitiesFactory.Get().CreatePurchase(31, 1, 8, 85, 12, new DateTime(2019, 10, 1)),
             EntitiesFactory.Get().CreatePurchase(32, null, 4, 45, 4, new DateTime(2019, 10, 1)),
             EntitiesFactory.Get().CreatePurchase(33, 3, 8, 32, 8, new DateTime(2019, 10, 1)),
             EntitiesFactory.Get().CreatePurchase(34, 3, 1, 73, 54, new DateTime(2019, 10, 2)),
             EntitiesFactory.Get().CreatePurchase(35, 4, 12, 54, 2, new DateTime(2019, 10, 4)),
             EntitiesFactory.Get().CreatePurchase(36, 2, 12, 62, 5, new DateTime(2019, 10, 5)),
             EntitiesFactory.Get().CreatePurchase(37, null, 4, 74, 45, new DateTime(2019, 10, 3)),
             EntitiesFactory.Get().CreatePurchase(38, 3, 7, 45, 7, new DateTime(2019, 10, 3)),
             EntitiesFactory.Get().CreatePurchase(39, null, 5, 18, 9, new DateTime(2019, 10, 4)),
             EntitiesFactory.Get().CreatePurchase(40, 1, 8, 46, 12, new DateTime(2019, 10, 5)),
        };

        public Mock<IPurchaseRepository> Mock
        {
            get
            {
                return _purchRepMock;
            }
        }
        private Mock<IPurchaseRepository> _purchRepMock = new Mock<IPurchaseRepository>();

        public static PurchaseRepMock Get()
        {
            if (_instance == null)
                _instance = new PurchaseRepMock();
            return _instance;
        }
        private static PurchaseRepMock _instance = new PurchaseRepMock();
    }
}
