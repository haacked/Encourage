using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace Haack.Encourage
{
    internal class EncourageQuickInfoSource : IQuickInfoSource
    {
        readonly ITextBuffer subjectBuffer;

        public EncourageQuickInfoSource(ITextBuffer subjectBuffer)
        {
            this.subjectBuffer = subjectBuffer;
        }

        bool isDisposed;

        public void Dispose()
        {
            if (!isDisposed)
            {
                GC.SuppressFinalize(this);
                isDisposed = true;
            }
        }

        public void AugmentQuickInfoSession(
            IQuickInfoSession session,
            IList<object> qiContent,
            out ITrackingSpan applicableToSpan)
        {
            // Map the trigger point down to our buffer.
            var subjectTriggerPoint = session.GetTriggerPoint(subjectBuffer.CurrentSnapshot);
            if (!subjectTriggerPoint.HasValue)
            {
                applicableToSpan = null;
                return;
            }

            var currentSnapshot = subjectTriggerPoint.Value.Snapshot;
            var querySpan = new SnapshotSpan(subjectTriggerPoint.Value, 0);
            applicableToSpan = currentSnapshot.CreateTrackingSpan(
                querySpan.Start.Position,
                9,
                SpanTrackingMode.EdgeInclusive);
            qiContent.Add("Great job!");
        }
    }
}
