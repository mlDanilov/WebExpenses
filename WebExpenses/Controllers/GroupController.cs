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
using System.Web.Routing;

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

        public ViewResult List()
        {
            ViewData["Title"] = "Группы и товары";
            var groupsQuery = _repository.GroupRep.GroupExt;

            var gListView = getMGroupExtList(_repository.GroupRep.CurrentGId);
            return View(gListView);
        }

        public ActionResult CreateGroupCard(int? idParent_)
        {
            int? idParent =  idParent_ ?? getGIdIfIdParentIsNull();

            var mGroup = new MGroupCard() { IdParent = idParent };
            ViewData["Title"] = "Добавить группу товаров";
            ViewData["Head"] = "Добавить";
            ViewData["SelectGroupId"] = "IdParent";
            ViewData["SelectGroupName"] = "IdParent";
            return View("GroupCard", mGroup);
        }

        public ActionResult CreateNewGroup(string name_, int? idParent_)
        {
            int? idParent = idParent_ ?? getGIdIfIdParentIsNull();

            var mGroup = new MGroupCard() { Name= name_, IdParent = idParent };
            return CreateGroupCard(mGroup);
        }

        [HttpPost]
        public ActionResult CreateGroupCard(MGroupCard mGroup_)
        {
            if (ModelState.IsValid)
            {
                
                var group = _repository.GroupRep.Create(mGroup_);
                return RedirectToAction("List", new { gId_ = mGroup_.IdParent });
            }
            else
                return CreateGroupCard(mGroup_.IdParent);
           // return RedirectToAction("GroupsAndItems", new { gId_ = parentGroupId_ });
        }

        public void SetCurrentGId(int gId_) =>  _repository.GroupRep.CurrentGId = gId_;
       
        public ViewResult EditGroup(int? gId_ = null)
        {
            if (gId_ != null)
            _repository.GroupRep.CurrentGId = gId_;

            int id = _repository.GroupRep.CurrentGId.Value;
            var group = _repository.GroupRep.Entities.Where(it => it.Id == id).FirstOrDefault();

            var gView = new MGroupCard(group);
            ViewData["Head"] = "Редактировать";
            ViewData["Title"] = "Редактировать группу";
            ViewData["SelectGroupId"] = "IdParent";
            ViewData["SelectGroupName"] = "IdParent";
            return View("GroupCard", gView);
        }

        public ActionResult EditGroupCard(int id_, string name_, int? idParent_)
        {
            int? idParent = idParent_ ?? getGIdIfIdParentIsNull();
            var mGroup = new MGroupCard() { Id = id_, Name = name_, IdParent = idParent };
            return EditGroup(mGroup);  
        }

        public ActionResult ChangeGroupName(int id_, string name_)
        {
            var group = _repository.GroupRep.Entities.Where(g => g.Id == id_).First();

            var mGroup = new MGroupCard() { Id = id_, Name = name_, IdParent = group.IdParent };
            return EditGroup(mGroup);
        }

        [HttpPost]
        public ActionResult EditGroup(MGroupCard mGroup_)
        {
            if ((ModelState.IsValid) && (mGroup_.IdParent != null))
            {
                _repository.GroupRep.Update(mGroup_);
                return RedirectToAction("List", new { gId_ = mGroup_.IdParent });
            }
            else
                return EditGroup();

        }

        [HttpPost]
        public PartialViewResult DeleteGroupAjax(int gId_)
        {
            var group = _repository.GroupRep.Entities.Where(g => g.Id == gId_).First();
            var idParent = group.IdParent;
            if (group != null)
            {
                _repository.GroupRep.Delete(group);
                _repository.GroupRep.CurrentGId = idParent;
            }

            var gList = getMGroupExtList(idParent);
            return PartialView("GroupsTableBodyRows", gList);
        }

      
        [ActionName("DeleteGroup"), HttpGet]
        public ActionResult DeleteGroupById(int gId_)
        {
            _repository.GroupRep.CurrentGId = gId_;
            return DeleteGroupByCurrentGId();
        }

        [HttpPost]
        [HttpDelete]
        [ActionName("DeleteGroup")]
        public ActionResult DeleteGroupByCurrentGId()
        {
            int? gId = _repository.GroupRep.CurrentGId;
            var group = getGroupById(gId);
            if (group != null)
            {
                _repository.GroupRep.Delete(group);
                _repository.GroupRep.CurrentGId = group.IdParent;
            }
            return RedirectToAction("List");
        }

        private int? getGIdIfIdParentIsNull()
        {
            if (_repository.GroupRep.CurrentGId != null)
                return _repository.GroupRep.CurrentGId;
            else
            {
                var group = _repository.GroupRep.Entities.Where(g => g.IdParent == null).FirstOrDefault();
                if (group != null)
                    return group.Id;
                else
                    return null;
            }
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