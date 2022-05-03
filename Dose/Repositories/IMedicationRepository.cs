using Dose.Models;
using System.Collections.Generic;

namespace Dose.Repositories
{
    public interface IMedicationRepository
    {
        List<Medication> GetAllMedications();
    }
}
