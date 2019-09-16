using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

using DomainExpenses;
using DomainExpenses.Abstract;
using DomainExpenses.Concrete;

using WebExpenses.Models.Item;
using WebExpenses.Models.Group;
using WebExpenses.Models.Group.Interface;
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

        //public PartialViewResult Groups(MGroupList gListView_)
        //{

        //    return PartialView(gListView_);
        //}


        //public PartialViewResult GroupDropDownList(int gId_,
        //    string selectGroupId_, string selectGroupName_)
        //{
        //    ViewData["SelectGroupId"] = selectGroupId_;
        //    ViewData["SelectGroupName"] = selectGroupName_;


        //    if (gId_ == -1)
        //        gId_ = _repository.GroupRep.GroupExt.Where(g => g.IdParent == null).FirstOrDefault().Id;

        //    var gList = getMGroupExtList();

        //    return PartialView(gList);
        //}

        //public PartialViewResult GroupsTableBodyRows(MGroupList gListView_)
        //{
        //    if ((gListView_ == null) || (gListView_.GroupList == null))
        //    {
        //        PartialView(getMGroupExtList());
        //    }
        //    return PartialView(gListView_);
        //}

        public ViewResult List()
        {
            ViewData["Title"] = "Группы и товары";
            var groupsQuery = _repository.GroupRep.GroupExt;

            var gListView = getMGroupExtList();
            return View(gListView);
        }

        public ActionResult CreateGroupCard(int? idParent_)
        {
            int? idParent = idParent_ ?? getGIdIfIdParentIsNull();

            var mGroup = new MGroupCard() { IdParent = idParent };
            ViewData["Title"] = "Добавить группу товаров";
            ViewData["Head"] = "Добавить";
            ViewData["SelectGroupId"] = "IdParent";
            ViewData["SelectGroupName"] = "IdParent";
            return View("CreateGroupCard", mGroup);
        }
        [HttpPost]
        public JsonResult CreateNewGroup(string name_, int? idParent_)
        {
            int? idParent = idParent_ ?? getGIdIfIdParentIsNull();

            var group = _repository.GroupRep.Create(new Group() { Name = name_, IdParent = idParent });
            return Json(group);
        }

        [HttpPost]
        public ActionResult CreateGroupCard(IMGroupCard mGroup_)
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
       
        public ViewResult EditGroup(int gId_)
        {
            var group = _repository.GroupRep.Entities.Where(it => it.Id == gId_).FirstOrDefault();
            var gView = new MGroupCard(group);
            return View("EditGroupCard", gView);
        }

        public JsonResult EditGroupCard(int id_, string name_, int? idParent_)
        {
            //Правим список групп
            var group = _repository.GroupRep.Entities.Where(g => g.Id == id_).FirstOrDefault();
            group.Name = name_;
            group.IdParent = idParent_;

            //Костыль
            var groupExt = _repository.GroupRep.GroupExt.Where(g => g.Id == id_).FirstOrDefault();
            groupExt.Name = name_;
            groupExt.IdParent = idParent_;

            IMGroupCard mGroup = new MGroupCard(group);
            return Json(mGroup, JsonRequestBehavior.AllowGet);  
        }

        //[HttpPost]
        //public PartialViewResult DeleteGroupAjax(int gId_)
        //{
        //    var group = _repository.GroupRep.Entities.Where(g => g.Id == gId_).First();
        //    var idParent = group.IdParent;
        //    if (group != null)
        //    {
        //        _repository.GroupRep.Delete(group);
        //        _repository.GroupRep.CurrentGId = idParent;
        //    }

        //    var gList = getMGroupExtList();
        //    return PartialView("GroupsTableBodyRows", gList);
        //}

      
        [ActionName("DeleteGroup"), HttpPost]
        public HttpStatusCodeResult DeleteGroupById(int groupId_)
        {
            try
            {
                var grp = _repository.GroupRep.Entities.Where(g => g.Id == groupId_).First();
                _repository.GroupRep.Delete(grp);
                return new HttpStatusCodeResult(HttpStatusCode.OK,
                    $"Удаление группы товаров c кодом {groupId_} прошло успешно");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, $"При удалении группы товаров c кодом {groupId_} произошла ошибка: {ex.Message}");
            }
        }

        //[HttpPost]
        //[HttpDelete]
        //[ActionName("DeleteGroup")]
        //public ActionResult DeleteGroupByCurrentGId()
        //{
        //    int? gId = _repository.GroupRep.CurrentGId;
        //    var group = getGroupById(gId);
        //    if (group != null)
        //    {
        //        _repository.GroupRep.Delete(group);
        //        _repository.GroupRep.CurrentGId = group.IdParent;
        //    }
        //    return RedirectToAction("List");
        //}

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

        private IMGroupList getMGroupExtList()
        {
            var gExtList = new List<IMGroupCard>();

            _repository.GroupRep.GroupExt.ToList().ForEach(g => gExtList.Add(new MGroupCard(g)));
            var gList = new MGroupList()
            {
                GroupList = gExtList
            };
            return gList;
        }
        /// <summary>
        /// Получить список групп 
        /// </summary>
        /// <param name="gId_"></param>
        /// <returns></returns>
        public JsonResult GetGroupList()
        {
            try
            {
                var groupList = getMGroupExtList();
                return Json(groupList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        private IExpensesRepository _repository = null;
    }
}