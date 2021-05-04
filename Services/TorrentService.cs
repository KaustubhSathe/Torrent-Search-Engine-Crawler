using crawler.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace crawler.Services
{
    public class TorrentService
    {
        private readonly IMongoCollection<Torrent> _torrents;
        public TorrentService(ITorrentsDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);

            var database = client.GetDatabase(settings.DatabaseName);
            _torrents = database.GetCollection<Torrent>(settings.CollectionName);
        }


        public List<Torrent> Get() => _torrents.Find(itr => true).ToList();


        public Torrent Get(string url) => _torrents.Find<Torrent>(itr => itr.url == url).FirstOrDefault();


        public async Task Create(Torrent torrent) {
            await _torrents.InsertOneAsync(torrent);
        }

       

        public List<Torrent> Search(string query,int page) {
            return _torrents.Find(x => x.Name.Contains(query))
                .Skip((page-1)*25)
                .Limit(25)
                .ToList();
        }

    }
}
