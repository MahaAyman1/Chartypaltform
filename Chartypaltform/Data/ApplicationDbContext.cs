using Chartypaltform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chartypaltform.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<CharityOrganization> CharityOrganizations { get; set; }
        public DbSet<Success_Story> Success_Story { get; set; }
        public DbSet<Volunteering> Volunteerings { get; set; }
        public DbSet<VolunteeringTaskSelection> VolunteeringTaskSelections { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Complaint> Complaints { get; set; }

    }

    }


