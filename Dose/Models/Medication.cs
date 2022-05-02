using System.ComponentModel;

namespace Dose.Models
{
    public class Medication
    {
        public int Id { get; set; }
        
        [DisplayName("Medication Name")]
        public string MedicationName { get; set; }  
        public string Interaction { get; set; } 
        public string Instruction { get; set; }

    }
}
