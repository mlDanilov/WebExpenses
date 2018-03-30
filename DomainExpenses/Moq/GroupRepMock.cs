using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Moq;
using DomainExpenses.Abstract;
using DomainExpenses.Abstract.Repositories;
using DomainExpenses.Concrete;

namespace DomainExpenses.Moq
{
    /// <summary>
    /// Мок-синглтон IGroupRepository
    /// </summary>
    public class GroupRepMock
    {

        private GroupRepMock()
        {
            setGroupBehavior();
        }


        private void setGroupBehavior()
        {
            var fBus = EntitiesFactory.Get();


            //Текущая группа(get)
            _gpRepMock.SetupGet(m => m.CurrentGId).Returns(
                () => _currentGId);

            //Текущая группа(set)
            _gpRepMock.SetupSet(m => m.CurrentGId = It.IsAny<int?>()).Callback(
                (int? gId_) =>
                {
                    _currentGId = gId_;
                    _gpRepMock.SetupGet(m => m.CurrentGId).Returns(
                      () => _currentGId);
                });
            
            //Группы товаров
            _gpRepMock.SetupGet(m => m.Entities).Returns(_groupsList.AsQueryable());
            //Группы товаров, с расширенными названиями
            _gpRepMock.SetupGet(m => m.GroupExt).Returns(_groupsExtList.AsQueryable());

            //Добавить новую группу
            _gpRepMock.Setup<Group>(m => m.Create(It.IsAny<IGroup>()))
                  .Returns((IGroup group_) =>
                  {
                      var group = fBus.CreateGroupC(_groupsList.Max(it => it.Id) + 1,
                          group_.IdParent, group_.Name);

                      _groupsList.Add(group);
                      _groupsExtList.Add(group);

                      _gpRepMock.Setup(m => m.Entities).Returns(_groupsList.AsQueryable());
                      _gpRepMock.Setup(m => m.Entities).Returns(_groupsExtList.AsQueryable());
                      return group;
                  });


            //Редактировать существующую группу товаров
            _gpRepMock.Setup(m => m.Update(It.IsAny<IGroup>()))
                 .Callback((IGroup group_) =>
                 {
                     var group = _groupsList.Where(gp => gp.Id == group_.Id).FirstOrDefault();
                     if (group == null)
                         return;

                     group.IdParent = group_.IdParent;
                     group.Name = group_.Name;

                     group = _groupsExtList.Where(gp => gp.Id == group_.Id).FirstOrDefault();
                     if (group == null)
                         return;

                     group.IdParent = group_.IdParent;
                     group.Name = group_.Name;
                 });

            //Удалить группу товаров
            _gpRepMock.Setup(m => m.Delete(It.IsAny<IGroup>()))
              .Callback(
                    (IGroup group_) =>
                    {
                        _groupsList.RemoveAll(gp => gp.Id == group_.Id);
                        _groupsExtList.RemoveAll(gp => gp.Id == group_.Id);

                        _gpRepMock.Setup(m => m.Entities).Returns(_groupsList.AsQueryable());
                        _gpRepMock.Setup(m => m.Entities).Returns(_groupsExtList.AsQueryable());
                    });

        }


        public Mock<IGroupRepository> GpRepMock
        {
            get
            {
                return _gpRepMock;
            }
        }

        private Mock<IGroupRepository> _gpRepMock = new Mock<IGroupRepository>();
        /// <summary>
        /// Текущая группа
        /// </summary>
        private int? _currentGId = null;

        /// <summary>
        /// Список групп
        /// </summary>
        private List<Group> _groupsList = new List<Group>
            {
                EntitiesFactory.Get().CreateGroupC(0, null,"Главная группа"),
                EntitiesFactory.Get().CreateGroupC(1, 0, "Мясо"),
                EntitiesFactory.Get().CreateGroupC(2, 1, @"Птица"),
                EntitiesFactory.Get().CreateGroupC(3, 1, @"Говядина"),
                EntitiesFactory.Get().CreateGroupC(4, 0, @"Остальное")
            };
        /// <summary>
        /// Список групп, название указывается с родительскими группами, кроме корневой
        /// </summary>
        private List<Group> _groupsExtList = new List<Group>
            {
                EntitiesFactory.Get().CreateGroupC(0, null,"Главная группа"),
                EntitiesFactory.Get().CreateGroupC(1, 0,"Мясо"),
                EntitiesFactory.Get().CreateGroupC(2, 1, @"Мясо\Птица"),
                EntitiesFactory.Get().CreateGroupC(3, 1, @"Мясо\Говядина"),
                EntitiesFactory.Get().CreateGroupC(4, 0, @"Остальное")
            };


        public static GroupRepMock Get()
        {
            if (_instance == null)
                _instance = new GroupRepMock();
            return _instance;
        }
        private static GroupRepMock _instance = new GroupRepMock();
    }
}
