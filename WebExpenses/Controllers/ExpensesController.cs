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
       
        public ViewResult GroupsAndItems(int? gId_ = null)
        {
            //var groups = _repository.Group.ToList();
            //var groups = _repository.Group.ToList();
            var groupsQuery = _repository.GroupExt;
            var groups = groupsQuery.ToList();

            var gListView = new MGroupList()
            {
                GroupId = gId_,
                GroupList = groups
            };
            return View(gListView);
        }

        public PartialViewResult Items(int? gId_ = null)
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

        [HttpPost]
        public ActionResult DeleteItemCard(int id_)
        {
            var item = _repository.Item.Where(it => it.Id == id_).FirstOrDefault();
            if (item == null)
                return Items();
            _repository.DeleteItem(id_);
            return Items(item.GId);
            //return RedirectToAction("GroupsAndItems", new { gId_ = item.GId });
        }

        #endregion

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

        public ViewResult EditGroup(int id_)
        {
            var group = _repository.Group.Where(it => it.Id == id_).FirstOrDefault();
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
        public ActionResult DeleteGroup(int id_)
        {
            var item = _repository.DeleteGroup(id_);
            return RedirectToAction("GroupsAndItems");
        }

        private IExpensesRepository _repository = null;
    }
}