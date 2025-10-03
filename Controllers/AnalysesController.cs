using Microsoft.AspNetCore.Mvc;
using nplBackEnd.DTOs;
using nplBackEnd.Services.Abstractions;

namespace nplBackEnd.Controllers;

    [ApiController]
    [Route("api/analyses")]
    public class AnalysesController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;

        public AnalysesController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var history = await _analysisService.GetAnalysisHistoryAsync();
            return Ok(history);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var analysis = await _analysisService.GetAnalysisByIdAsync(id);
            return analysis == null ? NotFound() : Ok(analysis);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAnalysisRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _analysisService.CreateAnalysisAsync(request);

            return result == null
                ? StatusCode(500, "Failed to process analysis.")
                : CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
    }

