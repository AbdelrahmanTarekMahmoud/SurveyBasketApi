using SurveyBasket.Api.Contracts.Polls;
using System.Threading;

namespace SurveyBasket.Api.Services
{
    public interface IPollService
    {
        Task<IEnumerable<PollResponse>> GetAllAsync(CancellationToken cancellationToken = default);

        //Must be published and between Start date and End date
        Task<IEnumerable<PollResponse>> GetCurrentAsync(CancellationToken cancellationToken = default);
        Task<Result<PollResponse>> GetAsync(int id , CancellationToken cancellationToken = default);
        Task<Result<PollResponse>> AddAsync(PollRequest poll , CancellationToken cancellationToken = default);
        Task<Result> UpdateAsync(int id , PollRequest poll, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id , CancellationToken cancellationToken = default);
        Task<Result> TogglePublishedStateAsync(int id , CancellationToken cancellationToken = default);
    }
}
