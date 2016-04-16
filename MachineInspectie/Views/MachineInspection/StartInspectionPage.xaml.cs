using System;
using System.Collections.Generic;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MachineInspectionLibrary;
using System.Net.Http;
using Windows.UI.Popups;
using MachineInspectie.Dal;
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
        public string ListHeaderLanguage { get; set; }

        public StartInspectionPage()
        {
            this.InitializeComponent();
        }

        #region TestData
        ///TestData
        //private List<MachineInspectionLibrary.Location> TestList()
        //{
        //    List<MachineInspectionLibrary.Location> temp = new List<MachineInspectionLibrary.Location>();
        //    temp.Add(new MachineInspectionLibrary.Location(1, 1111, "Locatie1"));
        //    temp.Add(new MachineInspectionLibrary.Location(2, 1222, "Locatie2"));
        //    temp.Add(new MachineInspectionLibrary.Location(3, 1333, "Locatie3"));
        //    temp.Add(new MachineInspectionLibrary.Location(4, 1444, "Locatie4"));
        //    temp.Add(new MachineInspectionLibrary.Location(5, 1555, "Locatie5"));
        //    temp.Add(new MachineInspectionLibrary.Location(6, 1666, "Locatie6"));
        //    temp.Add(new MachineInspectionLibrary.Location(7, 1777, "Locatie7"));
        //    temp.Add(new MachineInspectionLibrary.Location(8, 1888, "Locatie8"));
        //    return temp;
        //}

        //private List<Matis> TestListMatis()
        //{
        //    List<Matis> temp = new List<Matis>();
        //    temp.Add(new Matis(1, 111, "Matis1", new MatisCategory(1, "MatisCath1"), new MachineInspectionLibrary.Location(1, 3366, "Locatie1")));
        //    temp.Add(new Matis(2, 222, "Matis2", new MatisCategory(2, "MatisCath2"), new MachineInspectionLibrary.Location(2, 8899, "Locatie2")));
        //    temp.Add(new Matis(3, 333, "Matis3", new MatisCategory(3, "MatisCath3"), new MachineInspectionLibrary.Location(3, 4561, "Locatie3")));
        //    temp.Add(new Matis(4, 444, "Matis4", new MatisCategory(4, "MatisCath4"), new MachineInspectionLibrary.Location(4, 7851, "Locatie4")));
        //    temp.Add(new Matis(5, 555, "Matis5", new MatisCategory(5, "MatisCath5"), new MachineInspectionLibrary.Location(5, 2564, "Locatie5")));
        //    return temp;
        //}
        #endregion


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _language = e.Parameter.ToString();
            if (_language == "Nl")
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
        }

        private async void ListPickerLocatie_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            MachineInspectionLibrary.Location temp = (MachineInspectionLibrary.Location)ListPickerLocatie.SelectedItem;
            btnLocation.Content = temp.name;
            Dal.Matis matisController = new Dal.Matis();
            ListPickerMatis.ItemsSource = await matisController.GetMatisByLocation(temp.name);
            ListPickerMatis.SelectedValuePath = "id";
            ListPickerMatis.DisplayMemberPath = "DisplayName";
            btnMatis.Content = _language == "Nl" ? "Selecteer matis" : "Choisissez un matis";
            btnMatis.IsEnabled = true;
        }

        private void ListPickerMatis_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            Matis temp = (Matis)ListPickerMatis.SelectedItem;
            btnMatis.Content = temp.name;
            btnStart.IsEnabled = true;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtHour.Text = "";
            btnMatis.Content = "";
            btnMatis.IsEnabled = false;
            ListPickerMatis.SelectedIndex = -1;
            if (_language == "Nl")
            {
                btnLocation.Content = "Selecteer een locatie";
            }
            else
            {
                btnLocation.Content = "Choisissez votre lieu";
            }
            ListPickerLocatie.SelectedIndex = -1;
            txtName.Text = "";
        }

        private async void btnLocation_Click(object sender, RoutedEventArgs e)
        {
            ListPickerLocatie.ItemsSource = null;
            Locatie locatie = new Locatie();
            ListPickerLocatie.ItemsSource = await locatie.LocationList();
            ListPickerLocatie.SelectedValuePath = "name";
            ListPickerLocatie.DisplayMemberPath = "DisplayName";
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                if (!string.IsNullOrEmpty(txtHour.Text))
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    CheckStart(_language, lblHour.Text);
                }
            }
            else
            {
                CheckStart(_language, lblName.Text);
            }
        }

        public async void CheckStart(string language, string field)
        {
            MessageDialog msg;
            if (language == "Nl")
            {
                msg = new MessageDialog("Veld vergeten " + field + " !");
            }
            else
            {
                msg = new MessageDialog("Champ oublié " + field + " !");
            }
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
