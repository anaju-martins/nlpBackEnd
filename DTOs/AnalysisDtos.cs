using nplBackEnd.Models;

namespace nplBackEnd.DTOs;
    // Para criar uma nova análise
    public record CreateAnalysisRequest(
        string Name,
        string InputText,
        List<string> Keywords
    );

    // Para listar análises no histórico
    public record AnalysisHistoryItemDto(
        int Id,
        string Name,
        DateTime CreatedAt,
        AnalysisStatus Status
    );

    // Para ver os detalhes de uma análise
    public record AnalysisDetailDto(
        int Id,
        string Name,
        string InputText,
        DateTime CreatedAt,
        AnalysisStatus Status,
        List<AnalysisResultDto> Results
    );

    public record AnalysisResultDto(
        string Keyword,
        bool WasFound,
        List<NlpOccurrence>? Occurrences 
    );
