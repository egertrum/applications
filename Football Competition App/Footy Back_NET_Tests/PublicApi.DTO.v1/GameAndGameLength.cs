namespace PublicApi.DTO.v1
{
    public class GameAndGameLength
    {
        public Game Game { get; set; } = default!;
        
        public GameLength GameLength { get; set; } = default!;
    }
}