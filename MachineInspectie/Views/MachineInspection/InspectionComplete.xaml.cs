using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using MachineInspectie.Dal;
using MachineInspectie.Model;
using MachineInspectionLibrary;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MachineInspectie.Views.MachineInspection
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InspectionComplete : Page
    {
        private string _language;
        private ControlReport _controlReport;
        private List<ControlAnswerByte> _controlAnswerImages;
        private List<ControlAnswer> _answers;


        public InspectionComplete()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //_controlAnswerImages = (List<ControlAnswerByte>)e.Parameter;
            _answers = (List<ControlAnswer>)e.Parameter;
            var localSave = ApplicationData.Current.LocalSettings;
            _language = localSave.Values["Language"].ToString();
            _controlReport = JsonConvert.DeserializeObject<ControlReport>(localSave.Values["TempControlReport"].ToString());
            if (_language == "nl")
            {
                lblComplete.Text = "Controle is gebeurt";
                btnSend.Content = "Controle verzenden";
            }
            else
            {
                lblComplete.Text = "Contrôle terminé";
                btnSend.Content = "Envoyez la contrôle";
            }
        }

        private async void btnSend_Click(object sender, RoutedEventArgs e)
        {
            btnSend.IsEnabled = false;
            if (_language == "nl")
            {
                lblComplete.Text = "uw controle wordt verzonden" + Environment.NewLine + "Even gedult aub ...";
            }
            else
            {
                lblComplete.Text = "Votre contrôle est en train d'être envoyé" + Environment.NewLine + "Veuillez patienter un instant svp ...";
            }
            prSendData.Visibility = Visibility.Visible;
            prSendData.IsActive = true;
            PostQuestionList pQL = new PostQuestionList();
            string responce = await pQL.SendControlRapport(_answers, _controlReport);
            if (responce == "Ok")
            {
                lblComplete.Text = _language == "nl" ? "Uw controle werd met succes verzonden" : "L'envoi de votre contrôle a réussi";
            }
            else
            {

            }
            prSendData.IsActive = false;
            prSendData.Visibility = Visibility.Collapsed;

        }
    }
}
