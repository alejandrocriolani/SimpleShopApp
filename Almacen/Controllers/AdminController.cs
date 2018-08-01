using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Almacen.Models.Data;
using Microsoft.AspNetCore.Authorization;

namespace Almacen.Controllers
{
    public class AdminController : Controller
    {
        private IUserValidator<User> userValidator;
        private UserManager<User> userManager;
        private IPasswordHasher<User> passwordHasher;
        private IPasswordValidator<User> passwordValidator;

        public AdminController(IUserValidator<User> usrVal,
                               UserManager<User> usrMgr,
                               IPasswordHasher<User> passMgr,
                               IPasswordValidator<User> passVal)
        {
            userManager = usrMgr;
            userValidator = usrVal;
            passwordHasher = passMgr;
            passwordValidator = passVal;
        }

        [Authorize(Roles ="Admin,Employee")]
        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Users()
        {
            return View(userManager.Users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail =
                    await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager, user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null) ||
                    (validEmail.Succeeded && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}