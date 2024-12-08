﻿

using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Api.Abstractions;
using SurveyBasket.Api.Entities;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController(IQuestionService questionService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;


        [HttpGet("{questionId}")]
       
        public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int questionId,CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAsync(pollId, questionId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value)
                : result.ToProblem();
        }


        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody]QuestionRequest questionRequest , [FromRoute]int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionService.AddAsync(questionRequest , pollId, cancellationToken);

            return result.IsSuccess ? 
                CreatedAtAction(nameof(Get), new { pollId = pollId, questionId = result.Value.Id }, result.Value)
                : result.ToProblem();
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAllAsync(pollId , cancellationToken);
            return result.IsSuccess ? Ok(result.Value)
                : result.ToProblem(); 
        }

        [HttpPut("{questionId}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int questionId, CancellationToken cancellationToken)
        {
            var result = await _questionService.ToggleActiveStatus(pollId, questionId , cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{questionId}")]
        public async Task<IActionResult> Update([FromRoute] int pollId , [FromRoute] int questionId , [FromBody] QuestionRequest request , CancellationToken cancellationToken)
        {
            var result = await _questionService.UpdateAsync(pollId , questionId , request , cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


    } 
}
