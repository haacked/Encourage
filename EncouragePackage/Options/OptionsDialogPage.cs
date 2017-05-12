using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;

namespace Haack.Encourage.Options
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false)]
    [ComVisible(true)]
    [Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9")]
    public class OptionsDialogPage : UIElementDialogPage
    {
        private OptionsDialogPageControl optionsDialogControl;

        protected override UIElement Child
        {
            get { return optionsDialogControl ?? (optionsDialogControl = new OptionsDialogPageControl()); }
        }

        protected override void OnActivate(CancelEventArgs e)
        {
            base.OnActivate(e);

            var encouragements = GetEncouragements();
            optionsDialogControl.Encouragements = string.Join(Environment.NewLine, encouragements.AllEncouragements);
            optionsDialogControl.Discouragements = string.Join(Environment.NewLine, encouragements.AllDiscouragements);
        }

        protected override void OnApply(PageApplyEventArgs args)
        {
            if (args.ApplyBehavior == ApplyKind.Apply)
            {
                var encouragements = GetEncouragements();

                string[] userEncouragments = optionsDialogControl.Encouragements.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                encouragements.AllEncouragements = userEncouragments;

                string[] userDiscouragements = optionsDialogControl.Discouragements.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                encouragements.AllDiscouragements = userDiscouragements;
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