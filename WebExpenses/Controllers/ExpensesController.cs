using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainExpenses;
using DomainExpenses.Abstract;
using DomainExpenses.Concrete;

using WebExpenses.Models;

namespace WebExpenses.Controllers
{


    public class ExpensesController : Controller
    {
        public ExpensesController(IExpensesRepository rep_)
        {
            _repository = rep_;
        }
       
        public ViewResult GroupsAndItems()
        {
            //var groups = _repository.Group.ToList();
            //var groups = _repository.Group.ToList();
            var groupsQuery = _repository.GroupExt;
            var groups = groupsQuery.ToList();

            var gListView = new MGroupList()
            {
                GroupId = _repository.CurrentGId,
                GroupList = groups
            };
            return View(gListView);
        }
        
      

        /* public PartialViewResult Items(int gId_)
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

             return PartialView(itView);
         }
         */
        public PartialViewResult ItemsTable()
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

            return PartialView(itView);
        }

        public JsonResult ItemsTableJSON()
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

            return Json(itView, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult Groups(MGroupList gListView_)
        {
            
            return PartialView(gListView_);
        }


        public PartialViewResult GroupDropDownList(int gId_, string optionName_)
        {
            var gList = new MGroupList()
            {
                GroupId = gId_,
                GroupList = _repository.GroupExt.ToList()
            };
            ViewData["OptionName"] = optionName_;
            return PartialView(gList);

        }

        public PartialViewResult GroupsTableBodyRows(MGroupList gListView_)
        {
            if ((gListView_ == null) || (gListView_.GroupList == null))
                gListView_ = new MGroupList()
                {
                    GroupId = null,
                    GroupList = _repository.GroupExt.Where(g => g.Id== 1).ToList()
                };
            return PartialView(gListView_);
        }

        public void SetCurrentGId(int gId_)
        {
            // _repository.SetCurrentGId(gId_);
            _repository.CurrentGId = gId_;
            //return RedirectToAction("GroupsAndItems");
        }

        #region CreateEditDeleteItem
        public ViewResult CreateItemCard(int gId_)
        {
            var itView = new MItemCard() { GId = gId_ };
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
            return RedirectToAction("GroupsAndItems", new { gId_ = groupId_ });
        }

        public ViewResult EditItemCard(int id_)
        {
            var item = _repository.Item.Where(it => it.Id == id_).FirstOrDefault();
            var itView = new MItemCard(item);
            ViewData["Head"] = "Редактировать";
            return View("ItemCard", itView);
        }

        [HttpPost]
        public ActionResult EditItemCard(string name, int groupId_, int id = -1)
        {
            var item = _repository.Item.Where(it => it.Id == id).FirstOrDefault();
            if (item != null)
            {
                item.Name = name;
                item.GId = groupId_;
            }

            return RedirectToAction("GroupsAndItems", new { gId_ = groupId_ });
        }

        // [HttpPost]
        public ActionResult DeleteItemCard(int id_)
        {
            var item = _repository.Item.Where(it => it.Id == id_).FirstOrDefault();
            _repository.DeleteItem(id_);
            return RedirectToAction("GroupsAndItems");
            //return RedirectToAction("GroupsAndItems", new { gId_ = item.GId });
        }
       

        #endregion

        #region CreateEditDeleteGroup

        public ActionResult CreateGroupCard(int gId_ = -1)
        {
            

            var mGroup = new MGroupCard() { IdParent = gId_ };
            ViewData["Head"] = "Добавить";
            return View("GroupCard", mGroup);
        }
        [HttpPost]
        public ActionResult CreateGroupCard(string name, int parentGroupId_)
        {
            var group = _repository.AddNewGroup(name, parentGroupId_);
            return RedirectToAction("GroupsAndItems", new { gId_ = parentGroupId_ });
        }

        public ViewResult EditGroup()
        {
            int? id = _repository.CurrentGId;
            var group = _repository.Group.Where(it => it.Id == id).FirstOrDefault();
            
            var gView = new MGroupCard(group);
            ViewData["Head"] = "Редактировать";
            return View("GroupCard", gView);
        }
        [HttpPost]
        public ActionResult EditGroup(string name, int parentGroupId_, int id = -1)
        {
            var group = _repository.Group.Where(g => g.Id == id).FirstOrDefault();
            _repository.EditGroup(id, name, parentGroupId_);
            if (group != null)
            {
                group.Name = name;
                group.IdParent = parentGroupId_;
            }

            return RedirectToAction("GroupsAndItems", new { gId_ = parentGroupId_ });
        }
        [HttpPost]
        public PartialViewResult DeleteGroupAjax()
        {
            int? gId = _repository.CurrentGId;
            if (gId != null)
                _repository.DeleteGroup(gId.Value);

            var gList = new MGroupList()
            {
                GroupId = null,
                GroupList = _repository.GroupExt.ToList()
            };
            //return GroupsTableBodyRows(gList);
            return PartialView("GroupsTableBodyRows", gList);
            //return RedirectToAction("GroupsAndItems");
        }
        public ActionResult DeleteGroup()
        {
            int? gId = _repository.CurrentGId;
            if (gId != null)
                _repository.DeleteGroup(gId.Value);

            var gList = new MGroupList()
            {
                GroupId = null,
                GroupList = _repository.GroupExt.ToList()
            };
            //return GroupsTableBodyRows(gList);
            //return PartialView("GroupsTableBodyRows", gList);
            return RedirectToAction("GroupsAndItems");
        }

        #endregion

        private IExpensesRepository _repository = null;
    }
}