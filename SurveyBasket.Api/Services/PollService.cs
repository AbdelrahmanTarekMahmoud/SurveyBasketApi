

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
            //Dealing with exception (duplicate)
            var isExistingTitle = await _context.polls.AnyAsync(x=>x.Title == pollRequest.Title , cancellationToken);
            if(isExistingTitle)
            {
                return Result.Failure<PollResponse>(PollErrors.DuplicatedTitle);
            }
            var poll = pollRequest.Adapt<Poll>();
            await _context.polls.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success<PollResponse>(poll.Adapt<PollResponse>());
        }

        
        public async Task<Result> UpdateAsync(int id, PollRequest pollRequest, CancellationToken cancellationToken)
        {
            //Dealing with exception (duplicate)
            var isExistingTitle = await _context.polls.AnyAsync(x => x.Title == pollRequest.Title &&  x.Id != id
              , cancellationToken);
            if(isExistingTitle)
            {
                return Result.Failure(PollErrors.DuplicatedTitle);
            }
            var UpdatedPoll = await _context.polls.FindAsync(id , cancellationToken);
            if (UpdatedPoll is null) return Result.Failure(PollErrors.PollNotFound);
            UpdatedPoll.Summary = pollRequest.Summary;
            UpdatedPoll.Title = pollRequest.Title;
            UpdatedPoll.StartsAt = pollRequest.StartsAt;
            UpdatedPoll.EndsAt = pollRequest.EndsAt;
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

        public async Task<IEnumerable<PollResponse>> GetCurrentAsyncV1(CancellationToken cancellationToken = default)
        {
            var PublishedPolls = await _context.polls.AsNoTracking()
                .Where(x => x.IsPublished == true 
                && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                .Select(x=>x.Adapt<PollResponse>())
                .ToListAsync(cancellationToken);

            return PublishedPolls;
        }

        public async Task<IEnumerable<PollResponseV2>> GetCurrentAsyncV2(CancellationToken cancellationToken = default)
        {
            var PublishedPolls = await _context.polls.AsNoTracking()
                .Where(x => x.IsPublished == true
                && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
                .Select(x => x.Adapt<PollResponseV2>())
                .ToListAsync(cancellationToken);

            return PublishedPolls;
        }
    }
}
