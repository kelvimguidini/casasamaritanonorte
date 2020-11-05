using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using casasamaritanonorte.DAL;
using casasamaritanonorte.Models;
using casasamaritanonorte.ViewModels;
using AutoMapper;
using Uol.PagSeguro.Resources;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Service;
using Uol.PagSeguro.Exception;

namespace casasamaritanonorte.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RelatorioController : Controller
    {
        private CasaContext db = new CasaContext();

        // GET: Relatorio
        public ActionResult Envios(string mensagem = null)
        {
            ViewBag.Mensagem = mensagem;
            List<Pagamento> pagamentos = db.Pagamento.Include(c => c.Campanha).Include(c => c.Captador).Where(p => p.Campanha != null && !p.Campanha.Doacao && p.CodigoTrasacao != null).ToList();
            List<EnvioViewModels> envios = new List<EnvioViewModels>();

            bool isSandbox = true;
            EnvironmentConfiguration.ChangeEnvironment(isSandbox);

            try
            {

                AccountCredentials credentials = PagSeguroConfiguration.Credentials(isSandbox);

                foreach (Pagamento pagamento in pagamentos)
                {
                    EnvioViewModels envio = new EnvioViewModels();

                    // TODO: Substitute the code below with a valid transaction code for your transaction
                    Transaction transaction = TransactionSearchService.SearchByCode(credentials, pagamento.CodigoTrasacao);

                    switch (transaction.TransactionStatus)
                    {
                        case 0:
                            envio.StatusPagamento = "Iniciada";
                            break;
                        default:
                        case 1:
                            envio.StatusPagamento = "Aguardando Pagamento";
                            envio.SituacaoEntrega = "Aguardando Pagamento";
                            break;
                        case 2:
                            envio.StatusPagamento = "Em Analise";
                            break;
                        case 3:
                            envio.StatusPagamento = "Pago";

                            envio.SituacaoEntrega = "Separando Para Envio";

                            break;
                        case 4:
                            envio.StatusPagamento = "Disponivel";
                            break;
                        case 5:
                            envio.StatusPagamento = "Em Disputa";
                            break;
                        case 6:
                            envio.StatusPagamento = "Recusado";
                            break;
                        case 7:
                            envio.StatusPagamento = "Cancelado";
                            break;
                    }

                    if (pagamento.SituacaoEntrega != null && pagamento.SituacaoEntrega != "")
                    {
                        envio.SituacaoEntrega = pagamento.SituacaoEntrega;
                    }

                    //Frete
                    envio.Frete = pagamento.Frete;
                    envio.TipoEntrega = pagamento.TipoEntrega;
                    envio.PrazoEntrega = pagamento.PrazoEntrega;
                    envio.Cep = pagamento.Cep;
                    envio.Logradouro = pagamento.Logradouro;
                    envio.Numero = pagamento.Numero;
                    envio.Complemento = pagamento.Complemento;
                    envio.UF = pagamento.UF;
                    envio.Bairro = pagamento.Bairro;
                    envio.Cidade = pagamento.Cidade;

                    envio.DataCompra = pagamento.Data;


                    //Cliente
                    envio.Nome = pagamento.Nome;
                    envio.Email = pagamento.Email;
                    envio.Telefone = pagamento.Telefone;
                    envio.DDD = pagamento.DDD;
                    envio.CPF = pagamento.CPF;

                    //Produto
                    envio.Campanha = pagamento.Campanha.Nome;
                    envio.IDCampanha = pagamento.Campanha.ID;

                    envio.CodigoTrasacao = pagamento.CodigoTrasacao;
                    envio.IDPagamento = pagamento.ID;
                  

                    envios.Add(envio);
                }


            }
            catch (PagSeguroServiceException exception)
            {
                ViewBag.Mensagem = exception.Message + "\n";

                foreach (ServiceError element in exception.Errors)
                {
                    ViewBag.Mensagem += element + "\n";
                }
            }
        

            return View(envios);
        }

        // GET: Relatorio/Details/5
        public ActionResult Vendas(string mensagem = null)
        {
            ViewBag.Mensagem = mensagem;
            Captador captador = db.Captador.FirstOrDefault(i => i.Email == User.Identity.Name);

            List<Pagamento> pagamentos = new List<Pagamento>();
            ViewBag.Captador = captador;
            if (User.IsInRole("Admin"))
            {
                pagamentos = db.Pagamento.Include(c => c.Campanha).Include(c => c.Captador).Where(p => p.CodigoTrasacao != null).OrderBy(p => p.Data).ToList();
            }
            else
            {
                pagamentos = db.Pagamento.Include(c => c.Campanha).Include(c => c.Captador).Where(p => p.CodigoTrasacao != null && p.Captador != null && p.Captador.ID == captador.ID).OrderBy(p => p.Data).ToList();
            }
            
            List<FinanceiroViewModels> financeiroList = new List<FinanceiroViewModels>();
            bool isSandbox = true;
            EnvironmentConfiguration.ChangeEnvironment(isSandbox);

            try
            {

                AccountCredentials credentials = PagSeguroConfiguration.Credentials(isSandbox);

                foreach (Pagamento pagamento in pagamentos)
                {
                    FinanceiroViewModels financeiro = new FinanceiroViewModels();

                    // TODO: Substitute the code below with a valid transaction code for your transaction
                    Transaction transaction = TransactionSearchService.SearchByCode(credentials, pagamento.CodigoTrasacao);

                    switch (transaction.TransactionStatus)
                    {
                        case 0:
                            financeiro.StatusPagamento = "Iniciada";
                            break;
                        default:
                        case 1:
                            financeiro.StatusPagamento = "Aguardando Pagamento";
                            break;
                        case 2:
                            financeiro.StatusPagamento = "Em Analise";
                            break;
                        case 3:
                            financeiro.StatusPagamento = "Pago";
                            break;
                        case 4:
                            financeiro.StatusPagamento = "Disponivel";
                            break;
                        case 5:
                            financeiro.StatusPagamento = "Em Disputa";
                            break;
                        case 6:
                            financeiro.StatusPagamento = "Recusado";
                            break;
                        case 7:
                            financeiro.StatusPagamento = "Cancelado";
                            break;
                    }

                    financeiro.IDPagamento = pagamento.ID;
                    switch (transaction.PaymentMethod.PaymentMethodType)
                    {
                        case 1:
                            financeiro.FormaPagamento = "Cartão de Crédito";
                            break;
                        case 2:
                            financeiro.FormaPagamento = "Boleto";
                            break;
                        case 3:
                            financeiro.FormaPagamento = "Transeferencia Online";
                            break;
                        case 4:
                            financeiro.FormaPagamento = "Balanço";
                            break;
                        case 5:
                            financeiro.FormaPagamento = "Oi Pago";
                            break;

                        case 7:
                            financeiro.FormaPagamento = "Depósito";
                            break;
                    }


                    financeiro.Valor = pagamento.Valor;
                    financeiro.Frete = pagamento.Frete;
                    financeiro.Quantidade = pagamento.Quantidade;
                    financeiro.Total = pagamento.Total;
                    financeiro.Data = pagamento.Data;
                    financeiro.ComissaoPaga = pagamento.ComissaoPaga;

                    //Produto
                    if(pagamento.Campanha != null)
                    {
                        financeiro.Campanha = Mapper.Map<Campanha, CampanhaViewModels>(pagamento.Campanha);
                    }

                    if (pagamento.Captador != null)
                    {
                        financeiro.Captador = Mapper.Map<Captador, CaptadorViewModels>(pagamento.Captador);
                    }

                    financeiroList.Add(financeiro);

                }


            }
            catch (PagSeguroServiceException exception)
            {
                ViewBag.Mensagem = exception.Message + "\n";

                foreach (ServiceError element in exception.Errors)
                {
                    ViewBag.Mensagem += element + "\n";
                }
            }


            return View(financeiroList);
        }

        // GET: Relatorio/Create
        public ActionResult Create()
        {
            return View();
        }

        
        // GET: Relatorio/Edit/5
        public async Task<ActionResult> ContatoDoadores()
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Pagamento pagamento = await db.Pagamento.FindAsync(id);
            //if (pagamento == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }

        // GET: Relatorio/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pagamento pagamento = await db.Pagamento.FindAsync(id);
            if (pagamento == null)
            {
                return HttpNotFound();
            }
            return View(pagamento);
        }

        // POST: Relatorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Pagamento pagamento = await db.Pagamento.FindAsync(id);
            db.Pagamento.Remove(pagamento);
            await db.SaveChangesAsync();
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
