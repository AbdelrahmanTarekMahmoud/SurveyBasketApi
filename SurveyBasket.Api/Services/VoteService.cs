using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Contracts.Votes;
using SurveyBasket.Api.Entities;

namespace SurveyBasket.Api.Services
{
    public class VoteService(ApplicationDbContext context) : IVoteService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result> AddAsync(int pollId, string userId, VoteRequest voteRequest, CancellationToken cancellationToken)
        {
            //checking if the user already voted in this poll
            var isUserAlreadyVotedToPoll = await _context.Votes.AnyAsync
                (x => x.PollId == pollId && x.UserId == userId, cancellationToken);
            if (isUserAlreadyVotedToPoll)
            {
                return Result.Failure(VoteErrors.DuplicatedVote);
            }
            //check if the poll itself exist or not
            var isPollExist = await _context.polls.AnyAsync(x => x.Id == pollId
                && x.IsPublished
                && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow), cancellationToken);
            if (!isPollExist)
            {
                return Result.Failure(PollErrors.PollNotFound);
            }

            //check if the question with the "pollId" aligns with the ones in "voteRequest"
            //we need only ids so we will select x.Id only
            var inDbQuestion = await _context.Questions.Where(x => x.PollId == pollId && x.isActive)
                .Select(x=>x.Id).ToListAsync(cancellationToken);

            var inRequestQuestions = voteRequest.Answers.Select(x => x.QuestionId).ToList();

            if (!inRequestQuestions.SequenceEqual(inDbQuestion))
            {
                return Result.Failure(VoteErrors.InvalidQuestions);
            }

            var vote = new Vote
            {
                PollId = pollId,
                UserId = userId,
                VoteAnswers = voteRequest.Answers.Adapt<IEnumerable<VoteAnswer>>().ToList()
            };
            await _context.AddAsync(vote);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
