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
using System.IO;

namespace casasamaritanonorte.Controllers
{
    public class EventoController : Controller
    {
        private CasaContext db = new CasaContext();

        // GET: Evento
        public async Task<ActionResult> Index()
        {
            var eventosVM = Mapper.Map<ICollection<Evento>, ICollection<EventoViewModels>>(await db.Evento.ToListAsync());
            return View(eventosVM.OrderByDescending(e => e.DataPublicacao));
        }

        // GET: Evento
        public async Task<ActionResult> Listar()
        {

            var eventosVM = Mapper.Map<ICollection<Evento>, ICollection<EventoViewModels>>(await db.Evento.ToListAsync());
            return View(eventosVM.OrderByDescending(e => e.DataPublicacao));
        }

        // GET: Evento/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                ViewBag.Mensagem = String.Format("Evento não encontrado!");
                return RedirectToAction("Index");
            }
            Evento evento = await db.Evento.Include(c => c.Album.Fotos).FirstOrDefaultAsync(i => i.ID == id);

            if (evento == null)
            {
                ViewBag.Mensagem = String.Format("Evento não encontrado!");
                return RedirectToAction("Index");
            }

            ICollection<EventoViewModels> eventosVM = Mapper.Map<ICollection<Evento>, ICollection<EventoViewModels>>(await db.Evento.ToListAsync());
            ViewBag.eventosVM = eventosVM.OrderByDescending(e => e.DataPublicacao);

            ViewBag.album = evento.Album;
            var eventoVM = Mapper.Map<Evento, EventoViewModels>(evento);
            return View(eventoVM);
        }


        // GET: Evento/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (TempData["album"] != null)
            {
                TempData["album"] = null;
            }

            return View();
        }

        // POST: Evento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(EventoViewModels eventoVM)
        {
            if (ModelState.IsValid)
            {
                Evento evento = new Evento();

                evento.Caption = eventoVM.Caption;
                evento.DataEvento = eventoVM.DataEvento;
                evento.DataPublicacao = DateTime.Now;
                evento.Texto = eventoVM.Texto;
                evento.Titulo = eventoVM.Titulo;

                var uploadPath = Server.MapPath("~/Imagens/Uploads/Eventos");

                string caminhoArquivo = string.Empty;
                int arquivosSalvos = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase arquivo = Request.Files[i];

                    //Salva o arquivo
                    if (arquivo.ContentLength > 0)
                    {

                        caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(arquivo.FileName));

                        arquivo.SaveAs(caminhoArquivo);
                        arquivosSalvos++;
                        caminhoArquivo = Path.GetFileName(arquivo.FileName);
                    }
                }


                evento.CaminhoCapa = caminhoArquivo;


                if (TempData["album"] != null)
                {
                    // Necessário TypeCasting para tipos complexos.
                    Album album = TempData["album"] as Album;
                    evento.Album = album;
                    TempData["album"] = null;
                }
                
                db.Evento.Add(evento);
                
                await db.SaveChangesAsync();

                ViewBag.Mensagem = String.Format("Evento cadastrado com sucesso!");
                return RedirectToAction("Index", "Evento");
            }

            return View(eventoVM);
        }

        
        [HttpPost]
        public async Task<JsonResult> DeleteFoto(string key)
        {
            int id = Convert.ToInt32(key);
            if (id == null)
            {
                return Json("Erro - foto não encontrada");
            }
            Foto foto = await db.Foto.FindAsync(id);
            if (foto == null)
            {
                return Json("Erro - foto não encontrada");
            }

            db.Foto.Remove(foto);
            await db.SaveChangesAsync();
            
            ViewBag.Mensagem = String.Format("Foto excluída com sucesso!");

            return Json("");
        }


        [HttpPost]
        public async Task<JsonResult> UploadAlbum()
        {
            Album album = new Album();

            if (TempData["album"] != null)
            {
                // Necessário TypeCasting para tipos complexos.
                album = TempData["album"] as Album;
            }
            else
            {
                album.Nome = DateTime.Now.ToString();
                album.Fotos = new List<Foto>();
                db.Album.Add(album);

                await db.SaveChangesAsync();
            }

            TempData["album"] = album;

            var uploadPath = Server.MapPath("~/Imagens/Uploads/Eventos/Albuns");

            int arquivosSalvos = 0;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase arquivo = Request.Files[i];

                //Salva o arquivo
                if (arquivo.ContentLength > 0)
                {

                    string caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(arquivo.FileName));

                    arquivo.SaveAs(caminhoArquivo);
                    Foto foto = new Foto();

                    foto.Caminho = Path.GetFileName(arquivo.FileName);

                    db.Foto.Add(foto);

                    album.Fotos.Add(foto);
                    
                    arquivosSalvos++;
                }
            }

            db.Entry(album).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return Json("");
        }

        // GET: Evento/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Mensagem = String.Format("Evento não encontrado!");
                return RedirectToAction("Index");
            }
            Evento evento = await db.Evento.Include(c => c.Album.Fotos).FirstOrDefaultAsync(i => i.ID == id);
            if (evento == null)
            {
                ViewBag.Mensagem = String.Format("Evento não encontrado!");
                return RedirectToAction("Index");
            }

            TempData["album"] = evento.Album;

            ViewBag.album = evento.Album;

            var eventoVM = Mapper.Map<Evento, EventoViewModels>(evento);
            return View(eventoVM);
        }

        // POST: Evento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(EventoViewModels eventoVM)
        {
            if (ModelState.IsValid)
            {

                Evento evento = await db.Evento.Include(c => c.Album.Fotos).FirstOrDefaultAsync(i => i.ID == eventoVM.ID);

                evento.Caption = eventoVM.Caption;
                evento.DataEvento = eventoVM.DataEvento;
                evento.Texto = eventoVM.Texto;
                evento.Titulo = eventoVM.Titulo;

                var uploadPath = Server.MapPath("~/Imagens/Uploads/Eventos");

                string caminhoArquivo = string.Empty;
                int arquivosSalvos = 0;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase arquivo = Request.Files[i];

                    //Salva o arquivo
                    if (arquivo.ContentLength > 0)
                    {

                        caminhoArquivo = Path.Combine(@uploadPath, Path.GetFileName(arquivo.FileName));

                        arquivo.SaveAs(caminhoArquivo);
                        arquivosSalvos++;
                        caminhoArquivo = Path.GetFileName(arquivo.FileName);
                    }
                }

                if(caminhoArquivo != string.Empty)
                {
                    evento.CaminhoCapa = caminhoArquivo;
                }
               

                db.Entry(evento).State = EntityState.Modified;
                await db.SaveChangesAsync();

                TempData["album"] = null;

                ViewBag.Mensagem = String.Format("Informações salvas com sucesso!");
                return RedirectToAction("Listar");
            }
            return View(eventoVM);
        }

        // GET: Evento/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.Mensagem = String.Format("Evento não encontrado!");
                return RedirectToAction("Index");
            }
            Evento evento = await db.Evento.FindAsync(id);
            if (evento == null)
            {
                ViewBag.Mensagem = String.Format("Evento não encontrado!");
                return RedirectToAction("Index");
            }
            return View(evento);
        }

        // POST: Evento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Evento evento = await db.Evento.FindAsync(id);
            db.Evento.Remove(evento);
            await db.SaveChangesAsync();


            ViewBag.Mensagem = String.Format("Evento Excluído com sucesso!");
            return RedirectToAction("Listar");
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
