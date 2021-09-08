using System.Collections.Generic;

namespace WebApp.ViewModels.Poll
{
    public class PollIndexViewModel
    {
        public IList<Domain.App.Poll> Polls { get; set; } = default!;

        public string? Error { get; set; }
        
        public string? Done { get; set; }
    }
}