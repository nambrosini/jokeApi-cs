using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JokeApi.Models;

namespace JokeApi.Controllers
{
    [Route("api/Jokes")]
    [ApiController]
    public class JokeItemsController : ControllerBase
    {
        private readonly JokeContext _context;

        public JokeItemsController(JokeContext context)
        {
            _context = context;
        }

        // GET: api/Jokes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JokeItemDTO>>> GetJokes()
        {
            return await _context.JokeItems
                .Select(x => JokeToDTO(x))
                .ToListAsync();
        }

        // GET: api/Jokes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JokeItemDTO>> GetJoke(long id)
        {
            var jokeItem = await _context.JokeItems.FindAsync(id);

            if (jokeItem == null)
            {
                return NotFound();
            }

            return JokeToDTO(jokeItem);
        }

        // PUT: api/Jokes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJokes(long id, JokeItemDTO jokeItemDTO)
        {
            if (id != jokeItemDTO.Id)
            {
                return BadRequest();
            }

            var jokeItem = await _context.JokeItems.FindAsync(id);
            if (jokeItem == null)
            {
                return NotFound();
            }

            jokeItem.Content = jokeItemDTO.Content;
            jokeItem.Author = jokeItemDTO.Author;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!JokeItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Jokes/upvote/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("upvote/{id}")]
        public async Task<IActionResult> UpvoteJoke(long id)
        {

            var jokeItem = await _context.JokeItems.FindAsync(id);
            if (jokeItem == null)
            {
                return NotFound();
            }

            jokeItem.Upvotes++;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!JokeItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // PUT: api/Jokes/downvote/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("downvote/{id}")]
        public async Task<IActionResult> DownvoteJoke(long id)
        {

            var jokeItem = await _context.JokeItems.FindAsync(id);
            if (jokeItem == null)
            {
                return NotFound();
            }

            jokeItem.Upvotes--;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!JokeItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Jokes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<JokeItem>> CreateJoke(JokeItemDTO jokeItemDTO)
        {
            var jokeItem = new JokeItem
            {
                Content = jokeItemDTO.Content,
                Author = jokeItemDTO.Author,
                Upvotes = 0,
                Downvotes = 0
            };

            _context.JokeItems.Add(jokeItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetJokes),
                new { id = jokeItem.Id },
                JokeToDTO(jokeItem));
        }

        // DELETE: api/Jokes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<JokeItem>> DeleteJoke(long id)
        {
            var jokeItem = await _context.JokeItems.FindAsync(id);
            if (jokeItem == null)
            {
                return NotFound();
            }

            _context.JokeItems.Remove(jokeItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Jokes
        [HttpDelete()]
        public async Task<ActionResult<JokeItem>> DeleteJokes()
        {
            var jokes = await _context.JokeItems.ToListAsync();

            if (jokes == null)
            {
                return NotFound();
            }

            _context.JokeItems.RemoveRange(jokes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JokeItemExists(long id)
        {
            return _context.JokeItems.Any(e => e.Id == id);
        }

        private static JokeItemDTO JokeToDTO(JokeItem jokeItem) =>
            new JokeItemDTO
            {
                Id = jokeItem.Id,
                Content = jokeItem.Content,
                Author = jokeItem.Author,
                Upvotes = jokeItem.Upvotes,
                Downvotes = jokeItem.Downvotes
            };
    }
}
