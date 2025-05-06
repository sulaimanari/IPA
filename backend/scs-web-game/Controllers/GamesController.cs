using Microsoft.AspNetCore.Mvc;
using scs_web_game.DTOs.Game;
using scs_web_game.Models;
using scs_web_game.Provider;
using ILogger = Serilog.ILogger;

namespace scs_web_game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController(WebGameContext context, ILogger logger, IGame game) : ControllerBase
    {
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            var deleteGame = await context.Game.FindAsync(id);
            if (deleteGame == null)
            {
                return NotFound();
            }

            context.Game.Remove(deleteGame);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("GetAllGames")]
        public async Task<ActionResult<List<GameDto>>> GetAllGames()
        {
            try
            {
                var getGame = await game.GetAllGame();
                return Ok(getGame);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while retrieving all games.");
                return BadRequest("Internal server error:" + ex.Message);
            }
        }

        [HttpPost("CreateGame")]
        public async Task<ActionResult<GameDto>> CreateGame([FromBody] Guid playerId)
        {
            try
            {
                var createdGame = await game.CreateGame(playerId);
                return Ok(createdGame);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while creating the game.");
                return BadRequest("Invalid playerId provided. " + ex.Message);
            }
        }

        [HttpPost("addScore")]
        public async Task<ActionResult<AddScoreDto>> AddScore([FromQuery] Guid gameId, [FromQuery] Guid employeeId, 
            [FromQuery] string employeeFirstName, [FromQuery] string employeeLastName)
        {
            try
            {
                var result = await game.AddScore(gameId, employeeId, employeeFirstName, employeeLastName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while adding the score."); 
                return BadRequest("Invalid input provided. " + ex.Message);
            }
        }

        [HttpGet("highScores")]
        public async Task<ActionResult<List<HighScoreDto>>> GetHighscores()
        {
            try
            {
                var getHighScore = await game.Highscore();
                return Ok(getHighScore);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while retrieving high scores.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("GetHighestScoreOfPlayer/{playerId:guid}")]
        public async Task<ActionResult<GameDto>> GetHighestScoreOfPlayer(Guid playerId)
        {
            try
            {
                var highestScore = await game.ScoreOfPlayer(playerId);
                return Ok(highestScore);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while retrieving the highest score of the player.");
                return BadRequest("Invalid playerId provided." + ex.Message);
            }
        }
    }
}
