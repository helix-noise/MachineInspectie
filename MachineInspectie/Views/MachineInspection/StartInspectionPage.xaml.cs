using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
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
        private ControlQuestions questions;
        private List<MachineInspectionLibrary.ControlQuestion> _questionList;
        public string ListHeaderLanguage { get; set; }
        private ControlReport _controlReport;

        public StartInspectionPage()
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
            //    _language = e.Parameter.ToString();
            GetLanguage();
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
                //btnCapture.Content = "Neem foto";
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
                //btnCapture.Content = "Prendre une photo";
            }
        }

        private void GetLanguage()
        {
            var localLanguage = Windows.Storage.ApplicationData.Current.LocalSettings;
            _language = localLanguage.Values["Language"].ToString();
        }

        private async void ListPickerLocatie_ItemsPicked(ListPickerFlyout sender, ItemsPickedEventArgs args)
        {
            _selectedLocation = (Location)ListPickerLocatie.SelectedItem;
            btnLocation.Content = _selectedLocation.name;
            Dal.Matis matisController = new Dal.Matis();
            ListPickerMatis.ItemsSource = await matisController.GetMatisByLocation(_selectedLocation.name);
            ListPickerMatis.SelectedValuePath = "id";
            ListPickerMatis.DisplayMemberPath = "DisplayName";
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
            if (_language == "nl")
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
            ListPickerLocatie.ItemsSource = await locatie.GetListLocation();
            ListPickerLocatie.SelectedValuePath = "name";
            ListPickerLocatie.DisplayMemberPath = "DisplayName";
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                if (!string.IsNullOrEmpty(txtHour.Text))
                {
                    //TODO: Start controle
                    //ControlReport zou ook mee moeten.
                    _controlReport = new ControlReport
                    {
                        languageId = _language == "nl" ? 1 : 2,
                        locationId = _selectedLocation.id,
                        matisId = _selectedMatis.id,
                        matisServiceTime = Int32.Parse(txtHour.Text),
                        user = txtName.Text,
                        controlAnswers = new List<ControlAnswer>()
                    };

                    questions = new ControlQuestions();
                    //_questionList = await questions.ControlQuestionList(_selectedMatis.Category.name, _language);
                    //VoerControleUit(_questionList);
                    //UserInputFrame.Visibility = Visibility.Collapsed;
                    //_questionList = await questions.ControlQuestionList(_selectedMatis.Category.name, _language);
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
            MessageDialog msg;
            msg = language == "nl"
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

        #region QuestionFrame

        //private static int _stepCounter = 0;
        //private ControlQuestion _cQ;
        //private Translation _t;
        //private DateTime _startTimeQuestion;
        //private DateTime _endTimeQuestion;
        //private bool _testOk;
        //private List<ControlAnswerImage> _listAnswersWithImage = new List<ControlAnswerImage>();

        //public async void VoerControleUit(List<MachineInspectionLibrary.ControlQuestion> questionList)
        //{
        //    if (_stepCounter != 0)
        //    {
        //        ControlAnswerImage answerWithImage = new ControlAnswerImage();
        //        answerWithImage.controlQuestionId = _cQ.id;
        //        answerWithImage.startTime = _startTimeQuestion;
        //        answerWithImage.endTime = DateTime.Now;
        //        answerWithImage.testOk = _testOk;
        //        if (_bmpImage != null)
        //        {
        //            answerWithImage.images = new List<BitmapImage> {new BitmapImage(_bmpImage.UriSource)};
        //        }
        //        _listAnswersWithImage.Add(answerWithImage);
        //    }

        //    _startTimeQuestion = DateTime.Now;
        //    if (_stepCounter < questionList.Count)
        //    {
        //        QuestionFrame.Visibility = Visibility.Visible;
        //        _cQ = questionList[_stepCounter];
        //        _t = _cQ.translations[0];
        //        lblQuestion.Text = _t.question;
        //        _stepCounter += 1;
        //    }
        //    else
        //    {
        //        MessageDialog msg = new MessageDialog("Conrole uitgevoerd");
        //        var okBtn = new UICommand("Ok");
        //        msg.Commands.Add(okBtn);
        //        IUICommand result = await msg.ShowAsync();
        //    }
        //}

        //private void btnOk_Nok_Click(object sender, RoutedEventArgs e)
        //{
        //    _testOk = sender == btnOk;
        //    _bmpImage = null;
        //    //ControlAnswerImage answerWithImage = new ControlAnswerImage();
        //    if (_cQ.imageRequired == true)
        //    {
        //        PhotoFrame.Visibility = Visibility.Visible;
        //        StartCamera();
        //        //BitmapImage photo = await TakePicture();
        //        //answerWithImage.images.Add(TakePicture().Result);
        //        //PhotoFrame.Visibility = Visibility.Collapsed;
        //        //this.Frame.Navigate(typeof (PhotoPage), _language);
        //        //answerWithImage.images.Add(photo);
        //    }
        //    else
        //    {
        //        VoerControleUit(_questionList);
        //    }
        //    //answerWithImage.controlQuestionId = _cQ.id;
        //    //answerWithImage.testOk = sender == btnOk;
        //    //answerWithImage.startTime = _startTimeQuestion;
        //    //answerWithImage.endTime = DateTime.Now;
        //}

        //#endregion

        //#region PhotoFrame

        //private Windows.Media.Capture.MediaCapture _captureManager;
        //private BitmapImage _bmpImage;
        //private StorageFile _file;

        //private async void btnCapture_Click(object sender, RoutedEventArgs e)
        //{
        //    ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();
        //    _file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Photo.jpg", CreationCollisionOption.ReplaceExisting);
        //    await _captureManager.CapturePhotoToStorageFileAsync(imgFormat, _file);
        //    _bmpImage = new BitmapImage(new Uri(_file.Path));
        //    imgPhoto.Source = _bmpImage;
        //    await _captureManager.StopPreviewAsync();
        //    btnCapture.IsEnabled = false;
        //    btnCaptureReset.IsEnabled = true;
        //    btnCaptureOk.IsEnabled = true;
        //}

        //private void btnCaptureReset_Click(object sender, RoutedEventArgs e)
        //{
        //    imgPhoto.Source = null;
        //    _bmpImage = null;
        //    btnCapture.IsEnabled = true;
        //    btnCaptureOk.IsEnabled = false;
        //    StartCamera();
        //}

        //private void btnCaptureOk_Click(object sender, RoutedEventArgs e)
        //{
        //    PhotoFrame.Visibility = Visibility.Collapsed;
        //    imgPhoto.Source = null;
        //    cePreview.Source = null;
        //    btnCaptureOk.IsEnabled = false;
        //    btnCaptureReset.IsEnabled = false;
        //    btnCapture.IsEnabled = true;
        //    VoerControleUit(_questionList);
        //}

        ////public async Task<BitmapImage> TakePicture()
        ////{
        ////    PhotoFrame.Visibility = Visibility.Visible;
        ////    StartCamera();
        ////    await WheneClicked(btnCaptureOk);
        ////    PhotoFrame.Visibility = Visibility.Collapsed;
        ////    return _bmpImage;
        ////}

        ////public static Task WheneClicked(Button button)
        ////{
        ////    var tcs = new TaskCompletionSource<bool>();
        ////    RoutedEventHandler handler = null;
        ////    handler = (sender, e) =>
        ////    {
        ////        tcs.TrySetResult(true);
        ////        button.Click -= handler;
        ////    };
        ////    button.Click += handler;
        ////    return tcs.Task;
        ////}

        //public async void StartCamera()
        //{
        //    var cameraId = await GetCameraId(Windows.Devices.Enumeration.Panel.Back);
        //    _captureManager = new MediaCapture();
        //    await _captureManager.InitializeAsync(new MediaCaptureInitializationSettings
        //    {
        //        StreamingCaptureMode = StreamingCaptureMode.Video,
        //        PhotoCaptureSource = PhotoCaptureSource.Photo,
        //        AudioDeviceId = string.Empty,
        //        VideoDeviceId = cameraId.Id
        //    });
        //    cePreview.Source = _captureManager;
        //    _captureManager.SetPreviewRotation(VideoRotation.Clockwise180Degrees);
        //    await _captureManager.StartPreviewAsync();
        //}
        //private static async Task<DeviceInformation> GetCameraId(Windows.Devices.Enumeration.Panel desired)
        //{
        //    DeviceInformation deviceId =
        //        (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture)).FirstOrDefault(
        //            x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desired);
        //    if (deviceId != null)
        //    {
        //        return deviceId;
        //    }
        //    else throw new Exception(string.Format("Camera of type {0} doesn't exist.", desired));
        //}


        #endregion


    }
}
