using AutoMapper;
using EmployeeTestApi.Contract;
using EmployeeTestApi.Domain;

namespace EmployeeTestApi.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<CreateUpdateEmployeeDto, Employee>();
            CreateMap<CreateUpdateEmployeeDto, EmployeeDto>();

        }
    }
}
