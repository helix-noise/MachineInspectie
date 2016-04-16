using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace MachineInspectie
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += BackButtonPress;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        private async void BackButtonPress(Object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            else
            {
                var msg = new MessageDialog("Applicatie sluiten? / fermer l'application");
                var okBtn = new UICommand("Ja / Oui");
                var cancelBtn = new UICommand("Nee / No");
                msg.Commands.Add(okBtn);
                msg.Commands.Add(cancelBtn);
                IUICommand result = await msg.ShowAsync();

                if (result != null && result.Label == "Ja / Oui")
                {
                    Application.Current.Exit();
                }
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void btnDutch_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SelectionPage), "Nl");
        }

        private void btnFrench_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SelectionPage), "Fr");
        }
    }
}
