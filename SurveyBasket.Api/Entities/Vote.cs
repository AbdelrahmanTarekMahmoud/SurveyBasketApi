﻿namespace SurveyBasket.Api.Entities
{
    public sealed class Vote
    {
        public int Id { get; set; }
        public int PollId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime SumbittedOn { get; set; } = DateTime.UtcNow;

        //Navigation Property
        public Poll Poll { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public ICollection<VoteAnswer> VoteAnswers { get; set; } = [];
    }
}
