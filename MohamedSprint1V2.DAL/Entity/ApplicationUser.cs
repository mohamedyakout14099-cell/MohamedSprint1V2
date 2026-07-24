using System.ComponentModel.DataAnnotations;namespace MohamedSprint1V2.DAL.Entity
{
    public class ApplicationUser:IdentityUser
    {
        public ApplicationUser()
        {
            
        }
        public ApplicationUser(string name , string address, string city, string UsarName)
        {
            Name = name;
            Address = address;
            City = city;
            UserName = name; // تعيين Name إلى UserName
            Email = $"{name}@example.com"; // تعيين email افتراضي
        }

        [Required]
        public string Name { get;private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
    }
}
