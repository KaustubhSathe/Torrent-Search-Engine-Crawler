using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using crawler.Crawlers;

namespace crawler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrawlController : ControllerBase
    {
        private readonly Func<Website, ICrawler> _crawlerDelegate;

        public CrawlController(Func<Website, ICrawler> crawlerDelegate) => _crawlerDelegate = crawlerDelegate;

        [HttpGet("katcr")]
        public void katcr() {
            Console.WriteLine("katcr crawling initiated at " + DateTime.UtcNow.ToLocalTime());

            ICrawler katcrCrawler = _crawlerDelegate(Website.katcr);
            katcrCrawler.Crawl();
        }


        [HttpGet("torlock")]
        public void torlock()
        {
            Console.WriteLine("torlock crawling initiated at " + DateTime.UtcNow.ToLocalTime());

            ICrawler torlockCrawler = _crawlerDelegate(Website.torlock);
            torlockCrawler.Crawl();
        }


        [HttpGet("tpb")]
        public void tpb()
        {
            Console.WriteLine("tpb crawling initiated at " + DateTime.UtcNow.ToLocalTime());

            ICrawler tpbCrawler = _crawlerDelegate(Website.tpb);
            tpbCrawler.Crawl();
        }


        [HttpGet("_1337x")]
        public void _1337x()
        {
            Console.WriteLine("_1337x crawling initiated at " + DateTime.UtcNow.ToLocalTime());

            ICrawler _1337Crawler = _crawlerDelegate(Website._1337x);
            _1337Crawler.Crawl();
        }
    }
}
