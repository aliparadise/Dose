using Dose.Models;
using System.Collections.Generic;

namespace Dose.Repositories
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatientsByUserId(int userProfileId);
        void AddPatient(Patient patient);
    }
}
