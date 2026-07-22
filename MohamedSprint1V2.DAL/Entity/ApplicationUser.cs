namespace MohamedSprint1V2.DAL.Entity
{
    public class ApplicationUser
    {
        protected ApplicationUser() { }
        [Required]
        public string Name { get;private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
    }
}
