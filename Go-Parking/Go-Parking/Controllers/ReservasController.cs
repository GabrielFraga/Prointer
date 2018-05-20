using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Go_Parking.Models;
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace Go_Parking.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservas
        public ActionResult Index(FazerReserva teste)
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

            model.Vagas= db.Vagas
                      .Where(v => v.Id == VagaID)
                      .FirstOrDefault();
             model.Vagas.Ocupada = true;

             model.Veiculos = db.Veiculos
                      .Where(v => v.Id == VeiculoID)
                      .FirstOrDefault();
            model.Veiculos.Users.UserName = db.Users.Find(usuarioId).UserName;


            var j = new FazerReserva();
            var li = new List<SelectListItem>();

            foreach (var item in j.FormasPagamentos())
                li.Add(new SelectListItem() { Value = item, Text = item });
            ViewBag.FormaPagamentos = li;

            var diff = model.Saida.Subtract(model.Entrada).Hours;
            Session["Diferenca"] = diff;
            model.Valor = 10 * diff;
            Session["Reserva"] = model;


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
                   
            var model = (Reserva)Session["Reserva"];
            model.FormaPagamento=(Request.Form["FormaPagamentos"]).ToString();

            db.Reservas.Add(model);
            db.SaveChanges();
            return View("Detalhes");
        }

       
        public ActionResult Detalhes()
        {
            var model = db.Reservas;

            return View(model.ToList());
        }
    }
}