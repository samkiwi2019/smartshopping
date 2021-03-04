using System.ComponentModel.DataAnnotations.Schema;

namespace Smartshopping.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public bool Latest { get; set; }
    }
}