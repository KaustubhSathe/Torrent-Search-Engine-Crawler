using crawler.Crawlers;
using crawler.Models;
using crawler.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace crawler
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
            services.Configure<TorrentsDatabaseSettings>(Configuration.GetSection(nameof(TorrentsDatabaseSettings)));

            services.AddSingleton<ITorrentsDatabaseSettings>(sp => sp.GetRequiredService<IOptions<TorrentsDatabaseSettings>>().Value);

            services.AddSingleton<TorrentService>();

            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());


            services.AddTransient<KatcrCrawler>();
            services.AddTransient<TorlockCrawler>();
            services.AddTransient<TpbCrawler>();
            services.AddTransient<_1337xCrawler>();


            services.AddTransient<Func<Website,ICrawler>>(serviceProvider => key => {
                switch (key) {
                    case Website.katcr:
                        return serviceProvider.GetService<KatcrCrawler>();
                    case Website.torlock:
                        return serviceProvider.GetService<TorlockCrawler>();
                    case Website.tpb:
                        return serviceProvider.GetService<TpbCrawler>();
                    case Website._1337x:
                        return serviceProvider.GetService<_1337xCrawler>();
                    default:
                        throw new KeyNotFoundException();
                }
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
