using App.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.API.Services
{
    public class ExpiredVacancyArchiver : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ExpiredVacancyArchiver(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                    var expiredVacancies = dbContext.Vacancy
                        .Where(v => v.ExpiryDate <= DateTime.Now)
                        .ToList();
                    expiredVacancies.All(c => { c.RecordStatus = Core.Entities.Base.RecordStatus.Archived; return true; });

                    dbContext.Vacancy.UpdateRange(expiredVacancies);

                    await dbContext.SaveChangesAsync();
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Run daily, adjust as needed
            }
        }
    }
}
