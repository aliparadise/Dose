using Dose.Models;
using System.Collections.Generic;

namespace Dose.Repositories
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        void AddPatient(Patient patient);
    }
}
