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
            var Funcionarios = new List<ApplicationUser>();
            var RoleId = db.Roles
                .Where(u => u.Name == "Funcionário")
                .Select(r => r.Id).SingleOrDefault();
         
            foreach (var u in db.Users)
                if (u.Roles.Select(o => o.RoleId).SingleOrDefault() == RoleId)
                {
                    Funcionarios.Add(u);
                }
           
            return View(Funcionarios);
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
                var user = new ApplicationUser { UserName = model.Nome, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Atribui o Peril ao usuário
                    result = await UserManager.AddToRoleAsync(user.Id, model.RoleName);
                    //termina aqui
                   // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

            return RedirectToAction("Index", "Funcionario");
                }
                AddErrors(result);
            }

            List<SelectListItem> list = new List<SelectListItem>();
            foreach(var role in RoleManager.Roles)
            {
                list.Add(new SelectListItem() { Value = role.Name, Text = role.Name });
            }

            ViewBag.Roles = list;

            return View(model);
        }

        // GET: Vagas/Edit/5
        public ActionResult Editar(string id ="")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var func = db.Users.Find(id);
            if (func == null)
            {
                return HttpNotFound();
            }
            return View(func);
        }

        // POST: Vagas/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,UserName,Email")] ApplicationUser func)
        {
            if (ModelState.IsValid)
            {
                db.Entry(func).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(func);
        }


        // GET: Funcionarios/Detalhes/5
        public ActionResult Detalhes(string id ="")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var func= db.Users.Find(id);
            if (func == null)
            {
                return HttpNotFound();
            }
            return View(func);
        }

        //GET: Funcionários/Deletar
        public ActionResult Deletar(string id ="")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var func = db.Users.Find(id);
            if (func == null)
            {
                return HttpNotFound();
            }
            return View(func);
        }
        // POST: Funcionários/Deletar/5
        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id = "")
        {
            var func = db.Users.Find(id);
            db.Users.Remove(func);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


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