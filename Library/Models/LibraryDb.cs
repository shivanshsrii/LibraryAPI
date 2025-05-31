using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class LibraryDb
    {
        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }
        public Author Author { get; set; }
        public string Genre { get; set; }
        public string? ImagePath { get; set; }
    }
}
