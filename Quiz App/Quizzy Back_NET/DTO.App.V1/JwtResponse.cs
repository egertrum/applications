namespace DTO.App.V1
{
    public class JwtResponse
    {
        public string Token { get; set; } = default!;
        public string Firstname { get; set; } = default!;
        public string Lastname { get; set; } = default!;

        public string Role { get; set; } = default!;
    }
}