using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Contract.CustomExceptions;
using SmartEnergy.Contract.CustomExceptions.Multimedia;
using SmartEnergy.Contract.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SmartEnergyDocuments.Controllers
{
    [Route("api/multimedia")]
    [ApiController]
    public class MultimediaController : ControllerBase
    {
        private readonly IMultimediaService _multimediaService;

        public MultimediaController(IMultimediaService multimediaService)
        {
            _multimediaService = multimediaService;
        }

        
        [HttpPost("users/{id}/avatar")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> AttachAvatarAsync(int id, IFormFile file)
        {
            try
            {
                await _multimediaService.AttachUserAvatar(file, id);
                return Ok();
            }
            catch (UserNotFoundException wnf)
            {
                return NotFound(wnf.Message);
            }
            catch (MultimediaInfectedException mie)
            {
                return BadRequest(mie.Message);
            }
            catch (MultimediaNotImageException mni)
            {
                return BadRequest(mni.Message);
            }
        }


        [HttpGet("users/{id}/avatar/{filename}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserAvatarAsync(int id, string filename)
        {

            try
            {
                FileStream stream = await _multimediaService.GetUserAvatarStreamAsync(id, filename);
                return File(stream, "application/octet-stream", filename);

            }
            catch (UserNotFoundException wnf)
            {
                return NotFound(wnf.Message);
            }
            catch (MultimediaNotFoundException mne)
            {
                return NotFound(mne.Message);
            }
        }

    }
}
