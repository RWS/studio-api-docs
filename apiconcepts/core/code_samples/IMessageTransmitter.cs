using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationStudio.Sdk.Documentation.Samples
{
    public interface IMessageTransmitter
    {
        void SendMessage(string message);
    }
}