

namespace MohamedSprint1V2.DAL.Entity
{
    public class Product
    {
        public Product() { }
        public int Id { get; private set; }

        public Product( string name, string description, string img, decimal Price, int categoryId)
        {
            //Id = id;
            Name = name;
            Description = description;
            Img = img;
            Price = Price;
            CategoryId = categoryId;
        }

        public Product(int id,string name, string description, string img, decimal Price, int categoryId)
        {
            Id = id;
            Name = name;
            Description = description;
            Img = img;
            Price = Price;
            CategoryId = categoryId;
        }
        [Required]
        public string Name { get; private set; }
        public string Description { get; private set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string? Img { get; private    set; }

        [Required]
        public decimal Price { get; private set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; private set; }
        [ValidateNever]
        public Category? Category { get; private set; }
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
