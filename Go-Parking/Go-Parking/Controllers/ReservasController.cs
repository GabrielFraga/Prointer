using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Go_Parking.Models;
using Microsoft.AspNet.Identity;
using System.Globalization;
using System.Net;

namespace Go_Parking.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservas
        public ActionResult Index()
        {
            var usuarioId = User.Identity.GetUserId();
            var model = new FazerReserva
            {
                Vagas = db.Vagas.ToList(),
                Veiculos = db.Veiculos
                         .Where(v => v.UserId == usuarioId)
                         .Select(v => new SelectListItem() { Value = v.Id.ToString(), Text = v.Modelo }),
                Reservas = db.Reservas.ToList()         
            };
         
            
                      


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Reserva model)
        {
                

                                                            

            var VagaID = Convert.ToInt32(Request.Form["Vagas"]);
            var VeiculoID = Convert.ToInt32(Request.Form["Veiculos"]);
            var usuarioId = User.Identity.GetUserId();

            model.UserId = usuarioId;

            model.VagaId = db.Vagas
                      .Where(v => v.Id == VagaID)
                      .Select(v => v.Id)
                      .FirstOrDefault();         

             model.VeiculoId  = db.Veiculos
                      .Where(v => v.Id == VeiculoID)
                      .Select(v=> v.Id)
                      .FirstOrDefault();

            var Pagamentos = new FazerReserva();
            var listaPagamentos = new List<SelectListItem>();
            foreach (var item in Pagamentos.FormasPagamentos())
            listaPagamentos.Add(new SelectListItem() { Value = item, Text = item });
            ViewBag.FormaPagamentos = listaPagamentos;

            var diff = model.Saida.Subtract(model.Entrada).Hours;
            ViewBag.Diferenca = diff;
            model.Valor = 10 * diff;

            ViewBag.VeiculoModelo= db.Veiculos
                      .Where(v => v.Id == VeiculoID)
                      .Select(v => v.Modelo)
                      .FirstOrDefault();

            ViewBag.VeiculoPlaca = db.Veiculos
                      .Where(v => v.Id == VeiculoID)
                      .Select(v => v.Placa)
                      .FirstOrDefault();

            ViewBag.Vaga= db.Vagas
                      .Where(v => v.Id == VagaID)
                      .Select(v => v.Nome)
                      .FirstOrDefault();

            TempData["Reserva"] = model;


            return View("_Confirmacao", model);
        }
        
        [HttpGet]
        public ActionResult Confirmacao(Reserva model)
        {
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Confirmacao()
        {
                  
            var model = (Reserva)TempData["Reserva"];
            model.FormaPagamento=(Request.Form["FormaPagamentos"]).ToString();

            var teste = new Reserva();
            teste = model;        
                
            db.Reservas.Add(teste);
            db.SaveChanges();

            return RedirectToAction("Detalhes");
        }


        public ActionResult Detalhes()
        {
            var model = db.Reservas;

            return View(model.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Deletar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva== null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // POST: Veiculoes/Delete/5
        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reserva reserva = db.Reservas.Find(id);
            db.Reservas.Remove(reserva);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
