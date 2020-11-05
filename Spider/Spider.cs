using AutoMapper;
using Smartshopping.Data;

namespace Smartshopping.Spider
{
    public class Spider
    {
        public void Crawl()
        {
            UrlManager.Add_newUrls(HtmlParser.Urls);
            
            // todo: 如何在外部使用 repository; 
            
            // var outputer = new Outputer();
            
            while (UrlManager.HasNewUrl())
            {
                var aNewUrl = UrlManager.GetNewUrl();
                var result = HtmlParser.Parse(aNewUrl);
                
                
            }
        }
    }
}