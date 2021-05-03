using crawler.Models;
using crawler.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace crawler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TorrentController : ControllerBase
    {
        private readonly TorrentService _torrentService;
        public TorrentController(TorrentService torrentService)
        {
            this._torrentService = torrentService;
        }

        [HttpGet]
        public ActionResult<List<Torrent>> Get() => _torrentService.Get();


        [HttpGet("{id}", Name = "GetTorrent")]
        public ActionResult<Torrent> Get(string url) {
            var torrent = _torrentService.Get(url);

            if (torrent == null)
            {
                return NotFound();
            }

            return torrent;
        }

        [HttpPost]
        public ActionResult<Torrent> Create(Torrent torrent)
        {
            _torrentService.Create(torrent);

            return CreatedAtRoute("GetTorrent", new { url = torrent.url.ToString() }, torrent) ;
        }

    }
}
