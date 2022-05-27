using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Sdl.Core.PluginFramework;

namespace TranslationStudio.Sdk.Documentation.Samples
{
    public class AdvancedPluginFramework
    {
        private void GetPluginResource()
        {
            IExtension extension = null;
            #region GetPluginResource
            ExtensionAttribute attribute = extension.ExtensionAttribute;
            Icon icon = extension.Plugin.GetPluginResource<Icon>(attribute.Icon);
            #endregion GetPluginResource

        }
    }

    #region PluginButtonAttribute

    [ExtensionPointInfo("Plug-in Buttons", ExtensionPointTypes.Static)]
    public class PluginButtonAttribute : ExtensionAttribute
    {
        public PluginButtonAttribute()
        {
        }

        [PluginResource()]
        public string ToolTipText { get; set; }
    }

    #endregion PluginButtonAttribute

    #region PluginButton

    [PluginButton(
        Id = "mypluginbutton",
        Name = "MyPluginButton_Name",
        ToolTipText = "MyPluginButton_ToolTipText")]
    public class MyPluginButton : IPluginButton
    {
        public void OnClick()
        {
            // do something
        }
    }

    #endregion PluginButton

    #region Auxiliary

    public class ToolBarLocationAttribute : AuxiliaryExtensionAttribute
    {
        public string ToolBarId { get; set; }
    }

    public class MenuLocationAttribute : AuxiliaryExtensionAttribute
    {
        public string MenuId { get; set; }
    }

    #endregion Auxiliary

    #region AuxiliaryExample

    [Sdl.Desktop.Platform.CommandBars.Action(
        Id = "mypluginbutton",
        Name = "MyPluginAction_Name",
        Description = "MyPluginAction_ToolTipText")]
    [ToolBarLocation(ToolBarId = "StandardToolBar")]
    [MenuLocation(MenuId = "FileMenu")]
    public class MyPluginButton2 : IPluginButton
    {
        // ...
    }

    #endregion AuxiliaryExample

    #region PluginButtonAttribute2

    [ExtensionPointInfo("Plug-in Buttons", ExtensionPointTypes.Static)]
    public class PluginButtonAttribute2
        : ExtensionAttribute,
          System.Xml.Serialization.IXmlSerializable
    {
        public PluginButtonAttribute2()
        {
        }

        [PluginResource()]
        public string ToolTipText { get; set; }

        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);

            ToolTipText = reader.ReadElementContentAsString("ToolTipText", "");
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteElementString("ToolTipText", ToolTipText);
        }
    }

    #endregion PluginButtonAttribute2

    public interface IPluginButton
    {
    }

}
