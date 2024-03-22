using System.ComponentModel.DataAnnotations;

namespace SC_WEBAPISOCKET.Models
{
    public class USER
    {
 
        public Guid? Id { get; set; }
        public string? User { get; set; }
        public string? Password { get; set; }
        public string? PasswordSalt { get; set; }
        public string? Token { get; set; }
    }
}
