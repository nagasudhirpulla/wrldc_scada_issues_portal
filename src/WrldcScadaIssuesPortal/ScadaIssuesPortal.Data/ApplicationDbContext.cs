using ScadaIssuesPortal.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ScadaIssuesPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<ReportingCase> ReportingCases { get; set; }
        public DbSet<ReportingCaseItem> ReportingCaseItems { get; set; }
        public DbSet<CaseItemTemplate> CaseItemTemplates { get; set; }
        public DbSet<CaseItemOption> CaseItemOptions { get; set; }
        public DbSet<ReportingCaseConcerned> ReportingCaseConcerneds { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
