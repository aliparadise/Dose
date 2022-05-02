namespace Dose.Models
{
    public class PatientMedication
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int MedicationId { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string Notes { get; set; }
        public Medication Medication { get; set; }
        public Patient Patient { get; set; }
        public UserProfile UserProfile { get; set; }

    }
}
