namespace MohamedSprint1V2.DLL.ModelVM.Category
{
    public class AddCategoryVM
    {
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }
    }
}
