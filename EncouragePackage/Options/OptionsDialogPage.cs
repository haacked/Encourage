using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Haack.Encourage.Options
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false)]
    [ComVisible(true)]
    [Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9")]
    [Export(typeof(IEncouragements))]
    public class OptionsDialogPage : UIElementDialogPage, IEncouragements
    {
        static readonly Random random = new Random();

        OptionsDialogPageControl optionsDialogControl;
        const string defaultEncouragements = @"Nice Job!
Way to go!
Wow, nice change!
So good!
Bravo!
You rock!
Well done!
I see what you did there!
Genius work!
Thumbs up!
Coding win!
FTW!
Yep!
Nnnnailed it!";

        // TODO: Could we make this a notify property so when it changes, we update the encouragements list
        //       rather than splitting on it every time?
        public string Encouragements { get; set; }

        protected override UIElement Child
        {
            get
            {
                return optionsDialogControl
                       ?? (optionsDialogControl = new OptionsDialogPageControl
                       {
                           Encouragements = Encouragements ?? defaultEncouragements
                       });
            }
        }

        protected override void OnApply(PageApplyEventArgs args)
        {
            if (args.ApplyBehavior == ApplyKind.Apply)
            {
                try
                {
                    Encouragements = optionsDialogControl.Encouragements;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Could not save your encouragements" + e.Message);
                }
            }
            base.OnApply(args);
        }

        IList<string> EncouragementsList
        {
            get
            {
                return (Encouragements ?? defaultEncouragements).Split(
                    new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }
        }

        public string GetRandomEncouragement()
        {
            if (!EncouragementsList.Any()) return null;

            int randomIndex = random.Next(0, EncouragementsList.Count);
            return EncouragementsList[randomIndex];
        }
    }
}
