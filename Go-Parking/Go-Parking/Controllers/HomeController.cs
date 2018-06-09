using Go_Parking.Models;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Go_Parking.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User.IsInRole("Cliente"))
            { 
            var userID = User.Identity.GetUserId();
            var VagasReservas = new VagaReserva();
                var lista = new List<Relatorio>();
            foreach (var r in db.Reservas.Where(u => u.UserId == userID))
            {
                var relatorio = new Relatorio();

                relatorio.VagaNome = db.Vagas
                    .Where(u => u.Id == r.VagaId)
                    .Select(o => o.Nome)
                    .FirstOrDefault();
                relatorio.UsuarioNome = db.Users
                    .Where(u => u.Id == r.UserId)
                    .Select(o => o.UserName)
                    .FirstOrDefault();
                relatorio.Email = db.Users
                    .Where(u => u.Id == r.UserId)
                    .Select(o => o.Email)
                    .FirstOrDefault();
                relatorio.Modelo = db.Veiculos
                    .Where(u => u.Id == r.VeiculoId)
                    .Select(o => o.Modelo)
                    .FirstOrDefault();
                relatorio.Placa = db.Veiculos
                    .Where(u => u.Id == r.VeiculoId)
                    .Select(o => o.Placa)
                    .FirstOrDefault();
                relatorio.Valor = r.Valor;
                var intervalo = (r.Saida.Subtract(r.Entrada));
                relatorio.HorasReservadas = intervalo.Hours + ":" + intervalo.Minutes.ToString("00.##");
                relatorio.Entrada = r.Entrada.ToString("dd/MM/yy  HH:mm"); 
                relatorio.Saida = r.Saida.ToString("dd/MM/yy  HH:mm"); 
                lista.Add(relatorio);
            }

            var listaReservas = from s in lista select s;
            listaReservas = listaReservas.OrderByDescending(s => DateTimeOffset.Parse(s.Saida));
                VagasReservas.Reservas = listaReservas.ToList();
            return View(VagasReservas);
        }
            else
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
                var lista = new VagaReserva();
                lista.Vagas = listaVagas;
                return View(lista);

            }
        }
        public ActionResult _ReservasCliente()
        {
            
            return View();
        }









    }
}