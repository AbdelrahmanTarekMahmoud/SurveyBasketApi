
namespace SurveyBasket.Api.Services
{
    public class PollService(ApplicationDbContext context) : IPollService
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken)
        { 
            return await _context.polls.AsNoTracking().ToListAsync();
        }

        public async Task<Poll?> GetAsync(int id , CancellationToken cancellationToken)
        {
            return await _context.polls.FindAsync(id);
        }

        public async Task<Poll> AddAsync(Poll poll , CancellationToken cancellationToken)
        {
            //poll.Id = _polls.Count + 1;
            await _context.polls.AddAsync(poll);
            await _context.SaveChangesAsync();
            return poll;
        }

        public async Task<bool> UpdateAsync(int id , Poll poll , CancellationToken cancellationToken)
        {
            var UpdatedPoll = await GetAsync(id ,  cancellationToken);
            if (UpdatedPoll is null) return false;
            UpdatedPoll.Summary = poll.Summary;
            UpdatedPoll.Title = poll.Title;
            UpdatedPoll.StartsAt = poll.StartsAt;
            UpdatedPoll.EndsAt = poll.EndsAt;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> DeleteAsync(int id , CancellationToken cancellationToken)
        {
            var DeletedPoll = await GetAsync(id , cancellationToken);
            if (DeletedPoll is null) return false;
            _context.Remove(DeletedPoll);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> TogglePublishedStateAsync(int id, CancellationToken cancellationToken )
        {
            var Poll = await GetAsync(id , cancellationToken);
            if(Poll is null) return false;
            Poll.IsPublished = !Poll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);
            return true;
            
        }
    }
}
