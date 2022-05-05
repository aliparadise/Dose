using System.Collections.Generic;

namespace Dose.Models
{
    public class PatientMedicationFormViewModel
    {
        public PatientMedication PatientMedication { get; set; }
        public List<Medication> Medications { get; set; }
    }
}
