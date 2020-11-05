using System;
using System.Linq;
using AngleSharp.Common;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Smartshopping.Data;

namespace Smartshopping.Spider
{
    public class SpiderMaker
    {
        public static void Crawl()
        {
            UrlManager.AddNewUrls(HtmlParser.Urls);

            var outputer = new Outputer();

            while (UrlManager.HasNewUrl())
            {
                var aNewUrl = UrlManager.GetNewUrl();
                var result = HtmlParser.Parse(aNewUrl);
                outputer.CollectData(result.Result.products);
                UrlManager.AddNewUrls(result.Result.urls.ToList());
                Console.WriteLine("Remaining: {0}", UrlManager.NewUrls.Count);
            }

            Console.WriteLine("end game");
        }
    }
}