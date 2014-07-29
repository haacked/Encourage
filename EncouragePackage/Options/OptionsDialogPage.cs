using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using MessageBox = System.Windows.Forms.MessageBox;
using Microsoft.VisualStudio.ComponentModelHost;

namespace Haack.Encourage.Options
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false)]
    [ComVisible(true)]
    [Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9")]
    public class OptionsDialogPage : UIElementDialogPage
    {
        OptionsDialogPageControl optionsDialogControl;

        protected override UIElement Child
        {
            get
            {
                return optionsDialogControl ?? (optionsDialogControl = new OptionsDialogPageControl());
            }
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);

            var encouraments = GetEncouragements();
            optionsDialogControl.Encouragements = string.Join(Environment.NewLine, encouraments.AllEncouragements);
        }

        protected override void OnApply(PageApplyEventArgs args)
        {
            if (args.ApplyBehavior == ApplyKind.Apply)
            {
                var userEncouragments = optionsDialogControl.Encouragements.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                GetEncouragements().AllEncouragements = userEncouragments;
            }

            base.OnApply(args);
        }

        private IEncouragements GetEncouragements()
        {
            var componentModel = (IComponentModel)(Site.GetService(typeof(SComponentModel)));
            return componentModel.DefaultExportProvider.GetExportedValue<IEncouragements>();
        }
    }
}
