using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using Go_Parking.Controllers;

namespace Go_Parking.Models
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Veiculo> Veiculos { get; set; }

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
            : base("ContextoBanco", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions
                .Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Veiculo>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<Veiculo>()
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Veiculo>()
                .HasRequired(c => c.Users)
                .WithMany(v => v.Veiculos)
                .HasForeignKey(v => v.UserId);
            modelBuilder.Entity<Veiculo>()
                .HasRequired(c => c.Users)
                .WithMany(v => v.Veiculos)
                .HasForeignKey(v => v.UserId)
                .WillCascadeOnDelete(true);
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Vaga> Vagas { get; set; }
        public DbSet<Veiculo> Veiculoes { get; set; }

        public System.Data.Entity.DbSet<Go_Parking.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public DbSet<EditUserViewModel> EditUserViewModels { get; set; }
        //public DbSet<Funcionario> Funcionario {get; set; }

        //public System.Data.Entity.DbSet<Go_Parking.Models.RoleViewModel> RoleViewModels { get; set; }
    }
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base(){ }
        public ApplicationRole(string roleName) : base(roleName) { }
    }

}