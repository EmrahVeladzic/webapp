using backend.Database;
using backend.Files;
using backend.Models;
using backend.Requests;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace backend.Converters
{
    public class AST_DATA
    {
        private ModelJson Input { get; set; }

        public AST? Output { get; set; }

        public GLB Model { get; set; }

        public AST_DATA(ModelJson input)
        {
            DarkforgeDBContext ctx = new DarkforgeDBContext();

            this.Input = input;

            this.Model = ctx.GLBs.Where(g=>g.Hash==input.ModelHash).First();

            this.Model.Setup(this.Model!.Serialized!,this.Model!.Hash!);

            this.Output = new AST();

            //ctx.Add(this.Output);

            ctx.SaveChanges();

            JsonElement joints = Model!.Metadata!.RootElement.GetProperty("skins[0]joints");

            if(joints.ValueKind == JsonValueKind.Array)
            {

                Console.WriteLine("Joints exist");

            }

            


            ctx.SaveChanges();

            ctx.Dispose();

        }



    }
}
