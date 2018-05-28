using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Go_Parking.Models;

namespace Go_Parking.Controllers
{
    public class VagasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Vagas
        public ActionResult Index()
        {
            return View(db.Vagas.ToList());
        }
            

        // GET: Vagas/Create
        public ActionResult Cadastrar()
        {
          
                var Lista = new List<string>()
             {
                {"Carro"},{"Moto"}
             };
                var ListaPortes = new List<SelectListItem>();
                foreach (var item in Lista)
                    ListaPortes.Add(new SelectListItem() { Value = item, Text = item }); //Preenche a lista com os portes presentes
                ViewBag.ListaPortes = ListaPortes;

                return View();
        }

        // POST: Vagas/Create    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar([Bind(Include = "Id,Nome,Porte")] Vaga vaga)
        {
            if (ModelState.IsValid)
            {
                db.Vagas.Add(vaga);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vaga);
        }

        // GET: Vagas/Edit/5
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
            Vaga vaga = db.Vagas.Find(id);
            if (vaga == null)
            {
                return HttpNotFound();
            }
            return View(vaga);
        }

        // POST: Vagas/Edit/5
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Nome,Porte")] Vaga vaga)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vaga).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vaga);
        }

        // GET: Vagas/Detalhes/5
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaga vaga = db.Vagas.Find(id);
            if (vaga == null)
            {
                return HttpNotFound();
            }
            return View(vaga);
        }
        // GET: Vagas/Deletar/5
        public ActionResult Deletar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vaga vaga = db.Vagas.Find(id);
            if (vaga == null)
            {
                return HttpNotFound();
            }
            return View(vaga);
        }

        // POST: Vagas/Deletar/5
        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vaga vaga = db.Vagas.Find(id);
            db.Vagas.Remove(vaga);
            db.SaveChanges();
            return RedirectToAction("Index");
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
