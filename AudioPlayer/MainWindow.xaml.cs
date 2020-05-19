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

namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double PlaylistWidth;
        public double PlaylistHeight {get;set;}

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Audio Player";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void LoadFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        void OnLoadPlayList(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Onload Playlist");
            for(int i = 1; i < 22; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = (i.ToString() + ".");
                Playlist.Items.Add(textBlock);
            }          
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VolumeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void Playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
