
namespace SurveyBasket.Api.Services
{
    public class PollService : IPollService
    {
        private static readonly List<Poll> _polls = [
           new Poll{Id = 1 , Description = "HelloDescp" , Title = "HelloTitle"}
           ];
        public IEnumerable<Poll> GetAll()
        {
            return _polls;
        }

        public Poll? Get(int id)
        {
            return _polls.SingleOrDefault(p => p.Id == id);
        }

        public Poll Add(Poll poll)
        {
            poll.Id = _polls.Count + 1;
           _polls.Add(poll);
            return poll;
        }

        public bool Update(int id , Poll poll)
        {
            var UpdatedPoll = Get(id);
            if (UpdatedPoll is null) return false;
            UpdatedPoll.Description = poll.Description;
            UpdatedPoll.Title = poll.Title;
            return true;
        }

        public bool Delete(int id)
        {
            var DeletedPoll = Get(id);
            if (DeletedPoll is null) return false;
            _polls.Remove(DeletedPoll);
            return true;
        }
    }
}
