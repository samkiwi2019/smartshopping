using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smartshopping.Models
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string Prefix { get; set; }
        public string Category { get; set; }
        public bool Latest { get; set; }
    }
}