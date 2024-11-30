
namespace SurveyBasket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // body json default
    public class PollsController (IPollService pollService) : ControllerBase
    {
        private readonly IPollService _pollService = pollService;

        [HttpGet]
        public IActionResult GetAll()
        {
            var polls = _pollService.GetAll();
            return Ok(polls.Adapt<IEnumerable<PollResponse>>());
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var element = _pollService.Get(id);
            return element is null ? NotFound() : Ok(element.Adapt<PollResponse>());
        }
        [HttpPost("")]
        public IActionResult Add([FromBody]PollRequest poll)
        {
            var newPoll = _pollService.Add(poll.Adapt<Poll>());
            return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);

        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]int id , [FromBody] PollRequest poll)
        {
            var isUpdated = _pollService.Update(id , poll.Adapt<Poll>());
            if(!isUpdated) return NotFound();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isDeleted = _pollService.Delete(id);
            if(!isDeleted) return NotFound();
            return NoContent();
        }

    }
}
