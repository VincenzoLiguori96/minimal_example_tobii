using Prove_Tobii.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Prove_Tobii.Views
{
   
    public partial class WelcomeScreen : Window
    {
        private Uri confirm = new Uri(@"/Eye Tracking Quiz;component/Resources/confirmation.wav", UriKind.Relative);
        private SoundPlayer player;
        private int counter = 3, currentAudioFile;
        private Timer timer1;
        WavListAudioFiles[] audioFiles;
        private bool ltopWatched = false, rtopWatched = false, lbotWatched = false, rbotWatched = false;
        public WelcomeScreen(WavListAudioFiles[] audioFiles, int currentAudioFile)
        {
            InitializeComponent();
            TobiiHost.Host.Commands.Input.SendActivationModeOn();
            this.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(HandleEsc);
            this.audioFiles = audioFiles;
            this.currentAudioFile = currentAudioFile;
            player = new SoundPlayer(System.Windows.Application.GetResourceStream(confirm).Stream);
            player.LoadAsync();
        }

        public WelcomeScreen()
        {
            InitializeComponent();
            player = new SoundPlayer(System.Windows.Application.GetResourceStream(confirm).Stream);
            player.LoadAsync();
        }

        private void HandleEsc(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Sei sicuro di voler uscire dall'applicazione?", "Attenzione", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        System.Windows.Application.Current.Shutdown();
                        break;
                    default:
                        break;
                }
            }
        }

        private void buttonHoCapito_Click(object sender, RoutedEventArgs e)
        {
            player.PlaySync();
            benvenuto.Visibility = Visibility.Collapsed;
            secondaLinea.Visibility = Visibility.Collapsed;
            terzaLinea.Visibility = Visibility.Collapsed;
            centrale.Text = "Perfetto. Prima di iniziare, esercitati con il Tobii Eye tracker, guardando tutti gli angoli della finestra!";
            buttonHoCapito.Visibility = Visibility.Collapsed;
            letImageAppear();

        }

        public void letImageAppear()
        {
            ltop.Visibility = Visibility.Visible;
            rtop.Visibility = Visibility.Visible;
            lbot.Visibility = Visibility.Visible;
            rbot.Visibility = Visibility.Visible;
        }

        private void ltop_HasGazeChanged(object sender, Tobii.Interaction.Wpf.HasGazeChangedRoutedEventArgs e)
        {
            if (e.HasGaze)
            {
                Debug.WriteLine("Entro");
                var bitmapPath = @"/Eye Tracking Quiz;component/Resources/green.png";
                BitmapImage bitmapImage = new BitmapImage(new Uri(bitmapPath, UriKind.Relative));
                ltop.Source = bitmapImage;
                ltopWatched = true;
                if(rtopWatched && lbotWatched && rbotWatched)
                {
                    terzaLinea.Visibility = Visibility.Visible;
                    terzaLinea.Text = "Ora sei abbastanza pratico! Preparati a rispondere alle domande in: ";
                    centrale.Text = "3";
                    startTimer();
                }
            }
        }

        private void rtop_HasGazeChanged(object sender, Tobii.Interaction.Wpf.HasGazeChangedRoutedEventArgs e)
        {
            if (e.HasGaze)
            {
                var bitmapPath = @"/Eye Tracking Quiz;component/Resources/green.png";
                BitmapImage bitmapImage = new BitmapImage(new Uri(bitmapPath, UriKind.Relative));
                rtopWatched = true;
                rtop.Source = bitmapImage;
                if (ltopWatched && lbotWatched && rbotWatched)
                {
                    terzaLinea.Visibility = Visibility.Visible;
                    terzaLinea.Text = "Ora sei abbastanza pratico! Preparati a rispondere alle domande in: ";
                    centrale.Text = "3";
                    startTimer();
                }
            }
        }

        private void lbot_HasGazeChanged(object sender, Tobii.Interaction.Wpf.HasGazeChangedRoutedEventArgs e)
        {
            if (e.HasGaze)
            {
                var bitmapPath = @"/Eye Tracking Quiz;component/Resources/green.png";
                BitmapImage bitmapImage = new BitmapImage(new Uri(bitmapPath, UriKind.Relative));
                lbotWatched = true;
                lbot.Source = bitmapImage;
                if (rtopWatched && ltopWatched && rbotWatched)
                {
                    terzaLinea.Visibility = Visibility.Visible;
                    terzaLinea.Text = "Ora sei abbastanza pratico! Preparati a rispondere alle domande in: ";
                    centrale.Text = "3";
                    startTimer();
                }
            }
        }

        private void rbot_HasGazeChanged(object sender, Tobii.Interaction.Wpf.HasGazeChangedRoutedEventArgs e)
        {
            if (e.HasGaze)
            {
                var bitmapPath = @"/Eye Tracking Quiz;component/Resources/green.png";
                BitmapImage bitmapImage = new BitmapImage(new Uri(bitmapPath, UriKind.Relative));
                rbot.Source = bitmapImage;      
                rbotWatched = true;
                if (rtopWatched && lbotWatched && ltopWatched)
                {
                    terzaLinea.Visibility = Visibility.Visible;
                    terzaLinea.Text = "Ora sei abbastanza pratico! Preparati a rispondere alle domande in: ";
                    centrale.Text = "3";
                    startTimer();
                }
            }
        }

        private void startTimer()
        {
            timer1 = new System.Windows.Forms.Timer();
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Interval = 1000; // 1 second
            timer1.Start();
            centrale.Text = counter.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--;
            if (counter == 0)
            {
                timer1.Stop();
                //Not necessary for the minimal example
                /*PlayWindow playWindow = new PlayWindow(audioFiles, 0);
                playWindow.Show();*/
                this.Close();
            }
            centrale.Text = counter.ToString();
            
        }

    }
}
