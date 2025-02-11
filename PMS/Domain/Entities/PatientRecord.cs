namespace PMS.Domain.Entities
{
    public class PatientRecord
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public DateTime RecordDate { get; set; }

        public string Diagnosis { get; set; }

        public string Treatment { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Patient Patient { get; set; }

    }
}
