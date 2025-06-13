using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SnackLab.WebApp.Models;

namespace SnackLab.WebApp.Controllers
{
    //[Authorize(Roles = "Admin")] in comment geplaatst.
    public class RolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Roles
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string RoleName)
        {
            if (!string.IsNullOrEmpty(RoleName))
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

                if (!roleManager.RoleExists(RoleName))
                {
                    roleManager.Create(new IdentityRole(RoleName));
                    TempData["Success"] = $"Rol '{RoleName}' is aangemaakt!";
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Rol bestaat al.");
            }
            else
            {
                ModelState.AddModelError("", "Rolnaam mag niet leeg zijn.");
            }

            return View();
        }
    }
}
