using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Almacen.Models.ViewModels;
using Almacen.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Almacen.Models;

namespace Almacen.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IUserValidator<User> userValidator;
        private UserManager<User> userManager;
        private IPasswordHasher<User> passwordHasher;
        private IPasswordValidator<User> passwordValidator;
        private SignInManager<User> signInManager;
        private Cart sessionCart;

        public AccountController(IUserValidator<User> usrVal,
                                 UserManager<User> usrMgr, 
                                 IPasswordHasher<User> passMgr,
                                 IPasswordValidator<User> passVal,
                                 SignInManager<User> signMan,
                                 Cart cart)
        {
            userManager = usrMgr;
            userValidator = usrVal;
            passwordHasher = passMgr;
            passwordValidator = passVal;
            signInManager = signMan;
            sessionCart = cart;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                User user = await userManager.FindByEmailAsync(details.Email);
                if(user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if(result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email),
                    "Usuario invalido o contraseña incorrecta");
            }
            return View(details);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            if(sessionCart != null)
            {
                sessionCart.Clear();
            }
            return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        public ViewResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if(ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                IdentityResult result =
                    await userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    IdentityResult roleResult = await userManager.AddToRoleAsync(user, "Client");
                    if (!roleResult.Succeeded)
                    {
                        AddErrorsFromResult(roleResult);
                    }

                    LoginModel loginModel = new LoginModel()
                    {
                        Email = model.Email,
                        Password = model.Password
                    };

                    await Login(loginModel, "/Home/Index");
                    return Redirect("/Home/Index");
                }
                else
                {
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if(user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Users", "Admin", userManager.Users);
                else
                    AddErrorsFromResult(result);
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return RedirectToAction("Users", "Admin", userManager.Users);
        }      

        public async Task<IActionResult> Edit(string username, string returnUrl)
        {
            User user = await userManager.FindByNameAsync(username);
            if(user != null)
            {
                UserEditViewModel userEditViewModel = new UserEditViewModel()
                {
                    Address = user.Address,
                    City = user.City,
                    Province = user.Province,
                    ZipCode = user.ZipCode,
                    DNI = user.DNI,
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.UserName
                };
                if (returnUrl != null)
                    userEditViewModel.ReturnUrl = returnUrl;
                else
                    userEditViewModel.ReturnUrl = "/";
                return View(userEditViewModel);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel details)
        {
            User user = await userManager.FindByIdAsync(details.Id);
            IdentityResult result;
            if (user != null)
            {
                if (!string.IsNullOrEmpty(details.Password))
                {
                    result = await passwordValidator.ValidateAsync(userManager, user, details.Password);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, details.Password);
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }

                if (user.Email != details.Email && !string.IsNullOrEmpty(details.Email))
                {
                    user.Email = details.Email;
                    result = await userValidator.ValidateAsync(userManager, user);
                    if (!result.Succeeded)
                    {
                        AddErrorsFromResult(result);
                    }
                }

                if (user.Name != details.Name)
                    user.Name = details.Name;
                if (user.LastName != details.LastName)
                    user.LastName = details.LastName;
                if (user.DNI != details.DNI)
                    user.DNI = details.DNI;
                if (user.Address != details.Address)
                    user.Address = details.Address;
                if (user.City != details.City)
                    user.City = details.City;
                if (user.Province != details.Province)
                    user.Province = details.Province;
                if (user.ZipCode != details.ZipCode)
                    user.ZipCode = details.ZipCode;

                result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Usuario no encontrado");
            }
            return Redirect(details.ReturnUrl ?? "/");
    }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}