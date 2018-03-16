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
        private MItemList getItemsViewByCurrentGId() => getItemsViewByGId(_repository.CurrentGId);
        private MItemList getItemsViewByGId(int? gId_)
        {
            var itView = new MItemList();
            itView.GroupId = gId_;
            if (gId_ != null)
            {
                //var iList = _repository.Item.ToList();
                itView.ItemList = _repository.Item
                    .Where(it => (it.GId == gId_))
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
            ViewData["SelectGroupName"] = "GId";
            return View("ItemCard", itView);
        }
      
        [HttpPost]
       public ActionResult CreateItemCard(MItemCard mItemCard_)
       {
            if (ModelState.IsValid)
            {
                var item = _repository.AddNewItem(mItemCard_.Name, mItemCard_.GId);
                return RedirectToAction("GroupsAndItems", "Group", new { gId_ = mItemCard_.GId });
            }
            else
                return CreateItemCard(mItemCard_.GId);

        }

        public ViewResult EditItemCard()
        {
            var item = _repository.Item.Where(it => it.Id == _repository.CurrentIId).FirstOrDefault();
            var itView = new MItemCard(item);
            ViewData["Title"] = "Редактировать карточку товара";
            ViewData["Head"] = "Редактировать";
            ViewData["SelectGroupName"] = "GId";
            return View("ItemCard", itView);
        }

        [HttpPost]
        public ActionResult EditItemCard(MItemCard mItemCard_)
        {
            if (ModelState.IsValid)
            {
                _repository.EditItem(mItemCard_.Id, mItemCard_.Name, mItemCard_.GId);
                return RedirectToAction("GroupsAndItems", "Group", new { gId_ = mItemCard_.GId });
            }
            else
                return EditItemCard();
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

        public PartialViewResult ItemDropDownList(IItem item_)
        {
            ViewData["SelectItemId"] = "itemId";
            ViewData["SelectItemName"] = "itemId";

            if (item_ == null)
                return PartialView("ItemDropDownList", new MItemDDList());

            var itemList = _repository.Item.Where(it => it.GId == item_.GId).ToList();
            var mItemList = new MItemDDList()
            {
                ItemId = item_.Id,
                ItemList = itemList
            };
            return PartialView("ItemDropDownList", mItemList);
        }

        public PartialViewResult ItemOptions(int groupId_)
        {

            var itemList = _repository.Item.Where(it => it.GId == groupId_).ToList();
            var mItemList = new MItemDDList()
            {
                ItemId = null,
                ItemList = itemList
            };
            return PartialView("ItemOptions", mItemList);
        }

        private IExpensesRepository _repository = null;
    }
}