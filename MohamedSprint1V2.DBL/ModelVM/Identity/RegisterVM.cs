using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DLL.ModelVM.Identity
{
    public class RegisterVM
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
