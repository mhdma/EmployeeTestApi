using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace EmployeeTestApi.IInfrastructure
{
    public interface IJwtRefreshTokenCache 
    {
      
        public Task StartAsync(CancellationToken stoppingToken);

        public Task StopAsync(CancellationToken stoppingToken);


        
    }
}
