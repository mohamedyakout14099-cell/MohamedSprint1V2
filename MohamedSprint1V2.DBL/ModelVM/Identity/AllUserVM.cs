using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DLL.ModelVM.Identity
{
    public class AllUserVM
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public List<byte> Img { get; set; } = new List<byte>();
        public bool IsDeleted { get; set; } = false;
        public string Role { get; set; } = "User";
    }
}
