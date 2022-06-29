using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Bot.Model
{
    public class RecommendModel
    {
        public List<tracks> tracks { get; set; }
    }

    public class tracks
    {
        public string title { get; set; }
        public string subtitle { get; set; }
        public string url { get; set; }
        public ___share share { get; set; }
    }
    public class ___share 
    {
        public string subject { get; set; }
        public string href { get; set; }
        public string image { get; set; }

    }

}
