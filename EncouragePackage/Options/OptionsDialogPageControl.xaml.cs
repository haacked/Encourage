using Microsoft.VisualStudio.Shell;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Haack.Encourage.Options
{
    /// <summary>
    ///     Interaction logic for OptionsDialogPageControl.xaml
    /// </summary>
    public partial class OptionsDialogPageControl : UserControl
    {
        public OptionsDialogPageControl()
        {
            InitializeComponent();

            encouragements.AddHandler(UIElementDialogPage.DialogKeyPendingEvent, new RoutedEventHandler(OnDialogKeyPendingEvent));
            discouragements.AddHandler(UIElementDialogPage.DialogKeyPendingEvent, new RoutedEventHandler(OnDialogKeyPendingEvent));
        }

        public string Discouragements
        {
            get { return discouragements.Text; }
            set { discouragements.Text = value; }
        }

        public string Encouragements
        {
            get { return encouragements.Text; }
            set { encouragements.Text = value; }
        }

        private void OnDialogKeyPendingEvent(object sender, RoutedEventArgs e)
        {
            var args = e as DialogKeyEventArgs;
            if (args != null && args.Key == Key.Enter)
            {
                e.Handled = true;
            }
        }
    }
}