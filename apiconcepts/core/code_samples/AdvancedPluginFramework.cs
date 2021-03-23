using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using Sdl.Core.PluginFramework;

namespace Sdl.TranslationStudio.Sdk.Documentation.Samples
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
        private string _toolTipText;

        public PluginButtonAttribute()
        {
        }

        [PluginResource()]
        public string ToolTipText
        {
            get { return _toolTipText; }
            set { _toolTipText = value; }
        }
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
        private string _toolbarId;

        public string ToolBarId
        {
            get { return _toolbarId; }
            set { _toolbarId = value; }
        }
    }

    public class MenuLocationAttribute : AuxiliaryExtensionAttribute
    {
        private string _menuId;

        public string MenuId
        {
            get { return _menuId; }
            set { _menuId = value; }
        }
    }

    #endregion Auxiliary

    #region AuxiliaryExample

    [Sdl.Desktop.Platform.CommandBars.Action(
    Id = "mypluginbutton",
    Name = "MyPluginAction_Name",
    Description = "MyPluginAction_ToolTipText")]
    [ToolBarLocation(ToolBarId="StandardToolBar")]
    [MenuLocation(MenuId="FileMenu")]
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
        private string _toolTipText;

        public PluginButtonAttribute2()
        {
        }

        [PluginResource()]
        public string ToolTipText
        {
            get { return _toolTipText; }
            set { _toolTipText = value; }
        }

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
