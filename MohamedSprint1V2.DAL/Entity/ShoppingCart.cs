
namespace MohamedSprint1V2.DAL.Entity
{
    public class ShoppingCart
    {
        protected ShoppingCart() { }
        public int Id { get;     private    set; }

        public int ProductId { get; private     set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; private set; }

        public int Count { get; private set; }

        public string ApplicationUserId { get; private set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; private set; }
    }
}
