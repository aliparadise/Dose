//using Dose.Models;
//using Dose.Utils;
//using Microsoft.Data.SqlClient;
using Dose.Models;
using Dose.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Dose.Repositories
{
    public class PatientMedicationRepository : IPatientMedicationRepository
    {
        private readonly IConfiguration _config;

        public PatientMedicationRepository(IConfiguration config)
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

        public PatientMedication GetPatientMedicationsById(int id)
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
                            
                    WHERE pm.Id = @id 
                    ";

                    cmd.Parameters.AddWithValue("@id", id);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        if (reader.Read())
                        {
                            PatientMedication patientMedication = new PatientMedication()
                            {
                                Id = id,
                                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                                MedicationId = reader.GetInt32(reader.GetOrdinal("MedicationId")),
                                Dosage = reader.GetString(reader.GetOrdinal("Dosage")),
                                Frequency = reader.GetString(reader.GetOrdinal("Frequency")),
                                Duration = reader.GetString(reader.GetOrdinal("Duration")),
                                Notes = DbUtils.GetNullableString(reader, "Notes"),
                                Medication = new Medication
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    MedicationName = reader.GetString(reader.GetOrdinal("MedicationName"))
                                },
                                Patient = new Patient
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                }

                            };
                            return patientMedication;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public List<PatientMedication> GetAllPatientMedicationsByPatientId(int patientId)
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
                        List<PatientMedication> patientMedications = new List<PatientMedication>();
                        while (reader.Read())
                        {
                            PatientMedication patientMedication = new PatientMedication
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                                MedicationId = reader.GetInt32(reader.GetOrdinal("MedicationId")),
                                Dosage = reader.GetString(reader.GetOrdinal("Dosage")),
                                Frequency = reader.GetString(reader.GetOrdinal("Frequency")),
                                Duration = reader.GetString(reader.GetOrdinal("Duration")),
                                Notes = DbUtils.GetNullableString(reader, "Notes"),
                                Medication = new Medication
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    MedicationName = reader.GetString(reader.GetOrdinal("MedicationName"))
                                },
                                Patient = new Patient
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                }

                            };

                            patientMedications.Add(patientMedication);

                        }

                        return patientMedications;

                    }
                }
            }
        }
        public void AddPatientMedication(PatientMedication patientMedication)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO PatientMedication (PatientId, MedicationId, Dosage, Frequency, Duration, Notes)
                    OUTPUT INSERTED.ID
                    VALUES (@patientId, @medicationId, @dosage, @frequency, @duration, @notes);
                ";

                    cmd.Parameters.AddWithValue("@patientId", patientMedication.PatientId);
                    cmd.Parameters.AddWithValue("@medicationId", patientMedication.MedicationId);
                    cmd.Parameters.AddWithValue("@dosage", patientMedication.Dosage);
                    cmd.Parameters.AddWithValue("@frequency", patientMedication.Frequency);
                    cmd.Parameters.AddWithValue("@duration", patientMedication.Duration);
                    cmd.Parameters.AddWithValue("@notes", DbUtils.ValueOrDBNull(patientMedication.Notes));

                    int id = (int)cmd.ExecuteScalar();

                    patientMedication.Id = id;
                    
                }
            }
        }

        public void UpdatePatientMedication(PatientMedication patientMedication)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE PatientMedication
                            SET
                                PatientId = @patientId,
                                MedicationId = @medicationId,
                                Dosage = @dosage,
                                Frequency = @frequency,
                                Duration = @duration,
                                Notes = @notes
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@patientId", patientMedication.PatientId);
                    cmd.Parameters.AddWithValue("@medicationId", patientMedication.MedicationId);
                    cmd.Parameters.AddWithValue("@dosage", patientMedication.Dosage);
                    cmd.Parameters.AddWithValue("@frequency", patientMedication.Frequency);
                    cmd.Parameters.AddWithValue("@duration", patientMedication.Duration);
                    cmd.Parameters.AddWithValue("@notes", DbUtils.ValueOrDBNull(patientMedication.Notes));
                    cmd.Parameters.AddWithValue("@id", patientMedication.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeletePatientMedication(PatientMedication patientMedication)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM PatientMedication
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", patientMedication.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

