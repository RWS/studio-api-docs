# Sub Content Component Builder

The following example implements [ISubContentComponentBuilder](../../api/filetypesupport/Sdl.FileTypeSupport.Framework.IntegrationApi.ISubContentComponentBuilder.yml):

# [C#](#tab/tabid-1)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.FileTypeSupport.Framework;
using Sdl.Core.Globalization;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Sdk.Snippets.Native
{
    [FileTypeComponentBuilderAttribute(Id = "SimpleText_SubContentComponentBuilder_Id",
                                       Name = "SimpleText_FilterComponentBuilderExtension_Name",
                                       Description = "SimpleText_FilterComponentBuilderExtension_Description")]
    class SimpleTextSubContentComponentBuilder : IFileTypeComponentBuilder, ISubContentComponentBuilder
    {
        public IAbstractGenerator BuildAbstractGenerator(string name)
        {
            return null;
        }

        public IAdditionalGeneratorsInfo BuildAdditionalGeneratorsInfo(string name)
        {
            return null;
        }

        public IBilingualDocumentGenerator BuildBilingualGenerator(string name)
        {
            return null;
        }

        public IFileExtractor BuildFileExtractor(string name)
        {
            return null;
        }

        public IFileGenerator BuildFileGenerator(string name)
        {
            return null;
        }

        public INativeFileSniffer BuildFileSniffer(string name)
        {
            return null;
        }

        public IFileTypeInformation BuildFileTypeInformation(string name)
        {
            var info = this.FileTypeManager.BuildFileTypeInformation();

            info.FileTypeDefinitionId = new FileTypeDefinitionId("Simple Text Embedded Content 1.0.0.0");
            info.FileTypeName = new LocalizableString("Simple text embedded content");
            info.FileTypeDocumentName = new LocalizableString("Test text embedded content");
            info.FileTypeDocumentsName = new LocalizableString("Simple text embedded content");
            info.Description = new LocalizableString("This sample sub-content processor handles simple text");
            info.Icon = new IconDescriptor("assembly://Sdl.Sdk.FileTypeSupport.Samples.SimpleText/Sdl.Sdk.FileTypeSupport.Samples.SimpleText.SimpleText.ico");

            info.WinFormSettingsPageIds = new string[]
            {
                "SimpleText_Settings",
            };

            return info;
        }

        public IAbstractPreviewApplication BuildPreviewApplication(string name)
        {
            return null;
        }

        public IAbstractPreviewControl BuildPreviewControl(string name)
        {
            return null;
        }

        public IPreviewSetsFactory BuildPreviewSetsFactory(string name)
        {
            return null;
        }

        public IQuickTagsFactory BuildQuickTagsFactory(string name)
        {
            return null;
        }

        public IVerifierCollection BuildVerifierCollection(string name)
        {
            return null;
        }

        public IFileTypeManager FileTypeManager { get; set; }

        public IFileTypeDefinition FileTypeDefinition { get; set; }

        public ISubContentExtractor BuildSubContentExtractor(string name)
        {
            var parser = new SimpleSubContentTextParser();
            var extractor = this.FileTypeManager.BuildSubContentExtractor(this.FileTypeManager.BuildNativeSubContentExtractor(parser), this);
            return extractor;
        }

        public ISubContentGenerator BuildSubContentGenerator(string name)
        {
            return this.FileTypeManager.BuildSubContentGenerator(this.FileTypeManager.BuildNativeSubContentGenerator(new SimpleSubContentTextWriter()));
        }
    }
}
```

>[!NOTE]
>
> This content may be out-of-date. To check the latest information on this topic, inspect the libraries using the Visual Studio Object Browser.
