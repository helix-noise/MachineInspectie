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
        private string _headerTaal;
        public Invulscherm()
        {
            this.InitializeComponent();
            //HardwareButtons.BackPressed += BackButtonPress;
            _locaties = TestList();
            ListPickerLocatie.ItemsSource = TestList();
            ListPickerLocatie.SelectedValuePath = "Id";
            ListPickerLocatie.DisplayMemberPath = "Naam";
        }

        //void BackButtonPress(Object sender, BackPressedEventArgs e)
        //{
        //    if (Frame.CanGoBack)
        //    {
        //        e.Handled = true;
        //        Frame.GoBack();
        //    }
        //}

        ///TestData
        private List<Locatie> TestList()
        {
            List<Locatie> temp = new List<Locatie>();
            temp.Add(new Locatie(1, 1111, "Test1"));
            temp.Add(new Locatie(2, 1222, "Test2"));
            temp.Add(new Locatie(3, 1333, "Test3"));
            temp.Add(new Locatie(4, 1444, "Test4"));
            temp.Add(new Locatie(5, 1555, "Test5"));
            temp.Add(new Locatie(6, 1666, "Test6"));
            temp.Add(new Locatie(7, 1777, "Test7"));
            temp.Add(new Locatie(8, 1888, "Test8"));
            return temp;
        }

        private List<Locatie> TestList2()
        {
            List<Locatie> temp = new List<Locatie>();
            temp.Add(new Locatie(1, 1111, "TestM1"));
            temp.Add(new Locatie(2, 1222, "TestM2"));
            temp.Add(new Locatie(3, 1333, "TestM3"));
            temp.Add(new Locatie(1, 1444, "TestM4"));
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
                _headerTaal = "Maak u keuze";
                btnLocatie.Content = "Selecteer een locatie";
            }
            else
            {
                lblNaam.Text = "Nom";
                lblLocatie.Text = "Emplacement";
                lblMatis.Text = "Matis";
                lblUur.Text = "Heure";
                btnStart.Content = "Lancer le contrôle";
                btnReset.Content = "Réinitialiser";
                _headerTaal = "Faites votre choix";
                btnLocatie.Content = "Choisissez votre lieu";
            }
        }

        private void ListPickerLocatie_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            Locatie temp = (Locatie)ListPickerLocatie.SelectedItem;
            btnLocatie.Content = temp.Naam;
            ListPickerMatis.ItemsSource = TestList2();
            ListPickerMatis.DisplayMemberPath = "Naam";
            ListPickerMatis.SelectedValuePath = "Id";
            if (_taal == "Nl")
            {
                btnMatis.Content = "Selecteer matis";
            }
            else
            {
                btnMatis.Content = "Choisissez un matis";
            }
            btnMatis.IsEnabled = true;
        }

        private void ListPickerMatis_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            Locatie temp = (Locatie)ListPickerMatis.SelectedItem;
            btnMatis.Content = temp.Naam;
        }
    }
}
