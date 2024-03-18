using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assi3_PRN221
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int countdownHours;
        private int countdownMinutes;
        private int countdownSeconds;
        private TimeSpan countdownTimeSpan;
        private Thread countdownThread;
        private bool isCountdownRunning = false;
        private bool isCountdownPaused = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartCountdown_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtHours.Text, out int hours) && int.TryParse(txtMinutes.Text, out int minutes) && int.TryParse(txtSeconds.Text, out int seconds))
            {
                countdownHours = hours;
                countdownMinutes = minutes;
                countdownSeconds = seconds;
                countdownTimeSpan = new TimeSpan(countdownHours, countdownMinutes, countdownSeconds);
                isCountdownRunning = true;
                countdownThread = new Thread(new ThreadStart(Countdown));
                countdownThread.Start();
            }
            else
            {
                MessageBox.Show("Please enter valid integer values for hours, minutes, and seconds.");
            }
        }

        private void PauseCountdown_Click(object sender, RoutedEventArgs e)
        {
            isCountdownPaused = true;
            btnPause.Visibility = Visibility.Collapsed;
            btnResume.Visibility = Visibility.Visible;
        }

        private void ResumeCountdown_Click(object sender, RoutedEventArgs e)
        {
            isCountdownPaused = false;
            btnPause.Visibility = Visibility.Visible;
            btnResume.Visibility = Visibility.Collapsed;
        }

        private void ResetCountdown_Click(object sender, RoutedEventArgs e)
        {
            isCountdownRunning = false;
            isCountdownPaused = false;
            txtCountdownDisplay.Text = "";
            txtHours.Text = "";
            txtMinutes.Text = "";
            txtSeconds.Text = "";
            btnPause.Visibility = Visibility.Visible;
            btnResume.Visibility = Visibility.Collapsed;
        }

        private void Countdown()
        {
            while (isCountdownRunning && countdownTimeSpan.TotalSeconds > 0)
            {
                if (!isCountdownPaused)
                {
                    UpdateCountdownDisplay(countdownTimeSpan.ToString(@"hh\:mm\:ss"));
                    countdownTimeSpan = countdownTimeSpan.Subtract(TimeSpan.FromSeconds(1));
                }
                Thread.Sleep(1000); // Sleep for 1 second
            }
            isCountdownRunning = false;
            UpdateCountdownDisplay("00:00:00");
        }

        private void UpdateCountdownDisplay(string time)
        {
            if (txtCountdownDisplay.Dispatcher.CheckAccess())
            {
                txtCountdownDisplay.Text = time;
            }
            else
            {
                txtCountdownDisplay.Dispatcher.Invoke(() => UpdateCountdownDisplay(time));
            }
        }
    }
}