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
        [BsonRepresentation(BsonType.ObjectId)]
        [JsonProperty("Url")]
        public string url { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }


        [JsonProperty("Size")]
        public int Size { get; set; }


        [JsonProperty("Seeders")]
        public int Seeders { get; set; }


        [JsonProperty("Leechers")]
        public int Leechers { get; set; }

    }
}
