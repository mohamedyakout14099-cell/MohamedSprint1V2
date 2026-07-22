namespace MohamedSprint1V2.DAL.Entity
{
    public class OrderDetail
    {
        protected OrderDetail() { }
        public int Id { get; private set; }

        public int OrderHeaderId { get; private set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; private set; }

        public int ProductId { get; private set; }
        [ValidateNever]
        public Product Product { get; private set; }

        public decimal Price { get; private set; }

        public int Count { get; private set; }


    }
}
