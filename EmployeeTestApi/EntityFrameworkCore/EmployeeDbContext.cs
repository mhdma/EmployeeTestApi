

using EmployeeTestApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTestApi.EntityFrameworkCore
{

    public class EmployeeDbContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }


        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {

        }

    }
}