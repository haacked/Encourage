using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using Haack.Encourage.Properties;
using Microsoft.VisualStudio.Shell;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Haack.Encourage.Options
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false)]
    [ComVisible(true)]
    [Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9")]
    public class OptionsDialogPage : UIElementDialogPage
    {
        OptionsDialogPageControl optionsDialogControl;

        public OptionsDialogPage()
        {
        }

        protected override UIElement Child
        {
            get
            {
                if (optionsDialogControl == null)
                {
                    optionsDialogControl = new OptionsDialogPageControl();
                }
                return optionsDialogControl;
            }
        }

        protected override void OnApply(PageApplyEventArgs args)
        {
            if (args.ApplyBehavior == ApplyKind.Apply)
            {
                try
                {
                    Settings.Default.Encouragements = optionsDialogControl.Encouragements;
                    Settings.Default.Save();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Could not save your encouragements" + e.Message);
                }
            }
            base.OnApply(args);
        }
    }
}
