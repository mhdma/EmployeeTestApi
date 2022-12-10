using System.Collections.Generic;
using EmployeeTestApi.Contract;
using EmployeeTestApi.Domain;
using Microsoft.Extensions.Logging;

namespace JwtAuthDemo.Services
{
    public interface IUserService
    {
        public Task AddUser(UserDto user);
        Task<string> GetUserRole(string userName);
        Task<bool> IsAnExistingUser(string userName);
        Task<bool> IsValidUserCredentials(UserDto user);
    
    }
}

    
