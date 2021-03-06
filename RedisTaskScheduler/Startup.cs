using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RedisTaskScheduler.Entities;
using RedisTaskScheduler.Entities.Tasks;
using RedisTaskScheduler.Repository;
using RedisTaskScheduler.Repository.Interfaces;
using StackExchange.Redis;

namespace RedisTaskScheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RedisTaskScheduler", Version = "v1" });
            });
            
            IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
            services.AddScoped(s => redis.GetDatabase());
            
            services.AddScoped<ISchedulerTaskRepository<TestSchedulerTask>, TestSchedulerTasksRepository>();
            services.AddScoped<ISchedulerTaskRepository<UrlSchedulerTask>, UrlTaskRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RedisTaskScheduler v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}