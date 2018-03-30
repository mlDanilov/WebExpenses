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


     
      
        public PartialViewResult GroupDropDownList(int gId_, 
            string selectGroupId_, string selectGroupName_)
        {
            ViewData["SelectGroupId"] = selectGroupId_;
            ViewData["SelectGroupName"] = selectGroupName_;
            

            if (gId_ == -1)
                gId_ = _repository.GroupRep.GroupExt.Where(g => g.IdParent == null).FirstOrDefault().Id;

            var gList = getMGroupExtList(gId_);

            return PartialView(gList);
        }

        public PartialViewResult GroupsTableBodyRows(MGroupList gListView_)
        {
            if ((gListView_ == null) || (gListView_.GroupList == null))
                gListView_ = getMGroupExtList(null);

            return PartialView(gListView_);
        }

        public ViewResult GroupsAndItems()
        {
            ViewData["Title"] = "Группы и товары";
            var groupsQuery = _repository.GroupRep.GroupExt;

            var gListView = getMGroupExtList(_repository.GroupRep.CurrentGId);
            return View(gListView);
        }

        public ActionResult CreateGroupCard(int gId_ = -1)
        {
            var mGroup = new MGroupCard() { IdParent = gId_ };
            ViewData["Title"] = "Добавить группу товаров";
            ViewData["Head"] = "Добавить";
            ViewData["SelectGroupId"] = "IdParent";
            ViewData["SelectGroupName"] = "IdParent";
            return View("GroupCard", mGroup);
        }

        [HttpPost]
        public ActionResult CreateGroupCard(MGroupCard mGroup_)
        {
            if ((ModelState.IsValid) && (mGroup_.IdParent != null))
            {

                var group = _repository.GroupRep.Create(mGroup_);
                return RedirectToAction("GroupsAndItems", new { gId_ = mGroup_.IdParent.Value });
            }
            else
                return CreateGroupCard(mGroup_.IdParent.Value);
           // return RedirectToAction("GroupsAndItems", new { gId_ = parentGroupId_ });
        }
       
        public void SetCurrentGId(int gId_) =>  _repository.GroupRep.CurrentGId = gId_;

        public ViewResult EditGroup()
        {
            int? id = _repository.GroupRep.CurrentGId;
            var group = _repository.GroupRep.Entities.Where(it => it.Id == id).FirstOrDefault();

            var gView = new MGroupCard(group);
            ViewData["Head"] = "Редактировать";
            ViewData["Title"] = "Редактировать товар";
            ViewData["SelectGroupId"] = "IdParent";
            ViewData["SelectGroupName"] = "IdParent";
            return View("GroupCard", gView);
        }
        [HttpPost]
        public ActionResult EditGroup(MGroupCard mGroup_)
        {
            if ((ModelState.IsValid) && (mGroup_.IdParent != null))
            {
                _repository.GroupRep.Update(mGroup_);
                return RedirectToAction("GroupsAndItems", new { gId_ = mGroup_.IdParent });
            }
            else
                return EditGroup();

        }
        [HttpPost]
        public PartialViewResult DeleteGroupAjax()
        {
            int? gId = _repository.GroupRep.CurrentGId;
            if (gId != null)
                _repository.GroupRep.Delete(_repository.GroupRep.Entities.Where(g=>g.Id == gId).First());

            var gExtList = new List<IGroup>();
            _repository.GroupRep.GroupExt.ToList().ForEach(g => gExtList.Add(g));

            var gList = new MGroupList()
            {
                GroupId = null,
                GroupList = gExtList
            };
            //return GroupsTableBodyRows(gList);
            return PartialView("GroupsTableBodyRows", gList);
            //return RedirectToAction("GroupsAndItems");
        }
        public ActionResult DeleteGroup()
        {
            int? gId = _repository.GroupRep.CurrentGId;
            var group = getGroupById(gId);
            if (group == null)
                return RedirectToAction("GroupsAndItems");

            if (group != null)
            {
                _repository.GroupRep.Delete(group);
                _repository.GroupRep.CurrentGId = group.IdParent;
            }
            return RedirectToAction("GroupsAndItems");
        }

        private IGroup getGroupById(int? gId_) => _repository.GroupRep.Entities.Where(g => g.Id == gId_).FirstOrDefault();

        private MGroupList getMGroupExtList(int? gId_)
        {
            List<IGroup> gExtList = new List<IGroup>();
            _repository.GroupRep.GroupExt.ToList().ForEach(g => gExtList.Add(g));
            var gList = new MGroupList()
            {
                GroupId = gId_,
                GroupList = gExtList
            };
            return gList;
        }

        private IExpensesRepository _repository = null;
    }
}