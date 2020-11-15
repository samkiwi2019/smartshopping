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
            var urlManager = new UrlManager();
            var htmlParser = new HtmlParser();
            
            try
            {
                var outputer = Program.CreateHostBuilder(new string[] { })
                    .Build().Services
                    .CreateScope().ServiceProvider
                    .GetRequiredService<IOutputer>();
                
                while (urlManager.HasNewUrl())
                {
                    var aNewUrl = urlManager.GetNewUrl();
                    var (products, urls) = await htmlParser.Parse(aNewUrl);
                    await outputer.CollectData(products);
                    urlManager.AddNewUrls(urls);
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