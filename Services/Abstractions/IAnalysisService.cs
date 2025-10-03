using nplBackEnd.DTOs;

namespace nplBackEnd.Services.Abstractions;
    public interface IAnalysisService
    {
        Task<AnalysisDetailDto?> CreateAnalysisAsync(CreateAnalysisRequest request);
        Task<IEnumerable<AnalysisHistoryItemDto>> GetAnalysisHistoryAsync();
        Task<AnalysisDetailDto?> GetAnalysisByIdAsync(int id);
        Task<LibraryKeywordDto?> AddLibraryKeywordAsync(CreateLibraryKeywordDto request);
        Task<IEnumerable<LibraryKeywordDto>> GetLibraryKeywordsAsync();
    }

