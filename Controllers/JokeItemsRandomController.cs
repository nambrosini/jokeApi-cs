using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JokeApi.Models;

namespace JokeApi.Controllers
{
    [Route("api/JokeItems/Random")]
    [ApiController]
    public class JokeItemsRandomController : ControllerBase
    {
        private readonly JokeContext _context;

        public JokeItemsRandomController(JokeContext context)
        {
            _context = context;
        }

        // GET: api/JokeItems/Random
        [HttpGet]
        public async Task<ActionResult<JokeItem>> GetRandomJokeItem()
        {
            var count = await _context.JokeItems.CountAsync();

            if (count == 0) {
                return NotFound();
            }

            long randomId = new Random().Next(1, count + 1);
            var jokeItem = await _context.JokeItems.FindAsync(randomId);
            
            if (jokeItem == null) {
                return NotFound();
            }

            return jokeItem;
        }

        // GET: api/JokeItems/Random/5
        [HttpGet("{count}")]
        public async Task<ActionResult<IEnumerable<JokeItem>>> GetRandomJokeItemCount(long count)
        {
            var savedCount = await _context.JokeItems.CountAsync();

            if (savedCount <= count) {
                return await _context.JokeItems.ToListAsync();
            }

            List<long> randomIds = new List<long>();

            var counter = 0;

            while (counter < count) {
                var random = new Random().Next(1, savedCount + 1);
                
                if (!randomIds.Contains(random)) {
                    randomIds.Add(random);
                    counter++;
                }
            }

            List<JokeItem> jokeItems = new List<JokeItem>();

            foreach (var id in randomIds) {
                var jokeItem = await _context.JokeItems.FindAsync(id);
                jokeItems.Add(jokeItem);
            }
            
            return jokeItems;
        }

        private bool JokeItemExists(long id)
        {
            return _context.JokeItems.Any(e => e.Id == id);
        }
    }
}
