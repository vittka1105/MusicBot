﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Bot.Model
{
 
        public class MusicSearchModel
        {
            public Tracks tracks { get; set; }
            public Artists artists { get; set; }

        }
        public class Tracks
        {
            public Hit[] hits { get; set; }
        }

        public class Hit
        {
            public Track track { get; set; }
            public string snippet { get; set; }
        }
        public class Track
        {
            public string layout { get; set; }
            public string type { get; set; }
            public string key { get; set; }
            public string title { get; set; }
            public string subtitle { get; set; }
            public Share share { get; set; }
            public Images images { get; set; }
            public Hub hub { get; set; }
            public Artist[] artists { get; set; }
            public string url { get; set; }
        }
        public class Share
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
        public class Images
        {
            public string background { get; set; }
            public string coverart { get; set; }
            public string coverarthq { get; set; }
            public string joecolor { get; set; }
        }

        public class Hub
        {
            public string type { get; set; }
            public string image { get; set; }
            public Action[] actions { get; set; }
            public Option[] options { get; set; }
            public Provider[] providers { get; set; }
            public bool _explicit { get; set; }
            public string displayname { get; set; }
        }
        public class Action
        {
            public string name { get; set; }
            public string type { get; set; }
            public string id { get; set; }
            public string uri { get; set; }
        }
        public class Option
        {
            public string caption { get; set; }
            public Action1[] actions { get; set; }
            public Beacondata beacondata { get; set; }
            public string image { get; set; }
            public string type { get; set; }
            public string listcaption { get; set; }
            public string overflowimage { get; set; }
            public bool colouroverflowimage { get; set; }
            public string providername { get; set; }
        }
        public class Beacondata
        {
            public string type { get; set; }
            public string providername { get; set; }
        }

        public class Action1
        {
            public string name { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }
        public class Provider
        {
            public string caption { get; set; }
            public Images1 images { get; set; }
            public Action2[] actions { get; set; }
            public string type { get; set; }
        }

        public class Images1
        {
            public string overflow { get; set; }
            public string _default { get; set; }
        }

        public class Action2
        {
            public string name { get; set; }
            public string type { get; set; }
            public string uri { get; set; }
        }
        public class Artist
        {
            public string id { get; set; }
            public string adamid { get; set; }
        }

        public class Artists
        {
            public Hit1[] hits { get; set; }
        }

        public class Hit1
        {
            public Artist1 artist { get; set; }
        }
        public class Artist1
        {
            public string avatar { get; set; }
            public string name { get; set; }
            public bool verified { get; set; }
            public string weburl { get; set; }
            public string adamid { get; set; }
        }

    
}
