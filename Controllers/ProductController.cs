using SnackLab.Bll;
using SnackLab.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace SnackLab.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private ProductBll bll = new ProductBll();

        // 📌 Openbare lijst van alle producten
        public ActionResult Index()
        {
            var producten = bll.GetAll();
            return View(producten);
        }

        // 📌 Openbare filtering op categorie
        public ActionResult Categorie(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var producten = bll.GetAll()
                .Where(p => p.Category.ToLower() == id.ToLower())
                .ToList();

            ViewBag.Categorie = id;
            return View("Index", producten); // hergebruik index view
        }

        // 📌 Openbare detailpagina van een product met reviews
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var product = bll.Get(id.Value);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        // 🛒 Toevoegen aan winkelmandje (openbaar)
        public ActionResult AddToCart(int id)
        {
            var cart = Session["Cart"] as List<int> ?? new List<int>();
            cart.Add(id);
            Session["Cart"] = cart;

            return RedirectToAction("Cart");
        }

        // 🛍 Inhoud van het winkelmandje bekijken (openbaar)
        public ActionResult Cart()
        {
            var cart = Session["Cart"] as List<int> ?? new List<int>();
            var producten = cart.Select(id => bll.Get(id)).ToList();

            return View(producten);
        }

        // 🔒 Product toevoegen (alleen voor Admin, Winkelbeheerder en Sitebeheerder)
        [Authorize(Roles = "Admin,Winkelbeheerder,Sitebeheerder")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Winkelbeheerder,Sitebeheerder")]
        public ActionResult Create(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Description))
            {
                product.Description = "Geen beschrijving opgegeven";
            }

            if (ModelState.IsValid)
            {
                bll.Add(product);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // 🔒 Product bewerken
        [Authorize(Roles = "Admin,Winkelbeheerder,Sitebeheerder")]
        public ActionResult Edit(int id)
        {
            var product = bll.Get(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Winkelbeheerder,Sitebeheerder")]
        public ActionResult Edit(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Description))
            {
                product.Description = "Geen beschrijving opgegeven";
            }

            if (ModelState.IsValid)
            {
                bll.Update(product);
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // 🔒 Product verwijderen
        [Authorize(Roles = "Admin,Winkelbeheerder,Sitebeheerder")]
        public ActionResult Delete(int id)
        {
            var product = bll.Get(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Winkelbeheerder,Sitebeheerder")]
        public ActionResult DeleteConfirmed(int id)
        {
            bll.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
