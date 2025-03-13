using Microsoft.AspNetCore.Identity;

namespace APIRestaurante.Domain.Entities
{
    public class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        public new string? PhoneNumber { get; set; }
    }
}
