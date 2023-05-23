using EasyCashIdentityProject.EntityLayer.Concrete;
using EasyCashIdentityProject.Presentation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyCashIdentityProject.Presentation.Controllers
{
    public class ConfirmMailController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public ConfirmMailController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var mail = TempData["Mail"];
            ViewBag.mail = mail;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ConfirmMailViewModel confirmMailViewModel)
        {
            AppUser user = await _userManager.FindByEmailAsync(confirmMailViewModel.Email);
            if (user.ConfirmCode == confirmMailViewModel.Code)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("index", "Login");
            }

            return View();
        }
    }
}
