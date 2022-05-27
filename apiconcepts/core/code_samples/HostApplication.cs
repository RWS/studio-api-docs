using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

using Sdl.Core.PluginFramework;

namespace TranslationStudio.Sdk.Documentation.Samples
{
    class HostApplication
    {
        private void Main()
        {
            #region GetExtensionPoint

            IExtensionPoint extensionPoint = PluginManager.DefaultPluginRegistry.
                                                GetExtensionPoint<MessageTransmitterAttribute>();

            #endregion GetExtensionPoint

            #region EnterMessage

            Console.Write("Enter your message: ");
            string message = Console.ReadLine();
            
            #endregion EnterMessage

            #region ListTransmitters

            int i = 0;
            Console.WriteLine("Available message transmitters:");
            foreach (IExtension extension in extensionPoint.Extensions)
            {
                MessageTransmitterAttribute extensionAttribute =
                    (MessageTransmitterAttribute)extension.ExtensionAttribute;

                Console.WriteLine(String.Format("{0}) {1} (cost per character: ${2})",
                    ++i,
                    extensionAttribute.Name,
                    extensionAttribute.CostPerCharacter));
            }

            Console.WriteLine(String.Format("Choose a message transmitter (1-{0}):", i));
            int number = Convert.ToInt32(Console.ReadLine());

            #endregion ListTransmitters

            #region GetSelectedTransmitter

            IExtension selectedExtension = extensionPoint.Extensions[number - 1];
            #endregion GetSelectedTransmitter

            #region CreateInstance
            IMessageTransmitter selectedTransmitter = 
                                    (IMessageTransmitter)selectedExtension.CreateInstance();
            #endregion CreateInstance
            
            selectedTransmitter.SendMessage(message);
                        
            #region ObjectRegistry
            ObjectRegistry<MessageTransmitterAttribute, IMessageTransmitter> objectRegistry 
                = new ObjectRegistry<MessageTransmitterAttribute, IMessageTransmitter>
                            (PluginManager.DefaultPluginRegistry);

            IMessageTransmitter[] messageTransmitters = objectRegistry.CreateObjects();
            #endregion ObjectRegistry
        }
    }
}