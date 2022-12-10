using System.Collections.Generic;
using EmployeeTestApi.Contract;
using EmployeeTestApi.Domain;
using EmployeeTestApi.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JwtAuthDemo.Services
{
   
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly EmployeeDbContext _dbContext;
        // inject your database here for user validation
        public UserService(ILogger<UserService> logger, EmployeeDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public Task AddUser(UserDto user)
        {
           var lastUserId= _dbContext.Users.Count();
            var userToAdd = new User()
            {
                Id = lastUserId,
                Password = user.Password,
                UserName = user.UserName

            };
            _dbContext.Users.Add(userToAdd);
            _dbContext.SaveChanges();
            return Task.CompletedTask;
        }
        public async Task<bool> IsValidUserCredentials(UserDto user)
        {
            _logger.LogInformation($"Validating user [{user.UserName}]");
            if (string.IsNullOrWhiteSpace(user.UserName))
            {
              return  false;
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                return false;
            }

            var isAny= await _dbContext.Users.AnyAsync(x=>x.UserName.ToLower().Equals(user.UserName.ToLower())&&x.Password== user.Password);
            return isAny;
        }

        public async Task<bool> IsAnExistingUser(string userName)
        {
            return await _dbContext.Users.AnyAsync(x => x.UserName.ToLower().Equals(userName.ToLower()));

        }

        public async Task<string> GetUserRole(string userName)
        {
            var user=  _dbContext.Users.Where(x => x.UserName.ToLower().Equals(userName.ToLower())).FirstOrDefault();
            return user.Role;

        }
    }

}
