using casasamaritanonorte.DAL;
using casasamaritanonorte.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(casasamaritanonorte.Startup))]
namespace casasamaritanonorte
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }




        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "kelvimguimg@gmail.com";
                user.Email = "kelvimguimg@gmail.com";

                string userPWD = "12345678";

                var chkUser = UserManager.Create(user, userPWD);

                // creating Creating Manager role    
                if (!roleManager.RoleExists("Captador"))
                {
                    var role2 = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role2.Name = "Captador";
                    roleManager.Create(role2);

                    CasaContext db = new CasaContext();

                    Captador captador = new Captador();
                    captador.Email = user.Email;
                    captador.DataNascimento = DateTime.Now;
                    captador.tipoConta = 1;
                    captador.Multirao = false;
                    captador.Integral = false;
                    db.Captador.Add(captador);
                    db.SaveChangesAsync();

                }

                //Add default User to Role Admin  
                if (chkUser.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Captador");
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }


            }



          
        }
    }
}
