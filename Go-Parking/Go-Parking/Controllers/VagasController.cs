using Go_Parking.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Go_Parking.Controllers
{
    public class VagasController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Vagas
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DetalheVaga()
        {
            /*string usuarioId = User.Identity.GetUserId();

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var veiculo in db.Veiculoes.Include(v => v.Users).Where(v => v.UserId.Equals(usuarioId)))
            list.Add(new SelectListItem() { Value = veiculo.Modelo , Text = veiculo.Modelo });
            ViewBag.Veiculos = list;*/
            return View();
        }
        
        
        public ActionResult Reservar()
        {
            string usuarioId = User.Identity.GetUserId();

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var veiculo in db.Veiculoes.Include(v => v.Users).Where(v => v.UserId.Equals(usuarioId)))
            list.Add(new SelectListItem() { Value = veiculo.Id.ToString(), Text = veiculo.Modelo });
            ViewBag.Veiculos = list; 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reservar(string Veiculo)
        {

            if (ModelState.IsValid)
            {       
                int veiculos = Convert.ToInt32(Veiculo);
                var vaga = new Vaga();
                string usuarioId = User.Identity.GetUserId();

                vaga.VeiculoId = veiculos;
                vaga.UserId = usuarioId;

                vaga.Modelo = (from a in db.Veiculoes
                             where a.Id == (veiculos)
                             select a.Modelo).Single();                            
                vaga.placa = (from a in db.Veiculoes
                              where a.Id == veiculos
                              select a.placa).Single();

                db.Vagas.Add(vaga);
                db.SaveChanges();
          
            }
            return RedirectToAction("Details");
        }


        public ActionResult Details()
        {
            string usuarioId = User.Identity.GetUserId();

            var vaga = db.Vagas.Include( a => a.veiculo).Where(a => a.UserId.Equals(usuarioId));
            return View(vaga.ToList());

        }
    }
}