using backend.Database;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace backend.Repositories
{
    public class BMP_Repository :IBMP_Repository
    {
        private DarkforgeDBContext DB;

        public BMP_Repository(DarkforgeDBContext DBCtx)
        {
            this.DB = DBCtx;
        }

       
        public async Task UploadBMP(byte[] BMPData, string sha1)
        {

            BMP temp=new BMP();

            await Task.Run(() =>{
                temp.Setup(BMPData, sha1);
            });

            this.DB = new DarkforgeDBContext();


            this.DB.BMPs.Add(temp);
            await this.DB.SaveChangesAsync();


            this.DB.Dispose();

            

        }
    }
}
