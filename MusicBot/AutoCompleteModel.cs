using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Bot.Model
{
   
        public class AutoCompleteModel
        {
            public List<Hints> hints { get; set; }
        }
        public class Hints
        {
            public string Term { get; set; }
        }
    
}
