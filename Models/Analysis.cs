using System.ComponentModel.DataAnnotations;

namespace nplBackEnd.Models;
    public enum AnalysisStatus
    {
        Processing,
        Completed,
        Failed
    }

    public class Analysis
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? InputText { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public AnalysisStatus Status { get; set; }

        public ICollection<AnalysisResult> Results { get; set; } = new List<AnalysisResult>();
    }

