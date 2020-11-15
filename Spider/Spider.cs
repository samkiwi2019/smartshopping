using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;  
using Quartz.Impl;

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
                .WithCronSchedule("0 0 6 * * ? *") // 每天的6：00运行
                .Build();
            sched.ScheduleJob(job, trigger).Wait();
        }

        public static async void Crawl()
        {
            UrlManager.AddNewUrls(HtmlParser.Urls);
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
        
        public class MessageJob : IJob
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