using EnvDTE;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Haack.Encourage
{
    internal class EncourageIntellisenseController : IIntellisenseController
    {
        ITextView textView;
        readonly EncourageIntellisenseControllerProvider provider;
        ISignatureHelpSession session;
        DocumentEvents documentEvents;

        public EncourageIntellisenseController(
            ITextView textView,
            DTE dte,
            EncourageIntellisenseControllerProvider provider)
        {
            this.textView = textView;
            this.provider = provider;
            this.documentEvents = dte.Events.DocumentEvents;
            documentEvents.DocumentSaved += OnSaved;
        }

        void OnSaved(Document document)
        {
            var point = textView.Caret.Position.BufferPosition;
            var triggerPoint = point.Snapshot.CreateTrackingPoint(point.Position, PointTrackingMode.Positive);
            if (!provider.SignatureHelpBroker.IsSignatureHelpActive(textView))
            {
                textView.Properties.AddProperty(EncourageSignatureHelpSource.SessionKey, null);
                session = provider.SignatureHelpBroker.TriggerSignatureHelp(textView, triggerPoint, true);
                textView.Properties.RemoveProperty(EncourageSignatureHelpSource.SessionKey);
            }
        }

        public void Detach(ITextView detacedTextView)
        {
            if (textView == detacedTextView)
            {
                textView = null;
            }
        }

        public void ConnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }

        public void DisconnectSubjectBuffer(ITextBuffer subjectBuffer)
        {
        }
    }
}