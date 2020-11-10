using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Common;
using AutoMapper;
using FluentScheduler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Smartshopping.Data;

namespace Smartshopping.Spider
{
    public class SpiderMaker
    {
        public static bool HasJob;

        public static void GetAJob()
        {
            if (HasJob) return;
            
            Console.WriteLine("set job");
            JobManager.Initialize(new Registry());
            JobManager.AddJob(
                Crawl,
                s => s.WithName("Every Minute")
                    .ToRunEvery(1)
                    .Days()
                    .At(6,1)
            );
            
            HasJob = true;
        }

        public static async void Crawl()
        {
            UrlManager.AddNewUrls(HtmlParser.Urls);

            // Create a DI container
            // var serviceCollection = new ServiceCollection();
            // serviceCollection.AddDbContext<MyContext>(opt => opt.UseMySql("server=45.77.50.164;user=root;password=66778899;database=DbSmartShopping"));
            // serviceCollection.AddScoped<IProductRepo, SqlProductRepo>();
            // serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            // var serviceProvider = serviceCollection.BuildServiceProvider();
            // Get a class with DI
            try
            {
                var host = Program.CreateHostBuilder(new string[] { }).Build();
                var serviceScope = host.Services.CreateScope();
                var serviceProvider = serviceScope.ServiceProvider;
                var outputer = serviceProvider.GetRequiredService<IOutputer>();
                while (UrlManager.HasNewUrl())
                {
                    var aNewUrl = UrlManager.GetNewUrl();
                    var (products, urls) = await HtmlParser.Parse(aNewUrl);
                    await outputer.CollectData(products);
                    UrlManager.AddNewUrls(urls);
                    Console.WriteLine("Remaining: {0}", UrlManager.NewUrls.Count);
                }
                Console.WriteLine("Game over!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}