using scs_web_game.DataAccessLayer;
using scs_web_game.DTOs.Player;
using scs_web_game.Models;
using scs_web_game.Provider;
using ILogger = Serilog.ILogger;
namespace scs_web_game.BusinessLogic
{
    public class PlayerBusinessLogic(ILogger logger, PlayerDal playerDal) : IPlayer
    {
        public async Task<PlayerDto> CreatePlayer(string email)
        {
            ValidateEmail(email);
            try
            {
                var playerDto = await playerDal.CreatePlayer(email);
                logger.Information("New player created with email: {Email}", email);
                return new PlayerDto
                {
                    PlayerId = playerDto.PlayerId,
                    PlayerEmail = playerDto.PlayerEmail,
                    PlayerName = playerDto.PlayerName
                };
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error when trying to create player with email: {Email}", email);
                throw;
            }
        }
        
        public async Task<PlayerDto> GetPlayerIdByEmail(string email)
        {
            
            ValidateEmail(email);
            try
            {
                var playerDto = await playerDal.GetPlayerIdByEmail(email);
                if (playerDto == null)
                {
                    logger.Warning("No player found with email: {Email}", email);
                    throw new KeyNotFoundException($"No player found with email: {email}");
                }
                logger.Information("Player found with ID: {PlayerId} for email: {Email}", playerDto.PlayerId, email);
                return new PlayerDto
                {
                    PlayerId = playerDto.PlayerId,
                    PlayerEmail = playerDto.PlayerEmail,
                    PlayerName = playerDto.PlayerName
                };
            }
            catch (Exception ex)  
            {
                logger.Error(ex, "Error when trying to get player by email: {Email}", email);
                throw;
            }
        }

        public async Task<List<PlayerDto>> GetPlayers()
        {
            try
            {
                var players = await playerDal.GetALlPlayers();
                if (players.Count == 0)
                {
                    logger.Warning("No players found in the database.");
                    return [];
                }

                logger.Information("Retrieved {Count} players from the database.", players.Count);
                var playerList = players.Select(player => new PlayerDto
                {
                    PlayerId = player.PlayerId,
                    PlayerEmail = player.PlayerEmail,
                    PlayerName = player.PlayerName
                }).ToList();
                return playerList;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while retrieving players from the database.");
                return []; 
            }
        }

        private static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or whitespace.");
            }

            if (email.Length > 100)
            {
                throw new ArgumentException("Email cannot be longer than 100 characters.");
            }

            if (!email.EndsWith("@scs.ch"))
            {
                throw new ArgumentException("Email must end with '@scs.ch'.");
            }

            var parts = email.Split('@');
            if (parts.Length != 2 || parts[0].Length == 0 || parts[1] != "scs.ch")
            {
                throw new ArgumentException("Email format is incorrect.");
            }

            var localPart = parts[0];
            if (localPart.Any(c => !char.IsLetterOrDigit(c) && c != '.' && c != '@' && c != '_'))
            {
                throw new ArgumentException("Email contains invalid characters.");
            }
        }
    }
}
