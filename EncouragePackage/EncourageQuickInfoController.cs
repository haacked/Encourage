using System;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace Haack.Encourage
{
    internal class EncourageQuickInfoController : IIntellisenseController
    {
        ITextView textView;
        readonly EncourageQuickInfoControllerProvider provider;
        IQuickInfoSession session;

        public EncourageQuickInfoController(
            ITextView textView,
            DTE dte,
            EncourageQuickInfoControllerProvider provider)
        {
            this.textView = textView;
            this.provider = provider;
            dte.Events.DocumentEvents.DocumentSaved += OnSaved;
        }

        void OnSaved(Document document)
        {
            var point = textView.Caret.Position.BufferPosition;
            var triggerPoint = point.Snapshot.CreateTrackingPoint(point.Position, PointTrackingMode.Positive);
            if (!provider.QuickInfoBroker.IsQuickInfoActive(textView))
            {
                session = provider.QuickInfoBroker.TriggerQuickInfo(textView, triggerPoint, true);
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