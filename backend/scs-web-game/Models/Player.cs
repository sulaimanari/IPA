namespace scs_web_game.Models
{
    public class Player
    {
        public Guid PlayerId { get; set; }
        public string PlayerEmail { get; set; } = null!;
        public string PlayerName { get; set; } = null!;
        public ICollection<Game> Game { get; set; } = new List<Game>();
    }
}
