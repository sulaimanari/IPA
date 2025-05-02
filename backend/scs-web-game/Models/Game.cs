namespace scs_web_game.Models
{
    public class Game
    {
        public Guid GameId { get; set; }
        public int Score { get; set; }
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }
    }
}
