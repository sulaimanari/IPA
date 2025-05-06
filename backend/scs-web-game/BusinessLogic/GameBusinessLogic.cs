using Microsoft.IdentityModel.Tokens;
using scs_web_game.DataAccessLayer;
using scs_web_game.DTOs.Game;
using scs_web_game.Provider;
using ILogger= Serilog.ILogger;
namespace scs_web_game.BusinessLogic
{
    public class GameBusinessLogic(ILogger logger, GameDal gameDal) :IGame
    {
        public async Task<GameDto> CreateGame(Guid playerId)
        {
            if (playerId == Guid.Empty)
            {
                logger.Warning("Invalid playerId: Guid.Empty encountered in CreateGame.");
                throw new ArgumentException("playerId must not be empty.", nameof(playerId));
            }

            logger.Information($"Calling CreateGame for playerId: {playerId}.");

            try
            {
                var gameDto = await gameDal.CreateGame(playerId);

                return gameDto;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error occurred while creating game for playerId: {playerId}.");
                throw;
            }
        }

        public async Task<List<GameDto>> GetAllGame()
        {
            logger.Information("Calling GetAllGame in GameServices.");

            try
            {
                var games = await gameDal.GetAllGame();
                return games;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred while retrieving all games.");
                throw;
            }
        }

        public async Task<AddScoreDto> AddScore(Guid gameId, Guid employeeId, string employeeFirstName, string employeeLastName)
        {
            logger.Information($"Calling AddScore for gameId: {gameId}, employeeId: {employeeId}.");

            try
            {
                var addScoreDto = await gameDal.AddScore(gameId, employeeId, employeeFirstName, employeeLastName);
                return addScoreDto;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error occurred while adding score for gameId: {gameId}, employeeId: {employeeId}.");
                throw;
            }
        }

        public async Task<List<HighScoreDto>> Highscore()
        {
            logger.Information("Calling Highscore in GameServices.");

            try
            {
                var highScores = await gameDal.Highscore();

                if (!highScores.IsNullOrEmpty()) return highScores;
                logger.Warning("No high scores found.");
                return [];

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error occurred while retrieving high scores.");
                throw;
            }
        }

        public async Task<GameDto> ScoreOfPlayer(Guid playerId)
        {
            logger.Information($"Calling HighestScoreOfPlayer for playerId: {playerId}.");

            try
            {
                var highestScore = await gameDal.ScoreOfPlayer(playerId);
                return highestScore;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error occurred while retrieving highest score for playerId: {playerId}.");
                throw;
            }
        }
    }
}
