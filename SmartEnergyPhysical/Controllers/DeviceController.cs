using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartEnergy.Contract.CustomExceptions.Device;
using SmartEnergy.Contract.CustomExceptions.DeviceUsage;
using SmartEnergy.Contract.CustomExceptions.Incident;
using SmartEnergy.Contract.CustomExceptions.Location;
using SmartEnergy.Contract.CustomExceptions.SafetyDocument;
using SmartEnergy.Contract.CustomExceptions.WorkRequest;
using SmartEnergy.Contract.DTO;
using SmartEnergy.Contract.Enums;
using SmartEnergy.Contract.Interfaces;

namespace SmartEnergyAPI.Controllers
{
    [Route("api/devices")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;
        private readonly IDeviceUsageService _deviceUsageService;

        public DeviceController(IDeviceService deviceService, IDeviceUsageService deviceUsageService)
        {
            _deviceService = deviceService;
            _deviceUsageService = deviceUsageService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER, ADMIN", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DeviceDto>))]
        public IActionResult GetAllDevices()
        {
            return Ok(_deviceService.GetAllAsync());

        }


        [HttpGet]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER, ADMIN", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DeviceDto>))]
        public IActionResult GetDevicesPaged([FromQuery] DeviceField sortBy, [FromQuery] SortingDirection direction,
                                  [FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage)
        {
            return Ok(_deviceService.GetDevicesPaged(sortBy, direction, page, perPage));
        }

        [HttpGet("search")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER, ADMIN", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DeviceDto>))]
        public IActionResult GetSearchDevicesPaged([FromQuery] DeviceField sortBy, [FromQuery] SortingDirection direction,
                                 [FromQuery][BindRequired] int page, [FromQuery][BindRequired] int perPage, [FromQuery] DeviceFilter type,
                                 [FromQuery] DeviceField field, [FromQuery] string searchParam)
        {
            return Ok(_deviceService.GetSearchDevicesPaged(sortBy, direction, page, perPage, type, field, searchParam));
        }
       



        [HttpGet("{id}")]
        [Authorize(Roles = "CREW_MEMBER, DISPATCHER, WORKER, ADMIN", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeviceDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDeviceByIdAsync(int id)
        {
            DeviceDto device = await _deviceService.GetAsync(id);
            if (device == null)
                return NotFound();

            return Ok(device);
        }


        [HttpPost]
        [Authorize(Roles = "ADMIN", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDeviceAsync([FromBody] DeviceDto newDevice)
        {
            try
            {
                DeviceDto device = await _deviceService.InsertAsync(newDevice);
                return CreatedAtAction(nameof(GetDeviceByIdAsync), new { id = device.ID }, device);
            }
            catch (InvalidDeviceException invalidDevice)
            {
                return BadRequest(invalidDevice.Message);
            }
            catch (LocationNotFoundException locationNotFound)
            {
                return NotFound(locationNotFound.Message);
            }
          
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeviceDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDeviceAsync(int id, [FromBody] DeviceDto modifiedDevice)
        {
            try
            {
                DeviceDto device = await _deviceService.UpdateAsync(modifiedDevice);
                return Ok(device);
            }
            catch (InvalidDeviceException invalidDevice)
            {
                return BadRequest(invalidDevice.Message);
            }
            catch (DeviceNotFoundException deviceNotFound)
            {
                return NotFound(deviceNotFound.Message);
            }
            catch (LocationNotFoundException locationNotFound)
            {
                return NotFound(locationNotFound.Message);
            }
           
           

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN", Policy = "ApprovedOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveDevice(int id)
        {
            try
            {
                _deviceService.Delete(id);
                return NoContent();
            }
            catch (DeviceNotFoundException deviceNotFound)
            {
                return NotFound(deviceNotFound.Message);
            }
        }


        [HttpPatch("from-work-plan/{workPlanID}/to-safety-document/{safetyDocumentID}")]
        [AllowAnonymous]//Don't know who can do this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CopyIncidentDevicesToSafetyDocument(int workPlanID, int safetyDocumentID)
        {
            try
            {
                _deviceUsageService.CopyIncidentDevicesToSafetyDocumentAsync(workPlanID, safetyDocumentID);
                return NoContent();
            }
            catch (SafetyDocumentNotFoundException deviceNotFound)
            {
                return NotFound(deviceNotFound.Message);
            }
        }


        [HttpPatch("from-incident/{incidentID}/to-work-request/{workRequestID}")]
        [AllowAnonymous]//Don't know who can do this
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CopyIncidentDevicesToWorkRequest(int incidentID, int workRequestID)
        {
            try
            {
                _deviceUsageService.CopyIncidentDevicesToWorkRequest(incidentID, workRequestID);
                return NoContent();
            }
            catch (WorkRequestNotFound deviceNotFound)
            {
                return NotFound(deviceNotFound.Message);
            }
        }


        [HttpGet("safety-document/{safetyDocumentID}")]
        [AllowAnonymous]//Don't know who can do this
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DeviceDto>))]
        public IActionResult GetSafetyDocumentDevices(int safetyDocumentID)
        { 
             return Ok(_deviceUsageService.GetSafetyDocumentDevices(safetyDocumentID));
        }



        [HttpGet("work-request/{workRequestID}")]
        [AllowAnonymous]//Don't know who can do this
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DeviceDto>))]
        public IActionResult GetWorkRequestDevices(int workRequestID)
        {

             return Ok(_deviceUsageService.GetWorkRequestDevices(workRequestID));

        }


        [HttpGet("incident/{incidentID}")]
        [AllowAnonymous]//Don't know who can do this
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<DeviceDto>))]
        public IActionResult GetIncidentDevices(int incidentID)
        {

            return Ok(_deviceUsageService.GetIncidentDevices(incidentID));

        }


        [HttpPost("/device-usage")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddDeviceUsageAsync([FromBody] DeviceUsageDto newDevice)
        {
            try
            {
                DeviceUsageDto device = await _deviceUsageService.InsertAsync(newDevice);
                return Ok(device);
            }
            catch (IncidentNotFoundException inc)
            {
                return NotFound(inc.Message);
            }

        }

        [HttpDelete("{deviceId}/incident/{incidentId}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RemoveDeviceFromIncident(int deviceId, int incidentId)
        {
            try
            {
                _deviceUsageService.RemoveDeviceFromIncident(incidentId, deviceId);
                return NoContent();
            }
            catch (DeviceUsageNotFoundException inc)
            {
                return NotFound(inc.Message);
            }

        }



    }
}
