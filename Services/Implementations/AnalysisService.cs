using nplBackEnd.Data;
using nplBackEnd.DTOs;
using nplBackEnd.Models;
using nplBackEnd.Services.Abstractions;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace nplBackEnd.Services.Implementations;
    public class AnalysisService : IAnalysisService
    {
        private readonly ApplicationDbContext _context;
        private readonly INlpService _nlpService;
        private readonly ILogger<AnalysisService> _logger;

        public AnalysisService(ApplicationDbContext context, INlpService nlpService, ILogger<AnalysisService> logger)
        {
            _context = context;
            _nlpService = nlpService;
            _logger = logger;
        }

        public async Task<AnalysisDetailDto?> CreateAnalysisAsync(CreateAnalysisRequest request)
        {
            // 1. Salva a "intenção" de análise com status "processando"
            var analysis = new Analysis
            {
                Name = request.Name,
                Status = AnalysisStatus.Processing,
                CreatedAt = DateTime.UtcNow
            };
            _context.Analyses.Add(analysis);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Analysis intent created with ID: {AnalysisId}", analysis.Id);

            // 2. Chama o serviço Python/spaCy
            var nlpRequest = new NlpRequest(request.InputText, request.Keywords);
            var nlpResponse = await _nlpService.AnalyzeTextAsync(nlpRequest);

            // 3. Recebe a resposta, atualiza a análise e salva os resultados
            if (nlpResponse == null)
            {
                analysis.Status = AnalysisStatus.Failed;
                await _context.SaveChangesAsync();
                _logger.LogError("NLP service failed for Analysis ID: {AnalysisId}", analysis.Id);
                return null;
            }

            analysis.InputText = request.InputText;
            analysis.Status = AnalysisStatus.Completed;

            // Mapeia todos os resultados, encontrados ou não
            var keywordSet = new HashSet<string>(request.Keywords, StringComparer.OrdinalIgnoreCase);
            foreach (var match in nlpResponse.Matches)
            {
                var result = new AnalysisResult
                {
                    AnalysisId = analysis.Id,
                    Keyword = match.Keyword,
                    FoundOccurrencesJson = JsonSerializer.Serialize(match.Occurrences)
                };
                analysis.Results.Add(result);
                keywordSet.Remove(match.Keyword); // Remove para saber quais não foram encontrados
            }

            // Adiciona resultados para palavras-chave que não foram encontradas
            foreach (var notFoundKeyword in keywordSet)
            {
                analysis.Results.Add(new AnalysisResult
                {
                    AnalysisId = analysis.Id,
                    Keyword = notFoundKeyword,
                    FoundOccurrencesJson = null
                });
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Analysis ID: {AnalysisId} completed successfully.", analysis.Id);

            return await GetAnalysisByIdAsync(analysis.Id);
        }

        public async Task<AnalysisDetailDto?> GetAnalysisByIdAsync(int id)
        {
            var analysis = await _context.Analyses
                .Include(a => a.Results)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (analysis == null) return null;

            return new AnalysisDetailDto(
                analysis.Id,
                analysis.Name,
                analysis.InputText ?? "",
                analysis.CreatedAt,
                analysis.Status,
                analysis.Results.Select(r => new AnalysisResultDto(
                    r.Keyword,
                    r.WasFound,
                    r.WasFound ? JsonSerializer.Deserialize<List<NlpOccurrence>>(r.FoundOccurrencesJson!) : null
                )).ToList()
            );
        }

        public async Task<IEnumerable<AnalysisHistoryItemDto>> GetAnalysisHistoryAsync()
        {
            return await _context.Analyses
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AnalysisHistoryItemDto(a.Id, a.Name, a.CreatedAt, a.Status))
                .ToListAsync();
        }

        public async Task<LibraryKeywordDto?> AddLibraryKeywordAsync(CreateLibraryKeywordDto request)
        {
            var existing = await _context.LibraryKeywords
                .AnyAsync(lk => lk.Keyword.ToLower() == request.Keyword.ToLower());

            if (existing)
            {
                return null;
            }

            var newKeyword = new LibraryKeyword { Keyword = request.Keyword };
            _context.LibraryKeywords.Add(newKeyword);
            await _context.SaveChangesAsync();

            return new LibraryKeywordDto(newKeyword.Id, newKeyword.Keyword);
        }

        public async Task<IEnumerable<LibraryKeywordDto>> GetLibraryKeywordsAsync()
        {
            return await _context.LibraryKeywords
                .AsNoTracking()
                .OrderBy(lk => lk.Keyword)
                .Select(lk => new LibraryKeywordDto(lk.Id, lk.Keyword))
                .ToListAsync();
        }
    }

