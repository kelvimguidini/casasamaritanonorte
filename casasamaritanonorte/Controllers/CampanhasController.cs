using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using casasamaritanonorte.DAL;
using casasamaritanonorte.Models;
using System.Collections;
using System.IO;
using casasamaritanonorte.ViewModels;
using AutoMapper;
using Uol.PagSeguro.Resources;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Service;
using casasamaritanonorte.Lib;
using System.Text;
using Newtonsoft.Json;

namespace casasamaritanonorte.Controllers
{
    [Authorize]
    public class CampanhasController : Controller
    {
        private CasaContext db = new CasaContext();

        // GET: Campanhas
        public async Task<ActionResult> Index()
        {
            var campanhas = await db.Campanha.ToListAsync();

            List<CampanhaViewModels> campanhasVM = new List<CampanhaViewModels>();

            // Transformando a Model Cliente em ClienteViewModel
            campanhasVM = Mapper.Map<List<Campanha>, List<CampanhaViewModels>>(campanhas);

            Captador captador = db.Captador.FirstOrDefault(i => i.Email == User.Identity.Name);

            ViewBag.Captador = captador;

            return View(campanhasVM);
        }

        // GET: Campanhas/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                ViewBag.Mensagem = String.Format("Campanha não encontrada!");
                return RedirectToAction("Index");
            }
            Campanha campanha = await db.Campanha.FindAsync(id);
            if (campanha == null)
            {
                ViewBag.Mensagem = String.Format("Campanha não encontrada!");
                return RedirectToAction("Index");
            }
            CampanhaViewModels campanhaVM = Mapper.Map<Campanha, CampanhaViewModels>(campanha);

            Captador captador = db.Captador.FirstOrDefault(i => i.Email == User.Identity.Name);

            ViewBag.Captador = captador;

            return View(campanhaVM);

        }

        // GET: Campanhas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Campanhas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CampanhaViewModels campanhaVM)
        {
            if (campanhaVM.Video != null)
            {
                string ExtencoesVideo = "video/mp4,video/3g2,video/3gp,video/3gpp,video/asf,video/avi,video/dat,video/divx,video/dv,video/f4v,video/flv,video/gif,video/m2ts,video/m4v,video/mkv,video/mod,video/mov,video/mpe,video/mpeg,video/mpeg4,video/mpg,mts,video/nsv,video/ogm,video/ogv,video/qt,video/tod,video/ts,video/vob,video/wmv";
                if (!ExtencoesVideo.Contains(campanhaVM.Video.ContentType))
                {
                    ModelState.AddModelError("Video", "Esse arquivo não é um formato de vídeo aceito.");
                }
            }
            else
            {
                ModelState.AddModelError("Video", "Campo vídeo é obrigatório.");
            }

            if (campanhaVM.Foto != null)
            {
                string ExtencoesImage = "image/jpg,image/jpeg,image/gif,image/png";
                if (!ExtencoesImage.Contains(campanhaVM.Foto.ContentType))
                {
                    ModelState.AddModelError("Foto", "Esse arquivo não é uma imagem.");
                }
            }
            else
            {
                ModelState.AddModelError("Foto", "Campo foto é obrigatório.");
            }
            if (campanhaVM.Doacao)
            {
                ModelState["Diametro"].Errors.Clear();
                ModelState["Valor"].Errors.Clear();
                ModelState["Altura"].Errors.Clear();
                ModelState["Largura"].Errors.Clear();
                ModelState["Comprimento"].Errors.Clear();
            }

            if (ModelState.IsValid)
            {

                Campanha campanha = new Campanha();

                campanha.Descricao = campanhaVM.Descricao;
                campanha.Mensagem = campanhaVM.Mensagem;
                campanha.Nome = campanhaVM.Nome;
                campanha.Doacao = campanhaVM.Doacao;
                campanha.Peso = campanhaVM.Peso;
                campanha.Valor = campanhaVM.Valor;
                campanha.Altura = campanhaVM.Altura;
                campanha.Largura = campanhaVM.Largura;
                campanha.Diametro = campanhaVM.Diametro;
                campanha.Comprimento = campanhaVM.Comprimento;
                campanha.TelaInicial = campanhaVM.TelaInicial;
                campanha.Ativo = campanhaVM.Ativo;

                var uploadPath = Server.MapPath("~/Imagens/Uploads/Campanhas");

                var fotoStr = "ft_" + DateTime.Now.ToString("yyyyMMddHHMMss") + campanhaVM.Foto.FileName.Substring(campanhaVM.Foto.FileName.ToString().LastIndexOf("."));
                var videoStr = "vd_" + DateTime.Now.ToString("yyyyMMddHHMMss") + campanhaVM.Video.FileName.Substring(campanhaVM.Video.FileName.ToString().LastIndexOf("."));

                string caminhoArquivoFoto = Path.Combine(@uploadPath, fotoStr);
                string caminhoArquivoVideo = Path.Combine(@uploadPath, videoStr);

                campanhaVM.Foto.SaveAs(caminhoArquivoFoto);
                campanhaVM.Video.SaveAs(caminhoArquivoVideo);

                campanha.fotoStr = fotoStr;
                campanha.VideoStr = videoStr;

                db.Campanha.Add(campanha);
                await db.SaveChangesAsync();

                if (campanha.TelaInicial)
                {
                    var campanhas = await db.Campanha.ToListAsync();

                    foreach(Campanha camp in campanhas)
                    {
                        if(camp.ID == campanha.ID)
                        {
                            camp.TelaInicial = false;

                            db.Entry(camp).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                }

                ViewBag.Mensagem = String.Format("Campanha cadastrada com sucesso!");
                return View(campanhaVM);
            }

            return View(campanhaVM);
        }

        // GET: Campanhas/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Mensagem = String.Format("Campanha não encontrada!");
                return RedirectToAction("Index");
            }
            Campanha campanha = await db.Campanha.FindAsync(id);

            if (campanha == null)
            {
                ViewBag.Mensagem = String.Format("Campanha não encontrada!");
                return RedirectToAction("Index");
            }
            CampanhaViewModels campanhaVM = Mapper.Map<Campanha, CampanhaViewModels>(campanha);

            return View(campanhaVM);
        }

        // POST: Campanhas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CampanhaViewModels campanhaVM)
        {

            if (campanhaVM.Video != null)
            {
                string ExtencoesVideo = "video/mp4,video/3g2,video/3gp,video/3gpp,video/asf,video/avi,video/dat,video/divx,video/dv,video/f4v,video/flv,video/gif,video/m2ts,video/m4v,video/mkv,video/mod,video/mov,video/mpe,video/mpeg,video/mpeg4,video/mpg,mts,video/nsv,video/ogm,video/ogv,video/qt,video/tod,video/ts,video/vob,video/wmv";
                if (!ExtencoesVideo.Contains(campanhaVM.Video.ContentType))
                {
                    ModelState.AddModelError("Video", "Esse arquivo não é um formato de vídeo aceito.");
                }
            }

            if (campanhaVM.Foto != null)
            {
                string ExtencoesImage = "image/jpg,image/jpeg,image/gif,image/png";
                if (!ExtencoesImage.Contains(campanhaVM.Foto.ContentType))
                {
                    ModelState.AddModelError("Foto", "Esse arquivo não é uma imagem.");
                }
            }
            if (campanhaVM.Doacao)
            {
                ModelState["Diametro"].Errors.Clear();
                ModelState["Valor"].Errors.Clear();
                ModelState["Altura"].Errors.Clear();
                ModelState["Largura"].Errors.Clear();
                ModelState["Comprimento"].Errors.Clear();
            }

            if (ModelState.IsValid)
            {

                Campanha campanha = new Campanha();

                campanha.Descricao = campanhaVM.Descricao;
                campanha.Mensagem = campanhaVM.Mensagem;
                campanha.Nome = campanhaVM.Nome;
                campanha.Doacao = campanhaVM.Doacao;
                campanha.Peso = campanhaVM.Peso;
                campanha.Valor = campanhaVM.Valor;
                campanha.Altura = campanhaVM.Altura;
                campanha.Largura = campanhaVM.Largura;
                campanha.Diametro = campanhaVM.Diametro;
                campanha.Comprimento = campanhaVM.Comprimento;
                campanha.ID = campanhaVM.ID;
                campanha.Ativo = campanhaVM.Ativo;
                campanha.TelaInicial = campanhaVM.TelaInicial;

                var uploadPath = Server.MapPath("~/Imagens/Uploads/Campanhas");

                if (campanhaVM.Foto != null)
                {
                    var fotoStr = "ft_" + DateTime.Now.ToString("yyyyMMddHHMMss") + campanhaVM.Foto.FileName.Substring(campanhaVM.Foto.FileName.ToString().LastIndexOf("."));
                    string caminhoArquivoFoto = Path.Combine(@uploadPath, fotoStr);
                    campanhaVM.Foto.SaveAs(caminhoArquivoFoto);
                    campanha.fotoStr = fotoStr;
                }
                if (campanhaVM.Video != null)
                {
                    var videoStr = "vd_" + DateTime.Now.ToString("yyyyMMddHHMMss") + campanhaVM.Video.FileName.Substring(campanhaVM.Video.FileName.ToString().LastIndexOf("."));
                    string caminhoArquivoVideo = Path.Combine(@uploadPath, videoStr);
                    campanhaVM.Video.SaveAs(caminhoArquivoVideo);
                    campanha.VideoStr = videoStr;
                }



                db.Entry(campanha).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (campanha.TelaInicial)
                {
                    var campanhas = await db.Campanha.ToListAsync();

                    foreach (Campanha camp in campanhas)
                    {
                        if (camp.ID == campanha.ID)
                        {
                            camp.TelaInicial = false;

                            db.Entry(camp).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                }

                ViewBag.Mensagem = String.Format("Campanha alterada com sucesso!");
                return View(campanhaVM);
            }

            return View(campanhaVM);
        }

        // GET: Campanhas/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {

            if (id == null)
            {
                ViewBag.Mensagem = String.Format("Campanha não encontrada!");
                return RedirectToAction("Index");
            }
            Campanha campanha = await db.Campanha.FindAsync(id);
            if (campanha == null)
            {
                ViewBag.Mensagem = String.Format("Campanha não encontrada!");
                return RedirectToAction("Index");
            }
            return View(campanha);
        }

        // POST: Campanhas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Campanha campanha = await db.Campanha.FindAsync(id);
            db.Campanha.Remove(campanha);
            await db.SaveChangesAsync();

            ViewBag.Mensagem = String.Format("Campanha Excluída com sucesso!");
            return RedirectToAction("Index");
        }


        [AllowAnonymous]
        public async Task<ActionResult> Doacao(int? Id, int? IdCaptador)
        {
            Session["IdCaptador"] = IdCaptador;
            Session["IdCampanha"] = Id;

            PagamentoViewModels pagamentoVM = new PagamentoViewModels();

            if (Id != null)
            {

                Campanha campanha = await db.Campanha.FindAsync(Id);
                if (campanha != null)
                {
                    pagamentoVM.Campanha = Mapper.Map<Campanha, CampanhaViewModels>(campanha);
                    MontaSelectListUf();
                }

            }

            if (IdCaptador != null)
            {

                Captador captador = await db.Captador.FindAsync(IdCaptador);
                if (captador == null)
                {
                    pagamentoVM.Captador = Mapper.Map<Captador, CaptadorViewModels>(captador);
                }

            }

            pagamentoVM.Quantidade = 1;

            return View(pagamentoVM);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Doacao(PagamentoViewModels pagamentoVM)
        {

            if (pagamentoVM.Campanha != null && !pagamentoVM.Campanha.Doacao)
            {
                ModelState.AddModelError("Cep", "Por favor insera o CEP da entrega.");
                ModelState.AddModelError("Logradouro", "Por favor informe o endereço");
                ModelState.AddModelError("Bairro", "Bairro é um campo obrigatório.");
                ModelState.AddModelError("UF", "Escolha o estado.");
            }


            if (ModelState.IsValid)
            {

                Pagamento pagamento = Mapper.Map<PagamentoViewModels, Pagamento>(pagamentoVM);

                //CRIA O PAGAMENTO
                bool isSandbox = true;
                EnvironmentConfiguration.ChangeEnvironment(isSandbox);

                // Instantiate a new payment request
                PaymentRequest payment = new PaymentRequest();

                // Sets the currency
                payment.Currency = Currency.Brl;

                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf8 = Encoding.UTF8;

                // Add an item for this payment request
                if (Session["IdCampanha"] != null)
                {
                    Campanha campanha = db.Campanha.Find(Convert.ToInt32(Session["IdCampanha"]));

                    CampanhaViewModels campanhaVM = Mapper.Map<Campanha, CampanhaViewModels>(campanha);
                    pagamentoVM.Campanha = campanhaVM;

                    byte[] utfBytes = utf8.GetBytes(pagamentoVM.Campanha.Nome);
                    byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                    string msg = iso.GetString(isoBytes);

                    pagamento.Campanha = campanha;

                    payment.Items.Add(new Item(pagamentoVM.Campanha.ID.ToString(), msg, pagamentoVM.Quantidade, pagamentoVM.Campanha.Valor));
                }
                else
                {
                    byte[] utfBytes = utf8.GetBytes("Doação");
                    byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                    string msg = iso.GetString(isoBytes);
                    payment.Items.Add(new Item("0001", msg, 1, pagamentoVM.Valor));
                }

                if (pagamentoVM.Campanha != null && !pagamentoVM.Campanha.Doacao)
                {
                    // Sets shipping information for this payment request
                    payment.Shipping = new Shipping();
                    payment.Shipping.ShippingType = ShippingType.Sedex;

                    //Frete, testar com e sem.
                    payment.Shipping.Cost = pagamentoVM.Frete;


                    payment.Shipping.Address = new Address(
                        "BRA",
                        pagamentoVM.UF,
                        pagamentoVM.UF,
                        pagamentoVM.Bairro,//iso.GetString(Encoding.Convert(utf8, iso, utf8.GetBytes(pagamentoVM.Bairro))),
                        pagamentoVM.Cep,
                        pagamentoVM.Logradouro,//iso.GetString(Encoding.Convert(utf8, iso, utf8.GetBytes(pagamentoVM.Logradouro))),
                        pagamentoVM.Numero,
                        pagamentoVM.Complemento//iso.GetString(Encoding.Convert(utf8, iso, utf8.GetBytes(pagamentoVM.Complemento)))
                    );
                }
                else
                {
                    payment.shippingAddressRequired = false;
                    payment.AddParameter("shippingAddressRequired", "false");
                }
                // Sets your customer information.
                payment.Sender = new Sender(
                    pagamentoVM.Nome,
                    pagamentoVM.Email,
                    new Phone(pagamentoVM.DDD.Replace("(", "").Replace(")", ""), pagamentoVM.Telefone.Replace("-", ""))
                );

                SenderDocument document = new SenderDocument(Documents.GetDocumentByType("CPF"), pagamentoVM.CPF.Replace(".", "").Replace("-", ""));
                payment.Sender.Documents.Add(document);

                // Add discount per payment method
                payment.AddPaymentMethodConfig(PaymentMethodConfigKeys.DiscountPercent, 5.00, PaymentMethodGroup.Boleto);

                // Add and remove groups and payment methods
                List<string> accept = new List<string>();
                accept.Add(ListPaymentMethodNames.DebitoItau);
                accept.Add(ListPaymentMethodNames.DebitoHSBC);
                accept.Add(ListPaymentMethodNames.Boleto);
                payment.AcceptPaymentMethodConfig(ListPaymentMethodGroups.CreditCard, accept);

                if (Session["IdCaptador"] != null)
                {
                    pagamento.Captador = db.Captador.Find(Convert.ToInt32(Session["IdCaptador"]));
                }
                pagamento.Data = DateTime.Now;
                pagamento.PrimeiraVisualizacao = true;
                pagamento.ComissaoPaga = false;
                try
                {
                    pagamento = db.Pagamento.Add(pagamento);
                    db.SaveChangesAsync();

                    // Sets a reference code for this payment request, it is useful to identify this payment in future notifications.
                    payment.Reference = pagamento.ID.ToString();

                    /// Create new account credentials
                    /// This configuration let you set your credentials from your ".cs" file.
                    AccountCredentials credentials = PagSeguroConfiguration.Credentials(isSandbox);

                    Uri paymentRedirectUri = payment.Register(credentials);

                    ViewBag.paymentRedirectUrl = paymentRedirectUri.ToString();

                    ViewBag.code = paymentRedirectUri.ToString().Substring(paymentRedirectUri.ToString().LastIndexOf("=") + 1);
                    ViewBag.PagamentoId = pagamento.ID;
                }
                catch (PagSeguroServiceException exception)
                {
                    ViewBag.erro = exception.Message + "\n";

                    foreach (ServiceError element in exception.Errors)
                    {
                        ViewBag.erro += element + "\n";
                    }
                }

            }
            MontaSelectListUf();
            return View(pagamentoVM);
        }

        [AllowAnonymous]
        public ActionResult DoacaoRetorno(int? id, string transactionCode = "")
        {

            Pagamento pagamento = db.Pagamento.Include(c => c.Campanha).FirstOrDefault(i => i.ID == id);

            if (pagamento == null)
            {
                ViewBag.Mensagem = String.Format("Código do pagamento não encontrado, se o erro persistir por favor entre em contato conosco!");
            }
            else
            {

                bool isSandbox = true;
                EnvironmentConfiguration.ChangeEnvironment(isSandbox);

                try
                {

                    AccountCredentials credentials = PagSeguroConfiguration.Credentials(isSandbox);

                    // TODO: Substitute the code below with a valid transaction code for your transaction
                    Transaction transaction = TransactionSearchService.SearchByCode(credentials, transactionCode);

                    switch (transaction.TransactionStatus)
                    {
                        case 0:
                            ViewBag.Status = "Iniciada";
                            break;
                        default:
                        case 1:
                            ViewBag.Status = "Aguardando Pagamento";
                            break;
                        case 2:
                            ViewBag.Status = "Em Analise";
                            break;
                        case 3:
                            ViewBag.Status = "Pago";
                            break;
                        case 4:
                            ViewBag.Status = "Disponivel";
                            break;
                        case 5:
                            ViewBag.Status = "Em Disputa";
                            break;
                        case 6:
                            ViewBag.Status = "Recusado";
                            break;
                        case 7:
                            ViewBag.Status = "Cancelado";
                            break;
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
            }

            PagamentoViewModels pagamentoVM = Mapper.Map<Pagamento, PagamentoViewModels>(pagamento);

            pagamentoVM.Campanha = Mapper.Map<Campanha, CampanhaViewModels>(pagamento.Campanha);

            pagamento.PrimeiraVisualizacao = false;
            pagamento.CodigoTrasacao = transactionCode;
            db.Entry(pagamento).State = EntityState.Modified;
            db.SaveChangesAsync();

            return View(pagamentoVM);
        }

        // POST: Campanhas/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InformarEntrega(int id, string status)
        {
            Pagamento pagamento = await db.Pagamento.FindAsync(id);

            pagamento.SituacaoEntrega = status;
            db.Entry(pagamento).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Envios", "Relatorio", new { mensagem = "Status da entrega alterado com sucesso!" });

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InformarPagamentoComissao(int id)
        {
            Pagamento pagamento = await db.Pagamento.FindAsync(id);

            pagamento.ComissaoPaga = true;
            db.Entry(pagamento).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return RedirectToAction("Vendas", "Relatorio", new { mensagem = "Pagamento da comissão foi informado com sucesso!" });

        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult CalculaFrete(string cep, CampanhaViewModels campanhaStr)
        {
            CampanhaViewModels campanha = campanhaStr;//JsonConvert.DeserializeObject<CampanhaViewModels>(campanhaStr);
            //instanciamos a classe
            ConsultaCorreios consulta = new ConsultaCorreios();
            //populamos a classe
            consulta.CdEmpresa = string.Empty;
            consulta.SenhaEmpresa = string.Empty;
            consulta.CdServico = CodServico.SedexSemContrato;
            consulta.CepOrigem = "72215215";
            consulta.CepDestino = cep;
            consulta.PesoEncomenda = campanha.Peso.ToString();
            consulta.FormatoEncomenda = FormatoEncomenda.CaixaPacote;
            consulta.Altura = campanha.Altura;
            consulta.Largura = campanha.Largura;
            consulta.Diametro = campanha.Diametro;
            consulta.Comprimento = campanha.Comprimento;
            consulta.MaoPropria = "N";
            consulta.AvisoRecebimento = "N";
            consulta.ValorDeclarado = 0;


            string Csedex = consulta.ValorServico();

            consulta.CdServico = CodServico.PacSemContrato;
            string Cpac = consulta.ValorServico();

            return Json(JsonConvert.SerializeObject(new { sedex = Csedex, pac = Cpac }));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private void MontaSelectListUf(string ufSelecionada = null)
        {
            var lista = new ArrayList();

            lista.Add(new SelectListItem() { Value = "AC", Text = "Acre" });
            lista.Add(new SelectListItem() { Value = "AL", Text = "Alagoas" });
            lista.Add(new SelectListItem() { Value = "AP", Text = "Amapá" });
            lista.Add(new SelectListItem() { Value = "AM", Text = "Amazonas" });
            lista.Add(new SelectListItem() { Value = "BA", Text = "Bahia" });
            lista.Add(new SelectListItem() { Value = "CE", Text = "Ceará" });
            lista.Add(new SelectListItem() { Value = "DF", Text = "Distrito Federal" });
            lista.Add(new SelectListItem() { Value = "ES", Text = "Espírito Santo" });
            lista.Add(new SelectListItem() { Value = "GO", Text = "Goiás" });
            lista.Add(new SelectListItem() { Value = "MA", Text = "Maranhão" });
            lista.Add(new SelectListItem() { Value = "MT", Text = "Mato Grosso" });
            lista.Add(new SelectListItem() { Value = "MS", Text = "Mato Grosso do Sul" });
            lista.Add(new SelectListItem() { Value = "MG", Text = "Minas Gerais" });
            lista.Add(new SelectListItem() { Value = "PA", Text = "Pará" });
            lista.Add(new SelectListItem() { Value = "PB", Text = "Paraíba" });
            lista.Add(new SelectListItem() { Value = "PR", Text = "Paraná" });
            lista.Add(new SelectListItem() { Value = "PE", Text = "Pernambuco" });
            lista.Add(new SelectListItem() { Value = "PI", Text = "Piauí" });
            lista.Add(new SelectListItem() { Value = "RJ", Text = "Rio de Janeiro" });
            lista.Add(new SelectListItem() { Value = "RN", Text = "Rio Grande do Norte" });
            lista.Add(new SelectListItem() { Value = "RS", Text = "Rio Grande do Sul" });
            lista.Add(new SelectListItem() { Value = "RO", Text = "Rondônia" });
            lista.Add(new SelectListItem() { Value = "RR", Text = "Roraima" });
            lista.Add(new SelectListItem() { Value = "SC", Text = "Santa Catarina" });
            lista.Add(new SelectListItem() { Value = "SP", Text = "São Paulo" });
            lista.Add(new SelectListItem() { Value = "SE", Text = "Sergipe" });
            lista.Add(new SelectListItem() { Value = "TO", Text = "Tocantins" });

            ViewBag.Ufs = lista;
        }
    }
}