using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Go_Parking.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Net;
using System.Data.Entity;

namespace Go_Parking.Controllers
{
    public class FuncionarioController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;


        public FuncionarioController()
        {
        }

        public FuncionarioController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;   
        }
        
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        public ActionResult Index()
        {
            return View();
        }


       

        // GET: /FuncionarioLogin
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        
        // POST: /FuncionarioLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            var user = await UserManager.FindAsync(model.Email, model.Password);

            switch (result)
            {
                case SignInStatus.Success:
                    {
                        if (UserManager.IsInRole(user.Id, "Admin"))
                            return RedirectToAction("Index", "Funcionario");

                       else if (UserManager.IsInRole(user.Id, "Funcionário"))
                            return RedirectToAction("Index", "Funcionario");

                        return RedirectToLocal(returnUrl);
                    }
                    
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return RedirectToAction("Index", "Funcionario");
            }
        }
        
        // GET: /Registrar/Funcionário
        [AllowAnonymous]
        public ActionResult Cadastrar()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach(var role in RoleManager.Roles.Where(r=>r.Name !="Cliente"))
                list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
            ViewBag.Roles = list;
            return View();
        }
            
        // POST: /Registrar/Funcionário
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>Cadastrar(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Atribui o Peril ao usuário
                    result = await UserManager.AddToRoleAsync(user.Id, model.RoleName);
                    //termina aqui
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            List<SelectListItem> list = new List<SelectListItem>();
            foreach(var role in RoleManager.Roles)
            {
                list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
            }

            ViewBag.Roles = list;

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Usuarios()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var usuario in User.Identity.Name)
                list.Add(new SelectListItem { Value = User.Identity.Name, Text =User.Identity.Name});
            return View(list);

        }


        /*

        [AllowAnonymous]
        public ActionResult UserList()
        {
            var context = new Models.ApplicationDbContext();
            return View(context.Users.ToList());
        }
        [AllowAnonymous]
        public ActionResult UserDelete(string id)
        {
            var context = new Models.ApplicationDbContext();
            var user = context.Users.Where(u => u.Id == id).FirstOrDefault();
            return View(user);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UserDelete(ApplicationUser appuser)
        {
            var context = new Models.ApplicationDbContext();
            var user = context.Users.Where(u => u.Id == appuser.Id).FirstOrDefault();
            context.Users.Remove(user);
            context.SaveChanges();
            //var user = context.Users.Where(u => u.Id == id.ToString()).FirstOrDefault();
            return RedirectToAction("UserList");
        }
        [AllowAnonymous]
        public ActionResult UserEdit(string id)
        {
            var context = new Models.ApplicationDbContext();
            var user = context.Users.Where(u => u.Id == id).FirstOrDefault();
            return View(user);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult UserEdit(ApplicationUser appuser)
        {
            var context = new Models.ApplicationDbContext();
            var user = context.Users.Where(u => u.Id == appuser.Id).FirstOrDefault();
            //context.Entry(appuser).State = EntityState.Modified;
            user.Email = appuser.Email;
            user.UserName = appuser.UserName;
            user.PhoneNumber = appuser.PhoneNumber;
            user.PasswordHash = user.PasswordHash;
            context.SaveChanges();
            //var user = context.Users.Where(u => u.Id == id.ToString()).FirstOrDefault();
            return RedirectToAction("UserList");
        }



        /*
                [Authorize(Roles = "Admin")]
                public ActionResult Index()
                {
                    var Db = new ApplicationDbContext();
                    var users = Db.Users;
                    var model = new List<EditUserViewModel>();
                    foreach (var user in users)
                    {
                        var u = new EditUserViewModel(user);
                        model.Add(u);
                    }
                    return View(model);
                }



                [Authorize(Roles = "Admin")]
                public ActionResult Delete(string id = null)
                {
                    var Db = new ApplicationDbContext();
                    var user = Db.Users.First(u => u.UserName == id);
                    var model = new EditUserViewModel(user);
                    if (user == null)
                    {
                        return HttpNotFound();
                    }
                    return View(model);
                }


                [HttpPost, ActionName("Delete")]
                [ValidateAntiForgeryToken]
                [Authorize(Roles = "Admin")]
                public ActionResult DeleteConfirmed(string id)
                {
                    var Db = new ApplicationDbContext();
                    var user = Db.Users.First(u => u.UserName == id);
                    Db.Users.Remove(user);
                    Db.SaveChanges();
                    return RedirectToAction("Index");
                }

            */


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

    }


}