using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Bot.Model
{
    class DetailsModel
    {
        public string layout { get; set; }
        public string type { get; set; }
        public string key { get; set; }
        public string title { get; set; }
        public _Images images { get; set; }
        public _share share { get; set; }
        public Genres genres { get; set; }
        public Section[] sections { get; set; }
        public class _Images
        {
            public string background { get; set; }

        }
        public class _share
        {
            public string subject { get; set; }
            public string href { get; set; }
            public string image { get; set; }
            public string html { get; set; }
            public string avatar { get; set; }
            public string snapchat { get; set; }
        }
        public class Genres
        {
            public string primary { get; set; }
        }
        public class Section
        {
            public string type { get; set; }
            public string tabname { get; set; }
            public Metadata[] metadata { get; set; }
            public Youtubeurl youtubeurl { get; set; }
        }
        public class Metadata
        {
            public string title { get; set; }
            public string text { get; set; }
        }
        public class Youtubeurl
        {
            public string caption { get; set; }
            public Action4[] actions { get; set; }
        }
        public class Action4
        {
            public string name { get; set; }
            public string type { get; set; }
            public Share1 share { get; set; }
            public string uri { get; set; }
        }
        public class Share1
        {
            public string subject { get; set; }
            public string text { get; set; }
            public string href { get; set; }
            public string image { get; set; }
            public string twitter { get; set; }
            public string html { get; set; }
            public string avatar { get; set; }
            public string snapchat { get; set; }
        }
    }
}
