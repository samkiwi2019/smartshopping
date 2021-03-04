using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smartshopping.Models
{
    public class Product: BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new int Id { get; set; }

        [Required] public string ProductId { get; set; }

        [Required] public string Supplier { get; set; }
        
        [Required] public new string Category { get; set; }
        
        [Required] public new string Name { get; set; }
        
        [Required] public string Img { get; set; }
        
        public string Prefix { get; set; }
        
        [Required] public string Price { get; set; }
        
        [Required] public string Unit { get; set; }
        
        [Required] public double Compare { get; set; }
        
        [Required] public new bool Latest { get; set; }
        
        [Required] public DateTime Date { get; set; }
    }
}