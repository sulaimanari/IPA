using scs_web_game.DTOs.Player;

namespace scs_web_game.Provider
{
    public interface IPlayer
    {
        public Task<PlayerDto> CreatePlayer(string email);
        public Task<PlayerDto> GetPlayerIdByEmail(string email);
        public Task<List<PlayerDto>> GetPlayers();
    }
}
