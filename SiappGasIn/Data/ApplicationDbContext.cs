using System;
using SiappGasIn.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using SiappGasIn.Models.AccountViewModels;

namespace SiappGasIn.Data
{


    public class GasDbContext : DbContext
    {
        public GasDbContext(DbContextOptions<GasDbContext> options) : base(options)
        { }

        public DbSet<MstParameter> MstParameter { get; set; }
        public DbSet<MstEnergy> MstEnergy { get; set; }
        public DbSet<MstHargaPRS> MstHargaPRS { get; set; }
        public DbSet<MstCraddle> MstCraddle { get; set; }
        public DbSet<MstHeadTruck> MstHeadTruck { get; set; }
        public DbSet<MstLokasiSPBG> MstLokasiSPBG { get; set; }
        public DbSet<SimulationCost> SimulationCost { get; set; }
        public DbSet<PipeCalculator> PipeCalculator { get; set; }
        public DbSet<HeaderSimulationCost> HeaderSimulationCost { get; set; }
        public DbSet<SP_GetGajiByLocationName> SP_GetGajiByLocationName { get; set; }
        public DbSet<MstGaji> MstGaji { get; set; }
        public DbSet<MstUnit> MstUnit { get; set; }
        public DbSet<MstKlasifikasi> MstKlasifikasi { get; set; }
        public DbSet<MstItemKlasifikasi> MstItemKlasifikasi { get; set; }
        public DbSet<MstFEL> MstFEL { get; set; }
        public DbSet<SysRoleViewModel> SysRoles { get; set; }
        public DbSet<SysUserProfile> SysUserProfile { get; set; }
        //public DbSet<MstFel2> MstFel2 { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<SP_GetGajiByLocationName>().HasNoKey();
            builder.Entity<MstListGaji>().HasNoKey();
            builder.Entity<SP_HeaderSimulation>().HasNoKey();
            builder.Entity<SP_DetailSimulation>().HasNoKey();
            builder.Entity<SP_CostSimulation>().HasNoKey();

        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private IConfiguration Configuration { get; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<SysUserProfile> SysUserProfile { get; set; }



    }
}

