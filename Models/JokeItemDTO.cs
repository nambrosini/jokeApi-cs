using System;
namespace JokeApi.Models
{
    public class JokeItemDTO
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string Author { get; set; }
    }
}
