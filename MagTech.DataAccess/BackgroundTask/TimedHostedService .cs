using MagTech.DataAccess.Repository;
using MagTech.DataAccess.Repository.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagTech.BackgroundTask
{
    public class TimedHostedService : IHostedService, IDisposable
    {

        private readonly ILogger<TimedHostedService> _logger;
        private IHanelRepository _hanel { get; set; }      

        private Timer _timer;

        public TimedHostedService(ILogger<TimedHostedService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _hanel = new HanelRepository(configuration);

        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _hanel.SetReadAMBBackup();            
            System.Threading.Thread.Sleep(1000);
            _hanel.SetReadOPJournalBackup();
            System.Threading.Thread.Sleep(10000);
            _hanel.GetFromOpjToSqlite();           
            System.Threading.Thread.Sleep(10000);
             _hanel.GetFromAmdToSqlite2();

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
