﻿namespace SurveyBasket.Api.Services
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default) where T : class;
        Task SetAsync<T>(string cacheKey, T Value, CancellationToken cancellationToken = default) where T : class;

        Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default);
    }
}
