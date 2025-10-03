using System.ComponentModel.DataAnnotations;

namespace nplBackEnd.Models;
    public class AnalysisResult
    {
        public int Id { get; set; }
        public int AnalysisId { get; set; }
        public Analysis Analysis { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Keyword { get; set; } = string.Empty;
        public string? FoundOccurrencesJson { get; set; }

        public bool WasFound => !string.IsNullOrEmpty(FoundOccurrencesJson);
    }
