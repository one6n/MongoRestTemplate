using Microsoft.AspNetCore.Mvc;
using MongoRestTemplate.Services;
using MongoRestTemplate.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MongoRestTemplate.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ILogger<BooksController> _logger;

        private readonly BooksService _booksService;

        public BooksController(ILogger<BooksController> logger, BooksService booksService)
        {
            _logger = logger;
            _booksService = booksService;
        }

        // GET: api/<BooksController>
        [HttpGet]
        public async Task<List<Book>> Get()
        {
            _logger.Log(LogLevel.Information, $"method={nameof(Get)}");

            return await _booksService.GetAsync();
        }

        // GET api/<BooksController>/5
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            _logger.Log(LogLevel.Information, $"method={nameof(Get)}, id={id}");

            var book = await _booksService.GetAsync(id);
            if (book == null)
                return NotFound();

            return book;
        }

        // POST api/<BooksController>
        [HttpPost]
        public async Task<IActionResult> Post(Book newBook)
        {
            _logger.Log(LogLevel.Information, $"method={nameof(Post)}, newBook={newBook}");
            await _booksService.CreateAsync(newBook);

            return CreatedAtAction(nameof(Post), new { id = newBook.Id }, newBook);
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Book updatedBook)
        {
            _logger.Log(LogLevel.Information, $"method={nameof(Put)}, id={id}, updatedBook={updatedBook}");

            var book = await _booksService.GetAsync(id);
            if (book == null)
                return NotFound();

            await _booksService.UpdateAsync(id, updatedBook);

            return NoContent();

        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.Log(LogLevel.Information, $"method={nameof(Delete)}, id={id}");

            var book = await _booksService.GetAsync(id);
            if (book == null)
                return NotFound();

            await _booksService.RemoveAsync(book.Id);

            return NoContent();
        }
    }
}
