using Sdl.Core.Settings;

using Sdl.Verification.Api;

namespace Sdl.Verification.Sdk.IdenticalCheck
{
    [GlobalVerifierSettingsPage(
    Id = "Identical Settings Definition ID",
    Name = "Context to check",
    Description = "The display code of the context for which the length check should be performed.",
    HelpTopic = "")]
    class IdenticalVerifierUIPage : AbstractSettingsPage
    {
        private IdenticalVerifierUI _Control;
        private IdenticalVerifierSettings _ControlSettings;
    }
}