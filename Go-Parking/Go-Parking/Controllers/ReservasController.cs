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

            var listaHorarios = new List<SelectListItem>();
            foreach (var item in model.HorasReserva())
                listaHorarios.Add(new SelectListItem() { Value = item, Text = item });
            ViewBag.ListaHorarios = listaHorarios;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Reserva model)
        {
            model.Entrada = model.Entrada.Add(TimeSpan.Parse((Request.Form["ListaEntrada"]).ToString()));
            model.Saida = model.Saida.Add(TimeSpan.Parse((Request.Form["ListaSaida"]).ToString()));

            model.VagaId = Convert.ToInt32(Request.Form["Vagas"]);
            model.VeiculoId = Convert.ToInt32(Request.Form["Veiculos"]);
            model.UserId = User.Identity.GetUserId();
                       
            var modelo = new FazerReserva();
            var listaPagamentos = new List<SelectListItem>();
            foreach (var item in modelo.FormasPagamentos())
                listaPagamentos.Add(new SelectListItem() { Value = item, Text = item });
            ViewBag.FormaPagamentos = listaPagamentos;

            var tempoReservado = model.Saida.Subtract(model.Entrada).Hours;           
            ViewBag.TempoReservado = tempoReservado;
            model.Valor = 10 * tempoReservado;

            ViewBag.VeiculoModelo= db.Veiculos
                      .Where(v => v.Id == model.VeiculoId)
                      .Select(v => v.Modelo)
                      .FirstOrDefault();

            ViewBag.VeiculoPlaca = db.Veiculos
                      .Where(v => v.Id == model.VeiculoId)
                      .Select(v => v.Placa)
                      .FirstOrDefault();

            ViewBag.Vaga= db.Vagas
                      .Where(v => v.Id == model.VagaId)
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
                
            db.Reservas.Add(model);
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
