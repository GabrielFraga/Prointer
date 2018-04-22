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
           /* string usuarioId = User.Identity.GetUserId();

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
            list.Add(new SelectListItem() { Value = veiculo.Modelo, Text = veiculo.Modelo });
            ViewBag.Veiculos = list;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reservar([Bind(Include = "Id,VeiculoId,Modelo,placa,UserId")] Vaga vaga)
        {

            if (ModelState.IsValid)
            {        //Continuar daqui

                var veiculo = ViewBag.Veiculos;
                   vaga.Modelo = veiculo.Modelo;
                   vaga.placa = veiculo.placa;
                   vaga.UserId = veiculo.UserId;
                   vaga.VeiculoId = veiculo.Id;
                   
                db.Vagas.Add(vaga);               
                db.SaveChanges();

                return RedirectToAction("Vagas/Details");
            }
            return View(vaga);
        }


        public ActionResult Details(int? id)
        {
            return View();

        }
    }
}