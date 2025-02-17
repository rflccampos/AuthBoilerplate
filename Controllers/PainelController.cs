using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TFTrainer.Models;

namespace TFTrainer.Controllers
{
    [Authorize]
    public class PainelController : Controller
    {
        private readonly UserManager<User> _userManager;

        public PainelController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Index", "Login");


            return View(user);
        }
    }
}
