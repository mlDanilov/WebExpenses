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
        public ActionResult ShopList()
        {
            var shops = getShopsView();
            return View(shops);
        }

        public ViewResult CreateShop()
        {
            var shView = new MShopCard();
            ViewData["Head"] = "Добавить";
            return View("ShopCard", shView);
        }
        [HttpPost]
        public ActionResult CreateShop(string name, string address)
        {
            var shop =  _repository.AddNewShop(name, address);
            _repository.CurrentShopId = shop.Id;
            return RedirectToAction("ShopList");
        }

        public ViewResult EditShop()
        {
            int? shId = _repository.CurrentShopId;
            var shop = _repository.Shop.Where(sh => sh.Id == shId).FirstOrDefault();

            var shView = new MShopCard(shop);
            ViewData["Head"] = "Редактировать";
            return View("ShopCard", shView);
        }
        [HttpPost]
        public ActionResult EditShop(string name, string address)
        {
            int? shopId = _repository.CurrentShopId;
            if (shopId != null)
                _repository.EditShop(shopId.Value, name, address);
            return RedirectToAction("ShopList");
        }

        public ActionResult DeleteShop()
        {
            int? shId = _repository.CurrentShopId;
            var shop = _repository.Shop.Where(sh => sh.Id == shId).FirstOrDefault();
            if ((shop != null) && (shId != null))
                _repository.DeleteShop(shop.Id);

            return RedirectToAction("ShopList");
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