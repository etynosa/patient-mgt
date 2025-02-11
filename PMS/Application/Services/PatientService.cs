using AutoMapper;
using PMS.Application.Repositories;
using PMS.Domain.Dtos;
using PMS.Domain.Entities;
using PMS.Infrastructure.Middlewares;

namespace PMS.Application.Services
{
    public interface IPatientService
    {
        Task<Patient> GetPatientByIdAsync(int id);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
        Task<Patient> CreatePatientAsync(PatientCreateDto patientCreateDto);
        Task UpdatePatientAsync(int id, PatientUpdateDto patientUpdateDto);
        Task DeletePatientAsync(int id);
    }

    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IPatientRepository patientRepository, IMapper mapper)
        {
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            return patient;
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _patientRepository.GetAllAsync();
        }

        public async Task<Patient> CreatePatientAsync(PatientCreateDto patientCreateDto)
        {
            var patient = _mapper.Map<Patient>(patientCreateDto);
            await _patientRepository.AddAsync(patient);
            return patient;
        }

        public async Task UpdatePatientAsync(int id, PatientUpdateDto patientUpdateDto)
        {
            var patient = await GetPatientByIdAsync(id);
            _mapper.Map(patientUpdateDto, patient);
            await _patientRepository.UpdateAsync(patient);
        }

        public async Task DeletePatientAsync(int id)
        {
            var patient = await GetPatientByIdAsync(id);
            await _patientRepository.DeleteAsync(patient);
        }

        public async Task<IEnumerable<PatientRecord>> GetPatientRecordsByPatientIdAsync(int patientId)
        {
            return await _patientRepository.GetPatientRecordsByPatientIdAsync(patientId);
        }

       
    }
}
