

using Microsoft.AspNetCore.Authorization;
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
                : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
        }


        [HttpPost("")]
        public async Task<IActionResult> Add([FromBody]QuestionRequest questionRequest , [FromRoute]int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionService.AddAsync(questionRequest , pollId, cancellationToken);
            if(result.IsFailure && result.Error == PollErrors.PollNotFound)
            {
                return  Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
            }
            if(result.IsFailure && result.Error == QuestionsErrors.DuplicatedQuestion)
            {
                return Problem(statusCode: StatusCodes.Status409Conflict, title: result.Error.Code, detail: result.Error.Description);
            }
            return CreatedAtAction(nameof(Get), new { pollId = pollId, id = result.Value.Id }, result.Value);
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAllAsync(pollId , cancellationToken);
            return result.IsSuccess ? Ok(result.Value)
                : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description); 
        }

        [HttpPut("{questionId}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int questionId, CancellationToken cancellationToken)
        {
            var state = await _questionService.ToggleActiveStatus(pollId, questionId , cancellationToken);
            return state.IsFailure ? Problem(statusCode: StatusCodes.Status404NotFound, title: state.Error.Code, detail: state.Error.Description) : NoContent();
        }

        [HttpPut("{questionId}")]
        public async Task<IActionResult> Update([FromRoute] int pollId , [FromRoute] int questionId , [FromBody] QuestionRequest request , CancellationToken cancellationToken)
        {
            var result = await _questionService.UpdateAsync(pollId , questionId , request , cancellationToken);
            if(result.IsFailure && result.Error == QuestionsErrors.DuplicatedQuestion)
            {
                return Problem(statusCode: StatusCodes.Status409Conflict , title: result.Error.Code, detail: result.Error.Description);
            }
            if (result.IsFailure && result.Error == QuestionsErrors.QuestionNoFound)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Description);
            }
            return NoContent();
        }


    } 
}
