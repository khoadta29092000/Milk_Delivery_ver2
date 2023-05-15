using BusinessObject.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyVerificationCodeCleanupProject.Services
{
    public class VerificationCodeCleanupService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly MilkDBContext _context;

        public VerificationCodeCleanupService(MilkDBContext context)
        {
            _context = context;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(15)); // Chạy công việc định kỳ mỗi giờ
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var expiredCodes = _context.TblVerificationCodes.Where(vc => vc.ExpirationTime < DateTime.UtcNow);
            _context.TblVerificationCodes.RemoveRange(expiredCodes);
            _context.SaveChanges();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}