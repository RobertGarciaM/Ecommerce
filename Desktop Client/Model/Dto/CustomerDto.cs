using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto
{
    public class CustomerDto
    {
        public Guid IdCustomer { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }

        public string City
        {
            get; set;
        }
    }
}
