#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Smartshopping.Spider
{
    public static class UrlManager
    {
        private static List<string> _newUrls = new List<string>();
        private static List<string> _oldUrls = new List<string>();

        private static void AddNewUrl(string url)
        {
            if (!_newUrls.Contains(url) && !_oldUrls.Contains(url))
                _newUrls = _newUrls.Append(url).ToList();
        }

        public static void Add_newUrls(List<string> urls)
        {
            if (urls.Count == 0)
                return;

            foreach (var t in urls)
                AddNewUrl(t);
        }

        public static bool HasNewUrl()
        {
            return _newUrls.Count != 0;
        }

        public static string? GetNewUrl()
        {
            if (_newUrls.Count <= 0) return null;
            
            var newUrl = _newUrls.First();
            _newUrls.Remove(newUrl);
            _oldUrls = _oldUrls.Append(newUrl).ToList();
            return newUrl;
        }
    }
}