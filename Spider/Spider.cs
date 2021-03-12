using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;  
using Quartz.Impl;
using Smartshopping.Data;
using Smartshopping.Data.IRepos;

namespace Smartshopping.Spider
{
    public class SpiderMaker
    {
        public static bool HasJob;
        public static void GetAJob()
        {
            if (HasJob) return;
            HasJob = true;
            
            ISchedulerFactory schedf = new StdSchedulerFactory();
            var sched = schedf.GetScheduler().Result;
            sched.Start();
            var job = JobBuilder.Create<MessageJob>().Build();   
            var trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithCronSchedule("0 35 15 * * ? *") // 每天的0：00运行  0 1 6 * * ? *   每天6点过1分
                .Build();
            sched.ScheduleJob(job, trigger).Wait();
        }

        public static async void Crawl()
        {
            var urlManager = new UrlManager();
            var htmlParser = new HtmlParser();
            
            try
            {
                var services = Program.CreateHostBuilder(new string[] { })
                    .Build().Services
                    .CreateScope().ServiceProvider;
                
                var outputer = services.GetRequiredService<IOutputer>();
                var repo = services.GetRequiredService<IProductRepo>();
                
                await repo.MarkProductsToHistory();
                
                while (urlManager.HasNewUrl())
                {
                    var aNewUrl = urlManager.GetNewUrl();
                    var (products, urls) = await htmlParser.Parse(aNewUrl);
                    await outputer.CollectData(products);
                    urlManager.AddNewUrls(urls);
                    Console.WriteLine("Remaining:" + urlManager.NewUrls.Count);
                }
                Console.WriteLine("Goodbye for today:" + DateTime.Now);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private class MessageJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                return Task.Run(() =>
                {
                    Console.WriteLine("go go go...");
                    Crawl();
                });
            }
        }
    }
}