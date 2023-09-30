using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Sales
{
    public class Sale
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_sale")]
        public Guid IdSale { get; set; }


        [Required(ErrorMessage = "The client is obligatory.")]
        [Column("id_customer")]
        public Guid IdCustomer { get; set; }

        [Required(ErrorMessage = "The product is mandatory.")]
        [Column("id_product")]
        public Guid IdProduct { get; set; }

        [Required(ErrorMessage = "The amount is mandatory.")]
        [Range(1, int.MaxValue, ErrorMessage = "The amount must be greater than or equal to 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "The date is mandatory.")]
        public DateTime Date { get; set; }
    }
}
