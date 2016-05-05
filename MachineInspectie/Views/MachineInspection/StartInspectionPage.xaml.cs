using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MachineInspectionLibrary;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using MachineInspectie.Dal;
using MachineInspectie.Views.MachineInspection;
using Newtonsoft.Json;
using Matis = MachineInspectionLibrary.Matis;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MachineInspectie
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartInspectionPage : Page
    {
        private string _language;
        private Location _selectedLocation;
        private Matis _selectedMatis;
        public string ListHeaderLanguage { get; set; }
        private ControlReport _controlReport;

        public StartInspectionPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += BackButtonPress;
        }

        private void BackButtonPress(Object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= BackButtonPress;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var localSaved = Windows.Storage.ApplicationData.Current.LocalSettings;
            _language = localSaved.Values["Language"].ToString();
            if (_language == "nl")
            {
                lblName.Text = "Naam";
                lblLocation.Text = "Locatie";
                lblMatis.Text = "Matis";
                lblHour.Text = "Uur";
                btnStart.Content = "Start";
                btnReset.Content = "Reset";
                ListHeaderLanguage = "Maak u keuze";
                btnLocation.Content = "Selecteer een locatie";
            }
            else
            {
                lblName.Text = "Nom";
                lblLocation.Text = "Centre de tri";
                lblMatis.Text = "Matis";
                lblHour.Text = "Heures";
                btnStart.Content = "Start";
                btnReset.Content = "Reset";
                ListHeaderLanguage = "Faites votre choix";
                btnLocation.Content = "Choisissez votre lieu";
            }
            if (localSaved.Values["TempControlReport"] == null) return;
            _controlReport = JsonConvert.DeserializeObject<ControlReport>(localSaved.Values["TempControlReport"].ToString());
            _selectedLocation = JsonConvert.DeserializeObject<Location>(localSaved.Values["Templocation"].ToString());
            _selectedMatis = JsonConvert.DeserializeObject<Matis>(localSaved.Values["TempMatis"].ToString());
            txtName.Text = _controlReport.user;
            txtHour.Text = _controlReport.matisServiceTime.ToString();
            btnLocation.Content = _selectedLocation.name;
            btnMatis.Content = _selectedMatis.name;
            btnMatis.IsEnabled = true;
            btnStart.IsEnabled = true;
        }

        private void ListPickerLocatie_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            _selectedLocation = (Location)ListPickerLocatie.SelectedItem;
            btnLocation.Content = _selectedLocation.name;
            btnMatis.Content = _language == "nl" ? "Selecteer matis" : "Choisissez un matis";
            btnMatis.IsEnabled = true;
        }

        private void ListPickerMatis_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            _selectedMatis = (Matis)ListPickerMatis.SelectedItem;
            btnMatis.Content = _selectedMatis.name;
            btnStart.IsEnabled = true;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtHour.Text = "";
            btnMatis.Content = "";
            btnMatis.IsEnabled = false;
            ListPickerMatis.SelectedIndex = -1;
            btnLocation.Content = _language == "nl" ? "Selecteer een locatie" : "Choisissez votre lieu";
            ListPickerLocatie.SelectedIndex = -1;
            txtName.Text = "";
        }

        private async void btnMatis_Click(object sender, RoutedEventArgs e)
        {
            Dal.Matis matisController = new Dal.Matis();
            ListPickerMatis.ItemsSource = await matisController.GetMatisByLocation(_selectedLocation.name);
            ListPickerMatis.SelectedValuePath = "id";
            ListPickerMatis.DisplayMemberPath = "name";
        }

        private async void btnLocation_Click(object sender, RoutedEventArgs e)
        {
            ListPickerLocatie.ItemsSource = null;
            Locatie locatie = new Locatie();
            ListPickerLocatie.ItemsSource = await locatie.GetListLocation();
            ListPickerLocatie.SelectedValuePath = "name";
            ListPickerLocatie.DisplayMemberPath = "name";
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                if (!string.IsNullOrEmpty(txtHour.Text))
                {
                    //TODO: Start controle
                    _controlReport = new ControlReport
                    {
                        languageId = _language == "nl" ? 1 : 2,
                        locationId = _selectedLocation.id,
                        matisId = _selectedMatis.id,
                        matisServiceTime = int.Parse(txtHour.Text),
                        user = txtName.Text,
                        controlAnswers = new List<ControlAnswer>(),
                        startTime = DateTime.Now
                    };
                    var tempSave = ApplicationData.Current.LocalSettings;
                    tempSave.Values["TempMatis"] = JsonConvert.SerializeObject(_selectedMatis);
                    tempSave.Values["TempLocation"] = JsonConvert.SerializeObject(_selectedLocation);
                    tempSave.Values["TempControlReport"] = JsonConvert.SerializeObject(_controlReport);
                    ControlQuestions questions = new ControlQuestions();
                    this.Frame.Navigate(typeof(QuestionPage), await questions.ControlQuestionList(_selectedMatis.Category.name, _language));
                }
                else
                {
                    txtHour.BorderBrush = new SolidColorBrush(Colors.Red);
                    CheckStart(_language, lblHour.Text);
                }
                txtName.ClearValue(BorderBrushProperty);
            }
            else
            {
                txtName.BorderBrush = new SolidColorBrush(Colors.Red);
                CheckStart(_language, lblName.Text);
            }
        }

        public async void CheckStart(string language, string field)
        {
            var msg = language == "nl"
                ? new MessageDialog("Veld vergeten " + field + " !")
                : new MessageDialog("Champ oublié " + field + " !");
            var okBtn = new UICommand("Ok");
            msg.Commands.Add(okBtn);
            IUICommand result = await msg.ShowAsync();
            if (result != null && result.Label == "Ok")
            {
                return;
            }
        }

    }
}
