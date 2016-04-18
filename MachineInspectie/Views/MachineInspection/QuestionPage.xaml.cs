using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using MachineInspectionLibrary;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace MachineInspectie
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QuestionPage : Page
    {
        private List<ControlQuestion> _questionList;
        private DateTime _start;
        private DateTime _end;

        public QuestionPage()
        {
            this.InitializeComponent();
        }

        public async Task TestLoop()
        {
            foreach (ControlQuestion controlQuestion in _questionList)
            {
                foreach (Translation translation in controlQuestion.translations)
                {
                    lblQuestion.Text = translation.question;
                }
                await WheneClicked(btnOk);
            }
            _end = DateTime.Now;
        }

        public static Task WheneClicked(Button button)
        {
            var tcs = new TaskCompletionSource<bool>();
            RoutedEventHandler handler = null;
            handler = (sender, e) =>
            {
                tcs.TrySetResult(true);
                button.Click -= handler;
            };
            button.Click += handler;
            return tcs.Task;
        }


        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _questionList = (List<ControlQuestion>)e.Parameter;
            _start = DateTime.Now;
            await TestLoop();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string s = "unknown button";
            if (sender == btnOk)
            {
                s = "Ok";
            }
            else
            {
                s = "Nok";
            }
        }
    }
}
