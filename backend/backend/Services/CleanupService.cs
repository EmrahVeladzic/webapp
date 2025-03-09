using backend.Database;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CleanupService : BackgroundService
{
   

    public CleanupService()
    {
        
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      

        while (!stoppingToken.IsCancellationRequested)
        {

            using (DarkforgeDBContext ctx = new DarkforgeDBContext())
            {


            }


            await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
        }

 
    }
}