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


        /// <summary>
        /// Получить список товаров по текущей группе
        /// </summary>
        /// <returns></returns>
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
        private MItemList getItemsViewByCurrentGId() => getItemsViewByGId(_repository.GroupRep.CurrentGId);
        private MItemList getItemsViewByGId(int? gId_)
        {
            var itView = new MItemList();
            itView.GroupId = gId_;
            if (gId_ != null)
            {
                 _repository.ItemRep.Entities
                    .Where(it => (it.GId == gId_))
                    .OrderBy(it => it.Name).ToList().ForEach(it => itView.ItemList.Add(it));
            }
            else
                itView.ItemList = new List<IItem>();
            return itView;
        }

        /// <summary>
        /// Установить текущую группу в DBContext
        /// </summary>
        /// <param name="iid_"></param>
        public void SetCurrentIId(int iid_) => _repository.ItemRep.CurrentIId = iid_;

        public ViewResult CreateItemCard(int gId_)
        {
            var itView = new MItemCard() { GId = gId_ };
            ViewData["SelectGroupName"] = "GId"; //Потом уберём
            return View("CreateItemCard", itView);
        }

        public ActionResult CreateItem(string name_, int gId_)
        {
            var item = new MItemCard() { Name = name_, GId = gId_ };
            return CreateItemCard(item);
        }
        public ActionResult CreateItem2(string name_)
        {
            if (_repository.GroupRep.CurrentGId == null)
                _repository.GroupRep.CurrentGId = _repository.GroupRep.Entities.Where(g => g.IdParent == null).First().Id;
            int? gId = _repository.GroupRep.CurrentGId;
            return CreateItem(name_, gId.Value);
        }

        /// <summary>
        /// Запрос на создание карточки товара и добавление в репозиторий
        /// </summary>
        /// <param name="mItemCard_"></param>
        /// <returns>Возвращает получившуюся карточку товара или ошибку</returns>
        [HttpPost]
       public JsonResult CreateItemCard(MItemCard mItemCard_)
       {
            try
            { 
                var item = _repository.ItemRep.Create(mItemCard_);
                return Json(item);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
                

        }


        public ViewResult EditItemCard()
        {
            var item = getItemById(_repository.ItemRep.CurrentIId);
            var itView = new MItemCard(item);
            ViewData["Title"] = "Редактировать карточку товара";
            ViewData["Head"] = "Редактировать";
            ViewData["SelectGroupName"] = "GId";
            return View("EditItemCard", itView);
        }

        public JsonResult EditItem(int id_, string name_, int gId_)
        {
            var item = _repository.ItemRep.Entities.Where(it => it.Id == id_).First();
            item.Name = name_;
            item.GId = gId_;
            var mItem = new MItemCard(item);
            return Json(mItem, JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditItem2(int id_, string name_)
        {
            var item = _repository.ItemRep.Entities.Where(it => it.Id == id_).First();
            var mItem = new MItemCard(item) { Name = name_ };
            return EditItemCard(mItem);
        }


        public ActionResult DeleteItem(int id_)
        {
            int? iid = _repository.ItemRep.CurrentIId = id_;
            return DeleteItemCard();
        }

        [HttpPost]
        public ActionResult EditItemCard(MItemCard mItemCard_)
        {
            if (ModelState.IsValid)
            {
                _repository.ItemRep.Update(mItemCard_);
                return RedirectToAction("List", "Group", new { gId_ = mItemCard_.GId });
            }
            else
                return EditItemCard();
        }

        public ActionResult DeleteItemCard()
        {
            int? iid = _repository.ItemRep.CurrentIId;
            if (iid.HasValue)
                _repository.ItemRep.Delete(getItemById(iid));
            return RedirectToAction("List", "Group");
        }
        [HttpPost]
        public PartialViewResult DeleteItemCardAjax()
        {
            int? iid = _repository.ItemRep.CurrentIId;
            if (iid != null)
                _repository.ItemRep.Delete(getItemById(iid));
            return PartialView("ItemsTableBodyRows", getItemsViewByCurrentGId());
        }

        public PartialViewResult ItemDropDownList(IItem item_)
        {
            ViewData["SelectItemId"] = "Item_Id";
            ViewData["SelectItemName"] = "Item_Id";

            if (item_ == null)
                return PartialView("ItemDropDownList", new MItemDDList());

            //var itemList = 

            var mItemList = new MItemDDList()
            {
                ItemId = item_.Id
            };
            _repository.ItemRep.Entities.Where(it => it.GId == item_.GId).ToList().
                ForEach(it => mItemList.ItemList.Add(it));

            return PartialView("ItemDropDownList", mItemList);
        }

        public PartialViewResult ItemOptions(int groupId_)
        {
            var mItemList = new MItemDDList() { ItemId = null };
            _repository.ItemRep.Entities.Where(it => it.GId == groupId_).ToList()
                .ForEach( it => mItemList.ItemList.Add(it));
            return PartialView("ItemOptions", mItemList);
        }

        private IItem getItemById(int? iid_)
            => _repository.ItemRep.Entities.Where(it => it.Id == iid_).FirstOrDefault();

        private IExpensesRepository _repository = null;
    }
}