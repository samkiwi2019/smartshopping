#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Smartshopping.Spider
{
    public class UrlManager
    {
        public IList<string> NewUrls = new List<string>
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
        private IList<string> _oldUrls = new List<string>();

        private void AddNewUrl(string url)
        {
            if (!NewUrls.Contains(url) && !_oldUrls.Contains(url))
                NewUrls = NewUrls.Append(url).ToList();
        }

        public void AddNewUrls(IList<string> urls)
        {
            if (urls.Count == 0)
                return;

            foreach (var t in urls)
                AddNewUrl(t);
        }

        public bool HasNewUrl()
        {
            return NewUrls.Count != 0;
        }

        public string? GetNewUrl()
        {
            if (NewUrls.Count <= 0) return null;
            
            var newUrl = NewUrls.First();
            NewUrls.Remove(newUrl);
            _oldUrls = _oldUrls.Append(newUrl).ToList();
            return newUrl;
        }
    }
}