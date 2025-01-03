﻿namespace SurveyBasket.Api.Errors
{
    public class PollErrors
    {
        public static readonly Error PollNotFound =
         new("Poll.PollNotFound", "There is no existing Poll with this Id", StatusCodes.Status404NotFound);
        public static readonly Error DuplicatedTitle =
         new("Poll.DuplicatedTitle", "There is existing Poll with this Title already", StatusCodes.Status409Conflict);
    }
}

