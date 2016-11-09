using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Acoustic_Sensor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static int maxDB = 100; // predefined max value of noise in DB
        static int timer = 5; // predefined time between noises
        int timeLeft = 0; // remaining time: if the remaining time is 0, and a noise reading is made, LED light will NOT flash.

        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer dispatcherFlasher = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherFlasher.Tick += new EventHandler(dispatcherTimer_Flash);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherFlasher.Interval = new TimeSpan(0, 0, 1);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //decreases the timer every second
            timeLeft--;

            // Stops the timer if the seconds has been reached
            if (timeLeft == 0)
            {
                dispatcherTimer.Stop();
            }

        }

        private void dispatcherTimer_Flash(object sender, EventArgs e)
        {
            if (light.Opacity == 1)
            {
                light.Opacity = 0;
            } else
            {
                light.Opacity = 1;
            }

        }

        private void detectButton_Click(object sender, RoutedEventArgs e)
        {

            // Assumption: 
            // Only numerical #s will be inputted into soundReadings
            // This program does not handle the exception of a non-numeric input

            int value = Int32.Parse(soundReadings.Text);

            if (value > maxDB)
            {
                // 1) Self-destruct feature

                System.Windows.Forms.MessageBox.Show("Self-destruct has been activated");
                Environment.Exit(0);

            } else {
                
                // displays the last read sound level
                lastRead.Text = value.ToString() + " DB";

                // Reset the textbox to default and opacity
                soundReadings.Text = "Insert a number here";
                soundReadings.Opacity = 0.5;


                // 2) Warning Light feature

                if (timeLeft > 0)
                {
                    // If the remaining time is greater than 0, and a noise reading is made, LED light will flash
                    dispatcherFlasher.Start();

                    Task.Delay(timer * 1000).ContinueWith(_ =>
                        {
                            dispatcherFlasher.Stop();
                        }
                    );
                    
                } else
                {
                    // Starts a timer in seconds based on global variable "timer"
                    timeLeft = timer;
                    dispatcherTimer.Start();
                }
                
            }
        }

        private void soundReadings_GotFocus(object sender, RoutedEventArgs e)
        {
            // clears the textbox when the user clicks.
            // opacity is also changed for UX purposes
            soundReadings.Text = "";
            soundReadings.Opacity = 1;
        }

    }
}
