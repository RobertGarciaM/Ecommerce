using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Products
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_product")]
        public Guid IdProduct { get; set; }

        [Required(ErrorMessage = "Description is mandatory.")]
        [StringLength(100, ErrorMessage = "The description must not exceed 100 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The price is mandatory.")]
        [Range(0.01, 999999.99, ErrorMessage = "The price must be between 0.01 and 999999.99.")]
        public decimal Price { get; set; }
    }
}
