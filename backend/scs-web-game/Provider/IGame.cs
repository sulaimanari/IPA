using scs_web_game.DTOs.Game;

namespace scs_web_game.Provider
{
    public interface IGame
    {
        public Task<GameDto> CreateGame(Guid playerId);
        public Task<List<GameDto>> GetAllGame();
        public Task<AddScoreDto> AddScore(Guid gameId, Guid employeeId, string employeeFirstName, string username);
        public Task<List<HighScoreDto>> Highscore();
        public Task<GameDto> ScoreOfPlayer(Guid gameId);
    }
}
