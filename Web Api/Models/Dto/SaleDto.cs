using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dto
{
    public class SaleDto
    {
        public int Quantity { get; set; }
        public string Product { get; set; }
        public string Customer { get; set; }
        public DateTime Date { get; set; }
    }

}
