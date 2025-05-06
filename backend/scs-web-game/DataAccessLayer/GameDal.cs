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
            logger.Information("Fetching high scores.");
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
            logger.Information("Fetched {Count} high scores.", highScores.Count);
            return highScores;
        }

        public async Task<GameDto> ScoreOfPlayer(Guid gameId)
        {
            logger.Information("Fetching score for game ID {GameId}.", gameId);
            var game = await context.Game
                .Where(g => g.GameId == gameId)
                .FirstOrDefaultAsync();

            if (game == null) {
                logger.Warning("No game found for game ID {GameId}.", gameId);
                throw new ArgumentException("No game found for the specified game ID.");
            }
            logger.Information("Found game ID {GameId} with score {Score}.", gameId, game.Score);
            return new GameDto
            {
                GameId = game.GameId,
                Score = game.Score,
                PlayerId = game.PlayerId,
            };
        }

        public async Task<AddScoreDto> AddScore(Guid gameId, Guid employeeId, string employeeFirstName, string employeeLastName)
        {
            logger.Information("Adding score for game ID {GameId} by employee ID {EmployeeId}.", gameId, employeeId);
            var employee = await context.Employee
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                logger.Warning("Employee not found for ID {EmployeeId}.", employeeId);
                throw new ArgumentException("Employee not found.");
            }
            if (!employee.FirstName.Equals(employeeFirstName, StringComparison.OrdinalIgnoreCase) ||
                !employee.LastName.Equals(employeeLastName, StringComparison.OrdinalIgnoreCase))
            {
                logger.Warning("Employee name mismatch: Provided '{ProvidedFirstName} {ProvidedLastName}', expected '{EmployeeFirstName} {EmployeeLastName}'.",
                    employeeFirstName, employeeLastName, employee.FirstName, employee.LastName);
                return new AddScoreDto
                {
                    Success = false,
                    CorrectFirstName = employee.FirstName,
                    CorrectLastName = employee.LastName
                };
            }

            var game = await context.Game.FirstOrDefaultAsync(g => g.GameId == gameId);

            if (game == null)
            {
                logger.Warning("Game not found for ID {GameId}.", gameId);
                throw new ArgumentException("Game not found.");
            }

            game.Score += 1;
            await context.SaveChangesAsync();
            logger.Information("Score updated for game ID {GameId}. New score: {Score}.", gameId, game.Score);
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
