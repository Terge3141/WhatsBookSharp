using System;

namespace MessageParser
{
    public class ImageMessage : IMessage
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

        public string Filename
        {
            get;
            set;
        }

        public string Subscription
        {
            get;
            set;
        }

        public ImageMessage(DateTime timepoint, string sender, string filename, string subscription)
        {
            Timepoint = timepoint;
            Sender = sender;
            Filename = filename;
            Subscription = subscription;
        }
    }
}

