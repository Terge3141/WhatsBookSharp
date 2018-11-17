using System;

namespace MessageParser
{
    public interface IMessage
    {
        DateTime Timepoint
        {
            get;
            set;
        }

        string Sender
        {
            get;
            set;
        }
    }
}

