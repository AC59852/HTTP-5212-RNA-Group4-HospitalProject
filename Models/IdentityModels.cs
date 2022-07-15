using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HTTP_5212_RNA_Group4_HospitalProject.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; } 
       
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HUpdate> HUpdates { get; set; }
        public DbSet<Article> Articles { get; set; }

        public DbSet<Research> Researches { get; set; }
        public DbSet<Donar> Donars { get; set; }
        public DbSet<Service> Services { get; set; }

        public DbSet<Department> Departments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}