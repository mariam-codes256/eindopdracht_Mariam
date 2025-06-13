using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SnackLab.WebApp.Models;

namespace SnackLab.WebApp.Controllers
{
    [Authorize(Roles = "Admin,Sitebeheerder,Winkelbeheerder")]
    public class AdminController : Controller
    {
        // 🔹 Dashboardpagina
        public ActionResult Index()
        {
            return View();
        }

        // 🔹 Gebruikerslijst met dynamische rollen
        public ActionResult Gebruikers()
        {
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // ✅ Alle beschikbare rollen ophalen uit de databank
            var alleRollen = context.Roles.Select(r => r.Name).ToList();

            var users = context.Users.ToList();

            var model = users.Select(u => new GebruikerViewModel
            {
                UserId = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                Rollen = userManager.GetRoles(u.Id).ToList(),
                AlleRollen = alleRollen
            }).ToList();

            return View(model);
        }

        // 🔹 Rol verwijderen
        [HttpPost]
        public ActionResult VerwijderRol(string userId, string rol)
        {
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            userManager.RemoveFromRole(userId, rol);

            return RedirectToAction("Gebruikers");
        }

        // 🔹 Rol toevoegen
        [HttpPost]
        public ActionResult VoegRolToe(string userId, string nieuweRol)
        {
            if (string.IsNullOrWhiteSpace(nieuweRol)) return RedirectToAction("Gebruikers");

            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // ❗ Vermijd dubbele rol
            if (!userManager.IsInRole(userId, nieuweRol))
            {
                userManager.AddToRole(userId, nieuweRol);
            }

            return RedirectToAction("Gebruikers");
        }
    }

    // ✅ ViewModel
    public class GebruikerViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Rollen { get; set; }

        // Nieuw veld om alle rollen te tonen in dropdown
        public List<string> AlleRollen { get; set; }
    }
}
