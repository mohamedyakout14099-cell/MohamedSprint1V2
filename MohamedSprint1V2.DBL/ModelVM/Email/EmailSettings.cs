namespace MohamedSprint1V2.DLL.ModelVM.Email
{
    public class EmailSettings
    {
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = "Mohamed Store";
        public string Password { get; set; } = string.Empty; // Gmail App Password (16 chars)
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
    }
}
