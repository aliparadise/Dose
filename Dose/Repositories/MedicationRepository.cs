using Dose.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace Dose.Repositories
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly IConfiguration _config;

        public MedicationRepository(IConfiguration config)
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

        public List<Medication> GetAllMedications()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, MedicationName, Interaction, Instruction
                    FROM Medication
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Medication> medications = new List<Medication>();
                        while (reader.Read())
                        {
                            Medication medication = new Medication
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                MedicationName = reader.GetString(reader.GetOrdinal("MedicationName")),
                                Interaction = reader.GetString(reader.GetOrdinal("Interaction")),
                                Instruction = reader.GetString(reader.GetOrdinal("Instruction")),
                            };

                            medications.Add(medication);
                        }

                        return medications;
                    }
                }
            }
        }

    }
}
