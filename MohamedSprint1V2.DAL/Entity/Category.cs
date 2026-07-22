namespace MohamedSprint1V2.DAL.EntityClasses
{
    public class Category
    {
        public Category() { }
        public Category(string name, string description)// for add
        {
            Name = name;
            Description = description;
        }

        public Category(int id ,string name, string description)// for update
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public int Id { get; private set; }

        [Required]
        public string Name { get; private set; }

        public string Description { get; private set; }
        public DateTime CreatedTime { get; private set; } = DateTime.Now;
        public bool update (string name, string description)
        {
            if(!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(description))
            {
                Name = name;
                Description = description;
                return true;
            }
            return false;
        }
    }
}
