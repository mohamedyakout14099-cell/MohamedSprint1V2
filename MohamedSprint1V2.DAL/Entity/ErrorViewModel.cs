namespace MohamedSprint1V2.DAL.Entity
{
    public class ErrorViewModel
    {
        protected ErrorViewModel() { }
        public string? RequestId { get; private set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}