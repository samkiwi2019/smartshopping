using System;
using System.ComponentModel.DataAnnotations;

namespace Smartshopping.Dtos
{
    public class ProductCreateDto
    {
        [Required] public string ProductId { get; set; }

        [Required] public string Supplier { get; set; }
        
        [Required] public string Category { get; set; }
        
        [Required] public string Name { get; set; }
        
        [Required] public string Img { get; set; }
        
        public string Prefix { get; set; }
        
        [Required] public string Price { get; set; }
        
        [Required] public string Unit { get; set; }
        
        [Required] public double Compare { get; set; }
        
        [Required] public bool Latest { get; set; }
        
        [Required] public DateTime Date { get; set; }
    }
}