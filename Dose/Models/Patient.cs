using System.ComponentModel;

namespace Dose.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public int UserProfileId { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public int Age { get; set; }    
        public decimal Weight { get; set; }
        public string Notes { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}

