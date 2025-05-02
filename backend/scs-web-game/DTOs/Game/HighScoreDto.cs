namespace scs_web_game.DTOs.Game
{
    public class HighScoreDto
    {
        public Guid GameId { get; set; }
        public required string PlayerName { get; set; }
        public int Score { get; set; }
    }
}
