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
            if (!TextDocumentFactoryService.TryGetTextDocument(textView.TextBuffer, out textDocument))
            {
                return null;
            }

            return new EncourageIntellisenseController(textView, textDocument, this);
        }
    }
}
