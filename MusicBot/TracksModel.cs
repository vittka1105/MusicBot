using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Bot.Model
{
    public class TracksModel
    {

        public Properties properties { get; set; }
        public _Track[] tracks { get; set; }


        public class Properties
        {
        }

        public class _Track
        {

            public string title { get; set; }
            public string subtitle { get; set; }
            public _Share share { get; set; }


        }
        public class _Share
        {
            public string subject { get; set; }
            public string href { get; set; }
            public string image { get; set; }
            public string html { get; set; }

        }
    }
}