using System.Collections.Generic;
using EmployeeTestApi.Contract;
using EmployeeTestApi.Domain;
using Microsoft.Extensions.Logging;

namespace JwtAuthDemo.Services
{
    public interface IEmployeeService
    {
        Task<List<EmployeeDto>> AddEmployee(CreateUpdateEmployeeDto employee);
        Task<List<EmployeeDto>> GetEmployees();
        Task<EmployeeDto> PutEmployee(int Id, CreateUpdateEmployeeDto employee);
        Task<EmployeeDto> GetEmployeeById(int id);
        Task<EmployeeDto> DeleteEmployee(int id);

    }
}

    
