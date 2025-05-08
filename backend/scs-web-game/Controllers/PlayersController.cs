using Microsoft.AspNetCore.Mvc;
using scs_web_game.DTOs.Player;
using scs_web_game.Provider;
using ILogger = Serilog.ILogger;

namespace scs_web_game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController(IPlayer player, ILogger logger) : ControllerBase
    {
        [HttpPost("GetOrCreatePlayer")]
        public async Task<ActionResult<PlayerDto>> GetOrCreatePlayerByEmail([FromBody] string email)
        {
            try
            {
                var playerResult = await player.GetOrCreatePlayer(email);
                var response = new PlayerDto
                {
                    PlayerId = playerResult.PlayerId,
                    PlayerEmail = playerResult.PlayerEmail,
                    PlayerName = playerResult.PlayerName
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                logger.Error(ex, "An error occurred while processing the email.");
                return BadRequest("Invalid email provided. " + ex.Message);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while retrieving or creating the player.");
                return StatusCode(500, "Internal server error. " + ex.Message);
            }
        }
    }
}
