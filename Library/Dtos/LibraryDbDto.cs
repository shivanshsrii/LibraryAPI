using Library.Models;

namespace Library.Dtos
{
    public class LibraryDbDto
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string AuthorName { get; set; }
        public IFormFile? Image { get; set; }
    }

}
