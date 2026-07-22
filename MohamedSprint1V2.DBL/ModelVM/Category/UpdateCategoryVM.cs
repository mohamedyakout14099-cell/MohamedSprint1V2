using System.ComponentModel.DataAnnotations;

public class UpdaeteCategoryVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category Name is required")]
    [StringLength(100)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(500)]
    public string Description { get; set; }
}