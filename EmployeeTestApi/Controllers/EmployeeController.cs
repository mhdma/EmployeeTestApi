using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EmployeeTestApi.Contract;
using EmployeeTestApi.IInfrastructure;
using JwtAuthDemo.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeTestApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IEmployeeService _employeeService;
 

        public EmployeeController(ILogger<AccountController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        
        }
        [HttpPost("AddEmployee")]
        public Task<List<EmployeeDto>> AddEmployee(CreateUpdateEmployeeDto employee)
        {
            return _employeeService.AddEmployee(employee);
        }
        [HttpGet("GetEmployeeById")]
        public Task<EmployeeDto> GetEmployeeById(int id)
        {
            return _employeeService.GetEmployeeById(id);
        }
        [HttpGet("GetEmployees")]
        public Task<List<EmployeeDto>> GetEmployees()
        {
            return _employeeService.GetEmployees();
        }
        [HttpPut("PutEmployee")]
        public Task<EmployeeDto> PutEmployee(int Id, CreateUpdateEmployeeDto employee)
        {
            return _employeeService.PutEmployee(Id, employee);
        }
        [HttpDelete("DeleteEmployee")]
        public Task<EmployeeDto> DeleteEmployee(int Id)
        {
            return _employeeService.DeleteEmployee(Id);
        }
    }
 
}
