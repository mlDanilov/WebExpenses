using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainExpenses.Abstract;
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
                var shop = _repository.AddNewShop(shCard_.Name, shCard_.Address);
                _repository.CurrentShopId = shop.Id;
                return RedirectToAction("List");
            }
            else
                return CreateShop();
        }

        public ViewResult EditShop()
        {
            int? shId = _repository.CurrentShopId;
            var shop = _repository.Shop.Where(sh => sh.Id == shId).FirstOrDefault();

            var shView = new MShopCard(shop);
            ViewData["Title"] = "Редактировать магазин";
            ViewData["Head"] = "Редактировать";
            return View("ShopCard", shView);
        }
        [HttpPost]
        public ActionResult EditShop(MShopCard shCard_)
        {
            int? shopId = _repository.CurrentShopId;
            if (ModelState.IsValid)
            {
                if (shopId != null)
                    _repository.EditShop(shopId.Value, shCard_.Name, shCard_.Address);
                return RedirectToAction("List");
            }
            else
                return EditShop();
        }

        public ActionResult DeleteShop()
        {
            int? shId = _repository.CurrentShopId;
            var shop = _repository.Shop.Where(sh => sh.Id == shId).FirstOrDefault();
            if ((shop != null) && (shId != null))
                _repository.DeleteShop(shop.Id);

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

        public void SetCurrentShopId(int shId_) => _repository.CurrentShopId = shId_;

        /// <summary>
        /// Получить список магазинов
        /// </summary>
        /// <returns></returns>
        private MShopList getShopsView()
        {
            int? shId = _repository.CurrentShopId;
            var shView = new MShopList()
            {
                ShopId = shId,
                ShopList = _repository.Shop.OrderBy(sh => sh.Name).ToList()
            };
            return shView;
        }

        private IExpensesRepository _repository = null;
    }
}