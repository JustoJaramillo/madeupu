using madeupu.API.Data;
using madeupu.API.Helpers;
using madeupu.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace madeupu.API.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IUserHelper _iuserHelper;
        private readonly DataContext _context;
        /*private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;*/

        public AccountsController(IUserHelper iuserHelper, DataContext context/*, ICombosHelper combosHelper, IBlobHelper blobHelper, IMailHelper mailHelper*/)
        {
            _iuserHelper = iuserHelper;
            _context = context;
            /*_combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;*/
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(Index), "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _iuserHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            }

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _iuserHelper.LogoutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
