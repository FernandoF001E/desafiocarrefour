using CashFlow.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CashFlow.Context
{
    public class CashFlowContext : IdentityDbContext
    {
        public DbSet<Account> Account { get; set; }
        public DbSet<FinancialRecords> FinancialRecords { get; set; }

        public CashFlowContext() { }
        public CashFlowContext(DbContextOptions<CashFlowContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>().HasData(InitialDataAccount.Account);
        }
    }
}
