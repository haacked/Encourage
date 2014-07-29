using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;

namespace Haack.Encourage
{
    internal class EncourageSignatureHelpSource : ISignatureHelpSource
    {
        sealed class Signature : ISignature
        {
            readonly ITrackingSpan trackingSpan;
            readonly string content;
            readonly string prettyPrintedContent;
            readonly string documentation;

            public ITrackingSpan ApplicableToSpan
            {
                get { return trackingSpan; }
            }

            public string Content
            {
                get { return content; }
            }

            public IParameter CurrentParameter
            {
                get { return null; }
            }

            public event EventHandler<CurrentParameterChangedEventArgs> CurrentParameterChanged;

            public string Documentation
            {
                get { return documentation; }
            }

            public ReadOnlyCollection<IParameter> Parameters
            {
                get { return new ReadOnlyCollection<IParameter>(new IParameter[] { }); }
            }

            public string PrettyPrintedContent
            {
                get { return prettyPrintedContent; }
            }

            internal Signature(ITrackingSpan trackingSpan, string content, string prettyPrintedContent, string documentation)
            {
                this.trackingSpan = trackingSpan;
                this.content = content;
                this.prettyPrintedContent = prettyPrintedContent;
                this.documentation = documentation;
            }
        }

        /// <summary>
        ///     This object needs to be added as a key to the property bag of an ITextView where
        ///     encouragement should be applied.  This prevents encouragement from being
        ///     introduced in places like signature overload.
        /// </summary>
        internal static readonly object SessionKey = new object();

        readonly ITextBuffer subjectBuffer;
        readonly IEncouragements encouragements;

        public EncourageSignatureHelpSource(ITextBuffer subjectBuffer, IEncouragements encouragements)
        {
            this.subjectBuffer = subjectBuffer;
            this.encouragements = encouragements;
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
            if (!session.TextView.Properties.ContainsProperty(SessionKey))
            {
                return;
            }

            // At the moment there is a bug in the javascript provider which causes it to 
            // repeatedly insert the same Signature values into an ISignatureHelpSession
            // instance.  There is no way, other than reflection, for us to prevent this
            // from happening.  Instead we just ensure that our provider runs after 
            // Javascript and then remove the values they add here 
            signatures.Clear();

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
                0,
                SpanTrackingMode.EdgeInclusive);

            string encouragement = encouragements.GetRandomEncouragement();
            if (encouragement != null)
            {
                var signature = new Signature(applicableToSpan, encouragement, "", "");
                signatures.Add(signature);
            }
        }

        public ISignature GetBestMatch(ISignatureHelpSession session)
        {
            return session.Signatures.Count > 0 ? session.Signatures[0] : null;
        }
    }
}
