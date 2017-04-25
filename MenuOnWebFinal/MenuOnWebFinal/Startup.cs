using MenuOnWebFinal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MenuOnWebFinal.Startup))]
namespace MenuOnWebFinal
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
            var UserManager = new UserManager<User>(new UserStore<User>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Administrator"))
            {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new User();
                user.UserName = "yulia.furta@gmail.com";
                user.Email = "yulia.furta@gmail.com";
                string userPWD = "aaaaaa";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Administrator");
                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Moderator"))
            {
                var role = new IdentityRole();
                role.Name = "Moderator";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

            }
        }
    }
}
