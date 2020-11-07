using System.Threading.Tasks;
using IntegrationTests.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IntegrationTestsContext dbContext;

        public BooksController(IntegrationTestsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> Get(int id)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<Book>> Create(Book book)
        {
            dbContext.Add(book);

            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(int id, Book newBbook)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return NotFound();

            book.Name = newBbook.Name;
            book.Author = newBbook.Author;
            book.YearPublished = newBbook.YearPublished;

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var book = await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return NotFound();

            dbContext.Remove(book);

            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
