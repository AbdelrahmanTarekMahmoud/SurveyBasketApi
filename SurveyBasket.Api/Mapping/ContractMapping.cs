////Manuel Mapping  
//namespace SurveyBasket.Api.Mapping
//{
//    public static class ContractMapping
//    {
//        public static PollResponse MapToPollResponse(this Poll poll)
//        {
//            return new()
//            {
//                Id = poll.Id,
//                Description = poll.Description,
//                Title = poll.Title
//            };
//        }
//        public static IEnumerable<PollResponse> MapToPollResponse(this  IEnumerable<Poll> poll)
//        {
//            return poll.Select(MapToPollResponse);
//        }
//        public static Poll MapToPoll(this PollRequest request)
//        {
//            return new()
//            {
//                Description = request.Description,
//                Title = request.Title
//            };
//        }
//    }
//}
