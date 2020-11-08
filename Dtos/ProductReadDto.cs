using System;
using System.Collections.Generic;

namespace Smartshopping.Dtos
{
    public class ProductReadDto
    {
        public int Id { get; set; }

        public string ProductId { get; set; }

        public string Supplier { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public string Img { get; set; }

        public string Prefix { get; set; }

        public string Price { get; set; }

        public string Unit { get; set; }

        public double Compare { get; set; }

        public bool Latest { get; set; }

        public DateTime Date { get; set; }
    }
}