using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Services;
using PMS.Domain.Dtos;
using PMS.Domain.Entities;

namespace PMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        public PatientsController(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatient(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            return Ok(_mapper.Map<PatientResponseDto>(patient));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(_mapper.Map<IEnumerable<PatientResponseDto>>(patients));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient(PatientCreateDto patientCreateDto)
        {
            var patient = await _patientService.CreatePatientAsync(patientCreateDto);
            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, _mapper.Map<PatientResponseDto>(patient));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, PatientUpdateDto patientUpdateDto)
        {
            await _patientService.UpdatePatientAsync(id, patientUpdateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            await _patientService.DeletePatientAsync(id);
            return NoContent();
        }
    }
}
