using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Go_Parking.Models;
using Microsoft.AspNet.Identity;

namespace Go_Parking.Controllers
{
    [Authorize (Roles ="Cliente")]
    public class VeiculosController : Controller
    {       
        private ApplicationDbContext db = new ApplicationDbContext();        
        // GET: Veiculoes
        public ActionResult Index()
        {
            string usuarioId = User.Identity.GetUserId();
            var veiculos = db.Veiculos.Where(v => v.UserId == usuarioId);

            return View(veiculos.ToList());
        }

        // GET: Veiculoes/Details/5
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veiculo veiculo = db.Veiculos.Find(id);
            if (veiculo == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", veiculo.UserId);
            return View(veiculo);
        }

        // GET: Veiculos/Cadastrar
        public ActionResult Cadastrar()
        {
            var Lista = new List<string>()
        {
                {"Carro"},{"Moto"}
        };
            var ListaPortes = new List<SelectListItem>();
            foreach (var item in Lista)
                ListaPortes.Add(new SelectListItem() { Value = item, Text = item}); //Preenche a lista com os portes presentes
            ViewBag.ListaPortes = ListaPortes;
            return View();
        }

        // POST: Veiculos/Cadastrar

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar([Bind(Include = "Id,Modelo,Placa,Porte,Marca,Cor")] Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                veiculo.UserId = User.Identity.GetUserId();
                db.Veiculos.Add(veiculo);
                db.SaveChanges();
                return RedirectToAction("Index"); 
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View(veiculo);
        }

        // GET: Veiculos/Editar/5
        public ActionResult Editar(int? id)
        {

            var Lista = new List<string>()
             {
                {"Carro"},{"Moto"}
              };
            var ListaPortes = new List<SelectListItem>();
            foreach (var item in Lista)
                ListaPortes.Add(new SelectListItem() { Value = item, Text = item }); //Preenche a lista com os portes presentes
            ViewBag.ListaPortes = ListaPortes;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veiculo veiculo = db.Veiculos.Find(id);
            if (veiculo == null)
            {
                return HttpNotFound();
            }
            return View(veiculo);
        }

        // POST: Veiculos/Editar/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Modelo,Placa,Porte,Marca,Cor")] Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                veiculo.UserId = User.Identity.GetUserId();
                db.Entry(veiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", veiculo.UserId);
            return View(veiculo);
        }

        // GET: Veiculoe/Deletar/5
        public ActionResult Deletar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veiculo veiculo = db.Veiculos.Find(id);
            if (veiculo == null)
            {
                return HttpNotFound();
            }
            return View(veiculo);
        }

        // POST: Veiculoes/Delete/5
        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Veiculo veiculo = db.Veiculos.Find(id);
            db.Veiculos.Remove(veiculo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult VeiculosDropDown()
        {

            var usuarioId = User.Identity.GetUserId();

            var list = db.Veiculos
                         .Where(v => v.UserId == usuarioId)
                         .Select(v => new SelectListItem() { Value = v.Id.ToString(), Text = v.Modelo });
            ViewBag.Veiculos = list;
            return View("_VeiculosDropDown");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
