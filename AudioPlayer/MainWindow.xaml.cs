using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Runtime.InteropServices;


namespace AudioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<String, String> playListCollection; //stores filenames and their corresponding paths
        private string mediaName = "media"; //alias used in MCI API
        bool isPlaying; //Player state variable
        bool isUserTriggered; // defines if event is cause by the user

        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(string command, StringBuilder buffer, int bufferSize, IntPtr hwndCallback);

        public MainWindow()
        {
            InitializeComponent();

            this.Title = "Audio Player";
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        void OnLoadPlayList(object sender, RoutedEventArgs e)
        {
            playListCollection = new Dictionary<String, String>();

            for (int i = 1; i < 22; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = (i.ToString() + ".");
                Playlist.Items.Add(textBlock);
            }
        }

        private void LoadFiles_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "WAV files (*.wav)|*.wav|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                Playlist.Items.Clear();
                int idx = 1; //file indexd on the list

                foreach (string filename in openFileDialog.FileNames)
                {
                    Playlist.Items.Add(idx.ToString() + "." + System.IO.Path.GetFileName(filename));
                    playListCollection[System.IO.Path.GetFileName(filename)] = filename; //populating dictionary with the filenames and their paths
                    idx++;
                }     
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            long millisecs = 120000;
            String playCommand = String.Format("Play " + mediaName + " from {0}", millisecs);
            long status = mciSendString(playCommand, null, 0, IntPtr.Zero);
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayPause_Click(object sender, RoutedEventArgs e)
        {
            Int32 ret = -1;
            String playCommand = "Close " + mediaName;
            mciSendString(playCommand, null, 0, IntPtr.Zero);
            var filepath = playListCollection.First().Value;
            playCommand = "Open \"" + filepath + "\" type MPEGVideo alias " + mediaName;
            ret = mciSendString(playCommand, null, 0, IntPtr.Zero);
            playCommand = "Play " + mediaName + " notify";
            ret = mciSendString(playCommand, null, 0, IntPtr.Zero);

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
