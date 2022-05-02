using Dose.Models;
using System.Collections.Generic;

namespace Dose.Repositories
{
    public interface IPatientMedicationRepository
    {
        List<PatientMedication> GetAllPatientMedicationsByPatientId(int patientId);
    }
}
