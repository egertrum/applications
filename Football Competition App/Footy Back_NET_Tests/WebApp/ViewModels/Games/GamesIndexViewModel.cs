using System.Collections.Generic;

namespace WebApp.ViewModels.Games
{
    public class GamesIndexViewModel
    {
        public IEnumerable<PublicApi.DTO.v1.Game> Games { get; set; } = default!;

        public bool? OrganiserGames { get; set; }
        
        public bool? TeamManagerGames { get; set; }

        public string Title { get; set; } = default!;
    }
}