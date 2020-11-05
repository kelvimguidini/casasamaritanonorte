using AutoMapper;
using casasamaritanonorte.DAL;
using casasamaritanonorte.Models;
using casasamaritanonorte.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace casasamaritanonorte.Controllers
{
    public class HomeController : Controller
    {
        private CasaContext db = new CasaContext();

        public async Task<ActionResult> Index()
        {
            Campanha campanha = await db.Campanha.Where(c=> c.TelaInicial).OrderByDescending(e => e.ID).FirstOrDefaultAsync();
            CampanhaViewModels campanhaVM = new CampanhaViewModels();

            campanhaVM = Mapper.Map<Campanha, CampanhaViewModels>(campanha);

            return View(campanhaVM);
        }

        public ActionResult About()
        {
            return View("About", "_LayoutSite");
        }

        public ActionResult Contact()
        {
            return View();
        }

        // POST: Evento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(ContatoViewModel contatoVM)
        {
            if (ModelState.IsValid)
            {
                Contato contato = Mapper.Map<ContatoViewModel, Contato>(contatoVM);

                contato.Data = DateTime.Now;
                contato.Lido = false;
                db.Contato.Add(contato);

                await db.SaveChangesAsync();
                ViewBag.Mensagem = String.Format("Mensagem enviada com sucesso!");
            }

            return View(contatoVM);
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Abrir(int? id)
        {
            if (id == null)
            {
                ViewBag.Mensagem = String.Format("Contato não encontrado!");
                return RedirectToAction("Index");
            }
            Contato contato = await db.Contato.FindAsync(id);
            if (contato == null)
            {
                ViewBag.Mensagem = String.Format("Contato não encontrado!");
                return RedirectToAction("Index");
            }

            contato.Lido = true;
            db.Entry(contato).State = EntityState.Modified;

            await db.SaveChangesAsync();

            ViewBag.Mensagem = String.Format("Mensagem marcada como lida com sucesso!");

            return RedirectToAction("Contatos");
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Contatos()
        {
            var ContatoVM = Mapper.Map<ICollection<Contato>, ICollection<ContatoViewModel>>(await db.Contato.ToListAsync());
            return View(ContatoVM.OrderByDescending(e => e.Data));
        }

        public async Task<ActionResult> Galeria()
        {
            List<Evento> eventos = await db.Evento.Include(c => c.Album.Fotos).ToListAsync();
            return View(eventos);
        }

    }
}