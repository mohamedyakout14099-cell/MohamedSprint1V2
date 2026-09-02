namespace MohamedSprint1V2.DLL.ModelVM.Product
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public string? Img { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive number")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }




    }
}
