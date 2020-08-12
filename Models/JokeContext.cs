using Microsoft.EntityFrameworkCore;

namespace JokeApi.Models
{
    public class JokeContext : DbContext
    {
        public JokeContext(DbContextOptions<JokeContext> options) : base(options)
        {

        }

        public DbSet<JokeItem> JokeItems { get; set; }
    }
}