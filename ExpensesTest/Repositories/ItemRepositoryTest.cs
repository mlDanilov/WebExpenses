using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

using DomainExpenses.Abstract.Repositories;
using DomainExpenses.Moq;
using WebExpenses.Controllers;
using WebExpenses.Models.Item;
using WebExpenses.Models.Item.Interfaces;


namespace ExpensesTest.Repositories
{
    [TestClass]
    public class ItemRepositoryTest
    {
        /// <summary>
        /// Проверка на добавление товара с валидными значениями
        /// </summary>
        [TestMethod]
        public void TryCreateValidItem()
        {
            var expRep = MockBus.Get().MockExpensesRep.Object;

            var gId = expRep.GroupRep.Entities.First().Id;
            var name = "тестовый";

            //arrange
            var itController = new ItemController(expRep);
            
            var itCard = new MItemCard()
            { GId = gId, Name = name };
            int defaultId = itCard.Id;

            var countBefore = expRep.ItemRep.Entities.Count();
            //Act
            var  res = itController.CreateItemCard(itCard);
            
            var countAfter = expRep.ItemRep.Entities.Count();

            //Assert
            Assert.AreEqual(countBefore + 1, countAfter, "Количество товаров в репозитории не изменилось");

            var itNew = expRep.ItemRep.Entities.Where(it => (it.Name == name) && (it.GId == gId)).FirstOrDefault();

            Assert.IsNotNull(itNew, "Не найден товар по имени и группе");
            Assert.AreNotEqual(defaultId, itNew.Id, "У нового товара не изменился Id");
            Assert.AreEqual(name, itNew.Name, "У нового товара неправильное имя");
            Assert.AreEqual(gId, itNew.GId, "У нового товара код группы");
        }
        /// <summary>
        /// Попытка добавить невалидный товар
        /// </summary>
        [TestMethod]
        public void TryCreateNotValidItem()
        {
            var expRep = MockBus.Get().MockExpensesRep.Object;

            var gId = expRep.GroupRep.Entities.First().Id;
            var name = "";

            //arrange
            var itController = new ItemController(expRep);

            var itCard = new MItemCard()
            { GId = gId, Name = name };
            int defaultId = itCard.Id;

            var countBefore = expRep.ItemRep.Entities.Count();
            validation(itCard, itController);
            //Act
            var result = itController.CreateItemCard(itCard);
            var countAfter = expRep.ItemRep.Entities.Count();

            //Assert
            Assert.AreEqual(countBefore, countAfter, "Количество товаров в репозитории изменилось");
            Assert.IsTrue(result is ViewResult, "Метод, не прошедший валидацию возвращает не возвращает объект типа 'ViewResult'");
            var vResult = result as ViewResult;
            Assert.IsTrue(vResult.Model is IMItemCard, "Модель, передаваемая в вид не является объектом типа 'MItemCard'");

            var mItCrdModel = vResult.Model as IMItemCard;
            Assert.IsTrue(mItCrdModel.Id == -1, "Значение свойства Id в модели имеет неверное значение");
            Assert.IsTrue(mItCrdModel.GId == gId, "Значение свойства GId в модели имеет неверное значение");
            Assert.IsTrue(mItCrdModel.Name == null, "Значение свойства Name в модели имеет неверное значение");


            var itNew = expRep.ItemRep.Entities.Where(it => (it.Name == name) && (it.GId == gId)).FirstOrDefault();

            Assert.IsNull(itNew, "Товар добавился");
        }

        [TestMethod]
        public void TryDeleteItem()
        {
            var expRep = MockBus.Get().MockExpensesRep.Object;

            //arrange
            var itController = new ItemController(expRep);

            var it = MockBus.Get().MockExpensesRep.Object.ItemRep.Entities.First();
            MockBus.Get().MockExpensesRep.Object.ItemRep.CurrentIId = it.Id;

            var countBefore = expRep.ItemRep.Entities.Count();
            //Act
            itController.DeleteItemCard();

            var countAfter = expRep.ItemRep.Entities.Count();

            bool res = MockBus.Get().MockExpensesRep.Object.ItemRep.Entities.Contains(it);

            //Assert
            Assert.AreEqual(countBefore - 1, countAfter, "Количество товаров в репозитории не изменилось");
            Assert.IsFalse(res, "Товар не был удалён");

        }


        private void validation(MItemCard model_, Controller controller_)
        {
            var validationContext = new ValidationContext(model_, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model_, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller_.ModelState.AddModelError(
                    validationResult.MemberNames.First(),
                    validationResult.ErrorMessage);
            }
        }
    }
}
