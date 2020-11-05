using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using Smartshopping.Dtos;

namespace Smartshopping.Spider
{
    public static class HtmlParser
    {
        public static List<string> Urls = new List<string>
        {
            "https://www.paknsaveonline.co.nz/category/fresh-foods-and-bakery?pg=1",
            "https://www.paknsaveonline.co.nz/category/pantry?pg=1",
            "https://www.paknsaveonline.co.nz/category/drinks?pg=1",
            "https://www.paknsaveonline.co.nz/category/beer-cider-and-wine?pg=1",
            "https://www.paknsaveonline.co.nz/category/personal-care?pg=1",
            "https://www.paknsaveonline.co.nz/category/baby-toddler-and-kids?pg=1",
            "https://www.paknsaveonline.co.nz/category/pets?pg=1",
            "https://www.paknsaveonline.co.nz/category/kitchen-dining-and-household?pg=1",
        };

        private static async Task<List<ProductCreateDto>> GetNewData(string url, IDocument document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var items = document.QuerySelectorAll("div.fs-product-card").Select(m => m.OuterHtml);
            var products = new List<ProductCreateDto>();
            var category = (url.Split("/").Last()).Split('?').First();
            foreach (var item in items)
            {
                var itemHtml = await context.OpenAsync(req => req.Content(item));
                var product = itemHtml.QuerySelector("div.js-product-card-footer").GetAttribute("data-options");

                using var doc = JsonDocument.Parse(product);
                var root = doc.RootElement;
                var details = root.GetProperty("ProductDetails");

                var productItem = new ProductCreateDto
                {
                    ProductId = root.GetProperty("productId").ToString(),
                    Supplier = "ParknSave",
                    Category = category,
                    Name = root.GetProperty("productName").ToString(),
                    Img = itemHtml.QuerySelector("div.fs-product-card__product-image").GetAttribute("data-src-s"),
                    Prefix = details.GetProperty("MultiBuyDeal").ToString(),
                    Price = details.GetProperty("PricePerItem").ToString(),
                    Unit = details.GetProperty("PriceMode").ToString(),
                    Compare = 1,
                    Latest = true,
                    Date = DateTime.Now
                };
                products.Add(productItem);
            }

            return products;
        }

        private static List<string> GetNewUrls(IDocument document)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));
            
            var items = document.QuerySelectorAll("ul.fs-pagination__items a");
            var aList = items.Select(item => item.GetAttribute("href")).ToList();
            return aList;
        }


        public static async Task<(List<ProductCreateDto> products, List<string> urls)> Parse(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);
            return (await GetNewData(url, document), GetNewUrls(document));
        }
    }
}