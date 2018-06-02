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
            var reservas = new List<Relatorio>();
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
                relatorio.HorasReservadas =  relatorio.HorasReservadas.Add(r.Saida.Subtract(r.Entrada));
                relatorio.Entrada = r.Entrada;
                relatorio.Saida = r.Saida;
                reservas.Add(relatorio);
            }

            var listaReservas = from s in reservas select s;
            listaReservas = listaReservas.OrderByDescending(s => s.Entrada);
            return View(listaReservas);
        }
            
                return View();

            
        }
        public ActionResult _ReservasCliente()
        {
            
            return View();
        }









    }
}