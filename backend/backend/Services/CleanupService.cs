using backend.Database;
using backend.Files;
using Microsoft.EntityFrameworkCore;
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

                List<GLB> Models = await ctx.GLBs.ToListAsync();

                foreach (GLB Model in Models)
                {
                    int Active = await ctx.ActiveASTs.Where(a=>a.FileID==Model.Hash).CountAsync();

                    if (Active == 0)
                    {
                        ctx.GLBs.Remove(Model);

                    }

                }

                List<BMP> Images = await ctx.BMPs.ToListAsync();

                foreach (BMP Image in Images)
                {
                    int Active = await ctx.ActiveRPFs.Where(a => a.FileID == Image.Hash).CountAsync();

                    if (Active == 0)
                    {
                        ctx.BMPs.Remove(Image);

                    }

                }

                List<WAV> Sounds = await ctx.WAVs.ToListAsync();

                foreach (WAV Sound in Sounds)
                {
                    int Active = await ctx.ActiveWLs.Where(a => a.FileID == Sound.Hash).CountAsync();

                    if (Active == 0)
                    {
                        ctx.WAVs.Remove(Sound);

                    }

                }


                await ctx.SaveChangesAsync();

                Models.Clear();
                Images.Clear();
                Sounds.Clear();


            }


            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }

 
    }
}