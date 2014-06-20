using System;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System.Windows.Threading;

namespace Haack.Encourage
{
    internal class EncourageQuickInfoController : IIntellisenseController
    {
        ITextView textView;
        readonly EncourageQuickInfoControllerProvider provider;
        ISignatureHelpSession session;
        DocumentEvents documentEvents;

        public EncourageQuickInfoController(
            ITextView textView,
            DTE dte,
            EncourageQuickInfoControllerProvider provider)
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
                session = provider.SignatureHelpBroker.TriggerSignatureHelp(textView, triggerPoint, true);
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