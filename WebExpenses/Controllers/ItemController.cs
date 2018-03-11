using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainExpenses;
using DomainExpenses.Abstract;
using DomainExpenses.Concrete;

using WebExpenses.Models.Item;
using WebExpenses.Models.Group;


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



        public PartialViewResult ItemsTable()
        {
            var itView = getItemsViewByCurrentGId();
            return PartialView(itView);
        }

        public JsonResult ItemsTableJSON()
        {
            var itView = getItemsViewByCurrentGId();
            return Json(itView, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Получить список товаров по текущей группе
        /// </summary>
        /// <returns></returns>
        private MItemList getItemsViewByCurrentGId()
        {
            int? gId = _repository.CurrentGId;
            var itView = new MItemList();
            itView.GroupId = gId;
            if (gId != null)
            {

                //var iList = _repository.Item.ToList();
                itView.ItemList = _repository.Item
                    .Where(it => (it.GId == gId))
                    .OrderBy(it => it.Name).ToList();
            }
            else
                itView.ItemList = new List<IItem>();
            return itView;
        }



        /// <summary>
        /// Установить текущую группу в DBContext
        /// </summary>
        /// <param name="iid_"></param>
        public void SetCurrentIId(int iid_)
        {
            _repository.CurrentIId = iid_;
        }
       
        public ViewResult CreateItemCard(int gId_)
        {
            var itView = new MItemCard() { GId = gId_ };
            ViewData["Title"] = "Добавить товар";
            ViewData["Head"] = "Добавить";
            return View("ItemCard", itView);
        }
        [HttpPost]
        public ActionResult CreateItemCard(string name, int groupId_)
        {
            /*
            if (string.IsNullOrEmpty(name))
                ModelState.AddModelError("name", "Не задано название товара");

            if (!ModelState.IsValid)
                return CreateItemCard(groupId_);
            */

            var item = _repository.AddNewItem(name, groupId_);
            return RedirectToAction("GroupsAndItems", "Group", new { gId_ = groupId_ });
        }

        public ViewResult EditItemCard()
        {
            var item = _repository.Item.Where(it => it.Id == _repository.CurrentIId).FirstOrDefault();
            var itView = new MItemCard(item);
            ViewData["Title"] = "Редактировать карточку товара";
            ViewData["Head"] = "Редактировать";
            return View("ItemCard", itView);
        }


        [HttpPost]
        public ActionResult EditItemCard(string name, int groupId_, int id = -1)
        {
            /*var item = _repository.Item.Where(it => it.Id == id).FirstOrDefault();
            if (item != null)
            {
                item.Name = name;
                item.GId = groupId_;
            }*/
            _repository.EditItem(id, name, groupId_);

            return RedirectToAction("GroupsAndItems","Group", new { gId_ = groupId_ });
        }


       /* public ActionResult DeleteItemCard(int id_)
        {
            var item = _repository.Item.Where(it => it.Id == id_).FirstOrDefault();
            _repository.DeleteItem(id_);
            return RedirectToAction("GroupsAndItems");
            //return RedirectToAction("GroupsAndItems", new { gId_ = item.GId });
        }*/
        public ActionResult DeleteItemCard()
        {
            if (_repository.CurrentIId.HasValue)
                _repository.DeleteItem(_repository.CurrentIId.Value);
            return RedirectToAction("GroupsAndItems", "Group");
            //return RedirectToAction("GroupsAndItems", new { gId_ = item.GId });
        }
        [HttpPost]
        public PartialViewResult DeleteItemCardAjax()
        {
            int? iid = _repository.CurrentIId;
            if (iid != null)
                _repository.DeleteItem(iid.Value);

            return PartialView("ItemsTableBodyRows", getItemsViewByCurrentGId());
            //return RedirectToAction("GroupsAndItems", new { gId_ = item.GId });
        }

        private IExpensesRepository _repository = null;
    }
}