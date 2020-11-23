using System;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Smartshopping.Data;
using Smartshopping.Spider;

namespace Smartshopping.Installers
{
    public class ControllerInstaller: IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddNewtonsoftJson(s =>
            {
                //设置时间格式
                // s.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //忽略循环引用
                s.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //数据格式首字母小写
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                //数据格式按原样输出
                // s.SerializerSettings.ContractResolver = new DefaultContractResolver();
                //忽略空值
                s.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.AddScoped<IProductRepo, SqlProductRepo>();
            
            services.AddScoped<IOutputer, Outputer>();
        }
    }
}