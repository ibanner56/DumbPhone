using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DumbPhone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private EventHandler handler;

        /// <summary>
        /// Initializes the EventHandler, opens the window, and starts the music.
        /// </summary>
        public MainWindow()
        {
            this.handler = new EventHandler(@"..\..\words.txt");
            try
            {
                InitializeComponent();
                whoPlays.LoadedBehavior = MediaState.Manual;
                try
                {
                    whoPlays.Source = new Uri(@"..\..\who1.mp3", UriKind.RelativeOrAbsolute);
                    whoPlays.Play();
                }
                catch (Exception e)
                {
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Data);
            }
        }

        /// <summary>
        /// Does the proper button sequence for the current settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            outBox.Text = handler.ButtonClick(sender, e);
            e.Handled = true;
        }

        /// <summary>
        /// Toggles predictive mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            handler.TogglePredictive();
            outBox.Text = "";
            e.Handled = true;
        }

        /// <summary>
        /// Switches to the next track.
        /// If the music doesn't work, then you're screwing with how Visual Studio
        /// builds and it's your fault the music isn't working.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void whoPlays_MediaEnded(object sender, RoutedEventArgs e)
        {
            try
            {
                MediaElement media = (MediaElement)sender;
                String uri = media.Source.OriginalString;
                String p1 = handler.GetNewUri(uri);
                whoPlays.Source = new Uri(p1, UriKind.RelativeOrAbsolute);
                whoPlays.Play();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Mutes and Unmutes the music.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void whoClicked(object sender, RoutedEventArgs e)
        {
            whoPlays.IsMuted = !whoPlays.IsMuted;
        }
        
    }
}
