using EmployeeTestApi.Domain;
using EmployeeTestApi.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeTestApi.Model
{
    public class DataSeeder: IDataSeeder
    {
        private readonly EmployeeDbContext employeeDbContext;

        public DataSeeder(EmployeeDbContext employeeDbContext)
        {
            this.employeeDbContext = employeeDbContext;
        }

        public void Seed()
        {
            if(!employeeDbContext.Users.Any())
            {
                
                var employees = new List<User>()
                {
                        new User()
                        {
                            UserName = "admin",
                            Password = "adminPassword",
                          //  Id = 1,
                            Role="admin",
                            
                        },
                        new User()
                        {
                            UserName = "User1",
                            Password = "User1Password",
                           // Id = 2,
                            Role="BasicUser",
                        },
                };

                employeeDbContext.Users.AddRange(employees);
                employeeDbContext.SaveChanges();
            }
        }
    }
}
