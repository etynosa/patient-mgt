using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Infrastructure;

namespace PMS.Application.Repositories;
public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(int id);
    Task<IEnumerable<Patient>> GetAllAsync();
    Task AddAsync(Patient patient);
    Task UpdateAsync(Patient patient);
    Task DeleteAsync(Patient patient);
    Task<PatientRecord> GetPatientRecordByIdAsync(int id);
    Task AddPatientRecordAsync(PatientRecord record);
    void UpdatePatientRecord(PatientRecord record);
    Task<IEnumerable<PatientRecord>> GetPatientRecordsByPatientIdAsync(int patientId);

}

public class PatientRepository : IPatientRepository
{
    private readonly ApplicationDbContext _context;
    public PatientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.Records)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }

    public async Task<IEnumerable<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .Where(p => !p.IsDeleted)
            .Include(p => p.Records)
            .ToListAsync();
    }

    public async Task AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Patient patient)
    {
        patient.IsDeleted = true;
        await UpdateAsync(patient);
    }

    public async Task<PatientRecord> GetPatientRecordByIdAsync(int id)
    {
        return await _context.PatientRecords.FindAsync(id);
    }

    public async Task AddPatientRecordAsync(PatientRecord record)
    {
        await _context.PatientRecords.AddAsync(record);
        await _context.SaveChangesAsync();
    }

    public void UpdatePatientRecord(PatientRecord record)
    {
        _context.Entry(record).State = EntityState.Modified;
        _context.SaveChanges();
    }
    public async Task<IEnumerable<PatientRecord>> GetPatientRecordsByPatientIdAsync(int patientId)
    {
        return await _context.PatientRecords.Where(r => r.PatientId == patientId).ToListAsync();
    }
}