using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crawler.Crawlers
{
    public enum Website { 
        katcr,
        _1337x,
        torlock,
        tpb
    }

    public interface ICrawler
    {
        void Crawl();
    }
}
