using nplBackEnd.DTOs;

namespace nplBackEnd.Services.Abstractions;
    public interface INlpService
    {
        Task<NlpResponse?> AnalyzeTextAsync(NlpRequest request);

    }

