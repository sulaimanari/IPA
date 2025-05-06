using Microsoft.AspNetCore.Mvc;
using scs_web_game.DTOs.Player;
using scs_web_game.Models;
using scs_web_game.Provider;
using ILogger = Serilog.ILogger;

namespace scs_web_game.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController(WebGameContext context, IPlayer player, ILogger logger) : ControllerBase
    {
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(Guid id)
        {
            var deletePlayer = await context.Player.FindAsync(id);
            if (deletePlayer == null)
            {
                return NotFound();
            }

            context.Player.Remove(deletePlayer);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("CreatePlayer")]
        public async Task<ActionResult<PlayerDto>> CreatePlayer([FromBody] string email)
        {
            try
            {
                var createdPlayer = await player.CreatePlayer(email);
                var response = new PlayerDto
                {
                    PlayerId = createdPlayer.PlayerId,
                    PlayerEmail = createdPlayer.PlayerEmail,
                    PlayerName = createdPlayer.PlayerName
                };
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                logger.Error(ex, "An error occurred while creating the player.");
                return BadRequest("Invalid email provided." + ex.Message);
            }
        }
        [HttpGet("getIdByEmail")]
        public async Task<ActionResult<PlayerDto>> GetPlayerIdByEmail([FromQuery] string email)
        {
            try
            {
                var getPlayer = await player.GetPlayerIdByEmail(email);
                var response = new PlayerDto
                {
                    PlayerId = getPlayer.PlayerId,
                    PlayerEmail = getPlayer.PlayerEmail,
                    PlayerName = getPlayer.PlayerName
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while retrieving all players.");
                return BadRequest("Internal server error."+ ex.Message);
            }
        }

        [HttpGet("GetAllPlayers")]
        public async Task<ActionResult<List<PlayerDto>>> GetAllPlayers()
        {
            try
            {
                var players = await player.GetPlayers();
                return Ok(players);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred while retrieving all players.");
                return BadRequest("Internal server error." + ex.Message);
            }
        }

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
