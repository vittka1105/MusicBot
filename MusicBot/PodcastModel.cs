using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Bot.Model
{
    public class PodcastModel
    {

        public string query { get; set; }
        public Podcasts podcasts { get; set; }


        public class Podcasts
        {
            public int totalCount { get; set; }
            public Item[] items { get; set; }
            public Paginginfo pagingInfo { get; set; }
        }

        public class Paginginfo
        {
            public int nextOffset { get; set; }
            public int limit { get; set; }
        }

        public class Item
        {
            public Data data { get; set; }
        }

        public class Data
        {
            public string uri { get; set; }
            public string name { get; set; }
            public Coverart coverArt { get; set; }
            public string type { get; set; }
            public Publisher publisher { get; set; }
            public string mediaType { get; set; }
        }

        public class Coverart
        {
            public Source[] sources { get; set; }
        }

        public class Source
        {
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        public class Publisher
        {
            public string name { get; set; }
        }

    }
}
