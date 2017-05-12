using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Haack.Encourage
{
    internal class EncourageSignatureHelpSource : ISignatureHelpSource
    {
        /// <summary>
        ///     This object needs to be added as a key to the property bag of an ITextView where
        ///     encouragement should be applied.  This prevents encouragement from being
        ///     introduced in places like signature overload.
        /// </summary>
        internal static readonly object SessionKey = new object();

        private readonly IEncouragements encouragements;

        private readonly ITextBuffer subjectBuffer;

        private bool isDisposed;

        public EncourageSignatureHelpSource(ITextBuffer subjectBuffer, IEncouragements encouragements)
        {
            this.subjectBuffer = subjectBuffer;
            this.encouragements = encouragements;
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

            IVsEnumTaskItems errors;
            IVsTaskItem[] errorItem = new IVsTaskItem[1];

            IVsTaskList errorList = encouragements.GetServiceProvider().GetService(typeof(SVsErrorList)) as IVsTaskList;
            errorList.EnumTaskItems(out errors);

            string signatureText = string.Empty;

            if (errors.Next(1, errorItem, null) == 0)
            {
                signatureText += encouragements.GetRandomDiscouragement();
            }
            else
            {
                signatureText += encouragements.GetRandomEncouragement();
            }

            var signature = new Signature(applicableToSpan, signatureText, "", "");
            signatures.Add(signature);
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                GC.SuppressFinalize(this);
                isDisposed = true;
            }
        }

        public ISignature GetBestMatch(ISignatureHelpSession session)
        {
            return session.Signatures.Count > 0 ? session.Signatures[0] : null;
        }

        private sealed class Signature : ISignature
        {
            private readonly string content;
            private readonly string documentation;
            private readonly string prettyPrintedContent;
            private readonly ITrackingSpan trackingSpan;

            internal Signature(ITrackingSpan trackingSpan, string content, string prettyPrintedContent, string documentation)
            {
                this.trackingSpan = trackingSpan;
                this.content = content;
                this.prettyPrintedContent = prettyPrintedContent;
                this.documentation = documentation;
            }

            public event EventHandler<CurrentParameterChangedEventArgs> CurrentParameterChanged;

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
        }
    }
}