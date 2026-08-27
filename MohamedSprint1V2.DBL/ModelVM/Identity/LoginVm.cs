using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DLL.ModelVM.Identity
{
    public class LoginVm
    {
        [Required(ErrorMessage = "الإيميل مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة الإيميل غلط")]
        public string Email { get; set; }

        [Required(ErrorMessage = "الباسورد مطلوب")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
