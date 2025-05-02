namespace scs_web_game.DTOs.Game
{
    public class GameDto
    {
        public Guid GameId { get; set; }
        public int Score { get; set; }
        public Guid PlayerId { get; set; }
    }
}
