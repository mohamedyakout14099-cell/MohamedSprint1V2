using System;
using System.Collections.Generic;
using System.Text;

namespace MohamedSprint1V2.DLL.ModelVM.Product
{
    public class GetAllProductVM
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string img { get; set; }
        public decimal price { get; set; }
        public int categoryId { get; set; }
    }
}
