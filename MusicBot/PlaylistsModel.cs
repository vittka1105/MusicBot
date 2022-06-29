using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Bot.Model
{
    public class PlaylistsModel
    {

        public _item[] items { get; set; }
        public int limit { get; set; }
        public object next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }


        public class _item
        {
            public DateTime added_at { get; set; }

            public bool is_local { get; set; }
            public object primary_color { get; set; }
            public ___track track { get; set; }

        }



        public class External_Urls
        {
            public string spotify { get; set; }
        }

        public class ___track
        {
            public Album album { get; set; }
            public _artist1[] artists { get; set; }
            public int disc_number { get; set; }
            public int duration_ms { get; set; }


            public External_Urls3 external_urls { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public int popularity { get; set; }
            public string preview_url { get; set; }
            public int track_number { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
            public Linked_From linked_from { get; set; }
        }

        public class Album
        {
            public string album_type { get; set; }
            public __artist[] artists { get; set; }
            public External_Urls1 external_urls { get; set; }
            public string id { get; set; }
            public __image[] images { get; set; }
            public string name { get; set; }
            public string release_date { get; set; }
            public string release_date_precision { get; set; }
            public int total_tracks { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls1
        {
            public string spotify { get; set; }
        }

        public class __artist
        {
            public External_Urls2 external_urls { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls2
        {
            public string spotify { get; set; }
        }

        public class __image
        {
            public int height { get; set; }
            public string url { get; set; }
            public int width { get; set; }
        }



        public class External_Urls3
        {
            public string spotify { get; set; }
        }

        public class Linked_From
        {
            public External_Urls4 external_urls { get; set; }
            public string id { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls4
        {
            public string spotify { get; set; }
        }

        public class _artist1
        {
            public External_Urls5 external_urls { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }

        public class External_Urls5
        {
            public string spotify { get; set; }
        }


    }
}
