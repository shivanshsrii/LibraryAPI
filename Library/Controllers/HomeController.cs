using Library.Dtos;
using Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public HomeController(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _dbContext.LibraryDbs
                .Include(b => b.Author)
                .ToListAsync();
            return Ok(books);
        }

        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            var book = await _dbContext.LibraryDbs
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == bookId);
            if (book == null)
                return NotFound();
            return Ok(book);
        }

        [HttpPost("add-with-photo")]
        public async Task<IActionResult> AddNewBook([FromForm] LibraryDbDto request)
        {
            string? imagePath = null;

            if (request.Image != null)
            {
                var folderPath = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(request.Image.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }

                imagePath = "/images/" + fileName;
            }

            var book = new LibraryDb
            {
                Title = request.Title,
                Genre = request.Genre,
                ImagePath = imagePath,
                Author = new Author { AuthorName = request.AuthorName }
            };

            await _dbContext.LibraryDbs.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook(int bookId, [FromForm] LibraryDbDto request)
        {
            var existingBook = await _dbContext.LibraryDbs
                .Include(b => b.Author)
                .FirstOrDefaultAsync(b => b.BookId == bookId);

            if (existingBook == null)
                return NotFound();

            existingBook.Title = request.Title;
            existingBook.Genre = request.Genre;

            if (request.Image != null)
            {
                var folderPath = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid() + Path.GetExtension(request.Image.FileName);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(stream);
                }

                existingBook.ImagePath = "/images/" + fileName;
            }

            if (existingBook.Author == null)
                existingBook.Author = new Author();

            existingBook.Author.AuthorName = request.AuthorName;

            _dbContext.LibraryDbs.Update(existingBook);
            await _dbContext.SaveChangesAsync();

            return Ok(existingBook);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var existingBook = await _dbContext.LibraryDbs.FindAsync(bookId);
            if (existingBook == null)
                return NotFound();

            _dbContext.LibraryDbs.Remove(existingBook);
            await _dbContext.SaveChangesAsync();
            return Ok(existingBook);
        }
    }
}
