using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RechargeApp.Models;

namespace RechargeApp.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        
        }
        public DbSet<User> Users { get; set; }
        public DbSet<PlanTable> planTables { get; set; }
        public DbSet<RechargeTable> rechargeTables { get; set; }
    }
}
