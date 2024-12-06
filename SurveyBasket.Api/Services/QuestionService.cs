using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Contracts.Answers;
using SurveyBasket.Api.Contracts.Questions;
using System.Collections.Generic;

namespace SurveyBasket.Api.Services
{
    public class QuestionService(ApplicationDbContext context) : IQuestionService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<QuestionResponse>> AddAsync(QuestionRequest questionRequest, int pollId,  CancellationToken cancellationToken = default)
        {
            //Validating if the poll is existed or not
            var isPollExisting = await _context.polls.AnyAsync(x => x.Id == pollId , cancellationToken);
            if(!isPollExisting)
            {
                return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);
            }
            //Validating if the same poll has the same question or not
            var isPollHavingSameQuestionAlready = await _context.Questions.AnyAsync(x => x.Content == questionRequest.Content
            && x.PollId == pollId , cancellationToken);
            if(isPollHavingSameQuestionAlready)
            {
                return Result.Failure<QuestionResponse>(QuestionsErrors.DuplicatedQuestion);
            }

            var Question = questionRequest.Adapt<Question>();
            //Assigning the pollId
            Question.PollId = pollId;
            

            await _context.AddAsync(Question , cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success<QuestionResponse>(Question.Adapt<QuestionResponse>());
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId , CancellationToken cancellationToken = default)
        {
            var isPollExisting = await _context.polls.AnyAsync(x => x.Id == pollId, cancellationToken);
            if (!isPollExisting)
            {
                return Result.Failure<IEnumerable< QuestionResponse >> (PollErrors.PollNotFound);
            }

            //include for eager loading the answers
            var Questions = await _context.Questions.Where(x=>x.PollId == pollId).Include(x => x.Answers)
                .AsNoTracking().Select(x => x.Adapt<QuestionResponse>()).ToListAsync(cancellationToken);
            return Result.Success<IEnumerable<QuestionResponse>>(Questions);
        }

        public async Task<Result<QuestionResponse>> GetAsync(int pollId, int questionId, CancellationToken cancellationToken = default)
        {
            //include for eager loading the answers
            var Question = await _context.Questions.Where(x => x.PollId == pollId && x.Id == questionId).Include(x => x.Answers)
                .AsNoTracking().Select(x => x.Adapt<QuestionResponse>()).SingleOrDefaultAsync(cancellationToken);
            if(Question is null)
            {
                return Result.Failure<QuestionResponse>(QuestionsErrors.QuestionNoFound);
            }
            return Result.Success<QuestionResponse>(Question);
        }



        public async Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId , CancellationToken cancellationToken = default)
        {
            //checking if the user already voted in this poll
            var isUserAlreadyVotedToPoll = await _context.Votes.AnyAsync
                (x=>x.PollId == pollId && x.UserId == userId , cancellationToken);
            if(isUserAlreadyVotedToPoll)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(VoteErrors.DuplicatedVote);
            }
            //check if the poll itself exist or not
            var isPollExist = await _context.polls.AnyAsync(x => x.Id == pollId && x.IsPublished
                && x.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                && x.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow)
                , cancellationToken);
            if(!isPollExist)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.PollNotFound);
            }
            //filter available questions(is active && in this poll) and filter withing the answers that is (is acitve = true)
            var AvailableQuestion = await _context.Questions
                .Where(x=> x.PollId == pollId && x.isActive)
                .Include(x=>x.Answers)
                .Select(x=> new QuestionResponse(
                
                     x.Id,
                     x.Content,
                     x.Answers.Where(x=>x.isActive).Select(x => new Contracts.Answers.AnswerResponse(x.Id , x.Content))
                ))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success<IEnumerable<QuestionResponse>>(AvailableQuestion);
        }



        public async Task<Result> ToggleActiveStatus(int pollId, int questionId, CancellationToken cancellationToken = default)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(x=>x.PollId == pollId && x.Id == questionId);
            if (question is null) return Result.Failure(QuestionsErrors.QuestionNoFound);
            question.isActive = !question.isActive;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();


        }

        public async Task<Result> UpdateAsync(int pollId, int questionId, QuestionRequest questionRequest, CancellationToken cancellationToken = default)
        {
            //Validating if its same poll same content its cannot be same questionId
            var IsQuestionAlreadyExist = await _context.Questions.
                AnyAsync(x => x.PollId == pollId 
                      && x.Id != questionId
                      && x.Content == questionRequest.Content, cancellationToken);

            if (IsQuestionAlreadyExist)
            {
                return Result.Failure(QuestionsErrors.DuplicatedQuestion);
            }
            //At this point iam sure that no duplicated question can be happen accidentally
            //so i will check now if there is a qustion has the criteria i am looking for
            var question =await  _context.Questions.Include(x => x.Answers)
                .SingleOrDefaultAsync(x=>x.PollId ==pollId && x.Id == questionId , cancellationToken);

            if (question is null)
            {
                return Result.Failure(QuestionsErrors.QuestionNoFound);
            }
            //at this level now i can apply the updates 

            question.Content = questionRequest.Content;

            //now we need to update answers
            //we have 3 cases
            //case 1 : answers in request but not in db (need to add the new answers to Db)
            //case 2 : answers in db but not request(Delete in Db ("Soft Delete => DeActivation"))
            //case 3 : answer that in both(Stay same)

            //current answers(Exists already in Db)
            var AnswersInDataBase = question.Answers.Select(x=>x.Content).ToList();

            //extract the answers that is new and not in Db
            var NewAnswers = questionRequest.Answers.Except(AnswersInDataBase).ToList();

            //Add new answers
            NewAnswers.ForEach(answer =>
            {
                question.Answers.Add(new Answer { Content = answer });
            });


            question.Answers.ToList().ForEach(answer =>
            {
                //if answer is already in Db and it comes again with the new request
                //we will make it active if it is existing in db but it didnt 
                //come with the new request in this case we will let it be false
                //as Contains return boolean values
                answer.isActive = questionRequest.Answers.Contains(answer.Content);
            });
            await _context.SaveChangesAsync();
            return Result.Success(cancellationToken);
        }
    }
}
