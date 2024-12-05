using SurveyBasket.Api.Contracts.Polls;
using SurveyBasket.Api.Contracts.Questions;

namespace SurveyBasket.Api.Services
{
    public interface IQuestionService
    {
        Task<Result<QuestionResponse>> AddAsync(QuestionRequest questionRequest ,int pollId , CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId , CancellationToken cancellationToken = default);
        Task<Result<QuestionResponse>> GetAsync(int pollId , int questionId , CancellationToken cancellationToken = default);
        Task<Result> ToggleActiveStatus(int pollId, int questionId, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int pollId, int questionId,QuestionRequest questionRequest , CancellationToken cancellationToken = default);


    }
}
