using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MohamedSprint1V2.DAL.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string name, string? address, string? city, string userName, string email)
        {
            Name = name;
            Address = address;
            City = city;
            UserName = userName;
            Email = email;
        }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? City { get; set; }
    }
}
