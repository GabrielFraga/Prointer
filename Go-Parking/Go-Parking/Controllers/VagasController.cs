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


        public ActionResult Index()
        {
            var vagas = db.Vagas.ToList();
            var listaVagas = new List<VagasViewModel>();
            foreach (var v in db.Vagas)
            {
                var model = new VagasViewModel();
                model.Id = v.Id;
                model.Nome = v.Nome;
                model.Tipo = v.Tipo;

                foreach (var i in db.Reservas.Where(u => u.VagaId == model.Id))
                {
                    var entrada = i.Entrada;
                    if (entrada.DateTime > DateTime.Today)
                    {
                        model.Reservas = +1;
                    }
                }
                listaVagas.Add(model);
            }

            return View(listaVagas);
        }
        



        // GET: Vagas
        public ActionResult _Listagem()
        {
            return View();
        }
            

        // GET: Vagas/Create
        public ActionResult Cadastrar()
        {
          
                var Lista = new List<string>()
             {
                {"Carro"},{"Moto"}
             };
                var ListaTipos = new List<SelectListItem>();
                foreach (var item in Lista)
                    ListaTipos.Add(new SelectListItem() { Value = item, Text = item }); //Preenche a lista com os portes presentes
                ViewBag.ListaTipos = ListaTipos;

                return View();
        }

        // POST: Vagas/Create    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar([Bind(Include = "Id,Nome,Tipo")] Vaga vaga)
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
                var ListaTipos = new List<SelectListItem>();
                foreach (var item in Lista)
                    ListaTipos.Add(new SelectListItem() { Value = item, Text = item }); //Preenche a lista com os portes presentes
                ViewBag.ListaTipos = ListaTipos;
  
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
        public ActionResult Editar([Bind(Include = "Id,Nome,Tipo")] Vaga vaga)
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

            var model = new List<VagaReservasViewModel>();

            ViewBag.VagaNome = db.Vagas.Where(d => d.Id == id).Select(u => u.Nome).FirstOrDefault();
            foreach (var i in db.Reservas.Where(u => u.VagaId == vaga.Id))
            {
                var entrada = i.Entrada;
                if (entrada.DateTime > DateTime.Today)
                {
                    var reserva = new VagaReservasViewModel();
                    reserva.UsuarioNome = db.Users
                        .Where(u => u.Id == i.UserId)
                        .Select(p => p.UserName)
                        .FirstOrDefault();
                    reserva.Email = db.Users
                        .Where(u => u.Id == i.UserId)
                        .Select(p => p.Email)
                        .FirstOrDefault();
                    reserva.Modelo = db.Veiculos
                        .Where(u => u.Id == i.VeiculoId)
                        .Select(o => o.Modelo)
                        .FirstOrDefault();
                    reserva.Placa = db.Veiculos
                        .Where(u => u.Id == i.VeiculoId)
                        .Select(o => o.Placa)
                        .FirstOrDefault();
                    reserva.Cor = db.Veiculos
                        .Where(u => u.Id == i.VeiculoId)
                        .Select(o => o.Cor)
                        .FirstOrDefault();
                    var intervalo = (i.Saida.Subtract(i.Entrada));
                    reserva.HorasReservadas = intervalo.Hours + ":" + intervalo.Minutes.ToString("00.##");
                    reserva.Entrada = i.Entrada.ToString("dd/MM/yy  HH:mm");
                    reserva.Saida = i.Saida.ToString("dd/MM/yy  HH:mm");
                    reserva.Valor = i.Valor;
                    reserva.ReservaId = i.Id;
                    model.Add(reserva);
                    TempData["VagaReservada"] = reserva;
                }
            }

            return View(model.OrderByDescending(s=> DateTimeOffset.Parse(s.Saida)));
        }

        public ActionResult _LiberarVaga(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            var model = (VagaReservasViewModel)TempData["VagaReservada"];
            TempData["Id"] = id;
            ViewBag.NomeVaga = db.Vagas.Where(o=>o.Id==id).Select(n=>n.Nome).FirstOrDefault();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _LiberarVaga()
        {
            var reserva = db.Reservas.Find((int)TempData["Id"]);

            if (ModelState.IsValid)
               {
                if ((reserva.Saida.DateTime < DateTime.Now) || (reserva.Saida.DateTime > DateTime.Now))
                    TempData["MensagemErro"] = "Vaga não está ocupada.";
                return RedirectToAction("Detalhes", new { id = reserva.VagaId });
                
                }
                else {
                    reserva.Saida = DateTimeOffset.Now;
                var tempoReservado = reserva.Saida.Subtract(reserva.Entrada); //É subtraído a hora de saída da hora de entrada para obter o tempo reservado.             
                if (tempoReservado.Minutes >= 30)
                    reserva.Valor = 5;
                reserva.Valor = reserva.Valor + 10 * tempoReservado.Hours;
                db.Entry(reserva).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["MensagemSucesso"] = "Vaga liberada com sucesso";
                    return RedirectToAction("Index");
                    }                       
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
