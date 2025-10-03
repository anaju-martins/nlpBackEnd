using Microsoft.AspNetCore.Mvc;
using nplBackEnd.DTOs;
using nplBackEnd.Services.Abstractions;

namespace nplBackEnd.Controllers;

    [ApiController]
    [Route("api/library/keywords")]
    public class LibraryKeywordsController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;

        public LibraryKeywordsController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var keywords = await _analysisService.GetLibraryKeywordsAsync();
            return Ok(keywords);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateLibraryKeywordDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Keyword))
            {
                return BadRequest("Keyword cannot be empty.");
            }

            var result = await _analysisService.AddLibraryKeywordAsync(request);

            return result == null
                ? Conflict("Keyword already exists.")
                : Ok(result);
        }
    }

