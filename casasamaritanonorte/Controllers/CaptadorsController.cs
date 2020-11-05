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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using casasamaritanonorte.ViewModels;
using AutoMapper;
using casasamaritanonorte.Lib;

namespace casasamaritanonorte.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CaptadorsController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public CaptadorsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public CaptadorsController()
        {
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private CasaContext db = new CasaContext();

        // GET: Captadors
        public async Task<ActionResult> Index()
        {
            var captadores = await db.Captador.ToListAsync();

            // Transformando a Model Cliente em ClienteViewModel
            var captadoresVM = Mapper.Map<List<Captador>, List<CaptadorViewModels>>(captadores);

            return View(captadoresVM);
        }

        // GET: Captadors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                ViewBag.Mensagem = String.Format("Captador não encontrado!");
                return RedirectToAction("Index");
            }
            Captador captador = await db.Captador.FindAsync(id);
            if (captador == null)
            {
                ViewBag.Mensagem = String.Format("Captador não encontrado!");
                return RedirectToAction("Index");
            }
            return View(captador);
        }

        // GET: Captadors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Captadors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CaptadorViewModels captadorVM)
        {

            if (ModelState.IsValid)
            {
                // Transformando a Model Cliente em ClienteViewModel
                Captador captador = Mapper.Map<CaptadorViewModels, Captador>(captadorVM);

                var user = new ApplicationUser { UserName = captador.Email, Email = captador.Email };
                var result = await UserManager.CreateAsync(user, "123456");
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Captador");
                }

                var enviaEmail = new EnviarNotificacoes();
                enviaEmail.EnviarEmail("Prezado "+ captadorVM.Nome + ", </br></br> Você foi cadastrado como captador de recurso no sistema da casa de repouso o bom samaritano. Por favor, entre no <a href=\""+ Url.Action("Login", "Account")+"\">sistema da casa de repouso</a> com o seu e-mail ("+ captadorVM.Email+ ") e senha (123456)  </br></br> troque sua senha o mais rápido possível. </br></br>Se tiver alguma díuvida, entre em contato joaodg@casasamaritanonorte.com.br", captadorVM.Email, "Cadastro de novo usuário");
                ViewBag.Mensagem = String.Format("Captador cadastrado com sucesso! O E-mail cadastrado recebeu um informativo contendo a senha");

                db.Captador.Add(captador);
                await db.SaveChangesAsync();
                return View(captadorVM);

            }

            return View(captadorVM);
        }

        // GET: Captadors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Mensagem = String.Format("Captador não encontrado!");
                return RedirectToAction("Index");
            }
            Captador captador = await db.Captador.FindAsync(id);

            // Transformando a Model Cliente em ClienteViewModel
            var captadorVM = Mapper.Map<Captador, CaptadorViewModels>(captador);

            if (captador == null)
            {
                ViewBag.Mensagem = String.Format("Captador não encontrado!");
                return RedirectToAction("Index");
            }
            return View(captadorVM);
        }

        // POST: Captadors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CaptadorViewModels captadorVM)
        {
            if (ModelState.IsValid)
            {
                var captador = Mapper.Map<CaptadorViewModels, Captador>(captadorVM);

                db.Entry(captador).State = EntityState.Modified;
                await db.SaveChangesAsync();

                ViewBag.Mensagem = String.Format("Informações salvas com sucesso!");
                return RedirectToAction("Index");
            }
            return View(captadorVM);
        }

        // GET: Captadors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.Mensagem = String.Format("Captador não encontrado!");
                return RedirectToAction("Index");
            }
            Captador captador = await db.Captador.FindAsync(id);
            var captadorVM = Mapper.Map<Captador, CaptadorViewModels>(captador);
            if (captador == null)
            {
                ViewBag.Mensagem = String.Format("Captador não encontrado!");
                return RedirectToAction("Index");
            }
            return View(captadorVM);
        }

        // POST: Captadors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Captador captador = await db.Captador.FindAsync(id);

            var user = UserManager.FindByEmail(captador.Email);
            var logins = user.Logins;
            var rolesForUser = await UserManager.GetRolesAsync(user.Id);


            foreach (var login in logins.ToList())
            {
                await UserManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }

            if (rolesForUser.Count() > 0)
            {
                foreach (var item in rolesForUser.ToList())
                {
                    // item should be the name of the role
                    var result = await UserManager.RemoveFromRoleAsync(user.Id, item);
                }
            }

            await UserManager.DeleteAsync(user);

            db.Captador.Remove(captador);
            await db.SaveChangesAsync();

            ViewBag.Mensagem = String.Format("Captador Excluído com sucesso!");
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
