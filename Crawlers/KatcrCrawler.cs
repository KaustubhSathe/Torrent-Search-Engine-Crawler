using Microsoft.Extensions.Configuration;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace crawler.Crawlers
{
    public class KatcrCrawler : ICrawler
    {
        public IConfiguration Configuration { get; }
        private readonly string url;
        private readonly string[] categories;

        public KatcrCrawler(IConfiguration configuration)
        {
            Configuration = configuration;
            url = Configuration.GetValue<string>("CrawlWebsites:katcr:url");
            categories = Configuration.GetSection("CrawlWebsites:katcr:categories").Get<string[]>();
        }
        public async void Crawl()
        {
            
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new string[] { "--no-sandbox"} 
            });
            Console.WriteLine("Ok reached till here");
            Console.WriteLine("Ok reached till here");
            Console.WriteLine("Ok reached till here");

            //foreach (var x in categories) {
            //    new Thread(() => {
            //        crawlCategory(x, browser);
            //    }).Start();
            //}
        }

        private async void crawlCategory(string category,Browser browser) {
            var page = await browser.NewPageAsync();
            await page.GoToAsync(url + category);
        }

        
    }
}
