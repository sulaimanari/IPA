namespace scs_web_game.DTOs.Game
{
    public class AddScoreDto
    {
        public bool Success { get; set; }
        public string? CorrectUserName { get; set; }
        public string? CorrectFirstName { get; set; }
        public string? CorrectLastName { get; set; }
        public GameDto? Game { get; set; }
    }
}
