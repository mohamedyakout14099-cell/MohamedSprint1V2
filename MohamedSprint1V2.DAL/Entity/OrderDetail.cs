namespace MohamedSprint1V2.DAL.Entity
{
    public class OrderDetail
    {
        public OrderDetail() { }
        public int Id { get; set; }

        public int OrderHeaderId { get; set; }
        public OrderHeader? OrderHeader { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }
    }
}
