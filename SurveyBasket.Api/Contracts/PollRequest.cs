﻿namespace SurveyBasket.Api.Contracts
{
    public record PollRequest(
         string Title ,
         string Summary,
         bool IsPublished,
         DateOnly StartsAt,
         DateOnly EndsAt
        );
    
}
