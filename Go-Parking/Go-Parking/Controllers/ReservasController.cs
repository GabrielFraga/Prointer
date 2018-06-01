using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Go_Parking.Models;
using Microsoft.AspNet.Identity;
using System.Globalization;
using System.Net;
using PagedList;

namespace Go_Parking.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {

        //Instancia a conexão com o banco
        ApplicationDbContext db = new ApplicationDbContext();


        //Preenche o objeto com a lista de vagas e de veículos do usuário
        // GET: Reservas
        [HttpGet]
        public ActionResult Index(string Entrada = "", string Saida = "", string VeiculoId = "", string HoraEntrada = "", string HoraSaida = "")
        {

            var usuarioId = User.Identity.GetUserId(); //Captura a Id do usuário conectado
            var model = new ListarObjetos // Instancia o objeto que mostrará o dados para o usuário
            {
                Veiculos = db.Veiculos     //Lista de veículos que possuíem o id do usuário (Veículos do usuário logado)
                         .Where(v => v.UserId == usuarioId)
                         .Select(v => new SelectListItem() { Value = v.Id.ToString(), Text = v.Modelo + " - " + v.Placa }),
               
            };
            //Lista horários prédefinidos
            var listaHorarios = new List<SelectListItem>();
            foreach (var item in model.HorasReserva())
                listaHorarios.Add(new SelectListItem() { Value = item, Text = item }); //Preenche a lista com os horários presentes no objeto
            ViewBag.ListaHorarios = listaHorarios; // Envia lista através do ViewBag para a view diretamente

           
            
            if (!string.IsNullOrEmpty(Entrada))
            {            
                var DataHoraEntrada = DateTimeOffset.Parse(Entrada).Add(TimeSpan.Parse(HoraEntrada));
                var DataHoraSaida = DateTimeOffset.Parse(Saida).Add(TimeSpan.Parse(HoraSaida));

                TempData["Entrada"] = DataHoraEntrada;
                TempData["Saida"] = DataHoraSaida;
                TempData["VeiculoId"] = VeiculoId;
                var VeiculoID = Convert.ToInt32(VeiculoId);
                var ListaReservas = db.Reservas.ToList();
                model.Vagas = db.Vagas.ToList();
                foreach( var reserva in ListaReservas)
                {
                   if ((DataHoraEntrada == reserva.Entrada) || (DataHoraEntrada == reserva.Saida) || (DataHoraEntrada> reserva.Entrada) && (DataHoraEntrada < reserva.Saida) )
                    {
                        var vaga = model.Vagas
                                    .Where(p => p.Id == reserva.VagaId).SingleOrDefault();
                        vaga.Ocupada = true;
                    }     
                   else if ((DataHoraSaida == reserva.Saida) || (DataHoraSaida == reserva.Entrada) || (DataHoraSaida < reserva.Saida) && (DataHoraSaida > reserva.Entrada))
                    {
                        var vaga = model.Vagas
                                    .Where(p => p.Id == reserva.VagaId).SingleOrDefault();
                        vaga.Ocupada = true;
                    }


                }
                model.Vagas = model.Vagas.Where(p => p.Ocupada == false)
                    .Where(p => p.Porte == (db.Veiculos.Where(o=>o.Id == VeiculoID).Select(i=>i.Porte).FirstOrDefault()))
                    .ToList();
                return View(model);
            }

            return View(model); // Retorna o objeto preenchido para a view Index
        }

        [HttpPost] //Post significa que o controller irá receber dados da view. Ou seja, usuário clicou em Reservar
        [ValidateAntiForgeryToken]
        public ActionResult Index() //Recebe como parâmetro a classe reserva, porém captura somente a data. Esta classe será salva no banco
        {
            var model = new Reserva();

            model.Entrada = (DateTimeOffset)TempData["Entrada"]; //Captura a hora selecionada na ViewBag 
            model.Saida = (DateTimeOffset)TempData["Saida"];       //Captura a hora selecionada na ViewBag
            model.VeiculoId = Convert.ToInt32((string)TempData["VeiculoId"]);
            model.UserId = User.Identity.GetUserId();                     //Salva a ID do usuário no model

            var tempoReservado = model.Saida.Subtract(model.Entrada).Hours; //É subtraído a hora de saída da hora de entrada para obter o tempo reservado.             
            ViewBag.TempoReservado = tempoReservado;                        //Envia o tempo reservado diretamente para a View 
            model.Valor = 10 * tempoReservado; ;                            //Calcula o valor do tempo reservado baseado em um valor simbólico para exemplo.

            model.VagaId = Convert.ToInt32(Request.Form["Vagas"]); //Captura a vaga selecionada na lista de vagas
                       
            var modelo = new ListarObjetos(); //Estancia o objeto novamente para listar as formas de pagamento
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

            return RedirectToAction("Index", "Home");    //Redireciona o usuário para a tela de detalhes, relatório
        }


        public ActionResult Detalhes(string sortOrder, string dataInicial,string dataFinal, string periodoInicial,string periodoFinal, int? Page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.VagaParam = sortOrder == "Vaga_asc" ? "Vaga_desc" : "Vaga_asc";
            ViewBag.ValorParam = sortOrder == "Valor_asc" ? "Valor_desc" : "Valor_asc";            
            ViewBag.DataEntradaParam = sortOrder == "DataEntrada_asc" ? "DataEntrada_desc" : "DataEntrada_asc";
            ViewBag.DataSaidaParam = sortOrder == "DataSaida_asc" ? "DataSaida_desc" : "DataSaida_asc";
            ViewBag.TempoParam = sortOrder == "Tempo_asc" ? "Tempo_desc" : "Tempo_asc";
            ViewBag.ClienteParam = sortOrder == "Cliente_asc" ? "Cliente_desc" : "Cliente_asc";
            ViewBag.EmailParam = sortOrder == "Email_asc" ? "Email_desc" : "Email_asc";
            ViewBag.VeiculoParam = sortOrder == "Veiculo_asc" ? "Veiculo_desc" : "Veiculo_asc";




            if ((periodoInicial != null)|| (periodoFinal != null))
            {
                Page = 1;
            }

            else
            {
                periodoInicial = dataInicial;
                periodoFinal = dataFinal;
            }
            ViewBag.dataInicial = periodoInicial;
            ViewBag.dataFinal = periodoFinal;
                        
            var reservas = new List<Relatorio>();

            foreach (var r in db.Reservas)
            {
                var relatorio = new Relatorio();

                relatorio.VagaNome = db.Vagas
                    .Where(u => u.Id == r.VagaId)
                    .Select(o=>o.Nome)
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
                relatorio.HorasReservadas = r.Saida.Subtract(r.Entrada);
                relatorio.Entrada = r.Entrada;
                relatorio.Saida = r.Saida;
                reservas.Add(relatorio);
            }

            var listaReservas = from s in reservas select s;

            if (!String.IsNullOrEmpty(periodoInicial) && (!String.IsNullOrEmpty(periodoFinal)))
            {
                var dataincialPesquisa = DateTimeOffset.Parse(periodoInicial);
                var datafinalPesquisa = DateTimeOffset.Parse(periodoFinal);
                datafinalPesquisa = datafinalPesquisa.AddHours(16);
                listaReservas = listaReservas.Where(s => s.Entrada >= dataincialPesquisa); 
                listaReservas = listaReservas.Where(s => s.Saida <= datafinalPesquisa);
            }

            switch (sortOrder)
            {
                case "Vaga_desc":
                    listaReservas = listaReservas.OrderByDescending(s => s.VagaNome);
                    break;
                case "DataEntrada_asc":
                    listaReservas = listaReservas.OrderBy(s => s.Entrada);
                    break;
                case "DataEntrada_desc":
                    listaReservas = listaReservas.OrderByDescending(s => s.Entrada );
                    break;
                case "DataSaida_asc":
                    listaReservas = listaReservas.OrderBy(s => s.Saida);
                    break;
                case "DataSaida_desc":
                    listaReservas = listaReservas.OrderByDescending(s => s.Entrada);
                    break;
                case "Tempo_asc":
                    listaReservas = listaReservas.OrderBy(s => s.HorasReservadas);
                    break;
                case "Tempo_desc":
                    listaReservas = listaReservas.OrderByDescending(s => s.HorasReservadas);
                    break;
                case "Cliente_asc":
                    listaReservas = listaReservas.OrderBy(s => s.UsuarioNome);
                    break;
                case "Cliente_desc":
                    listaReservas = listaReservas.OrderByDescending(s => s.UsuarioNome);
                    break;
                case "Email_asc":
                    listaReservas = listaReservas.OrderBy(s => s.Email);
                    break;
                case "Email_desc":
                    listaReservas = listaReservas.OrderByDescending(s => s.Email);
                    break;
                case "Veiculo_asc":
                    listaReservas = listaReservas.OrderBy(s => s.Modelo);
                    break;
                case "Veiculo_desc":
                    listaReservas = listaReservas.OrderByDescending(s => s.Modelo);
                    break;
                case "Valor_asc":
                    listaReservas = listaReservas.OrderBy(s => s.Valor);
                    break;
                case "Valor_desc":
                    listaReservas = listaReservas.OrderByDescending(s => s.Valor);
                    break;
                default:
                    listaReservas = listaReservas.OrderBy(s => s.VagaNome);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (Page ?? 1);
            return View(listaReservas.ToPagedList(pageNumber, pageSize));
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult _Deletar(int? id)
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
        [HttpPost, ActionName("_Deletar")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reserva reserva = db.Reservas.Find(id);
            db.Reservas.Remove(reserva);
            db.SaveChanges();
            TempData["Deletado"] = "Reserva deletada com sucesso";
            return RedirectToAction("Index","Vagas");
        }

    }
}
