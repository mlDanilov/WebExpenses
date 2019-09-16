using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainExpenses;
using DomainExpenses.Abstract;
using DomainExpenses.Concrete;

using WebExpenses.Models.Item;
using WebExpenses.Models.Item.Interfaces;
using WebExpenses.Models.Group;
using System.Net;

namespace WebExpenses.Controllers
{
    /// <summary>
    /// Контроллер для работы с товарами
    /// </summary>
    public class ItemController : Controller
    {
        public ItemController(IExpensesRepository rep_)
        {
            _repository = rep_;
        }

        /// Получить товары по группе товаров
        /// </summary>
        /// <param name="gId_"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetItemListByGroupId(int groupId_)
        {
            var mItCrdList = new MItemList();
            mItCrdList.GroupId = groupId_;
          
            _repository.ItemRep.Entities
                .Where(it => (it.GId == groupId_))
                .OrderBy(it => it.Name).ToList().ForEach(it => mItCrdList.ItemList.Add(new MItemCard(it)));

            return Json(mItCrdList, JsonRequestBehavior.AllowGet);
        }


        public ViewResult CreateItemCard(int gId_)
        {
            IMItemCard itView = new MItemCard() { GId = gId_ };
            ViewData["SelectGroupName"] = "GId"; //Потом уберём
            return View("CreateItemCard", itView);
        }

        public ActionResult CreateItem(string name_, int gId_)
        {
            try
            {
                IMItemCard item = new MItemCard() { Name = name_, GId = gId_ };
                var res = _repository.ItemRep.Create(item);
                return Json(res);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        public JsonResult EditItem(int id_, string name_, int gId_)
        {
            var item = _repository.ItemRep.Entities.Where(it => it.Id == id_).First();
            item.Name = name_;
            item.GId = gId_;
            var mItem = new MItemCard(item);
            return Json(mItem, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Получить вид "Редактировать карточку товара"
        /// </summary>
        /// <param name="itemId_">Уникальный код товара</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditItemCard(int itemId_)
        {
            var item = _repository.ItemRep.Entities.Where(it => it.Id == itemId_).First();
            IMItemCard mItem = new MItemCard(item);
            return View("EditItemCard", mItem);
        }


        [HttpPost]
        public HttpStatusCodeResult DeleteItemCard(int itemId_)
        {
            try
            {
                var item = _repository.ItemRep.Entities.First(it => it.Id == itemId_);
                _repository.ItemRep.Delete(item);
                return new HttpStatusCodeResult(HttpStatusCode.OK, $"Товар с кодом {item.Id} удалён успешно");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, $"Ошибка при удалении товара с кодом {itemId_} : {ex.Message.ToString()}");
            }
        }
        /// <summary>
        /// Получить список товаров по текущей группе
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ItemsTableNew()
        {
            //int? iid = _repository.ItemRep.CurrentIId;
            
            //IMItemList itList =
            return PartialView("ItemsTableNew", getItemsViewByCurrentGId());
        }

        /// <summary>
        /// Получить список товаров по текущей группе
        /// </summary>
        /// <returns></returns>
        private IMItemList getItemsViewByCurrentGId() => getItemsViewByGId(_repository.GroupRep.CurrentGId);
        private IMItemList getItemsViewByGId(int? gId_)
        {
            IMItemList itView = new MItemList() { GroupId = gId_, ItemList = new List<IMItemCard>() };
            if (gId_ != null)
            {
                _repository.ItemRep.Entities
                   .Where(it => (it.GId == gId_))
                   .OrderBy(it => it.Name).ToList().ForEach(it => itView.ItemList.Add(new MItemCard(it)));
            }

            
            
            return itView;
        }

        private IItem getItemById(int? iid_)
            => _repository.ItemRep.Entities.Where(it => it.Id == iid_).FirstOrDefault();

        private IExpensesRepository _repository = null;
    }
}