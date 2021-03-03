using Sdl.Terminology.TerminologyProvider.Core;
using System;

namespace SDL_Terminology_Provider_Plug_in
{
    [TerminologyProviderFactory(Id = "My_Terminology_Provider_Id", Name = "My_Terminology_Provider_Name", Description = "My_Terminology_Provider_Description")]
    public class MyTerminologyProviderFactory : ITerminologyProviderFactory
    {
        #region "CreateProvider"
        //Create the terminology provider and pass the provider uri, which is the glossary text file name and path
        public ITerminologyProvider CreateTerminologyProvider(Uri terminologyProviderUri, ITerminologyProviderCredentialStore credentials)
        {
            MyTerminologyProvider _terminologyProvider = new MyTerminologyProvider(terminologyProviderUri.ToString());
            return _terminologyProvider;
        }
        #endregion

        public bool SupportsTerminologyProviderUri(Uri terminologyProviderUri)
        {
            return true;
        }
    }
}



