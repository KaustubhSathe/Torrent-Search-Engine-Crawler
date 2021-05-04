using Microsoft.Extensions.Configuration;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Supremes;
using Supremes.Nodes;
using crawler.Models;
using crawler.Services;

namespace crawler.Crawlers
{
    public class KatcrCrawler : ICrawler
    {
        public IConfiguration Configuration { get; }
        private readonly string siteURL;
        private readonly string[] categories;
        private readonly int maxPages = 200;
        private readonly string Source = "KATCR";
        private readonly TorrentService _torrentService;

        public KatcrCrawler(IConfiguration configuration, TorrentService torrentService)
        {
            Configuration = configuration;
            siteURL = Configuration.GetValue<string>("CrawlWebsites:katcr:url");
            categories = Configuration.GetSection("CrawlWebsites:katcr:categories").Get<string[]>();
            this._torrentService = torrentService;
        }
        public async void Crawl()
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new string[] { "--no-sandbox" }
            });
            Console.WriteLine("OK started the browser.");

            foreach (var x in categories)
            {

                await crawlCategory(x, browser);

            }




        }

        private async Task crawlCategory(string category, Browser browser)
        {
            // iterate over all the pages 
            for (int i = 1; i <= maxPages; i++)
            {
                await crawlPageNumber(i, category, browser);
            }

            Console.WriteLine("Finished crawling " + category);
        }

        private async Task crawlPageNumber(int pageNumber, string category, Browser browser)
        {
            // create a new page
            var page = await browser.NewPageAsync();
            await page.GoToAsync(siteURL + category + "/" + pageNumber);

            // log to console
            Console.WriteLine("Reached to website " + siteURL + category + "/" + pageNumber);

            // extract entire document and store it in dcsoup
            string entireHTML = await page.GetContentAsync();
            Document dc = Dcsoup.Parse(entireHTML);


            // get the table of torrents select first index
            Element table = dc.Select("#wrapperInner > div.mainpart > table > tbody > tr > td:nth-child(1) > div:nth-child(2) > table > tbody")[0];

            // index 1 to 20
            for (int i = 1; i <= 20; i++)
            {
                Element torrentRow = table.Child(i);
                Elements tableCols = torrentRow.Children.Select("td");


                string torrentName = tableCols[0].Children.Select("div")[2].Children.Select("a").Text.ToString().Trim();
                string torrentURL = siteURL.Remove(siteURL.Length - 1) + tableCols[0].Children.Select("div")[2].Children.Select("a").Attr("href").ToString().Trim();
                string torrentSize = tableCols[1].Text.ToString().Trim();
                string uploadDate = tableCols[3].Text.ToString().Trim();
                string seeders = tableCols[4].Text.ToString().Trim();
                string leechers = tableCols[5].Text.ToString().Trim();

                Torrent currentTorrent = new TorrentBuilder().setName(torrentName)
                    .setURL(torrentURL)
                    .setSource(Source)
                    .setSize(torrentSize)
                    .setUploadDate(cleanifyDate(uploadDate))
                    .setSeeders(cleanifySeeders(seeders))
                    .setLeechers(cleanifyLeechers(leechers))
                    .build();

                await _torrentService.Create(currentTorrent);
                Console.WriteLine("Added new Torrent " + torrentURL);

            }

            Console.WriteLine("Finished crawling" + siteURL + category + "/" + pageNumber);


        }

        private int cleanifySeeders(string seeders)
        {
            return Int32.Parse(seeders.Trim());
        }

        private int cleanifyLeechers(string leechers)
        {
            return Int32.Parse(leechers.Trim());
        }

        private DateTime cleanifyDate(string uploadDate)
        {
            if (uploadDate.ToLower().Contains("hour"))
            {
                uploadDate = uploadDate.Replace("hours", "");
                uploadDate = uploadDate.Replace("hour", "");
                uploadDate = uploadDate.Trim();
                return DateTime.UtcNow.AddHours(-1 * Int32.Parse(uploadDate));
            }


            if (uploadDate.ToLower().Contains("day"))
            {
                uploadDate = uploadDate.Replace("days", "");
                uploadDate = uploadDate.Replace("day", "");
                uploadDate = uploadDate.Trim();
                return DateTime.UtcNow.AddDays(-1 * Int32.Parse(uploadDate));
            }


            return DateTime.UtcNow;

        }





    }
}
