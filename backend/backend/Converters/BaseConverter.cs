using backend.Database;

namespace backend.Converters
{
    public class BaseConverter
    {

        public virtual async Task Convert(DarkforgeDBContext ctx)
        {
            await ctx.SaveChangesAsync();
        }

    }
}
