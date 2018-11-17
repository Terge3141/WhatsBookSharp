using System;

namespace MessageParser
{
    public class TextMessage : IMessage
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

        public string Content
        {
            get;
            set;
        }

        public TextMessage(DateTime timepoint, string sender, string content)
        {
            Timepoint = timepoint;
            Sender = sender;
            Content = content;
        }
    }
}

