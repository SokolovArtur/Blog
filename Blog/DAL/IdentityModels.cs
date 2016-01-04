using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Blog.Models;

namespace Blog.DAL
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Posts> Posts { get; set; }

        public DbSet<Comments> Comments { get; set; }

        public DbSet<Images> Images { get; set; }

        public DbSet<Tags> Tags { get; set; }

        public DbSet<Sessions> Sessions { get; set; }

        public DbSet<Visits> Visits { get; set; }

        public ApplicationDbContext() : base("DefaultConnection") { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}