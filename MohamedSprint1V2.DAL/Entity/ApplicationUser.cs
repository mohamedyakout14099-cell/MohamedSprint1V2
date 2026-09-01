using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MohamedSprint1V2.DAL.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

        public ApplicationUser(string name, string? address, string? city, string userName, string email, List<byte> img)
        {
            Name = name;
            Address = address;
            City = city;
            UserName = userName;
            Email = email;
            Img = img;
        }

        public List<byte> Img { get; set; } 
        [Required]
        public string Name { get; set; } 

        public string? Address { get; set; }

        public string? City { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
