using System.Collections.Generic;

namespace Dose.Models
{
    public class CreatePatientMedicationFormViewModel
    {
        public PatientMedication PatientMedication { get; set; }
        public List<Medication> Medications { get; set; }
    }
}
