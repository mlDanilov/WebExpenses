using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DomainExpenses.Abstract;
using DomainExpenses.Concrete;
using WebExpenses.Models.Shop;
using WebExpenses.Models.Shop.Interfaces;
using System.Net;

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

        /// <summary>
        /// Получить список магазинов
        /// </summary>
        /// <returns></returns>
        public JsonResult GetList()
        {
            try
            {
                var shops = getShopsView();
                
                return Json(shops, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        public ViewResult CreateShop()
        {
            return View("CreateShopCard");
        }

        /// <summary>
        /// Создать магазин
        /// </summary>
        /// <param name="name_"></param>
        /// <param name="address_"></param>
        /// <returns></returns>
        public HttpStatusCodeResult CreateNewShop(string name_, string address_)
        {
            try
            {
                var mShop = new MShopCard() { Name = name_, Address = address_ };
                _repository.ShopRep.Create(mShop);
                //return new HttpStatusCodeResult()
                return new HttpStatusCodeResult(HttpStatusCode.OK, 
                    $"Магазин с кодом {mShop.Id} создан успешно");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                    $"При создании магазина{name_} - {address_} произошла ошибка: {ex.Message}");
            }
        }


        public ViewResult EditShop(int shopId_)
        {
            var shop = _repository.ShopRep.Entities.Where(sh => sh.Id == shopId_).FirstOrDefault();

            var shView = new MShopCard(shop);
            ViewData["Title"] = "Редактировать магазин";
            ViewData["Head"] = "Редактировать";
            return View("EditShopCard", shView);
        }

        [HttpPost]
        public JsonResult EditShopCard(int id_, string name_, string address_)
        {
            //Правим список групп
            var shop = _repository.ShopRep.Entities.Where(sh => sh.Id == id_).FirstOrDefault();
            shop.Name = name_;
            shop.Address = address_;

            IMShopCard mShop = new MShopCard(shop);
            return Json(mShop, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Удалить магазин
        /// </summary>
        /// <param name="shopId_"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpStatusCodeResult DeleteShopById(int shopId_)
        {
            try
            {
                var shop = _repository.ShopRep.Entities.First(sh => sh.Id == shopId_);
                _repository.ShopRep.Delete(shop);
                return new HttpStatusCodeResult(HttpStatusCode.OK, $"Магазин с кодом {shop.Id} удалён успешно");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, $"Ошибка при удалении магазина с кодом {shopId_} : {ex.Message.ToString()}");
            }
        }

        /// <summary>
        /// Получить список магазинов
        /// </summary>
        /// <returns></returns>
        private IMShopList getShopsView()
        {
            IMShopList shopList = new MShopList();
            _repository.ShopRep.Entities.ToList().ForEach(sh=> shopList.Shops.Add(new MShopCard(sh)));
            //Сортируем по названию магазина
            shopList.Shops.OrderBy(sh => sh.Name);
            //Ставим текущим магазином первый в списке
            shopList.Current = (shopList.Shops.Count == 0) ? shopList.Shops[0] : null;
            return shopList;
        }

        private IExpensesRepository _repository = null;
    }
}