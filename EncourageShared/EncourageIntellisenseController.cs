using System;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Haack.Encourage.Shared
{
    internal class EncourageIntellisenseController : IIntellisenseController
    {
        readonly ITextView textView;
        readonly ITextDocument textDocument;
        readonly EncourageIntellisenseControllerProvider provider;
        ISignatureHelpSession session;

        public EncourageIntellisenseController(
            ITextView textView,
            ITextDocument textDocument,
            EncourageIntellisenseControllerProvider provider)
        {
            this.textView = textView;
            this.textDocument = textDocument;
            this.provider = provider;

            textDocument.DirtyStateChanged += OnDocumentDirtyStateChanged;
        }

        void OnDocumentDirtyStateChanged(object sender, EventArgs e)
        {
            if (!textDocument.IsDirty)
            {
                DisplayEncouragement();
            }
        }

        void DisplayEncouragement()
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
                textDocument.DirtyStateChanged -= OnDocumentDirtyStateChanged;
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
