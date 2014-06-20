using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;

namespace Haack.Encourage
{
    [Export(typeof(ISignatureHelpSourceProvider))]
    [Name("ToolTip QuickInfo Source")]
    [Order(Before = "Default Quick Info Presenter")]
    [ContentType("text")]
    internal class EncourageQuickInfoSourceProvider : ISignatureHelpSourceProvider
    {
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }

        [Import]
        internal ITextBufferFactoryService TextBufferFactoryService { get; set; }

        public ISignatureHelpSource TryCreateSignatureHelpSource(ITextBuffer textBuffer)
        {
            return new EncourageQuickInfoSource(textBuffer);
        }
    }
}