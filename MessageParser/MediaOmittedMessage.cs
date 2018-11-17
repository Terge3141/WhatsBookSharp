using System;
using Helper;
using System.Collections.Generic;
using System.Linq;

namespace MessageParser
{
    public class MediaOmittedMessage : IMessage
    {
        public DateTime Timepoint
        {
            get;
            set;
        }

        public string Sender
        {
            get;
            set;
        }

        public List<string> Relpaths
        {
            get;
            set;
        }

        public MediaOmittedMessage(ImageMatcher.MatchEntry matchEntry, string sender)
        {
            Timepoint = matchEntry.Timepoint;
            Sender = sender;
            Relpaths = matchEntry.Filematches.Select(x => x.Relpath).ToList();
        }
    }
}

