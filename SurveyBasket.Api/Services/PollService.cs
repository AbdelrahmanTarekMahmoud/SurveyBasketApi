

using SurveyBasket.Api.Contracts.Polls;

namespace SurveyBasket.Api.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.polls
                .AsNoTracking()  // This ensures no tracking of the entity, which improves performance when you don't need to modify the data.
                .Select(p => p.Adapt<PollResponse>())  // Map Poll entity to PollResponse
                .ToListAsync(cancellationToken);  // Execute the query and return a list
        }


        public async Task<Result<PollResponse>> GetAsync(int id , CancellationToken cancellationToken)
        {
            var poll =  await _context.polls.FindAsync(id , cancellationToken);
            if(poll == null)
            {
                return Result.Failure<PollResponse>(PollErrors.PollNotFound);
            }
            return Result.Success<PollResponse>(poll.Adapt<PollResponse>());
        }

        public async Task<Result<PollResponse>> AddAsync(PollRequest pollRequest, CancellationToken cancellationToken)
        {
            var poll = pollRequest.Adapt<Poll>();
            await _context.polls.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success<PollResponse>(poll.Adapt<PollResponse>());
        }

        
        public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken)
        {
            var UpdatedPoll = await _context.polls.FindAsync(id);
            if (UpdatedPoll is null) return Result.Failure(PollErrors.PollNotFound);
            UpdatedPoll.Summary = poll.Summary;
            UpdatedPoll.Title = poll.Title;
            UpdatedPoll.StartsAt = poll.StartsAt;
            UpdatedPoll.EndsAt = poll.EndsAt;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id , CancellationToken cancellationToken)
        {
            var DeletedPoll = await _context.polls.FindAsync(id , cancellationToken);
            if (DeletedPoll is null) return Result.Failure(PollErrors.PollNotFound);
            _context.Remove(DeletedPoll);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> TogglePublishedStateAsync(int id, CancellationToken cancellationToken )
        {
            var Poll = await _context.polls.FindAsync(id , cancellationToken);
            if (Poll is null) return Result.Failure(PollErrors.PollNotFound);
            Poll.IsPublished = !Poll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
            
        }
    }
}
