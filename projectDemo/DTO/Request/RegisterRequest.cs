using System.ComponentModel.DataAnnotations;

namespace projectDemo.DTO.Request
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string password { get; set; }

    }
}
