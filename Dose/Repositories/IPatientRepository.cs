using Dose.Models;
using System.Collections.Generic;

namespace Dose.Repositories
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatientsByUserId(int userProfileId);
        Patient GetPatientById(int id);
        void AddPatient(Patient patient);
        void UpdatePatient(Patient patient);
        void DeletePatient(int id);
    }
}
