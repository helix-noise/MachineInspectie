using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using MachineInspectie.Views.MachineInspection;
using MachineInspectionLibrary;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MachineInspectie
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuestionPage : Page
    {
        #region Private members
        private List<ControlQuestion> _questionList = new List<ControlQuestion>();
        private DateTime _startTimeQuestion;
        private static int _stepCounter;
        private string _language;
        private bool _testResult;
        private List<ControlAnswerImage> _listAnswersWithImage = new List<ControlAnswerImage>();
        private ControlQuestion _controlQuestion;
        private Translation _translation;
        private BitmapImage _photo;
        private MediaCapture _captureManager;
        private bool _inspectionStarted = false;
        #endregion

        public QuestionPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += BackButtonPress;
        }

        private async void BackButtonPress(Object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            if (_inspectionStarted)
            {
                if (_language == "nl")
                {
                    var msg = new MessageDialog("Het ingevoerde antwoord wordt verwijdert.", "U keert terug naar vorige vraag");
                    var okBtn = new UICommand("Doorgaan");
                    var cancelBtn = new UICommand("Annuleren");
                    msg.Commands.Add(okBtn);
                    msg.Commands.Add(cancelBtn);
                    IUICommand result = await msg.ShowAsync();

                    if (result != null && result.Label == "Doorgaan")
                    {
                        //TODO:
                        _stepCounter -= 2;
                        if (_stepCounter == 0)
                        {
                            _inspectionStarted = false;
                            _listAnswersWithImage = new List<ControlAnswerImage>();
                        }
                        else
                        {
                            _inspectionStarted = false;
                            _listAnswersWithImage.RemoveAt(_stepCounter);
                        }
                        DoInspection();
                    }
                }
                else
                {
                    var msg = new MessageDialog("Réponse importée est supprimée.", "Vous revenez à la question précédente");
                    var okBtn = new UICommand("Continuer");
                    var cancelBtn = new UICommand("Annuler");
                    msg.Commands.Add(okBtn);
                    msg.Commands.Add(cancelBtn);
                    IUICommand result = await msg.ShowAsync();

                    if (result != null && result.Label == "Continuer")
                    {
                        //TODO:
                        _stepCounter -= 1;
                        if (_stepCounter == 0)
                        {
                            _inspectionStarted = false;
                            _listAnswersWithImage = new List<ControlAnswerImage>();
                        }
                        else
                        {
                            _inspectionStarted = false;
                            _listAnswersWithImage.RemoveAt(_stepCounter);
                        }
                        DoInspection();
                    }
                }
            }
            else
            {
                if (Frame.CanGoBack)
                {
                    _stepCounter = 0;
                    Frame.GoBack();
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
            var locallanguage = Windows.Storage.ApplicationData.Current.LocalSettings;
            _language = locallanguage.Values["Language"].ToString();
            _questionList = (List<ControlQuestion>)e.Parameter;
            btnCapture.Content = _language == "nl" ? "Neem foto" : "Prendre une photo";
            DoInspection();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= BackButtonPress;
        }

        #region QuestionListFrame

        private void btnOk_Nok_Click(object sender, RoutedEventArgs e)
        {
            _inspectionStarted = true;
            _testResult = sender == btnOk;
            _photo = null;
            if (_controlQuestion.imageRequired)
            {
                PhotoFrame.Visibility = Visibility.Visible;
                StartCamera();
            }
            else
            {
                DoInspection();
            }
        }

        public async void DoInspection()
        {
            if (_stepCounter != 0)
            {
                ControlAnswerImage answerWithImage = new ControlAnswerImage();
                answerWithImage.controlQuestionId = _controlQuestion.id;
                answerWithImage.startTime = _startTimeQuestion;
                answerWithImage.endTime = DateTime.Now;
                answerWithImage.testOk = _testResult;


                _listAnswersWithImage.Add(answerWithImage);
            }

            _startTimeQuestion = DateTime.Now;
            if (_stepCounter < _questionList.Count)
            {
                _controlQuestion = _questionList[_stepCounter];
                _translation = _controlQuestion.translations[0];
                lblQuestion.Text = _translation.question;
                _stepCounter += 1;
            }
            else
            {
                MessageDialog msg = new MessageDialog("Conrole uitgevoerd");
                var okBtn = new UICommand("Ok");
                msg.Commands.Add(okBtn);
                IUICommand result = await msg.ShowAsync();
            }

        }

        #endregion

        #region PhotoFrame

        private async void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Photo.jpg", CreationCollisionOption.ReplaceExisting);
            await _captureManager.CapturePhotoToStorageFileAsync(imgFormat, file);
            _photo = new BitmapImage(new Uri(file.Path));
            imgPhoto.Source = _photo;
            await _captureManager.StopPreviewAsync();
            btnCapture.IsEnabled = false;
            btnCaptureReset.IsEnabled = true;
            btnCaptureOk.IsEnabled = true;
        }

        private void btnCaptureReset_Click(object sender, RoutedEventArgs e)
        {
            imgPhoto.Source = null;
            _photo = null;
            btnCapture.IsEnabled = true;
            btnCaptureOk.IsEnabled = false;
            StartCamera();
        }

        private void btnCaptureOk_Click(object sender, RoutedEventArgs e)
        {
            PhotoFrame.Visibility = Visibility.Collapsed;
            imgPhoto.Source = null;
            cePreview.Source = null;
            btnCaptureOk.IsEnabled = false;
            btnCaptureReset.IsEnabled = false;
            btnCapture.IsEnabled = true;
            DoInspection();
        }

        public async void StartCamera()
        {
            var cameraId = await GetCameraId(Windows.Devices.Enumeration.Panel.Back);
            _captureManager = new MediaCapture();
            await _captureManager.InitializeAsync(new MediaCaptureInitializationSettings
            {
                StreamingCaptureMode = StreamingCaptureMode.Video,
                PhotoCaptureSource = PhotoCaptureSource.Photo,
                AudioDeviceId = string.Empty,
                VideoDeviceId = cameraId.Id
            });
            cePreview.Source = _captureManager;
            _captureManager.SetPreviewRotation(VideoRotation.Clockwise180Degrees);
            await _captureManager.StartPreviewAsync();
        }
        private static async Task<DeviceInformation> GetCameraId(Windows.Devices.Enumeration.Panel desired)
        {
            DeviceInformation deviceId =
                (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture)).FirstOrDefault(
                    x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desired);
            if (deviceId != null)
            {
                return deviceId;
            }
            else throw new Exception(string.Format("Camera of type {0} doesn't exist.", desired));
        }

        #endregion


        //public static Task WheneClicked(Button button)
        //{
        //    var tcs = new TaskCompletionSource<bool>();
        //    RoutedEventHandler handler = null;
        //    handler = (sender, e) =>
        //    {
        //        tcs.TrySetResult(true);
        //        button.Click -= handler;
        //    };
        //    button.Click += handler;
        //    return tcs.Task;
        //}


    }
}
