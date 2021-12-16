
using Microsoft.EntityFrameworkCore;

namespace Clinic_Web.Models.Models
{
    public class Database_controller : DbContext
    {
        public Database_controller(DbContextOptions<Database_controller> options) : base(options)
        {


        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Visit> Visits { get; set; }

    }
}
