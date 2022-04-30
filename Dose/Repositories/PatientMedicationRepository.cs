using Dose.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Dose.Repositories
{
    public class PatientMedicationRepository : IPatientMedicationRepository
    {
        private readonly IConfiguration _config;

        public PatientRepository(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<PatientMedication> GetAllPatientMedicationByPatientId(int patientId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT pm.Id, pm.PatientId, pm.MedicationId, pm.Dosage, pm.Frequency, pm.Duration, pm.Notes, 
                            m.MedicationName,
                            p.FirstName
                            
                    FROM PatientMedication pm 
                            LEFT JOIN Medication m ON pm.MedicationId = m.Id
                            LEFT JOIN Patient p ON pm.PatientId = p.Id
                            
                    WHERE pm.PatientId = @patientId 
                    ";

                    cmd.Parameters.AddWithValue("@patientid", patientId);
                    

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Patient> patients = new List<Patient>();
                        while (reader.Read())
                        {
                            Patient patient = new Patient
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Age = reader.GetInt32(reader.GetOrdinal("Age")),
                                Weight = reader.GetDecimal(reader.GetOrdinal("Weight")),
                                Notes = DbUtils.GetNullableString(reader, "Notes"),
                            };

                            patients.Add(patient);

                        }

                        return patients;

                    }
                }
            }
        }
    }
}
