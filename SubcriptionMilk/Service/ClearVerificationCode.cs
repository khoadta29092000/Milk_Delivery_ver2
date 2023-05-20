using BusinessObject.Models;


namespace MyVerificationCodeCleanupProject.Services
{
    public class VerificationCodeCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public VerificationCodeCleanupService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<MilkDBContext>();
                    var expiredCodes = dbContext.TblVerificationCodes.Where(vc => vc.ExpirationTime < DateTime.Now);
                    dbContext.TblVerificationCodes.RemoveRange(expiredCodes);
                    dbContext.SaveChanges();
                }

  
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}