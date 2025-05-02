using Microsoft.EntityFrameworkCore;
using scs_web_game.DTOs.Game;
using scs_web_game.Models;
using ILogger = Serilog.ILogger;

namespace scs_web_game.DataAccessLayer
{
    public class GameDal(WebGameContext context, ILogger logger)
    {
        public async Task<GameDto> CreateGame(Guid playerId)
        {
            logger.Information("CreateGame method started.");
            var newGame = new Game
            {
                PlayerId = playerId,
                Score = 0,
            };

            context.Game.Add(newGame);
            await context.SaveChangesAsync();
            logger.Information("Game created successfully with ID: {GameId}", newGame.GameId);

            var gameDao = new GameDto
            {
                GameId = newGame.GameId,
                Score = newGame.Score,
                PlayerId = newGame.PlayerId
            };

            return gameDao;
        }

        public async Task<List<GameDto>> GetAllGame()
        {
            logger.Information("GetAllGame method started.");
            var games = await context.Game.ToListAsync();
            var gameDao = games.Select(game => new GameDto
            {
                GameId = game.GameId,
                Score = game.Score,
                PlayerId = game.PlayerId
            }).ToList();

            logger.Information("Successfully retrieved {Count} games.", gameDao.Count);
            return gameDao;
        }

        public async Task<List<HighScoreDto>> Highscore()
        {
            var highScores = await context.Game
                .OrderByDescending(game => game.Score)
                .Take(5)
                .Select(game => new HighScoreDto
                {
                    GameId = game.GameId,
                    PlayerName = game.Player.PlayerName,
                    Score = game.Score
                })
                .ToListAsync();

            return highScores;
        }
        public async Task<GameDto> ScoreOfPlayer(Guid gameId)
        {
            var game = await context.Game
                .Where(g => g.GameId == gameId)
                .FirstOrDefaultAsync();

            if (game == null) throw new ArgumentException("No game found for the specified game ID.");

            return new GameDto
            {
                GameId = game.GameId,
                Score = game.Score,
                PlayerId = game.PlayerId,
            };
        }

        public async Task<AddScoreDto> AddScore(Guid gameId, Guid employeeId, string username)
        {
            var employee = await context.Employee
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null) throw new ArgumentException("Employee not found.");
            
            if (!employee.UserName.Equals(username, StringComparison.OrdinalIgnoreCase))
                return new AddScoreDto
                {
                    Success = false,
                    CorrectUserName = employee.UserName,
                    CorrectFirstName = employee.FirstName,
                    CorrectLastName = employee.LastName
                };
            var game = await context.Game.FirstOrDefaultAsync(g => g.GameId == gameId);

            if (game == null) throw new ArgumentException("Game not found.");

            game.Score += 1;
            await context.SaveChangesAsync();

            return new AddScoreDto
            {
                Success = true,
                Game = new GameDto
                {
                    GameId = game.GameId,
                    PlayerId = game.PlayerId,
                    Score = game.Score
                }
            };
        }
    }
}
