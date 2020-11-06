using System;
using System.Linq;
using AngleSharp;
using AngleSharp.Common;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Smartshopping.Data;

namespace Smartshopping.Spider
{
    public class SpiderMaker
    {
        public static void Crawl()
        {
            UrlManager.AddNewUrls(HtmlParser.Urls);

            // Create a DI container
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<MyContext>(opt => opt.UseMySql("server=localhost;user=root;password=66778899;database=DbSmartShopping"));
            serviceCollection.AddTransient<IProductRepo, SqlProductRepo>();
            serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var serviceProvider = serviceCollection.BuildServiceProvider();
            // Get a class with DI
            var outputer = ActivatorUtilities.CreateInstance<Outputer>(serviceProvider);
            
            while (UrlManager.HasNewUrl())
            {
                var aNewUrl = UrlManager.GetNewUrl();
                var result = HtmlParser.Parse(aNewUrl);
                outputer.CollectData(result.Result.products);
                UrlManager.AddNewUrls(result.Result.urls.ToList());
                Console.WriteLine("Remaining: {0}", UrlManager.NewUrls.Count);
            }

            Console.WriteLine("Game over!");
        }
    }
}