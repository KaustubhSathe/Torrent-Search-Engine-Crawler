using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace crawler.Models
{
    public class Torrent
    {
        [JsonProperty("Source")]
        public string Source { get; set; }

        [BsonId]
        [JsonProperty("Url")]
        public string url { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }


        [JsonProperty("Size")]
        public string Size { get; set; }


        [JsonProperty("Seeders")]
        public int Seeders { get; set; }


        [JsonProperty("Leechers")]
        public int Leechers { get; set; }


        [JsonProperty("UploadDate")]
        public DateTime UploadDate { get; set; }


        public Torrent(TorrentBuilder builder)
        {
            this.Leechers = builder.Leechers;
            this.Seeders = builder.Seeders;
            this.Name = builder.Name;
            this.Size = builder.Size;
            this.Source = builder.Source;
            this.url = builder.url;
            this.UploadDate = builder.UploadDate;
        }

    }
    public class TorrentBuilder
    {
        public string Source { get; set; }
        public string url { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public int Seeders { get; set; }
        public int Leechers { get; set; }
        public DateTime UploadDate { get; set; }

        public TorrentBuilder()
        {

        }

        public TorrentBuilder setSource(string Source)
        {
            this.Source = Source;
            return this;
        }

        public TorrentBuilder setURL(string url)
        {
            this.url = url;
            return this;
        }

        public TorrentBuilder setName(string Name)
        {
            this.Name = Name;
            return this;
        }

        public TorrentBuilder setSize(string Size)
        {
            this.Size = Size;
            return this;
        }
        public TorrentBuilder setSeeders(int Seeders)
        {
            this.Seeders = Seeders;
            return this;
        }
        public TorrentBuilder setLeechers(int Leechers)
        {
            this.Leechers = Leechers;
            return this;
        }

        public TorrentBuilder setUploadDate(DateTime UploadDate)
        {
            this.UploadDate = UploadDate;
            return this;
        }

        public Torrent build()
        {
            Torrent torrent = new Torrent(this);
            return torrent;
        }
    }
}
