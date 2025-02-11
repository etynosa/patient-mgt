using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Repositories;
using PMS.Domain.Entities;

namespace PMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientRecordsController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;

        public PatientRecordsController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientRecord>> GetPatientRecord(int id)
        {
            var record = await _patientRepository.GetPatientRecordByIdAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return record;
        }

        [HttpPost]
        public async Task<ActionResult<PatientRecord>> CreatePatientRecord(PatientRecord record)
        {
            await _patientRepository.AddPatientRecordAsync(record);
            return CreatedAtAction(nameof(GetPatientRecord), new { id = record.Id }, record);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePatientRecord(int id, PatientRecord record)
        {
            if (id != record.Id)
            {
                return BadRequest();
            }

            _patientRepository.UpdatePatientRecord(record);
            return NoContent();
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<PatientRecord>>> GetPatientRecordsByPatientId(int patientId)
        {
            var records = await _patientRepository.GetPatientRecordsByPatientIdAsync(patientId);
            return Ok(records);
        }
    }
}
