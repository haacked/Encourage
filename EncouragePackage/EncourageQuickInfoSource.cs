using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace Haack.Encourage
{
    internal class EncourageQuickInfoSource : ISignatureHelpSource
    {
        sealed class Signature : ISignature
        {
            readonly ITrackingSpan trackingSpan;
            readonly string content;
            readonly string prettyPrintedContent;
            readonly string documentation;

            public ITrackingSpan ApplicableToSpan
            {
                get { return this.trackingSpan; }
            }

            public string Content
            {
                get { return this.content; }
            }

            public IParameter CurrentParameter
            {
                get { return null; }
            }

            public event EventHandler<CurrentParameterChangedEventArgs> CurrentParameterChanged;

            public string Documentation
            {
                get { return this.documentation; }
            }

            public ReadOnlyCollection<IParameter> Parameters
            {
                get { return new ReadOnlyCollection<IParameter>(new IParameter[] { });  }
            }

            public string PrettyPrintedContent
            {
                get { return this.prettyPrintedContent; }
            }

            internal Signature(ITrackingSpan trackingSpan, string content, string prettyPrintedContent, string documentation)
            {
                this.trackingSpan = trackingSpan;
                this.content = content;
                this.prettyPrintedContent = prettyPrintedContent;
                this.documentation = documentation;
            }
        }


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


        public void AugmentSignatureHelpSession(ISignatureHelpSession session, IList<ISignature> signatures)
        {
            // Map the trigger point down to our buffer.
            var subjectTriggerPoint = session.GetTriggerPoint(subjectBuffer.CurrentSnapshot);
            if (!subjectTriggerPoint.HasValue)
            {
                return;
            }

            var currentSnapshot = subjectTriggerPoint.Value.Snapshot;
            var querySpan = new SnapshotSpan(subjectTriggerPoint.Value, 0);
            var applicableToSpan = currentSnapshot.CreateTrackingSpan(
                querySpan.Start.Position,
                1,
                SpanTrackingMode.EdgeInclusive);

            var signature = new Signature(applicableToSpan, "Good Job!!!", "", "");
            signatures.Add(signature);
        }

        public ISignature GetBestMatch(ISignatureHelpSession session)
        {
            return session.Signatures.Count > 0 ? session.Signatures[0] : null;
        }
    }
}
