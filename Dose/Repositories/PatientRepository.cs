using Dose.Models;
using Dose.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;


namespace Dose.Repositories
{
    public class PatientRepository : IPatientRepository
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

        public Patient GetPatientById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, UserProfileId, FirstName, LastName, Age, Weight, Notes
                        FROM Patient
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
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

                            return patient;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }


        public List<Patient> GetAllPatientsByUserId(int userProfileId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, UserProfileId, FirstName, LastName, Age, Weight, Notes
                    FROM Patient 
                    WHERE UserProfileId = @userProfileId
                    ";

                    cmd.Parameters.AddWithValue("@userProfileId", userProfileId);

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
        public void AddPatient(Patient patient)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Patient (UserProfileId, FirstName, LastName, Age, Weight, Notes)
                    OUTPUT INSERTED.ID
                    VALUES (@userProfileId, @firstName, @lastName, @age, @weight, @notes);
                ";

                    cmd.Parameters.AddWithValue("@userProfileId", patient.UserProfileId);
                    cmd.Parameters.AddWithValue("@firstName", patient.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", patient.LastName);
                    cmd.Parameters.AddWithValue("@age", patient.Age);
                    cmd.Parameters.AddWithValue("@weight", patient.Weight);
                    cmd.Parameters.AddWithValue("@notes", DbUtils.ValueOrDBNull(patient.Notes));

                    int id = (int)cmd.ExecuteScalar();

                    patient.Id = id;
                }
            }
        }
        public void UpdatePatient(Patient patient)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Patient
                            SET
                                UserProfileId = @userProfileId,
                                FirstName = @firstName,
                                LastName = @lastName,
                                Age = @age,
                                Weight = @weight,
                                Notes = @notes
                            WHERE Id= @id";

                    cmd.Parameters.AddWithValue("@userProfileId", patient.UserProfileId);
                    cmd.Parameters.AddWithValue("@firstName", patient.FirstName);
                    cmd.Parameters.AddWithValue("@lastName", patient.LastName);
                    cmd.Parameters.AddWithValue("@age", patient.Age);
                    cmd.Parameters.AddWithValue("@weight", patient.Weight);
                    cmd.Parameters.AddWithValue("@notes", DbUtils.ValueOrDBNull(patient.Notes));
                    cmd.Parameters.AddWithValue("@id", patient.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeletePatient(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Patient
                            WHERE Id = @id
                            ";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}





