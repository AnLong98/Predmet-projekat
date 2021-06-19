using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartEnergy.Contract.CustomExceptions;
using SmartEnergy.Contract.CustomExceptions.Multimedia;
using SmartEnergy.Contract.CustomExceptions.SafetyDocument;
using SmartEnergy.Contract.CustomExceptions.WorkPlan;
using SmartEnergy.Contract.CustomExceptions.WorkRequest;
using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Enums;
using SmartEnergy.Contract.Interfaces;

namespace SmartEnergy.Documents.Controllers
{
    [Route("api/safety-documents")]
    [ApiController]
    public class SafetyDocumentController : ControllerBase
    {

        private readonly ISafetyDocumentService _safetyDocumentService;
        private readonly IStateChangeService _stateChangeService;
        private readonly IMultimediaService _multimediaService;

        public SafetyDocumentController(ISafetyDocumentService safetyDocumentService, IStateChangeService stateChangeService, IMultimediaService multimediaService)
        {
            _safetyDocumentService = safetyDocumentService;
            _stateChangeService = stateChangeService;
            _multimediaService = multimediaService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SafetyDocumentDto>))]
        public async  Task<IActionResult> GetAllSafetyDocuments()
        {
            var ret = await _safetyDocumentService.GetAllAsync();
            return Ok(ret);

        }

        [HttpGet]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SafetyDocumentDto>))]
        public async Task<IActionResult> GetAllMineSafetyDocuments([FromQuery] OwnerFilter owner)
        {
            var ret = await _safetyDocumentService.GetAllMineSafetyDocumentsAsync(owner, User);
            return Ok(ret);

        }


        [HttpGet("{id}/devices")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DeviceDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSafetyDocumentDevicesAsync(int id)
        {
            try
            {
                var ret = await _safetyDocumentService.GetSafetyDocumentDevicesAsync(id);
                return Ok(ret);

            }
            catch (SafetyDocumentNotFoundException sfnf)
            {
                return NotFound(sfnf.Message);
            }



        }



        [HttpGet("{id}")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SafetyDocumentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSafetyDocumentById(int id)
        {
            try
            {
                SafetyDocumentDto sd = await _safetyDocumentService.GetAsync(id);

                return Ok(sd);

            }
            catch(SafetyDocumentNotFoundException sfnf)
            {
                return NotFound(sfnf.Message);
            }
           

            
        }


        [HttpGet("{id}/crew")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SafetyDocumentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCrewForSafetyDocumentAsync(int id)
        {
            try
            {
                CrewDto crew = await _safetyDocumentService.GetCrewForSafetyDocumentAsync(id);

                return Ok(crew);

            }
            catch (SafetyDocumentNotFoundException sfnf)
            {
                return NotFound(sfnf.Message);
            }



        }


        

        [HttpPost]
        [Authorize(Roles = "DISPATCHER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddSafetyDocumentAsync([FromBody] SafetyDocumentDto newSafetyDocument)
        {
            try
            {
                SafetyDocumentDto sf = await _safetyDocumentService.InsertAsync(newSafetyDocument);

                return CreatedAtAction(nameof(GetSafetyDocumentById), new { id = sf.ID }, sf);
            }
            catch (WorkPlanNotFoundException wpnf)
            {
                return NotFound(wpnf.Message);
            }
            catch (UserNotFoundException usernf)
            {
                return NotFound(usernf.Message);
            }
            catch (InvalidSafetyDocumentException invalid)
            {
                return NotFound(invalid.Message);
            }

        }


        [HttpPut("{id}")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SafetyDocumentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSafetyDocumentAsync(int id, [FromBody] SafetyDocumentDto modifiedSafetyDocument)
        {
            try
            {
                SafetyDocumentDto sf = await _safetyDocumentService.UpdateAsync(modifiedSafetyDocument);

                return Ok(sf);
            }
            catch (WorkPlanNotFoundException wpnf)
            {
                return NotFound(wpnf.Message);
            }
            catch (UserNotFoundException usernf)
            {
                return NotFound(usernf.Message);
            }
            catch (InvalidSafetyDocumentException invalid)
            {
                return BadRequest(invalid.Message);
            }
            catch(SafetyDocumentNotFoundException sfnf)
            {
                return NotFound(sfnf.Message);
            }
            catch(SafetyDocumentInvalidStateException sfinv)
            {
                return BadRequest(sfinv.Message);
            }
            


        }



        [HttpPut("{id}/checklist")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChecklistDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateSafetyDocumentChecklist([FromBody] ChecklistDto checklist)
        {
            try
            {
                ChecklistDto updated = _safetyDocumentService.UpdateSafetyDocumentChecklist(checklist);

                return Ok(updated);
            }
            catch (SafetyDocumentNotFoundException sfnf)
            {
                return NotFound(sfnf.Message);
            }
           



        }

        
        


        [HttpGet("{id}/state-changes")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<StateChangeHistoryDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSafetyDocumentStateChanges(int id)
        {
            try
            {
                var ret = await _stateChangeService.GetSafetyDocumentStateHistoryAsync(id);
                return Ok(ret);
            }
            catch (SafetyDocumentNotFoundException wnf)
            {
                return NotFound(wnf.Message);
            }catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SafetyDocumentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ApproveSafetyDocument(int id)
        {
            try
            {
                SafetyDocumentDto sd = _stateChangeService.ApproveSafetyDocument(id, User);
                return Ok(sd);
            }
            catch (SafetyDocumentNotFoundException wnf)
            {
                return NotFound(wnf.Message);
            }
            catch (SafetyDocumentInvalidStateException wnf)
            {
                return BadRequest(wnf.Message);
            }
        }

        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SafetyDocumentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CancelSafetyDocument(int id)
        {
            try
            {
                SafetyDocumentDto sd = _stateChangeService.CancelSafetyDocument(id, User);
                return Ok(sd);
            }
            catch (SafetyDocumentNotFoundException wnf)
            {
                return NotFound(wnf.Message);
            }
            catch (SafetyDocumentInvalidStateException wnf)
            {
                return BadRequest(wnf.Message);
            }
        }


        [HttpPut("{id}/deny")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SafetyDocumentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DenySafetyDocument(int id)
        {
            try
            {
                SafetyDocumentDto sd = _stateChangeService.DenySafetyDocument(id, User);
                return Ok(sd);
            }
            catch (SafetyDocumentNotFoundException wnf)
            {
                return NotFound(wnf.Message);
            }
            catch (SafetyDocumentInvalidStateException wnf)
            {
                return BadRequest(wnf.Message);
            }
        }

        #region Multimedia
        [HttpPost("{id}/attachments")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> AttachFileAsync(int id, IFormFile file)
        {
            try
            {
                await _multimediaService.AttachFileToSafetyDocumentAsync(file, id);
                return Ok();
            }
            catch (SafetyDocumentNotFoundException wnf)
            {
                return NotFound(wnf.Message);
            }
            catch (MultimediaAlreadyExists mae)
            {
                return BadRequest(mae.Message);
            }
            catch (MultimediaInfectedException mie)
            {
                return BadRequest(mie.Message);
            }
            catch (SafetyDocumentInvalidStateException mnf)
            {
                return BadRequest(mnf.Message);
            }
        }

        [HttpGet("{id}/attachments/{filename}")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileStreamResult))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetFile(int id, string filename)
        {
            try
            {
                return File(_multimediaService.GetSafetyDocumentAttachmentStream(id, filename), "application/octet-stream", filename);
            }
            catch (SafetyDocumentNotFoundException wnf)
            {
                return NotFound(wnf.Message);
            }
            catch (MultimediaNotFoundException mne)
            {
                return NotFound(mne.Message);
            }
        }


        [HttpGet("{id}/attachments")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MultimediaAttachmentDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSafetyDocumentAttachments(int id)
        {
            try
            {
                return Ok(_multimediaService.GetSafetyDocumentAttachments(id));
            }
            catch (SafetyDocumentNotFoundException wnf)
            {
                return NotFound(wnf.Message);
            }
        }


        [HttpDelete("{id}/attachments/{filename}")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteAttachment(int id, string filename)
        {
            try
            {
                _multimediaService.DeleteSafetyDocumentAttachment(id, filename);
                return Ok();
            }
            catch (SafetyDocumentNotFoundException wnf)
            {
                return NotFound(wnf.Message);
            }
            catch (MultimediaNotFoundException mnf)
            {
                return NotFound(mnf.Message);
            }
            catch (SafetyDocumentInvalidStateException mnf)
            {
                return BadRequest(mnf.Message);
            }

        }
        #endregion


        //[HttpDelete("{id}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult RemoveSafetyDocument(int id)
        //{
        //    try
        //    {
        //        _safetyDocumentService.Delete(id);
        //        return NoContent();
        //    }
        //    catch (SafetyDocumentNotFoundException sfnf)
        //    {
        //        return NotFound(sfnf.Message);
        //    }
        //}


    }
}
