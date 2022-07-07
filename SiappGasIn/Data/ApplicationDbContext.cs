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
      
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

          
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

        //public DbSet<SysUserProfile> SysUserProfile { get; set; }



    }
}

