using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainExpenses.Abstract;
using DomainExpenses.Concrete;

using WebExpenses.Models.Item;
using WebExpenses.Models.Group;
using WebExpenses.Models.Purchase;
using WebExpenses.Models.Shop;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// Создать новую покупку
        /// </summary>
        /// <returns></returns>
        public ViewResult CreatePurchaseCard()
        {
            return View("CreatePurchaseCard");
        }

      
        /// <summary>
        /// Открыть на редактирование карточку покупки
        /// </summary>
        /// <param name="purchaseId_"></param>
        /// <returns></returns>
        public ViewResult EditPurchaseCard(int purchaseId_)
        {
            var purchase =
                (from p in _repository.PurchaseRep.Entities
                 join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
                 join g in _repository.GroupRep.Entities on it.GId equals g.Id
                 join sh in _repository.ShopRep.Entities on p.Shop_Id equals sh.Id into p_sh
                 from pSh in p_sh.DefaultIfEmpty()
                 where p.Id == purchaseId_
                 select new MPurchaseCard()
                 {
                     Id = p.Id,
                     Item = new MItemCard(it), //Перепишем. Будем брать из контейнера(паттерн Proxy)
                     Shop = (pSh == null) ? null: new MShopCard(pSh),
                     Price = p.Price,
                     Count = p.Count,
                     Date = p.Date,
                 }).FirstOrDefault();
            //.Where(p => p.Id == _repository.CurrentPurchaseId).FirstOrDefault();
            ViewData["Title"] = "Редактировать покупку";
            ViewData["Head"] = "Редактировать";

            return View("EditPurchaseCard", purchase);
        }


        #region Web Api


        /// <summary>
        /// Создать покупку
        /// </summary>
        /// <param name="purchArgs_"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpStatusCodeResult CreatePurchase(PurchaseCreateParams purchArgs_)
        {
            try
            {
                var p = _repository.PurchaseRep.Create(purchArgs_.Convert());
                return new HttpStatusCodeResult(HttpStatusCode.OK, $"Покупка с кодом {p.Id} успешно создана");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Изменить покупку
        /// </summary>
        /// <param name="purchaseCard_"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpStatusCodeResult EditPurchase(PurchaseEditParams purchaseCard_)
        {
            try
            {
                _repository.PurchaseRep.Update(purchaseCard_);
                return new HttpStatusCodeResult(HttpStatusCode.OK, $"Покупка с кодом {purchaseCard_.Id} успешно изменена");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Создать покупку
        /// </summary>
        /// <param name="purchArgs_"></param>
        /// <returns></returns>
        [HttpDelete]
        public HttpStatusCodeResult DeletePurchase(int purchaseId_)
        {
            try
            {
                var purch =_repository.PurchaseRep.Entities.First(p => p.Id == purchaseId_);
                _repository.PurchaseRep.Delete(purch);
                return new HttpStatusCodeResult(HttpStatusCode.OK, $"Покупка с кодом {purch.Id} успешно создана");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        public ActionResult GetPurchaseGroupsByBeginAndEndDates(DateTime bDate_, DateTime eDate_)
        {
            try
            {
             
                var purchases = _repository.PurchaseRep.SelectPurchaseByBeginAndEndDates(bDate_, eDate_).ToList();

                //Берем все покупки и связываем с объектами "Товар", "Магазин", "Группа"
                var qPurchExt = (from purch in purchases
                                 join it in _repository.ItemRep.Entities on purch.Item_Id equals it.Id
                                 join sh in _repository.ShopRep.Entities on purch.Shop_Id equals sh.Id into nullShop
                                 join g in _repository.GroupRep.Entities on it.GId equals g.Id
                                 let purchGroup = new
                                 {
                                     Id = purch.Id,
                                     Item = it,
                                     Group = g,
                                     Shop = nullShop.FirstOrDefault(),
                                     Price = purch.Price,
                                     Count = purch.Count,
                                     Date = purch.Date
                                 }
                                 select purchGroup
                 );
                var purchExt = qPurchExt.ToList();

                //Группируем
                var purchGrp = (from pExt in qPurchExt
                                group pExt by pExt.Group into purchGroup
                                select new MGroupOfPurchases
                                {
                                    Group = new MGroupCard(purchGroup.Key),
                                    Purchases = (from p in purchGroup //!!!Содержит поле Group, по которому группируем
                                                                      //Создаем покупки, чтобы исключить свойство Group
                                                 select new MPurchaseCard()
                                                 {
                                                     Id = p.Id,
                                                     Item = new MItemCard(p.Item),
                                                     Shop = (p.Shop != null) ? new MShopCard(p.Shop) : null,
                                                     Price = p.Price,
                                                     Count = p.Count,
                                                     Date = p.Date
                                                 }).ToArray()
                                }
                                ).ToList();


                return Json(purchGrp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(
                    HttpStatusCode.InternalServerError, 
                    $"Ошибка в Purchase/GetPurchaseGroupsByBeginAndEndDates: {ex.Message}");
            }
        }

        #endregion


        private IExpensesRepository _repository = null;
    }
}