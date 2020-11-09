#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Smartshopping.Spider
{
    public static class UrlManager
    {
        public static IList<string> NewUrls = new List<string>();
        private static IList<string> _oldUrls = new List<string>();

        private static void AddNewUrl(string url)
        {
            if (!NewUrls.Contains(url) && !_oldUrls.Contains(url))
                NewUrls = NewUrls.Append(url).ToList();
        }

        public static void AddNewUrls(IList<string> urls)
        {
            if (urls.Count == 0)
                return;

            foreach (var t in urls)
                AddNewUrl(t);
        }

        public static bool HasNewUrl()
        {
            return NewUrls.Count != 0;
        }

        public static string? GetNewUrl()
        {
            if (NewUrls.Count <= 0) return null;
            
            var newUrl = NewUrls.First();
            NewUrls.Remove(newUrl);
            _oldUrls = _oldUrls.Append(newUrl).ToList();
            return newUrl;
        }
    }
}