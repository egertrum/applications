using System.Collections.Generic;

namespace WebApp.ViewModels.Poll
{
    public class PollDetailsViewModel
    {
        public Domain.App.Poll Poll { get; set; } = default!;
        
        public IList<Domain.App.PollQuestion> PollQuestions { get; set; } = default!;
    }
}