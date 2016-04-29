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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MachineInspectie.Views.MachineInspection
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PhotoPage : Page
    {
        private Windows.Media.Capture.MediaCapture _captureManager;
        

        public PhotoPage()
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
            if (e.Parameter.ToString() == "nl")
            {
                btnCapture.Content = "Neem foto";
            }
            StartCamera();
        }

        private async void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();
            StorageFile file =
                await
                    ApplicationData.Current.LocalFolder.CreateFileAsync("Photo.jpg",
                        CreationCollisionOption.GenerateUniqueName);
            await _captureManager.CapturePhotoToStorageFileAsync(imgFormat, file);
            BitmapImage bmpImage = new BitmapImage(new Uri(file.Path));
            imgPhoto.Source = bmpImage;
            await _captureManager.StopPreviewAsync();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            imgPhoto.Source = null;
            StartCamera();
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

    private async static Task<DeviceInformation> GetCameraId(Windows.Devices.Enumeration.Panel desired)
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

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
