using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationStudio.Sdk.Documentation.Samples
{
    [MessageTransmitter(
        Id = "email",
        Name = "E-mail Transmitter",
        Description = "Send messages via e-mail",
        CostPerCharacter = 0.1)]
    public class EmailMessageTransmitter : IMessageTransmitter
    {
        public void SendMessage(string message)
        {
            Console.WriteLine();
            Console.WriteLine(String.Format("Email: {0}", message));
            Console.WriteLine();
        }
    }
}