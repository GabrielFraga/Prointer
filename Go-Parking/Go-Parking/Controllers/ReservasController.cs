﻿using System;
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

        //Instancia a conexão com o banco
        ApplicationDbContext db = new ApplicationDbContext();


        //Preenche o objeto com a lista de vagas e de veículos do usuário
        // GET: Reservas
        public ActionResult Index()
        {

         
                
            var usuarioId = User.Identity.GetUserId(); //Captura a Id do usuário conectado
            var model = new FazerReserva // Instancia o objeto que mostrará o dados para o usuário
            {
                Vagas = db.Vagas.ToList(), //Lista de vagas vinda do banco
                Veiculos = db.Veiculos     //Lista de veículos que possuíem o id do usuário (Veículos do usuário logado)
                         .Where(v => v.UserId == usuarioId)
                         .Select(v => new SelectListItem() { Value = v.Id.ToString(), Text = v.Modelo +" - "+ v.Placa }),
                Reservas = db.Reservas.ToList()         
            };

            //Lista horários prédefinidos
            var listaHorarios = new List<SelectListItem>(); 
            foreach (var item in model.HorasReserva())
                listaHorarios.Add(new SelectListItem() { Value = item, Text = item }); //Preenche a lista com os horários presentes no objeto
            ViewBag.ListaHorarios = listaHorarios; // Envia lista através do ViewBag para a view diretamente
            return View(model); // Retorna o objeto preenchido para a view Index
        }

        [HttpPost] //Post significa que o controller irá receber dados da view. Ou seja, usuário clicou em Reservar
        [ValidateAntiForgeryToken]
        public ActionResult Index(Reserva model) //Recebe como parâmetro a classe reserva, porém captura somente a data. Esta classe será salva no banco
        {
            model.Entrada = model.Entrada.Add(TimeSpan.Parse((Request.Form["ListaEntrada"]).ToString())); //Captura a hora selecionada na ViewBag 
            model.Saida = model.Saida.Add(TimeSpan.Parse((Request.Form["ListaSaida"]).ToString()));       //Captura a hora selecionada na ViewBag

            var tempoReservado = model.Saida.Subtract(model.Entrada).Hours; //É subtraído a hora de saída da hora de entrada para obter o tempo reservado.   

            
            ViewBag.TempoReservado = tempoReservado;                        //Envia o tempo reservado diretamente para a View 
            model.Valor = 10 * tempoReservado; ;                            //Calcula o valor do tempo reservado baseado em um valor simbólico para exemplo.

            model.VagaId = Convert.ToInt32(Request.Form["Vagas"]); //Captura a vaga selecionada na lista de vagas
            model.VeiculoId = Convert.ToInt32(Request.Form["Veiculos"]); //Captura o veículo selecionado na lista de veículos
            model.UserId = User.Identity.GetUserId();                     //Salva a ID do usuário no model
                       
            var modelo = new FazerReserva(); //Estancia o objeto novamente para listar as formas de pagamento
            var listaPagamentos = new List<SelectListItem>();
            foreach (var item in modelo.FormasPagamentos())
                listaPagamentos.Add(new SelectListItem() { Value = item, Text = item });
            ViewBag.FormaPagamentos = listaPagamentos; //Envia diretamente para a view, através da ViewBag, a lista de formas de pagamento

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

            TempData["Reserva"] = model; //Salva dados da reserva para posteriormente salvar no banco. Aguardando confirmação


            return View("_Confirmacao", model); //Passa os dados para o model Confirmação
        }
        
        [HttpGet]
        public ActionResult Confirmacao(Reserva model) //Recebe os dados
        {
            
            return View(model); //Exibe os dados
        }

        [HttpPost] //Assim que o usuário confirma a reserva, esta classe é chamada. POST
        public ActionResult Confirmacao()
        {
                  
            var model = (Reserva)TempData["Reserva"]; //Estancia a reserva para receber o que já foi salvo da view anterior
            model.FormaPagamento=(Request.Form["FormaPagamentos"]).ToString(); //Recebe por fim  a forma de pagamento escolhida                
                
            db.Reservas.Add(model); //adiciona a reserva ao banco na tabela reservas
            db.SaveChanges();       //Salva as alterações feitas

            return RedirectToAction("Detalhes");    //Redireciona o usuário para a tela de detalhes, relatório
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
