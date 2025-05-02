namespace scs_web_game.DTOs.Player
{
    public class PlayerDto
    {
        public Guid PlayerId { get; set; }
        public string PlayerEmail { get; set; } = null!;
        public string PlayerName { get; set; } = null!;

    }
}
