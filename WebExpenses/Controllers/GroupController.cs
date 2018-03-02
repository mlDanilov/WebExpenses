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
    /// Контроллер для работы с группами товаров
    /// </summary>
    public class GroupController : Controller
    {
        public GroupController(IExpensesRepository rep_)
        {
            _repository = rep_;
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
                    GroupList = _repository.GroupExt.Where(g => g.Id == 1).ToList()
                };
            return PartialView(gListView_);
        }

        public ViewResult GroupsAndItems()
        {
            //var groups = _repository.Group.ToList();
            //var groups = _repository.Group.ToList();
            ViewData["Title"] = "Группы и товары";
            var groupsQuery = _repository.GroupExt;
            var groups = groupsQuery.ToList();

            var gListView = new MGroupList()
            {
                GroupId = _repository.CurrentGId,
                GroupList = groups
            };
            return View(gListView);
        }

        public ActionResult CreateGroupCard(int gId_ = -1)
        {
            var mGroup = new MGroupCard() { IdParent = gId_ };
            ViewData["Title"] = "Добавить группу товаров";
            ViewData["Head"] = "Добавить";
            return View("GroupCard", mGroup);
        }
        [HttpPost]
        public ActionResult CreateGroupCard(string name, int parentGroupId_)
        {
            var group = _repository.AddNewGroup(name, parentGroupId_);
            return RedirectToAction("GroupsAndItems", new { gId_ = parentGroupId_ });
        }
        public void SetCurrentGId(int gId_)
        {
            _repository.CurrentGId = gId_;
        }

        public ViewResult EditGroup()
        {
            int? id = _repository.CurrentGId;
            var group = _repository.Group.Where(it => it.Id == id).FirstOrDefault();

            var gView = new MGroupCard(group);
            ViewData["Head"] = "Редактировать";
            ViewData["Title"] = "Редактировать товар";
            return View("GroupCard", gView);
        }
        [HttpPost]
        public ActionResult EditGroup(string name, int parentGroupId_, int id = -1)
        {
            var group = _repository.Group.Where(g => g.Id == id).FirstOrDefault();
            _repository.EditGroup(id, name, parentGroupId_);
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
            var group = _repository.Group.Where(g => g.Id == gId).FirstOrDefault();
            if (group == null)
                return RedirectToAction("GroupsAndItems");

            if (gId != null)
            {
                _repository.DeleteGroup(gId.Value);
                _repository.CurrentGId = group.IdParent;
            }

            var gList = new MGroupList()
            {
                GroupId = null,
                GroupList = _repository.GroupExt.ToList()
            };
            //return GroupsTableBodyRows(gList);
            //return PartialView("GroupsTableBodyRows", gList);
            return RedirectToAction("GroupsAndItems");
        }

        private IExpensesRepository _repository = null;
    }
}