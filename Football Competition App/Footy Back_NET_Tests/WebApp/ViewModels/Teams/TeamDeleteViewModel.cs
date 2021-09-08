namespace WebApp.ViewModels.Teams
{
    public class TeamDeleteViewModel
    {
        public PublicApi.DTO.v1.Team Team { get; set; } = default!;

        public string? Error { get; set; }
    }
}