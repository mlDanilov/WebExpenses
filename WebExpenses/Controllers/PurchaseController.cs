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
            ViewData["Title"] = "Покупки";
            return View();

        }

        //public ActionResult ListBody()
        //{
        //    return View();
        //}
        //public PartialViewResult PeriodSelect()
        //{
        //    var periods = getPeriodModel();
        //    ViewData["OptionName"] = "period";
        //    return PartialView("PeriodSelect", periods);
        //}

        //public PartialViewResult WeekSelect()
        //{
        //    var weeks = getWeekModel();
        //    ViewData["OptionName"] = "week";
        //    return PartialView("WeekSelect", weeks);
        //}

        //public PartialViewResult WeekOptions()
        //{
        //    var weeks = getWeekModel();
        //    ViewData["OptionName"] = "week";
        //    return PartialView("WeekOptions", weeks);
        //}

     
        /// <summary>
        /// Получить все периоды(месяц-год) в которых есть покупки
        /// </summary>
        /// <returns></returns>
        private List<IPeriod> getAllPeriods()
        {
            var periodList = _repository.PurchaseRep.SelectAllPeriods().OrderByDescending(p => p.MonthYear).ToList();
            var pList = new List<IPeriod>();

            periodList.ForEach(p => pList.Add(p));
            return pList;
        }

        /// <summary>
        /// Сумма расходов по группам за текущую выбранный месяц
        /// </summary>
        /// <returns></returns>
        //private IQueryable<MPeriodPurchaseSumByGroup> periodPurchSumByGroupTotal2()
        //{
        //    var periodPurchSumList = new List<MPeriodPurchaseSumByGroup>();

        //    var period = _repository.PurchaseRep.CurrentPeriod;
        //    var pp = new Period(period);
        //    var purchases = _repository.PurchaseRep.SelectPurchasesByPeriod(period);

        //    //Суммы затрат по группам за месяц
        //    var groupSum = (from p in purchases
        //                    join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
        //                    join g in _repository.GroupRep.Entities on it.GId equals g.Id
        //                    group p by it.GId into pG
        //                    select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
        //               );
        //    var gList = groupSum.ToList();
        //    //результат
        //    var res =
        //        (from g in _repository.GroupRep.GroupExt
        //         join gSum in groupSum
        //         on g.Id equals gSum.GroupId
        //         select
        //         new
        //           MPeriodPurchaseSumByGroup()
        //         {
        //             Group = g,
        //             TimeSpan = pp,
        //             Sum = gSum.Sum
        //         });
        //    var list = res.ToList();

        //    return res;
        //}
        //private IQueryable<MPeriodPurchaseSumByGroup> periodPurchSumByGroupTotal()
        //{
        //    var period = _repository.PurchaseRep.CurrentPeriod;
        //    var pp = new Period(period);
        //    //var purchases = _repository.SelectPurchasesByPeriod(period);
        //    //var pList = purchases.ToList();
        //    //Суммы затрат по группам за месяц
        //    var groupSum = (from p in  _repository.PurchaseRep.Entities
        //                    join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
        //                    join g in _repository.GroupRep.Entities on it.GId equals g.Id
        //                    where (p.Date.Month == period.MonthYear.Month) && (p.Date.Year == period.MonthYear.Year)
        //                    group p by it.GId into pG
        //                    select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
        //               );
        //    var gList = groupSum.ToList();
        //    //результат
        //    var res =
        //        (from g in _repository.GroupRep.Entities
        //         join gSum in groupSum
        //         on g.Id equals gSum.GroupId
        //         select 
        //         new
        //           MPeriodPurchaseSumByGroup()
        //         {
        //             Group = g,
        //             //TimeSpan = pp,
        //             Sum = gSum.Sum
        //         }
        //         );
        //    var list = res.ToList();
        //    list.ForEach(mp => mp.TimeSpan = pp);


        //    return res;
        //}
        /// <summary>
        /// Сумма расходов по группам за текущую выбранную неделю
        /// </summary>
        /// <returns></returns>
        //private IQueryable<MWeekPurchaseSumByGroup> weekPurchSumByGroupTotal()
        //{
        //    var weekPurchSumList = new List<MWeekPurchaseSumByGroup>();

        //    IWeek week = _repository.PurchaseRep.CurrentWeek;
        //    var ww = new Week() { BDate = week.BDate, EDate = week.EDate };
        //    var purchases = _repository.PurchaseRep.SelectPurchasesByWeek(week);
        //    //Суммы затрат по группам за неделю
        //    var groupSum = (from p in purchases
        //                    join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
        //                    join g in _repository.GroupRep.Entities on it.GId equals g.Id
        //                    group p by it.GId into pG
        //                    select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
        //               );

        //    //var t = purchases.ToList();
        //    //результат
        //    var res =
        //        (
        //         //from g in _repository.GroupRep.GroupExt
        //         from g in _repository.GroupRep.Entities
        //         join gSum in groupSum
        //         on g.Id equals gSum.GroupId
        //         select
        //         new MWeekPurchaseSumByGroup()
        //         {
        //             Group = g,
        //             //Week = week,
        //             Sum = gSum.Sum
        //         });
        //    var list = res.ToList();
        //    list.ForEach(mp => mp.Week = ww);


        //    return res;
        //}
        //private IQueryable<MDayPurchaseSumByGroup> dayPurchSumByGroupTotal()
        //{
        //    var date = _repository.PurchaseRep.CurrentDay;
        //    if (date == null)
        //        throw new Exception("такого не может быть");

        //    var purchases = _repository.PurchaseRep.SelectPurchaseByDate(date.Value);
        //    //Суммы затрат по группам за неделю
        //    var groupSum = (from p in purchases
        //                    join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
        //                    join g in _repository.GroupRep.Entities on it.GId equals g.Id
        //                    group p by it.GId into pG
        //                    select new { GroupId = pG.Key, Sum = pG.Sum(p => p.Price * p.Count) }
        //               );
        //    //результат
        //    var res =
        //        (
        //        //from g in _repository.GroupRep.GroupExt
        //        from g in _repository.GroupRep.Entities
        //        join gSum in groupSum
        //         on g.Id equals gSum.GroupId
        //         select
        //         new
        //           MDayPurchaseSumByGroup()
        //         {
        //             Group = g,
        //             Day = date.Value,
        //             Sum = gSum.Sum
        //         });
        //    return res;
        //}
        /// <summary>
        /// Вид, отображающий суммы расходов, сгруппированных по группам товаров
        /// </summary>
        /// <returns></returns>
        //public PartialViewResult PurchaseSumByGroupTotal()
        // {
        //    if (_repository.PurchaseRep.CurrentDay != null)
        //    {
        //        IQueryable<MDayPurchaseSumByGroup> purchTotal = dayPurchSumByGroupTotal();
        //        var list = purchTotal.ToList();
        //        return PartialView("DayPurchaseSumByGroupTotal", list);
        //    }
        //    else if (_repository.PurchaseRep.CurrentWeek != null)
        //    {
        //        IQueryable<MWeekPurchaseSumByGroup> purchTotal = weekPurchSumByGroupTotal();
        //        var list = purchTotal.ToList();
        //        return PartialView("WeekPurchaseSumByGroupTotal", list);
        //    }
        //    else
        //    {
        //        IQueryable<MPeriodPurchaseSumByGroup> purchTotal = periodPurchSumByGroupTotal();
        //        var list = purchTotal.ToList();
        //        return PartialView("PeriodPurchaseSumByGroupTotal", list);
        //    }

            
        //}

        //public JsonResult PurchaseTotalSum()
        //{
        //    IQueryable<Purchase> purchases = null;
        //    if (_repository.PurchaseRep.CurrentDay != null)
        //    {
        //        var day = _repository.PurchaseRep.CurrentDay;
        //        purchases = _repository.PurchaseRep.SelectPurchaseByDate(day.Value);
        //    }
        //    else if (_repository.PurchaseRep.CurrentWeek != null)
        //    {
        //        var week = _repository.PurchaseRep.CurrentWeek;
        //        purchases = _repository.PurchaseRep.SelectPurchasesByWeek(week);
        //    }
        //    else
        //    {
        //        var period = _repository.PurchaseRep.CurrentPeriod;
        //        purchases = _repository.PurchaseRep.SelectPurchasesByPeriod(period);
        //    }
        //    var sum = purchases.Sum(p => p.Count * p.Price).ToString("### ##0.00");


        //    return Json(sum, JsonRequestBehavior.AllowGet);
        //}

        public PartialViewResult PurchaseItem(int itemId_)
        {
            var item = _repository.ItemRep.Entities.Where(it => it.Id == itemId_).FirstOrDefault();

            MItemCard mItem = null;
            if (item != null)
                mItem = new MItemCard(item);
            else
                mItem = new MItemCard();

            return PartialView("PurchaseItem", mItem);
        }

        //public PartialViewResult InnerPurchases()
        //{
        //    var purchGId = _repository.PurchaseRep.CurrentPurchaseGId;
            
        //    IQueryable<IPurchase> purchases = null;
        //    if (_repository.PurchaseRep.CurrentDay != null)
        //        purchases = _repository.PurchaseRep.SelectPurchaseByDate(_repository.PurchaseRep.CurrentDay.Value);
        //    else if (_repository.PurchaseRep.CurrentWeek != null)
        //        purchases = _repository.PurchaseRep.SelectPurchasesByWeek(_repository.PurchaseRep.CurrentWeek);
        //    else
        //        purchases = _repository.PurchaseRep.SelectPurchasesByPeriod(_repository.PurchaseRep.CurrentPeriod);

        //    var res = purchases.ToList();
            
        //    //Суммы затрат с группой
        //    var purchasesDetail = (from p in purchases
        //                    join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
        //                    join sh in _repository.ShopRep.Entities on p.Shop_Id equals sh.Id into p_sh
        //                    from pSh in p_sh.DefaultIfEmpty()
        //                    join g in _repository.GroupRep.GroupExt on it.GId equals g.Id
        //                    where it.GId == purchGId
        //                    select
        //                    new MPurchase(p.Id) {
        //                        Item_Id = p.Item_Id,
        //                        ItemName = it.Name,
        //                        Date = p.Date,
        //                        Price = p.Price,
        //                        GroupId = it.GId,
        //                        GroupExtName = g.Name,
        //                        Shop_Id = p.Shop_Id,
        //                        ShopName = (pSh == null) ? string.Empty : pSh.Name,
        //                        ShopAddress = (pSh == null) ? string.Empty : pSh.Address,
        //                        Count = p.Count
        //                    }
        //               );

        //    var purchDetailList = purchasesDetail.ToList();

        //    return PartialView("PurchaseDetail", purchDetailList);

        //}

        //public PartialViewResult InnerPurchasesByGId(int gId_)
        //{
        //    var purchGId = gId_;
        //    ViewData["PurchGId"] = gId_;
        //    IQueryable<IPurchase> purchases = null;
        //    if (_repository.PurchaseRep.CurrentDay != null)
        //        purchases = _repository.PurchaseRep.SelectPurchaseByDate(_repository.PurchaseRep.CurrentDay.Value);
        //    else if (_repository.PurchaseRep.CurrentWeek != null)
        //        purchases = _repository.PurchaseRep.SelectPurchasesByWeek(_repository.PurchaseRep.CurrentWeek);
        //    else
        //        purchases = _repository.PurchaseRep.SelectPurchasesByPeriod(_repository.PurchaseRep.CurrentPeriod);

        //    var res = purchases.ToList();

        //    //Суммы затрат с группой
        //    var purchasesDetail = (from p in purchases
        //                           join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
        //                           join sh in _repository.ShopRep.Entities on p.Shop_Id equals sh.Id into p_sh
        //                           from pSh in p_sh.DefaultIfEmpty()
        //                               //join g in _repository.GroupRep.GroupExt on it.GId equals g.Id
        //                           join g in _repository.GroupRep.Entities on it.GId equals g.Id
        //                           where it.GId == purchGId
        //                           orderby p.Date descending
        //                           select
        //                           new MPurchase()
        //                           {
        //                               Id = p.Id,
        //                               Item_Id = p.Item_Id,
        //                               ItemName = it.Name,
        //                               Date = p.Date,
        //                               Price = p.Price,
        //                               GroupId = it.GId,
        //                               GroupExtName = g.Name,
        //                               Shop_Id = p.Shop_Id,
        //                               ShopName = (pSh == null) ? string.Empty : pSh.Name,
        //                               ShopAddress = (pSh == null) ? string.Empty : pSh.Address,
        //                               Count = p.Count
        //                           }
        //               );

        //    var purchDetailList = purchasesDetail.ToList();

        //    return PartialView("PurchaseDetail", purchDetailList);

        //}

        //public PartialViewResult InnerPurchasesByGId_сrutchVersion(int gId_)
        //{
        //    var purchGId = gId_;
        //    ViewData["PurchGId"] = gId_;
        //    IQueryable<IPurchase> purchasesTemp = null;
        //    if (_repository.PurchaseRep.CurrentDay != null)
        //        purchasesTemp = _repository.PurchaseRep.SelectPurchaseByDate(_repository.PurchaseRep.CurrentDay.Value);
        //    else if (_repository.PurchaseRep.CurrentWeek != null)
        //        purchasesTemp = _repository.PurchaseRep.SelectPurchasesByWeek(_repository.PurchaseRep.CurrentWeek);
        //    else
        //        purchasesTemp = _repository.PurchaseRep.SelectPurchasesByPeriod(_repository.PurchaseRep.CurrentPeriod);

        //    var res = purchasesTemp.ToList();

        //    //Суммы затрат с группой
        //    var purchasesDetail = (from p in purchasesTemp
        //                           join it in _repository.ItemRep.Entities on p.Item_Id equals it.Id
        //                           join sh in _repository.ShopRep.Entities on p.Shop_Id equals sh.Id into p_sh
        //                           from pSh in p_sh.DefaultIfEmpty()
        //                           //join g in _repository.Group on it.GId equals g.Id
        //                           join g in _repository.GroupRep.GroupExt on it.GId equals g.Id
        //                           where it.GId == purchGId
        //                           orderby p.Date descending
        //                           select
        //                           new MPurchase()
        //                           {
        //                               Id = p.Id,
        //                               Item_Id = p.Item_Id,
        //                               ItemName = it.Name,
        //                               Date = p.Date,
        //                               Price = p.Price,
        //                               GroupId = it.GId,
        //                               GroupExtName = g.Name,
        //                               Shop_Id = p.Shop_Id,
        //                              // ShopName = sh.Name,
        //                              // ShopAddress = sh.Address,
        //                               ShopName = (p_sh == null) ? string.Empty : pSh.Name,
        //                               ShopAddress = (p_sh == null) ? string.Empty : pSh.Address,
        //                               Count = p.Count
        //                           }
        //               );

        //    var purchDetailList = purchasesDetail.ToList();

        //    return PartialView("PurchaseDetail", purchDetailList);

        //}

        //public void SetCurrentPurchaseId(int purchaseId_) => _repository.PurchaseRep.CurrentPurchaseId = purchaseId_;

        /// <summary>
        /// Создать новую покупку
        /// </summary>
        /// <returns></returns>
        public ViewResult CreatePurchaseCard()
        {
            return View("CreatePurchaseCard");
        }

      


        //public JsonResult ValidateDate(string Date)
        //{
        //    DateTime parsedDate;
        //    if (!DateTime.TryParse(Date, out parsedDate))
        //    {
        //        return Json($"Введите допустимое значение даты", JsonRequestBehavior.AllowGet);
        //    }
        //    else if (DateTime.Now <= parsedDate)
        //    {
        //        return Json($"Дата покупки должна быть раньше чем {DateTime.Now}", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //}

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

        /// <summary>
        /// Создать покупку
        /// </summary>
        /// <param name="purchArgs_"></param>
        /// <returns></returns>
        public ActionResult DeletePurchaseView(int purchaseId_)
        {
                var purch = _repository.PurchaseRep.Entities.First(p => p.Id == purchaseId_);
                _repository.PurchaseRep.Delete(purch);
            return View("List");
        }


        public ActionResult GetPurchaseGroupsByBeginAndEndDates(DateTime bDate_, DateTime eDate_)
        {
            try
            {
                //bDate_ = bDate_ ?? new DateTime(2019,10,01);
                //eDate_ = eDate_ ?? new DateTime(2019,10,05);
                //var purchases = _repository.PurchaseRep.SelectPurchaseByBeginAndEndDates(bDate_.Value, eDate_.Value).ToList();

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