using System.Text.Json.Serialization;

namespace nplBackEnd.DTOs;
    // --- Request para o serviço Python ---
    public record NlpRequest(
        string Text,
        List<string> Keywords
    );

    // --- Response do serviço Python ---
    public record NlpResponse(
        [property: JsonPropertyName("matches")] List<NlpMatch> Matches,
        [property: JsonPropertyName("summary")] NlpSummary Summary
    );

    public record NlpMatch(
        [property: JsonPropertyName("keyword")] string Keyword,
        [property: JsonPropertyName("occurrences")] List<NlpOccurrence> Occurrences
    );

    public record NlpOccurrence(
        [property: JsonPropertyName("text")] string Text,
        [property: JsonPropertyName("lemma")] string Lemma,
        [property: JsonPropertyName("start")] int Start,
        [property: JsonPropertyName("end")] int End,
        [property: JsonPropertyName("sent")] string Sentence,
        [property: JsonPropertyName("snippet")] string Snippet
    );

    public record NlpSummary(
        [property: JsonPropertyName("totalKeywords")] int TotalKeywords,
        [property: JsonPropertyName("keywordsFound")] int KeywordsFound,
        [property: JsonPropertyName("totalOccurrences")] int TotalOccurrences
    );

