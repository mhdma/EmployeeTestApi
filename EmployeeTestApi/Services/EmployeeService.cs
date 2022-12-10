using System.Collections.Generic;
using AutoMapper;
using EmployeeTestApi.Contract;
using EmployeeTestApi.Domain;
using EmployeeTestApi.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JwtAuthDemo.Services
{

    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeDbContext _db;
        private IMapper _mapper { get; }
        public EmployeeService(EmployeeDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<EmployeeDto>> GetEmployees()
        {


            return _mapper.Map<List<EmployeeDto>>(_db.Employees.ToList());
        }


        public async Task<EmployeeDto> PutEmployee(int id, CreateUpdateEmployeeDto employee)
        {
            var employeeToDb = await _db.Employees.FindAsync(id);
            employeeToDb.Name = employee.Name;
            employeeToDb.Email = employee.Email;
            employeeToDb.Phone = employee.Phone;
            _db.Employees.Update(employeeToDb);
            _db.SaveChanges();
            return _mapper.Map<EmployeeDto>(employeeToDb);
        }

        public async Task<List<EmployeeDto>> AddEmployee(CreateUpdateEmployeeDto employee)
        {
            var employeeToDb = _mapper.Map<Employee>(employee);
            _db.Employees.Add(employeeToDb);
            _db.SaveChanges();
            return _mapper.Map<List<EmployeeDto>>(_db.Employees.ToList());
        }

        public async Task<EmployeeDto> GetEmployeeById(int Id)
        {

            return _mapper.Map<EmployeeDto>(_db.Employees.Where(x => x.Id == Id).FirstOrDefault());
        }

        public  async Task<EmployeeDto> DeleteEmployee(int Id)
        {
          var employeeToDelete=  _db.Employees.Find(Id);
            _db.Employees.Remove(employeeToDelete);
            _db.SaveChanges();
            return _mapper.Map<EmployeeDto>(employeeToDelete);
        }
    }

}
