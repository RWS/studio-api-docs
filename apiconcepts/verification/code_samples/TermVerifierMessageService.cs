using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.NativeApi;

namespace Verification.TermVerifier
{
    class TermVerifierMessageService
    {
        private BilingualContentMessageReporterProxy MessageReporterProxy
        {
            get { return MessageReporter != null ? new BilingualContentMessageReporterProxy(MessageReporter) : null; }
        }

        public object MessageOriginator { get; set; }

        public TermVerifierMessageService()
        {
            MessageOriginator = this;
        }

        public IBilingualContentMessageReporter MessageReporter { get; set; }

        public void AddMessage(string message, ErrorLevel errorLevel,
                                TextLocation from, TextLocation upto, ISegment sourceSegment, ISegment targetSegment)
        {
            if (MessageReporter != null)
            {
                MessageReporterProxy.ReportMessage(MessageOriginator, StringResources.TermVerifier_Name, errorLevel,
                    message, from, upto,
                    new CustomMessageData(null, null, sourceSegment.ToString(), targetSegment.ToString()));
            }
        }

        public void AddMessage(string message, ErrorLevel errorLevel,
                                ISegment sourceSegment, ISegment targetSegment)
        {
            TextLocation from = new TextLocation(new Location(targetSegment, true), 0);

            if (MessageReporter != null)
            {
                MessageReporterProxy.ReportMessage(MessageOriginator, StringResources.TermVerifier_Name, errorLevel,
                    message, from, from,
                    new CustomMessageData(null, null, sourceSegment.ToString(), targetSegment.ToString()));
            }
        }
    }
}
