using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationStudio.Sdk.Documentation.Samples
{
    #region IMessageTransmitter

    public interface IMessageTransmitter
    {
        void SendMessage(string message);
    }

    #endregion IMessageTransmitter
}
