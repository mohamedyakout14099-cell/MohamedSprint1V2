

namespace MohamedSprint1V2.DAL.Entity
{
    public class Product
    {
        public Product() { }
        public int Id { get; private set; }

        public Product( string name, string description, string img, decimal price, int categoryId)
        {
            //Id = id;
            Name = name;
            Description = description;
            Img = img;
            Price = price;
            CategoryId = categoryId;
        }

        public Product(int id,string name, string description, string img, decimal price, int categoryId)
        {
            Id = id;
            Name = name;
            Description = description;
            Img = img;
            Price = price;
            CategoryId = categoryId;
        }

        public string Name { get; private set; }
        public string Description { get; private set; }

        public string? Img { get; private set; }

        public decimal Price { get; private set; }

        public int CategoryId { get; private set; }
        public Category? Category { get; private set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public bool IsDeleted { get; set; } = false;
        public bool update(string name, string description, string img, decimal price, int categoryId)
        {
            Name = name;
            Description = description;
            Img = img;
            Price = price;
            CategoryId = categoryId;
            return true;
        }
    }
}
