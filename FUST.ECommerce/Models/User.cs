using System.Reflection.Metadata.Ecma335;

namespace FUST.ECommerce.Models
{
    public class User
    {
        // id, nome, cognome, email, password, data
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
        public string EMail { get; set; } = default!;
        public string Password { get; set; } = default!;
        public DateTime RegistrationDate { get; set; }
    }
}
