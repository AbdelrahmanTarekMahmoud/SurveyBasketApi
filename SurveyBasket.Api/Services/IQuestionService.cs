using SurveyBasket.Api.Contracts.Polls;
using SurveyBasket.Api.Contracts.Questions;

namespace SurveyBasket.Api.Services
{
    public interface IQuestionService
    {
        /*
         * When we add new question we need to consider :
         * 1-if there is a existing poll with the "pollId"
         * 2-if there is a existing question in this poll with same content
         */
        Task<Result<QuestionResponse>> AddAsync(QuestionRequest questionRequest, int pollId, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<QuestionResponse>>> GetAllAsync(int pollId , CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<QuestionResponse>>> GetAvailableAsync(int pollId, string userId , CancellationToken cancellationToken = default);
        Task<Result<QuestionResponse>> GetAsync(int pollId , int questionId , CancellationToken cancellationToken = default);
        Task<Result> ToggleActiveStatus(int pollId, int questionId, CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int pollId, int questionId,QuestionRequest questionRequest , CancellationToken cancellationToken = default);


    }
}
