using Microsoft.EntityFrameworkCore;
using scs_web_game.DTOs.Player;
using scs_web_game.Models;
using ILogger = Serilog.ILogger;
namespace scs_web_game.DataAccessLayer
{
    public class PlayerDal(WebGameContext context, ILogger logger)
    {
        public async Task<PlayerDto> CreatePlayer(string email)
        {
            var newPlayer = new Player
            {
                PlayerEmail = email
            };
            var playerName = ExtractPlayerNameFromEmail(email);
            context.Player.Add(newPlayer);
            await context.SaveChangesAsync();

            logger.Information("Player created successfully with ID: {PlayerId}", newPlayer.PlayerId);

            return new PlayerDto
            {
                PlayerId = newPlayer.PlayerId,
                PlayerEmail = newPlayer.PlayerEmail,
                PlayerName = playerName
            };
        }
        private string ExtractPlayerNameFromEmail(string email)
        {
            var atIndex = email.IndexOf("@scs.ch", StringComparison.OrdinalIgnoreCase);
            return email[..atIndex];
            logger.Information("Extracted player name: {PlayerName}", email[..atIndex]);
        }

        public async Task<PlayerDto> GetPlayerIdByEmail(string email)
        {
            var player = await context.Player
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PlayerEmail == email);

            logger.Information("Player found with ID: {PlayerId}", player.PlayerId);

            return new PlayerDto
            {
                PlayerId = player.PlayerId,
                PlayerEmail = player.PlayerEmail,
                PlayerName = player.PlayerName
            };
        }

        public async Task<List<PlayerDto>> GetALlPlayers()
        {
            var players = await context.Player
                .AsNoTracking()
                .ToListAsync();

            logger.Information("Retrieved {Count} players from the database.", players.Count);

            return players.Select(player => new PlayerDto
            {
                PlayerId = player.PlayerId,
                PlayerEmail = player.PlayerEmail,
                PlayerName = player.PlayerName
            }).ToList();
        }
    }
}
