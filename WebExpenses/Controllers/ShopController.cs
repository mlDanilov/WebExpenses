using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainExpenses.Abstract;
using DomainExpenses.Concrete;
using WebExpenses.Models.Shop;

namespace WebExpenses.Controllers
{
    public class ShopController : Controller
    {
        public ShopController(IExpensesRepository rep_)
        {
            _repository = rep_;
        }
        // GET: Shop
        public ActionResult List()
        {
            var shops = getShopsView();
            ViewData["Title"] = "Магазины";
            return View(shops);
        }

        public ViewResult CreateShop()
        {

            var shView = new MShopCard();
            ViewData["Title"] = "Добавить магазин";
            ViewData["Head"] = "Добавить";
            return View("ShopCard", shView);
        }
        [HttpPost]
        public ActionResult CreateShop(MShopCard shCard_)
        {
            if (ModelState.IsValid)
            {
                var sh = EntitiesFactory.Get().CreateShop(shCard_.Id, shCard_.Name, shCard_.Address);
                var shop = _repository.ShopRep.Create(sh);
                _repository.ShopRep.CurrentShopId = shop.Id;
                return RedirectToAction("List");
            }
            else
                return CreateShop();
        }

        public ViewResult EditShop()
        {
            int? shId = _repository.ShopRep.CurrentShopId;
            var shop = _repository.ShopRep.Entities.Where(sh => sh.Id == shId).FirstOrDefault();

            var shView = new MShopCard(shop);
            ViewData["Title"] = "Редактировать магазин";
            ViewData["Head"] = "Редактировать";
            return View("ShopCard", shView);
        }
        [HttpPost]
        public ActionResult EditShop(MShopCard shCard_)
        {
            int? shopId = _repository.ShopRep.CurrentShopId;
            if (ModelState.IsValid)
            {
                if (shopId != null)
                {
                    var shop = EntitiesFactory.Get().CreateShop(shopId.Value, shCard_.Name, shCard_.Address);
                    _repository.ShopRep.Update(shop);
                }
                return RedirectToAction("List");
            }
            else
                return EditShop();
        }

        public ActionResult DeleteShop()
        {
            int? shId = _repository.ShopRep.CurrentShopId;
            var shop = _repository.ShopRep.Entities.Where(sh => sh.Id == shId).FirstOrDefault();
            if ((shop != null) && (shId != null))
                _repository.ShopRep.Delete(shop);

            return RedirectToAction("List");
        }
        /// <summary>
        /// Список магазинов
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ShopDropDownList(int? shopId_ = -1)
        {
            var shops = getShopsView();
            if (shopId_ != -1)
                shops.ShopId = shopId_;
            else
                shops.ShopId = null;

            return PartialView(shops);
        }

        public void SetCurrentShopId(int shId_) => _repository.ShopRep.CurrentShopId = shId_;

        /// <summary>
        /// Получить список магазинов
        /// </summary>
        /// <returns></returns>
        private MShopList getShopsView()
        {
            int? shId = _repository.ShopRep.CurrentShopId;
            List<IShop> shopList = new List<IShop>();
            _repository.ShopRep.Entities.ToList().ForEach(sh=> shopList.Add(sh));

            var shList =  shopList.OrderBy(sh => sh.Name);
            var shView = new MShopList()
            {
                ShopId = shId,
                ShopList = shList.ToList()
            };
            return shView;
        }

        private IExpensesRepository _repository = null;
    }
}