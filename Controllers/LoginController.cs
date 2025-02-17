using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TFTrainer.Models;
using TFTrainer.ViewModel;

namespace TFTrainer.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;        

        public LoginController(
            UserManager<User> userManager,
            SignInManager<User> signInManager) 
        { 
            _userManager = userManager;
            _signInManager = signInManager;            
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Register()
        {
            return View();
        }
        
        public async Task<IActionResult> Auth(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);            

            if (user == null)
            {
                return Unauthorized("Usuário ou senha inválidos");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Usuário ou senha inválidos");
            }

            return RedirectToAction("Index", "Painel");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }

        public async Task<IActionResult> RegisterSave(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Register", model);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            
            if(existingUser != null)
            {
                ModelState.AddModelError("", "Este e-mail já está em uso. Escolha outro.");
                return View("Register", model);
            }

            try
            {
                var user = new User
                {
                    Name = model.Name,
                    UserName = model.UserName,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        string mensagemErro = error.Description;

                        if (error.Code == "PasswordTooShort")
                            mensagemErro = "A senha deve conter pelo menos 6 caracteres.";

                        if (error.Code == "PasswordRequiresNonAlphanumeric")
                            mensagemErro = "A senha deve conter pelo menos um caractere especial (@, #, !, etc.).";

                        if (error.Code == "PasswordRequiresUpper")
                            mensagemErro = "A senha deve conter pelo menos uma letra maiúscula.";

                        if (error.Code == "PasswordRequiresDigit")
                            mensagemErro = "A senha deve conter pelo menos um número.";

                        ModelState.AddModelError("", mensagemErro);
                    }
                    return View("Register", model);
                }

                TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Por favor, realize o login.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocorreu um erro ao processar o cadastro. Tente novamente!");
                return View("Register", model);
            }
        }
    }
}
