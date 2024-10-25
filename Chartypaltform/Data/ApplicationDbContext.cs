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








    }

}


