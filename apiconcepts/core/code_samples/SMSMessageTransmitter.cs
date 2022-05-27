using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationStudio.Sdk.Documentation.Samples
{
    [MessageTransmitter(
        Id = "sms",
        Name = "SMS Transmitter",
        Description = "Send messages via SMS",
        CostPerCharacter = 0.5)]
    public class SMSMessageTransmitter : IMessageTransmitter
    {
        #region IMessageTransmitter Members

        public int CostPerMessage
        {
            get { return 10; }
        }

        public void SendMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine(String.Format("SMS: {0}", message));
            Console.WriteLine();
        }

        #endregion
    }
}