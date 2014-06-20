using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace Haack.Encourage
{
    [Export(typeof(ISignatureHelpSourceProvider))]
    [Name("ToolTip SignatureHelp Source")]
    [Order(Before = "Default Signature Help Presenter")]
    [ContentType("text")]
    internal class EncourageSignatureHelpSourceProvider : ISignatureHelpSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        [Import]
        internal ITextBufferFactoryService TextBufferFactoryService { get; set; }

        public ISignatureHelpSource TryCreateSignatureHelpSource(ITextBuffer textBuffer)
        {
            return new EncourageSignatureHelpSource(textBuffer);
        }
    }
}