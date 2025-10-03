using System.ComponentModel.DataAnnotations;

namespace nplBackEnd.Models;
    public class LibraryKeyword
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Keyword { get; set; } = string.Empty;
    }

