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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MachineInspectie
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectionPage : Page
    {
        private string _language;

        public SelectionPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += BackButtonPress;
            CheckLanguage();
            btnMachineInspection.Content = _language == "nl" ? "Machine inspectie" : "Inspection de la machine";
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
                if (_language == "nl")
                {
                    var msg = new MessageDialog("Applicatie sluiten?");
                    var okBtn = new UICommand("Ja");
                    var cancelBtn = new UICommand("Nee");
                    msg.Commands.Add(okBtn);
                    msg.Commands.Add(cancelBtn);
                    IUICommand result = await msg.ShowAsync();

                    if (result != null && result.Label == "Ja")
                    {
                        Application.Current.Exit();
                    }
                }
                else
                {
                    var msg = new MessageDialog("Fermer l'application");
                    var okBtn = new UICommand("Oui");
                    var cancelBtn = new UICommand("No");
                    msg.Commands.Add(okBtn);
                    msg.Commands.Add(cancelBtn);
                    IUICommand result = await msg.ShowAsync();

                    if (result != null && result.Label == "Oui")
                    {
                        Application.Current.Exit();
                    }
                }

            }
        }

        public void CheckLanguage()
        {
            var localSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (!localSetting.Values.ContainsKey("Language"))
            {
                if (tgbNl.IsChecked == true)
                {
                    localSetting.Values["Language"] = "nl";
                }
                else
                {
                    localSetting.Values["Language"] = "fr";
                }
            }
            else
            {
                _language = localSetting.Values["Language"].ToString();
                if (_language == "nl")
                {
                    tgbNl.IsChecked = true;
                    tgbFr.IsChecked = false;
                }
                else
                {
                    tgbFr.IsChecked = true;
                    tgbNl.IsChecked = false;
                }
            }
        }

        public void SaveLanguage(string language)
        {
            var localSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSetting.Values["Language"] = language;
            _language = language;
            btnMachineInspection.Content = _language == "nl" ? "Machine inspectie" : "Inspection de la machine";
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= BackButtonPress;
        }

        private void btnMachineInspection_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(StartInspectionPage));
        }

        private void tgbNl_Click(object sender, RoutedEventArgs e)
        {
            tgbFr.IsChecked = false;
            SaveLanguage("nl");
        }

        private void tgbFr_Click(object sender, RoutedEventArgs e)
        {
            tgbNl.IsChecked = false;
            SaveLanguage("fr");

        }
    }
}
