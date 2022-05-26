using System;
using System.Collections.Generic;
using System.Text;

using Sdl.Core.PluginFramework;

namespace TranslationStudio.Sdk.Documentation.Samples
{
    #region MessageTransmitterAttribute

    [ExtensionPointInfo("Message Transmitters", ExtensionPointBehavior.Static)]
    public class MessageTransmitterAttribute : ExtensionAttribute
    {       
        /// <summary>
        /// Constructor for XML serialization
        /// </summary>
        public MessageTransmitterAttribute()
        {
        }
        
        /// <summary>
        /// Constructor using basic properties.
        /// </summary>
        public MessageTransmitterAttribute(string id, string name, string description)
            : base(id, name, description)
        {
        }

        /// <summary>
        /// Gets or sets the cost in dollar per character in the message.
        /// </summary>
        public double CostPerCharacter { get; set; }

        /// <summary>
        /// Validates that the extension implements the IMessageTransmitter interface.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void Validate(
            Sdl.Core.PluginFramework.Validation.IExtensionAttributeInfo info,
            Sdl.Core.PluginFramework.Validation.IExtensionValidationContext context)
        {
            base.Validate(info, context);

            context.ValidateRequiredInterface(typeof(IMessageTransmitter));
        }
    }

    #endregion MessageTransmitterAttribute
}