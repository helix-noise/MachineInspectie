using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
    public sealed partial class Invulscherm : Page
    {
        private List<Locatie> _locaties;
        private string _taal;
        public Invulscherm()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += BackButtonPress;
            _locaties = TestList();
            ddlLocatie.ItemsSource = _locaties;
            ddlLocatie.DisplayMemberPath = "Naam";
            ddlLocatie.SelectedValuePath = "Id";
        }

        void BackButtonPress(Object sender, BackPressedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
            }
        }

        ///TestData
        private List<Locatie> TestList()
        {
            List<Locatie> temp = new List<Locatie>();
            temp.Add(new Locatie(1, 1111, "Test1"));
            temp.Add(new Locatie(1, 1222, "Test2"));
            temp.Add(new Locatie(1, 1333, "Test3"));
            temp.Add(new Locatie(1, 1444, "Test4"));
            return temp;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _taal = e.Parameter.ToString();
            if (_taal == "Nl")
            {
                lblNaam.Text = "Naam";
                lblLocatie.Text = "Locatie";
                lblMatis.Text = "Matis";
                lblUur.Text = "Uur";
                btnStart.Content = "Start controle";
                btnReset.Content = "Reset";
            }
            else
            {
                lblNaam.Text = "Nom";
                lblLocatie.Text = "Emplacement";
                lblMatis.Text = "Matis";
                lblUur.Text = "Heure";
                btnStart.Content = "Lancer le contrôle";
                btnReset.Content = "Réinitialiser";
            }
        }
    }
}
