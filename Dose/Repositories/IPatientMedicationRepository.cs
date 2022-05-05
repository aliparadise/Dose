using Dose.Models;
using System.Collections.Generic;

namespace Dose.Repositories
{
    public interface IPatientMedicationRepository
    {
        PatientMedication GetPatientMedicationsById(int id);
        List<PatientMedication> GetAllPatientMedicationsByPatientId(int patientId);
        void AddPatientMedication(PatientMedication patientMedication);
        void UpdatePatientMedication(PatientMedication patientMedication);
    }
}
