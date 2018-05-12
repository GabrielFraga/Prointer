using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Go_Parking.Models;
using Microsoft.AspNet.Identity;

namespace Go_Parking.Controllers
{
    public class ReservasController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reservas
        public ActionResult Index()
        {
            var usuarioId = User.Identity.GetUserId();

            var model = new FazerReserva();
            model.Vagas = db.Vagas.ToList();
            model.Veiculos = db.Veiculos
                         .Where(v => v.UserId == usuarioId)
                         .Select(v => new SelectListItem() { Value = v.Id.ToString(), Text = v.Modelo });
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Reserva model)
        {           
            var VagaID = Convert.ToInt32(Request.Form["Vagas"]);
            var VeiculoID = Convert.ToInt32(Request.Form["Veiculos"]);

            model.Vagas= db.Vagas
                      .Where(v => v.Id == VagaID)
                      .FirstOrDefault();

             model.Veiculos = db.Veiculos
                      .Where(v => v.Id == VeiculoID)
                      .FirstOrDefault();

            
             db.Reservas.Add(model);
             db.SaveChanges();

            return RedirectToAction("Detalhes");
        }
        public ActionResult Detalhes()
        {
            var model = db.Reservas;
            return View(model.ToList());
        }




    }
}