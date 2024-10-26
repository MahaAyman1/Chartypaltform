// Models/ApplicationDbContext.cs
using Chartypaltform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chartypaltform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

	


		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
      
        public DbSet<Donor> Donors { get; set; }
        public DbSet<CharityOrganization> CharityOrganizations { get; set; }
        public DbSet<Volunteering> Volunteerings { get; set; }
        public DbSet<VolunteeringTaskSelection> VolunteeringTaskSelections { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Category> categories { get; set; } 
        public DbSet<AdminUser> adminUsers { get; set; }    
        public DbSet<AdminAction> AdminActions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<SuccessCampaign> successCampaigns { get; set; }
        public DbSet<NavItem> navItems { get; set; }    
        public DbSet<Donation> donations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Campaign)
                .WithMany(c => c.Donations)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Donation>()
                .HasOne(d => d.Donor)
                .WithMany(donor => donor.Donations)
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.Restrict);  

            modelBuilder.Entity<Campaign>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);  
        }






    }

}


