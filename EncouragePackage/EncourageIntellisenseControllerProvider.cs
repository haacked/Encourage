using System.Collections.Generic;
using System.ComponentModel.Composition;
using EnvDTE;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;

namespace Haack.Encourage
{
    [Export(typeof(IIntellisenseControllerProvider))]
    [Name("Encourage Intellisense Controller")]
    [ContentType("text")]
    internal class EncourageIntellisenseControllerProvider : IIntellisenseControllerProvider
    {
        [Import]
        internal ISignatureHelpBroker SignatureHelpBroker { get; set; }

        [Import]
        internal ITextDocumentFactoryService TextDocumentFactoryService = null;

        public IIntellisenseController TryCreateIntellisenseController(ITextView textView, IList<ITextBuffer> subjectBuffers)
        {
            ITextDocument textDocument;
            if (!TextDocumentFactoryService.TryGetTextDocument(textView.TextDataModel.DocumentBuffer, out textDocument))
            {
                return null;
            }

            // In general having an ITextDocument is sufficient to determine if a given ITextView is 
            // back by an actual document.  There are some windows though, like the Immediate Window, 
            // which aren't documents that do still have a backing temporary file.  These files are
            // uninteresting to Encourage because they are temporary files that exist as an
            // implementation detail
            //
            // The easiest way to filter for real documents is to check for the Document role 
            if (!textView.Roles.Contains(PredefinedTextViewRoles.Document))
            {
                return null;
            }

            return new EncourageIntellisenseController(textView, textDocument, this);
        }
    }
}
