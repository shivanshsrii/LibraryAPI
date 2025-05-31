using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Author
    {
        [Key]
        public int AuthoId { get; set; }
        public string AuthorName { get; set; }
    }
}