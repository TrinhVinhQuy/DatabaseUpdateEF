using DatabaseUpdateEF.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseUpdateEF
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var companyId = Guid.NewGuid();

            modelBuilder.Entity<Company>(builder =>
            {
                builder.ToTable("Company");

                builder.HasMany(c => c.Employees)
                .WithOne()
                .HasForeignKey(e => e.CompanyId)
                .IsRequired();

                builder.HasData(new Company { Id = companyId, Name = "Google Company" });
            });

            modelBuilder.Entity<Employee>(builder =>
            {
                builder.ToTable(nameof(Employee));

                var Employees = Enumerable
                .Range(1, 1000)
                .Select(index => new Employee
                {
                    Id = Guid.NewGuid(),
                    Name = $"Employee {index}",
                    Salary = 1000,
                    CompanyId = companyId,
                }).ToList();

                builder.HasData(Employees);
            });
        }
    }
}
